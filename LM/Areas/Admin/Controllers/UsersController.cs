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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LM.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UsersController(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            UsersViewModel vm = new UsersViewModel();
            vm.AppUsers = _context.AppUsers.Include(u => u.Team).ToList();
            return View(vm);
        }

        public IActionResult Create()
        {
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel user)
        {

            if (ModelState.IsValid && _userManager.FindByEmailAsync(user.Email).Result == null)
            {
                var NewUser = new AppUser()
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    TeamId = user.TeamId
                };

                IdentityResult result = _userManager.CreateAsync(NewUser, user.Password).Result;

                if(result.Succeeded)
                {
                    _userManager.AddToRoleAsync(NewUser, "Generic").Wait();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Name", user.TeamId);
                    return View();
                }
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Name", user.TeamId);
            return View();
        }



    }
}