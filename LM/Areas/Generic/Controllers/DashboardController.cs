using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LM.Areas.Generic.ViewModels;
using LM.Data;
using LM.Models.LM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LM.Areas.Generic.Controllers
{
    [Authorize(Roles = "Admin, Generic")]
    [Area("Generic")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<AppUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;

            var vm = new DashboardViewModel();
            vm.TotalSoftwares = _context.Softwares.Where(s => s.AppUserId == currentUser.Id).Count();
            return View(vm);
        }
    }
}