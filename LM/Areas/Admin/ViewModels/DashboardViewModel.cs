using LM.Models.LM;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Areas.Admin.ViewModels
{
    public class DashboardViewModel
    {
        public ICollection<Department> Departments { get; set; }
        public ICollection<AppUser> AppUsers { get; set; }
        public ICollection<Team> Teams { get; set; }
        public ICollection<Tipi> Tipis { get; set; }
        public ICollection<TechArea> TechAreas { get; set; }
    }
}
