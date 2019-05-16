using LM.Models.Graphs;
using LM.Models.LM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LM.ViewModels
{
    public class SoftwareDetailsViewModel
    {
        public Software Software{ get; set; }
        public ICollection<Software> SeeAlso { get; set; }
        public string BackgroundColor { get; set; }
        public string FontColor { get; set; }
        public TestGraph Graph { get; set; }

        public string GetLicenseUsagePercentage()
        {
            decimal percentage;
            if (Software.TotalLicenses == 0)
            {
                percentage = 0;
            }
            else
            {
                percentage = ((decimal) Software.LicensesInUse / Software.TotalLicenses).Value * 100;
            }
            return String.Format("{0:0.00}", percentage);
        }
    }

    
}
