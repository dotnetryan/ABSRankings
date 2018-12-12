using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ABSRankings.Data;
using ABSRankings.Models;

namespace ABSRankings.Controllers
{
    public class MetricsController : Controller
    {
        private ABSRankingsContext db = new ABSRankingsContext();

        // GET: Metrics
        public ActionResult Index()
        {
            return View(db.Metrics.ToList()); //TODO add to repo
        }

        // GET: Metrics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metric metric = db.Metrics.Find(id); //TODO add to repo
            if (metric == null)
            {
                return HttpNotFound();
            }
            return View(metric);
        }

        // GET: Metrics/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Metrics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Type,Description")] Metric metric)
        {
            if (ModelState.IsValid)
            {
                db.Metrics.Add(metric); //TODO add to repo
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(metric);
        }

        // GET: Metrics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metric metric = db.Metrics.Find(id); //TODO add to repo
            if (metric == null)
            {
                return HttpNotFound();
            }
            return View(metric);
        }

        // POST: Metrics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Type,Description")] Metric metric)
        {
            if (ModelState.IsValid)
            {
                db.Entry(metric).State = EntityState.Modified; //TODO add to repo
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(metric);
        }

        // GET: Metrics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metric metric = db.Metrics.Find(id); //TODO add to repo
            if (metric == null)
            {
                return HttpNotFound();
            }
            return View(metric);
        }

        // POST: Metrics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Metric metric = db.Metrics.Find(id); //TODO add to repo
            db.Metrics.Remove(metric);
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
