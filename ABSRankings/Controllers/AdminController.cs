using ABSRankings.Data;
using ABSRankings.Models;
using ABSRankings.Services;
using System.Data;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABSRankings.Controllers
{
    public class AdminController : Controller
    {
        private WCLService wclService = new WCLService();
        private ABSRankingsContext db = new ABSRankingsContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdatePlayerDataFromWCL(Ranking ranking)
        {
            var players = db.Players.Where(p => p.Status == true).ToList(); //TODO add to repo

            foreach (var player in players)
            {
                var rankings = wclService.GetPlayerDataFromWCL(player);

                rankings.ForEach(delegate (Ranking r) { db.Rankings.Add(r); }); //TODO add to repo and check for duplicates
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //TODO update player logs, zones, encounters, and specs
    }
}