using LM.Models.LM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Areas.Admin.ViewModels
{
    public class DashboardViewModel
    {
        public ICollection<Department> Departments { get; set; }
    }
}
