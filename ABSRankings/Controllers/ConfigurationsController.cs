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
using ABSRankings.Services;

namespace ABSRankings.Controllers
{
    public class ConfigurationsController : Controller
    {
        private ABSRankingsContext db = new ABSRankingsContext();
        private WCLService wclService = new WCLService();

        // GET: Configurations
        public ActionResult Index()
        {
            return View(db.Configurations.ToList());
        }

        // GET: Configurations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Configuration configuration = db.Configurations.Find(id);
            if (configuration == null)
            {
                return HttpNotFound();
            }
            return View(configuration);
        }

        // GET: Configurations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Configurations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Key,Value")] Configuration configuration)
        {
            if (ModelState.IsValid)
            {
                db.Configurations.Add(configuration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(configuration);
        }

        // GET: Configurations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Configuration configuration = db.Configurations.Find(id);
            if (configuration == null)
            {
                return HttpNotFound();
            }
            return View(configuration);
        }

        // POST: Configurations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Key,Value")] Configuration configuration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(configuration).State = EntityState.Modified;

                // when changing current zone, need to get encounters
                if (configuration.Key == "currentWclRaidZoneId")
                {
                    var zoneId = Convert.ToInt32(configuration.Value);

                    var existingZone = db.Zones.Where(z => z.ID == zoneId).FirstOrDefault();

                    if (existingZone == null)
                    {
                        var zone = wclService.GetZones().Where(z => z.Id == zoneId).FirstOrDefault();

                        var newZone = new Zone { Name = zone.Name, WCLZoneId = zone.Id, WCLFrozen = zone.Frozen };
                        db.Zones.Add(newZone); //TODO add to repo
                        db.SaveChanges();

                        foreach (var encounter in zone.Encounters)
                        {
                            db.Encounters.Add(new Encounter { Name = encounter.Name, WCLEncounterId = encounter.Id, Zone = newZone }); //TODO add to repo
                        }
                        db.SaveChanges();
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(configuration);
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
