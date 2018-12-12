using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using ABSRankings.Models;
using wcl = ABSRankings.Models.WCL;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ABSRankings.Data;

namespace ABSRankings.Services
{
    public interface IWCLService
    {
        List<wcl.Ranking> GetRankingsByName(string name, string metric);
        List<wcl.Zone> GetZones();
        List<wcl.Class> GetClasses();
        List<Ranking> GetPlayerDataFromWCL(Player player);
    }

    public class WCLService : IWCLService
    {
        private ABSRankingsContext db = new ABSRankingsContext();

        public List<wcl.Ranking> GetRankingsByName(string name, string metric)
        {
            string _apiUrl = db.Configurations.Where(c => c.Key == "apiUrl").FirstOrDefault().Value;
            string _wowServer = db.Configurations.Where(c => c.Key == "wowServer").FirstOrDefault().Value;
            string _apiKey = db.Configurations.Where(c => c.Key == "apiKey").FirstOrDefault().Value;
            string _zone = db.Configurations.Where(c => c.Key == "currentWclRaidZoneId").FirstOrDefault().Value;
            string _partition = db.Configurations.Where(c => c.Key == "partition").FirstOrDefault().Value;

            var uri = "";

            if (_zone != null)
            {
                uri = string.Format("{0}/rankings/character/{1}/{2}/us?metric={3}&api_key={4}&zone={5}&partition={6}", _apiUrl, name, _wowServer, metric, _apiKey, _zone, _partition);
            }
            else
            {
                uri = string.Format("{0}/rankings/character/{1}/{2}/us?metric={3}&api_key={4}&partition={5}", _apiUrl, name, _wowServer, metric, _apiKey, _partition);
            }

            using (HttpClient httpClient = new HttpClient())
            {
                Task<string> response = httpClient.GetStringAsync(uri);

                try
                {
                    return JsonConvert.DeserializeObject<List<wcl.Ranking>>(response.Result);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Get zones from WCL and saves to DB, inlcudes encounters class
        /// </summary>
        /// <returns></returns>
        public List<wcl.Zone> GetZones()
        {
            string _apiUrl = db.Configurations.Where(c => c.Key == "apiUrl").FirstOrDefault().Value;
            string _apiKey = db.Configurations.Where(c => c.Key == "apiKey").FirstOrDefault().Value;

            var uri = string.Format("{0}/zones?api_key={1}", _apiUrl, _apiKey);

            using (HttpClient httpClient = new HttpClient())
            {
                Task<string> response = httpClient.GetStringAsync(uri);

                try
                {
                    return JsonConvert.DeserializeObject<List<wcl.Zone>>(response.Result);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Get specs and classes from WCL and save to DB, includes specs class
        /// </summary>
        /// <returns></returns>
        public List<wcl.Class> GetClasses()
        {
            string _apiUrl = db.Configurations.Where(c => c.Key == "apiUrl").FirstOrDefault().Value;
            string _apiKey = db.Configurations.Where(c => c.Key == "apiKey").FirstOrDefault().Value;

            var uri = string.Format("{0}/classes?api_key={1}", _apiUrl, _apiKey);

            using (HttpClient httpClient = new HttpClient())
            {
                Task<string> response = httpClient.GetStringAsync(uri);

                try
                {
                    return JsonConvert.DeserializeObject<List<wcl.Class>>(response.Result);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Update player data from warcraftlogs and save their rankings
        /// </summary>
        /// <param name="player"></param>
        public List<Ranking> GetPlayerDataFromWCL(Player player)
        {
            var rankings = new List<Ranking>();

            var spec = db.Specs.ToList().Where(s => s.ID == player.SpecId).FirstOrDefault(); //TODO add to repo

            // get all metrics and rankings for each metric
            var metrics = db.Metrics.Where(m => player.MetricIds.Contains(m.ID)).ToList();

            foreach (var metric in metrics)
            {
                var wclRankings = GetRankingsByName(player.Name, metric.Type);

                foreach (var wclRanking in wclRankings.Where(s => s.Spec == spec.WCLSpecId))
                {
                    if (wclRanking.Total > 0)
                    {
                        var encounter = db.Encounters.ToList().Where(e => e.WCLEncounterId == wclRanking.Encounter).FirstOrDefault(); //TODO add to repo
                        var difficulty = db.Difficulties.ToList().Where(d => d.WCLDifficultyId == wclRanking.Difficulty).FirstOrDefault(); //TODO add to repo

                        if (encounter != null && difficulty != null)
                        {
                            rankings.Add(new Ranking
                            {
                                PlayerId = player.ID,
                                EncounterId = encounter.ID,
                                DifficultyId = difficulty.ID,
                                MetricId = metric.ID,
                                WCLFightId = wclRanking.FightId,
                                WCLReportId = wclRanking.ReportId,
                                PlayerILevel = wclRanking.ItemLevel,
                                Total = wclRanking.Total
                            });
                        }
                    }
                }
            }

            return rankings;
        }
    }
}