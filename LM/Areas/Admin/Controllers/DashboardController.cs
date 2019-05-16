using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LM.Areas.Admin.ViewModels;
using LM.Data;
using LM.Models.LM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LM.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        [Area("Admin")]
        public IActionResult Index()
        {
            var vm = new DashboardViewModel();
            vm.Departments = _context.Departments.AsNoTracking().ToList();
            vm.AppUsers = _userManager.Users.AsNoTracking().ToList();
            vm.Teams = _context.Teams.AsNoTracking().ToList();
            vm.Tipis = _context.Tipies.AsNoTracking().ToList();
            vm.TechAreas = _context.TechAreas.AsNoTracking().ToList();
            vm.Resellers = _context.Reseller.AsNoTracking().ToList();
            return View(vm);
        }
    }
}