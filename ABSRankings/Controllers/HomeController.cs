using ABSRankings.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using ABSRankings.Models;
using ABSRankings.Services;
using ABSRankings.ViewModels;
using System.Web.Script.Serialization;
using System.Text;
using System.Dynamic;

namespace ABSRankings.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ABSRankingsContext db = new ABSRankingsContext();

        public ActionResult Index()
        {
            var metrics = db.Metrics.ToList(); //TODO add to repo
            var difficulties = db.Difficulties.ToList(); //TODO add to repo

            var difficultiesList = new List<HomeMatrixDisplay>();

            foreach (var difficulty in difficulties)
            {
                difficultiesList.Add(new HomeMatrixDisplay {
                    Difficulty = difficulty,
                    Metrics = metrics
                });
            }

            return View(difficultiesList.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}