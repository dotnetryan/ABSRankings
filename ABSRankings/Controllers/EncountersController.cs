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
    public class EncountersController : Controller
    {
        private ABSRankingsContext db = new ABSRankingsContext();

        // GET: Encounters
        public ActionResult Index()
        {
            var encounters = db.Encounters.Include(e => e.Zone); //TODO add to repo
            return View(encounters.ToList());
        }

        // GET: Encounters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encounter encounter = (Encounter)db.Encounters.Include(e => e.Zone).Where(e => e.ID == id).FirstOrDefault(); //TODO add to repo
            if (encounter == null)
            {
                return HttpNotFound();
            }
            return View(encounter);
        }

        // POST: Encounters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Encounter encounter = db.Encounters.Find(id); //TODO add to repo
            db.Encounters.Remove(encounter);
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
