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
    public class CoefficientsController : Controller
    {
        private ABSRankingsContext db = new ABSRankingsContext();

        // GET: Coefficients
        public ActionResult Index()
        {
            var coefficients = db.Coefficients.Include(c => c.Difficulty).Include(c => c.Metric);
            return View(coefficients.ToList());
        }

        // GET: Coefficients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coefficient coefficient = db.Coefficients.Find(id);
            if (coefficient == null)
            {
                return HttpNotFound();
            }
            return View(coefficient);
        }

        // GET: Coefficients/Create
        public ActionResult Create()
        {
            ViewBag.DifficultyId = new SelectList(db.Difficulties, "ID", "Name");
            ViewBag.MetricId = new SelectList(db.Metrics, "ID", "Type");
            return View();
        }

        // POST: Coefficients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DifficultyId,MetricId,Value")] Coefficient coefficient)
        {
            if (ModelState.IsValid)
            {
                db.Coefficients.Add(coefficient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DifficultyId = new SelectList(db.Difficulties, "ID", "Name", coefficient.DifficultyId);
            ViewBag.MetricId = new SelectList(db.Metrics, "ID", "Type", coefficient.MetricId);
            return View(coefficient);
        }

        // GET: Coefficients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coefficient coefficient = db.Coefficients.Find(id);
            if (coefficient == null)
            {
                return HttpNotFound();
            }
            ViewBag.DifficultyId = new SelectList(db.Difficulties, "ID", "Name", coefficient.DifficultyId);
            ViewBag.MetricId = new SelectList(db.Metrics, "ID", "Type", coefficient.MetricId);
            return View(coefficient);
        }

        // POST: Coefficients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DifficultyId,MetricId,Value")] Coefficient coefficient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coefficient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DifficultyId = new SelectList(db.Difficulties, "ID", "Name", coefficient.DifficultyId);
            ViewBag.MetricId = new SelectList(db.Metrics, "ID", "Type", coefficient.MetricId);
            return View(coefficient);
        }

        // GET: Coefficients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coefficient coefficient = db.Coefficients.Find(id);
            if (coefficient == null)
            {
                return HttpNotFound();
            }
            return View(coefficient);
        }

        // POST: Coefficients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Coefficient coefficient = db.Coefficients.Find(id);
            db.Coefficients.Remove(coefficient);
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
