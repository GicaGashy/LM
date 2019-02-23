using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LM.Areas.Admin.ViewModels;
using LM.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LM.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        [Area("Admin")]
        public IActionResult Index()
        {
            var vm = new DashboardViewModel();
            vm.Departments = _context.Departments.ToList();
            vm.AppUsers = _userManager.Users.ToList();
            vm.Teams = _context.Teams.ToList();
            return View(vm);
        }
    }
}