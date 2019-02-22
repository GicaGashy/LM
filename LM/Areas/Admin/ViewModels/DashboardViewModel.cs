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
        public ICollection<IdentityUser> AppUsers { get; set; }
    }
}
