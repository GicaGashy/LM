using LM.Models.LM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Areas.Generic.ViewModels
{
    public class DashboardViewModel
    {
        public ICollection<Software> Softwares{ get; set; }
        public int TotalSoftwares { get; set; }
    }
}
