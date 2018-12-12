using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ABSRankings.Data;
using ABSRankings.Models;
using ABSRankings.Services;
using ABSRankings.ViewModels;
using Chart.Mvc.ComplexChart;

namespace ABSRankings.Controllers
{
    public class PlayersController : Controller
    {
        private WCLService wclService = new WCLService();
        private ABSRankingsContext db = new ABSRankingsContext();

        // GET: Players
        public ActionResult Index()
        {
            var players = db.Players.Include(p => p.Spec); //TODO add to repo
            return View(players.ToList());
        }

        // GET: Players/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(PlayerDetails(id));
        }

        public ActionResult FullDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(PlayerDetails(id));
        }

        // GET: Players/Create
        public ActionResult Create()
        {
            ViewBag.SpecId = Utilities.SpecList(db.Specs.ToList());
            ViewBag.Metrics = Utilities.MetricList(db.Metrics.ToList());

            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Player player)
        {
            if (ModelState.IsValid)
            {
                // check for existing player
                var existingPlayer = db.Players.Where(p => p.Name == player.Name && p.SpecId == player.SpecId).FirstOrDefault(); //TODO add to repo

                if (existingPlayer == null)
                {
                    player.LastUpdated = DateTime.Now;

                    db.Players.Add(player);

                    //add player rankings
                    if (player.Status)
                    {
                        var rankings = wclService.GetPlayerDataFromWCL(player);
                        rankings.ForEach(delegate (Ranking r) { db.Rankings.Add(r); }); //TODO add to repo

                        // add percentages for player in all difficulties and metrics, not individual fights
                        AddPercentages(player, rankings);
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Player with that spec and metric already exists";
                }
            }


            ViewBag.SpecId = Utilities.SpecList(db.Specs.ToList());
            ViewBag.Metrics = Utilities.MetricList(db.Metrics.ToList());

            return View(player);
        }

        // GET: Players/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }

            ViewBag.SpecId = Utilities.SpecList(db.Specs.ToList(), player.SpecId);
            ViewBag.Metrics = Utilities.MetricList(db.Metrics.ToList());

            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Player player)
        {
            if (ModelState.IsValid)
            {
                player.LastUpdated = DateTime.Now;

                db.Entry(player).State = EntityState.Modified; //TODO add to repo

                //update player rankings
                if (player.Status)
                {
                    var rankings = wclService.GetPlayerDataFromWCL(player);

                    //delete existing rankings
                    db.Rankings.RemoveRange(db.Rankings.Where(r => r.PlayerId == player.ID));

                    rankings.ForEach(delegate (Ranking r) { db.Rankings.Add(r); }); //TODO add to repo

                    // add percentages for player in all difficulties and metrics, not individual fights
                    AddPercentages(player, rankings);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SpecId = Utilities.SpecList(db.Specs.ToList(), player.SpecId);
            ViewBag.Metrics = Utilities.MetricList(db.Metrics.ToList());

            return View(player);
        }

        // GET: Players/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Include(p => p.Spec).Include(r => r.Rankings).Where(p => p.ID == id).FirstOrDefault(); //TODO add to repo
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player = db.Players.Find(id); //TODO add to repo
            db.Players.Remove(player);

            //TODO delete rankings

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void AddPercentages(Player player, List<Ranking> rankings)
        {
            foreach (var difficulty in db.Difficulties.ToList())
            {
                foreach (var metric in db.Metrics.ToList())
                {
                    var coefficient = db.Coefficients.Where(c => c.DifficultyId == difficulty.ID && c.MetricId == metric.ID).FirstOrDefault();

                    decimal sum = 0, overallSpecPercentage = 0;
                    foreach (var rank in rankings.Where(r => r.DifficultyId == difficulty.ID && r.MetricId == metric.ID).ToList())
                    {
                        var percentage = (rank.Total / (player.Sim * coefficient.Value)) - 1;
                        sum += percentage;
                    }

                    if (sum != 0)
                    {
                        overallSpecPercentage = sum / rankings.Count();

                        // there should only be one of these saved per day, so check if it's there and update it
                        var existingOverallPercentage = db.OverallPercentages.Where(o => o.PlayerId == player.ID && o.DifficultyId == difficulty.ID && o.MetricId == metric.ID && DbFunctions.TruncateTime(o.DateAdded) == DbFunctions.TruncateTime(DateTime.Today)).FirstOrDefault();

                        if (existingOverallPercentage == null)
                        {
                            db.OverallPercentages.Add(new OverallPercentages
                            {
                                Player = player,
                                Difficulty = difficulty,
                                Metric = metric,
                                Percentage = overallSpecPercentage,
                                DateAdded = DateTime.Now
                            });
                        }
                        else
                        {
                            existingOverallPercentage.Percentage = overallSpecPercentage;
                            db.Entry(existingOverallPercentage).State = EntityState.Modified; //TODO add to repo
                        }
                    }
                }
            }
        }

        private PlayerDetailsDisplay PlayerDetails(int? id)
        {
            var player = db.Players.Include(p => p.Spec).Include(r => r.Rankings).Where(p => p.ID == id).FirstOrDefault(); //TODO add to repo

            var difficulties = db.Difficulties.ToList();
            var coefficients = db.Coefficients.ToList();
            var rankingPercentages = new List<RankingPercentageDisplay>();
            var benchmarks = new List<BenchmarkDisplay>();
            var playerMetrics = db.Rankings.Where(r => r.PlayerId == player.ID).Select(m => new { m.Metric }).Distinct();
            var dataSets = new List<ComplexDataset>();

            foreach (var difficulty in difficulties)
            {
                // add player benchmarks to model
                foreach (var playerMetric in playerMetrics)
                {
                    foreach (var coefficient in coefficients.Where(c => c.MetricId == playerMetric.Metric.ID && c.DifficultyId == difficulty.ID).ToList())
                    {
                        benchmarks.Add(new BenchmarkDisplay
                        {
                            Difficulty = coefficient.Difficulty,
                            Metric = playerMetric.Metric,
                            Value = player.Sim * coefficient.Value
                        });
                    }
                }

                var rankings = db.Rankings.Include(p => p.Player).Include(s => s.Player.Spec).Include(m => m.Metric).Include(e => e.Encounter).Include(m => m.Metric)
                    .Where(r => r.DifficultyId == difficulty.ID && r.PlayerId == player.ID).ToList(); //TODO move to repo

                decimal percentageSum = 0;

                foreach (var rank in rankings)
                {
                    //get coefficient
                    var coefficient = coefficients.Where(c => c.DifficultyId == difficulty.ID && c.MetricId == rank.Metric.ID).FirstOrDefault();

                    //calculate percentage
                    var percentage = (rank.Total / (player.Sim * coefficient.Value)) - 1;

                    // get css class for overall percentage range
                    string cssClass = Utilities.CssClassForPercent(percentage);

                    percentageSum += percentage;

                    //construct WCL url
                    var wclReportUrl = db.Configurations.Where(c => c.Key == "wclReportUrl").FirstOrDefault();
                    string reportUrl = string.Format("{0}/{1}#fight={2}", wclReportUrl.Value, rank.WCLReportId, rank.WCLFightId.ToString());

                    rankingPercentages.Add(new RankingPercentageDisplay
                    {
                        ID = rank.ID,
                        Player = player,
                        Encounter = rank.Encounter.Name,
                        Difficulty = difficulty.Name,
                        PlayerILevel = rank.PlayerILevel,
                        ReportUrl = reportUrl,
                        Metric = rank.Metric.Description,
                        Total = rank.Total,
                        Percentage = percentage,
                        CssClass = cssClass
                    });
                }

                decimal overallPercentage = 0;
                if (rankings.Count() != 0)
                {
                    overallPercentage = percentageSum / rankings.Count();
                }

                // add previous percentages to show trends
                foreach (var metric in db.Metrics.ToList())
                {
                    var percentages = db.OverallPercentages.Where(o => o.PlayerId == player.ID && o.DifficultyId == difficulty.ID && o.MetricId == metric.ID);

                    if (percentages.Count() > 0)
                    {
                        var dataPoints = new List<double>();
                        foreach (var percent in percentages)
                        {
                            dataPoints.Add(Convert.ToDouble(percent.Percentage * 100));
                        }

                        //generate random number for colors
                        Random random = new Random();
                        int r1 = random.Next(0, 255);
                        int r2 = random.Next(0, 255);
                        int r3 = random.Next(0, 255);

                        var dataSet = new ComplexDataset();
                        dataSet.Label = string.Format("{0} ({1}) {2} - {3}", player.Name, player.Spec.Name, difficulty.Name, metric.Description);
                        dataSet.FillColor = "rgba(0, 0, 0, 0)";
                        dataSet.Data = dataPoints;
                        dataSet.PointColor = string.Format("rgba({0}, {1}, {2}, 0.7)", r1, r2, r3);
                        dataSet.StrokeColor = string.Format("rgba({0}, {1}, {2}, 0.7)", r1, r2, r3);
                        dataSet.PointHighlightFill = string.Format("rgba({0}, {1}, {2}, 0.7)", r1, r2, r3);
                        dataSet.PointHighlightStroke = string.Format("rgba({0}, {1}, {2}, 0.7)", r1, r2, r3);
                        dataSet.PointStrokeColor = string.Format("rgba({0}, {1}, {2}, 0.7)", r1, r2, r3);

                        dataSets.Add(dataSet);
                    }
                }
            };

            var dateLabels = new List<string>();

            var dates = db.OverallPercentages.Where(o => o.PlayerId == id).OrderBy(o => o.DateAdded).GroupBy(d => DbFunctions.TruncateTime(d.DateAdded)).ToList();

            foreach (var date in dates)
            {
                var temp = date.Select(d => new { d.DateAdded }).FirstOrDefault();

                dateLabels.Add(temp.DateAdded.ToShortDateString());
            }

            var overallPercentageTrends = new OverallPercentageTrendDisplay
            {
                Labels = dateLabels,
                ComplexDataSets = dataSets
            };

            return new PlayerDetailsDisplay { Player = player, Benchmarks = benchmarks, Rankings = rankingPercentages, OverallPercentageTrends = overallPercentageTrends };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
