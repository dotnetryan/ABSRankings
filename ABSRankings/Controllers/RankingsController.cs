using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ABSRankings.Data;
using ABSRankings.Models;
using ABSRankings.ViewModels;
using Chart.Mvc.ComplexChart;

namespace ABSRankings.Controllers
{
    public class RankingsController : Controller
    {
        private ABSRankingsContext db = new ABSRankingsContext();

        // GET: Rankings
        public ActionResult Index()
        {
            var wclReportUrl = db.Configurations.Where(c => c.Key == "wclReportUrl").FirstOrDefault();
            var rankings = db.Rankings.Include(p => p.Player).Include(s => s.Player.Spec).Include(e => e.Encounter).Include(d => d.Difficulty).Include(m => m.Metric); //TODO add to repo

            var rankingPercentages = new List<RankingPercentageDisplay>();
            foreach (var rank in rankings)
            {
                string reportUrl = string.Format("{0}/{1}#fight={2}", wclReportUrl.Value, rank.WCLReportId, rank.WCLFightId.ToString());

                rankingPercentages.Add(new RankingPercentageDisplay
                {
                    ID = rank.ID,
                    Player = rank.Player,
                    Encounter = rank.Encounter.Name,
                    Difficulty = rank.Difficulty.Name,
                    PlayerILevel = rank.PlayerILevel,
                    ReportUrl = reportUrl,
                    Metric = rank.Metric.Description,
                    Total = rank.Total
                });
            }

            return View(rankingPercentages.ToList());
        }

        // GET: Rankings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ranking = db.Rankings.Include(p => p.Player).Include(s => s.Player.Spec).Include(e => e.Encounter).Include(d => d.Difficulty).Include(m => m.Metric).Where(r => r.ID == id).FirstOrDefault(); //TODO add to repo
            if (ranking == null)
            {
                return HttpNotFound();
            }
            return View(ranking);
        }

        // GET: Rankings/Create
        public ActionResult Create()
        {
            ViewBag.DifficultyId = new SelectList(db.Difficulties, "ID", "Name");
            ViewBag.EncounterId = new SelectList(db.Encounters, "ID", "Name");
            ViewBag.MetricId = new SelectList(db.Metrics, "ID", "Type");
            ViewBag.PlayerId = new SelectList(db.Players, "ID", "Name");
            return View();
        }

        // POST: Rankings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PlayerId,EncounterId,DifficultyId,MetricId,WCLReportId,WCLFightId,PlayerILevel,Total")] Ranking ranking)
        {
            if (ModelState.IsValid)
            {
                db.Rankings.Add(ranking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DifficultyId = new SelectList(db.Difficulties, "ID", "Name", ranking.DifficultyId);
            ViewBag.EncounterId = new SelectList(db.Encounters, "ID", "Name", ranking.EncounterId);
            ViewBag.MetricId = new SelectList(db.Metrics, "ID", "Type", ranking.MetricId);
            ViewBag.PlayerId = new SelectList(db.Players, "ID", "Name", ranking.PlayerId);
            return View(ranking);
        }

        // GET: Rankings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ranking ranking = db.Rankings.Find(id);
            if (ranking == null)
            {
                return HttpNotFound();
            }
            ViewBag.DifficultyId = new SelectList(db.Difficulties, "ID", "Name", ranking.DifficultyId);
            ViewBag.EncounterId = new SelectList(db.Encounters, "ID", "Name", ranking.EncounterId);
            ViewBag.MetricId = new SelectList(db.Metrics, "ID", "Type", ranking.MetricId);
            ViewBag.PlayerId = new SelectList(db.Players, "ID", "Name", ranking.PlayerId);
            return View(ranking);
        }

        // POST: Rankings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PlayerId,EncounterId,DifficultyId,MetricId,WCLReportId,WCLFightId,PlayerILevel,Total")] Ranking ranking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ranking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DifficultyId = new SelectList(db.Difficulties, "ID", "Name", ranking.DifficultyId);
            ViewBag.EncounterId = new SelectList(db.Encounters, "ID", "Name", ranking.EncounterId);
            ViewBag.MetricId = new SelectList(db.Metrics, "ID", "Type", ranking.MetricId);
            ViewBag.PlayerId = new SelectList(db.Players, "ID", "Name", ranking.PlayerId);
            return View(ranking);
        }

        // GET: Rankings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ranking ranking = db.Rankings.Find(id); //TODO add to repo
            if (ranking == null)
            {
                return HttpNotFound();
            }
            return View(ranking);
        }

        // POST: Rankings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ranking ranking = db.Rankings.Find(id); //TODO add to repo
            db.Rankings.Remove(ranking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Overall(int? metricId, int? difficultyId)
        {
            if (metricId == null || difficultyId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var rankingPercentages = new List<RankingPercentageDisplay>();
            var players = db.Players;
            var dataSets = new List<ComplexDataset>();

            //get coefficient
            var coefficient = db.Coefficients.Where(c => c.DifficultyId == difficultyId && c.MetricId == metricId).FirstOrDefault();

            foreach (var uniquePlayer in players.Where(p => p.Status == true).Select(n => n.Name).Distinct().ToList())
            {
                var rankingPercentageBySpec = new List<RankingPercentageBySpecDisplay>();

                List<decimal> sumOfAllRanks = new List<decimal>();

                foreach (var player in players.Include(s => s.Spec).Where(n => n.Name == uniquePlayer).ToList())
                {
                    var rankings = db.Rankings.Where(r => r.DifficultyId == difficultyId && r.PlayerId == player.ID && r.MetricId == metricId).ToList();

                    decimal specSum = 0, overallSpecPercentage = 0;

                    foreach (var rank in rankings)
                    {
                        var percentage = (rank.Total / (player.Sim * coefficient.Value)) - 1;
                        specSum += percentage;
                        sumOfAllRanks.Add(percentage);
                    }

                    if (rankings.Count() > 0)
                    {
                        overallSpecPercentage = specSum / rankings.Count();
                    }

                    // get css class for overall percentage range
                    string cssClassBySpec = Utilities.CssClassForPercent(overallSpecPercentage);

                    rankingPercentageBySpec.Add(new RankingPercentageBySpecDisplay
                    {
                        Player = player,
                        Percentage = overallSpecPercentage,
                        CssClass = cssClassBySpec
                    });

                    // add previous percentages to show trends
                    var percentages = db.OverallPercentages.Where(o => o.PlayerId == player.ID && o.DifficultyId == difficultyId && o.MetricId == metricId);

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
                        dataSet.Label = string.Format("{0} ({1})", player.Name, player.Spec.Name);
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

                // calculate your percentage across all specs and ranks
                decimal sum = 0, overallPercentage = 0;
                if (sumOfAllRanks.Count() > 0)
                {
                    foreach (var rankPercentage in sumOfAllRanks)
                    {
                        sum += rankPercentage;
                    }

                    overallPercentage = sum / sumOfAllRanks.Count();
                }

                string cssClass = Utilities.CssClassForPercent(overallPercentage);

                rankingPercentages.Add(new RankingPercentageDisplay {
                    PlayerName = uniquePlayer,
                    SpecPercentages = rankingPercentageBySpec,
                    Percentage = overallPercentage,
                    CssClass = cssClass
                });
            }

            ViewBag.Difficulty = db.Difficulties.Where(d => d.ID == difficultyId).FirstOrDefault(); //TODO add to repo
            ViewBag.Metric = db.Metrics.Where(m => m.ID == metricId).FirstOrDefault(); //TODO add to repo

            var overallRankingPercentages = new OverallRankingPercentageDisplay();
            overallRankingPercentages.RankingPercentages = rankingPercentages.OrderByDescending(p => p.Percentage).ToList();

            // construct trends object
            var dateLabels = new List<string>();
            var dates = db.OverallPercentages.OrderBy(o => o.DateAdded).GroupBy(d => DbFunctions.TruncateTime(d.DateAdded)).ToList();

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
            overallRankingPercentages.OverallPercentageTrends = overallPercentageTrends;

            return View(overallRankingPercentages);
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
