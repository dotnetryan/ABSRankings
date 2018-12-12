using ABSRankings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABSRankings
{
    public static class Utilities
    {
        public static string CssClassForPercent(decimal percent)
        {
            string cssClass = "overall-percent-normal";

            if (percent > 0.15m)
            {
                cssClass = "overall-percent-bright-green";
            }
            else if (percent > 0 && percent <= 0.15m)
            {
                cssClass = "overall-percent-green";
            }
            else if (percent < 0 && percent >= -0.05m)
            {
                cssClass = "overall-percent-yellow";
            }
            else if (percent < -0.05m && percent >= -0.1m)
            {
                cssClass = "overall-percent-orange";
            }
            else if (percent < -0.1m && percent >= -0.15m)
            {
                cssClass = "overall-percent-red";
            }
            else if (percent < -0.15m)
            {
                cssClass = "overall-percent-deep-red";
            }

            return cssClass;
        }

        public static SelectList PlayerList(List<Player> players)
        {
            return new SelectList((from p in players
                                   select new {
                                       Value = p.ID,
                                       Text = string.Format("{0} - {1}", p.Name, p.Spec.Name)
                                   }), "Value", "Text"); //TODO add to repo
        }

        public static SelectList PlayerList(List<Player> players, int selectedId)
        {
            return new SelectList((from p in players
                                   select new
                                   {
                                       Value = p.ID,
                                       Text = string.Format("{0} - {1}", p.Name, p.Spec.Name)
                                   }), "Value", "Text", selectedId); //TODO add to repo 
        }

        public static SelectList SpecList(List<Spec> specs)
        {
            return new SelectList((from s in specs
                                           select new {
                                               Value = s.ID,
                                               Text = string.Format("{0} - {1}", s.ClassName, s.Name)
                                           }), "Value", "Text"); //TODO add to repo
        }

        public static SelectList SpecList(List<Spec> specs, int selectedId)
        {
            return new SelectList((from s in specs.ToList()
                        select new {
                            Value = s.ID,
                            Text = string.Format("{0} - {1}", s.ClassName, s.Name)
                        }), "Value", "Text", selectedId); //TODO add to repo;
        }

        public static MultiSelectList MetricList(List<Metric> metrics)
        {
            return new MultiSelectList((from m in metrics.ToList()
                select new {
                    Value = m.ID,
                    Text = m.Description
                }), "Value", "Text"); //TODO add to repo
        }

        public static MultiSelectList MetricList(List<Metric> metrics, ICollection<int> selectedIds)
        {
            return new MultiSelectList((from m in metrics.ToList()
                                        select new {
                                            Value = m.ID,
                                            Text = m.Description
                                        }), "Value", "Text", selectedIds); //TODO add to repo
        }
    }
}