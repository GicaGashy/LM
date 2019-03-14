using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LM.Areas.Generic.ViewModels;
using LM.Data;
using LM.Models.LM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LM.Areas.Generic.Controllers
{
    [Authorize(Roles = "Admin, Generic")]
    [Area("Generic")]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProfileController(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var vm = new ProfileViewModel();
            
            vm.AppUser = _context.AppUsers.Include(t => t.Team).FirstOrDefault(u => u.Id == userId);

            return View(vm);
        }
    }
}