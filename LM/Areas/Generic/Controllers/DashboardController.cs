using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LM.Areas.Generic.ViewModels;
using LM.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LM.Areas.Generic.Controllers
{
    [Authorize(Roles = "Admin, Generic")]
    [Area("Generic")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context) {
            _context = context;
        }

        public IActionResult Index()
        {
            var vm = new DashboardViewModel();
            vm.TotalSoftwares = _context.Softwares.Count();
            return View(vm);
        }
    }
}