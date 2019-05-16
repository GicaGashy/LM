using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Models.Graphs
{
    public class TestGraph
    {
        public double TotalDays { get; set; }
        public double DaysLeft { get; set; }
        public double DaysPassed { get; set; }
        public string PercentageLeft { get; set; }
        public string PercentagePassed { get; set; }

        public string GetPercentageDaysLeft()
        {
            double value = Math.Round(((DaysLeft / TotalDays) * 100), 1);
            return (value).ToString() + "%"; 
        }

        public string GetPercentageDaysPassed()
        {
            double value = Math.Round((DaysPassed / TotalDays) * 100, 1);
            return (value).ToString() + "%";
        }


    }
}
