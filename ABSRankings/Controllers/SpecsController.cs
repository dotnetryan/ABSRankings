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
    public class SpecsController : Controller
    {
        private ABSRankingsContext db = new ABSRankingsContext();

        // GET: Specs
        public ActionResult Index()
        {
            return View(db.Specs.ToList()); //TODO add to repo
        }

        // GET: Specs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spec spec = db.Specs.Find(id); //TODO add to repo
            if (spec == null)
            {
                return HttpNotFound();
            }
            return View(spec);
        }

        // GET: Specs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spec spec = db.Specs.Find(id); //TODO add to repo
            if (spec == null)
            {
                return HttpNotFound();
            }
            return View(spec);
        }

        // POST: Specs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Spec spec = db.Specs.Find(id);
            db.Specs.Remove(spec);
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
