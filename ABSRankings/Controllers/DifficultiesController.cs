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
    public class DifficultiesController : Controller
    {
        private ABSRankingsContext db = new ABSRankingsContext();

        // GET: Difficulties
        public ActionResult Index()
        {
            return View(db.Difficulties.ToList()); //TODO add to repo
        }

        // GET: Difficulties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Difficulty difficulty = db.Difficulties.Find(id); //TODO add to repo
            if (difficulty == null)
            {
                return HttpNotFound();
            }
            return View(difficulty);
        }

        // GET: Difficulties/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Difficulty difficulty = db.Difficulties.Find(id); //TODO add to repo
            if (difficulty == null)
            {
                return HttpNotFound();
            }
            return View(difficulty);
        }

        // POST: Difficulties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Difficulty difficulty = db.Difficulties.Find(id); //TODO add to repo
            db.Difficulties.Remove(difficulty);
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
