using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ABSRankings.Data;

namespace ABSRankings.Models
{
    public class OverallPercentagesController : Controller
    {
        private ABSRankingsContext db = new ABSRankingsContext();

        // GET: OverallPercentages
        public ActionResult Index()
        {
            var overallPercentages = db.OverallPercentages.Include(o => o.Player).Include(s => s.Player.Spec).Include(d => d.Difficulty).Include(m => m.Metric);
            return View(overallPercentages.ToList());
        }

        // GET: OverallPercentages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OverallPercentages overallPercentages = db.OverallPercentages.Include(o => o.Player).Include(s => s.Player.Spec).Include(d => d.Difficulty).Include(m => m.Metric).Where(o => o.ID == id).FirstOrDefault();
            if (overallPercentages == null)
            {
                return HttpNotFound();
            }
            return View(overallPercentages);
        }

        // GET: OverallPercentages/Create
        public ActionResult Create()
        {
            ViewBag.PlayerId = Utilities.PlayerList(db.Players.Include(s => s.Spec).ToList());
            ViewBag.DifficultyId = new SelectList(db.Difficulties, "ID", "Name");
            ViewBag.MetricId = new SelectList(db.Metrics, "ID", "Type");
            return View();
        }

        // POST: OverallPercentages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PlayerId,DifficultyId,MetricId,Percentage,DateAdded")] OverallPercentages overallPercentages)
        {
            if (ModelState.IsValid)
            {
                db.OverallPercentages.Add(overallPercentages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.PlayerId = Utilities.PlayerList(db.Players.ToList(), overallPercentages.PlayerId);
            return View(overallPercentages);
        }

        // GET: OverallPercentages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OverallPercentages overallPercentages = db.OverallPercentages.Include(o => o.Player).Include(s => s.Player.Spec).Where(o => o.ID == id).FirstOrDefault();
            if (overallPercentages == null)
            {
                return HttpNotFound();
            }

            ViewBag.PlayerId = Utilities.PlayerList(db.Players.Include(s => s.Spec).ToList(), overallPercentages.PlayerId);
            ViewBag.DifficultyId = new SelectList(db.Difficulties, "ID", "Name");
            ViewBag.MetricId = new SelectList(db.Metrics, "ID", "Type");
            return View(overallPercentages);
        }

        // POST: OverallPercentages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PlayerId,DifficultyId,MetricId,Percentage,DateAdded")] OverallPercentages overallPercentages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(overallPercentages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PlayerId = new SelectList(db.Players, "ID", "Name", overallPercentages.PlayerId);
            return View(overallPercentages);
        }

        // GET: OverallPercentages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OverallPercentages overallPercentages = db.OverallPercentages.Include(o => o.Player).Include(s => s.Player.Spec).Include(d => d.Difficulty).Include(m => m.Metric).Where(o => o.ID == id).FirstOrDefault();
            if (overallPercentages == null)
            {
                return HttpNotFound();
            }
            return View(overallPercentages);
        }

        // POST: OverallPercentages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OverallPercentages overallPercentages = db.OverallPercentages.Find(id);
            db.OverallPercentages.Remove(overallPercentages);
            db.SaveChanges();
            return RedirectToAction("Index");
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
