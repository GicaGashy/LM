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
                    return  RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email allready exists");
                    ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Name", user.TeamId);
                    return View();
                }
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Name", user.TeamId);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Name");
            var vm = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TeamId = user.TeamId ?? default(int),
                UserName = user.UserName
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel appuser)
        {
            if (ModelState.IsValid)
            {
                var OriginalUser = await _userManager.FindByIdAsync(appuser.Id);

                if (OriginalUser == null)
                {
                    return NotFound();
                }

                if (OriginalUser.Email != appuser.Email)
                {
                    if (_userManager.FindByEmailAsync(appuser.Email).Result == null)
                    {
                        OriginalUser.Email = appuser.Email;
                        await _userManager.UpdateAsync(OriginalUser);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Email allready exists");
                        return View();
                    }
                }


                if (OriginalUser.FirstName != appuser.FirstName)
                {
                        OriginalUser.FirstName = appuser.FirstName;
                    await _userManager.UpdateAsync(OriginalUser);
                }

                if (OriginalUser.LastName != appuser.LastName)
                {
                    OriginalUser.LastName = appuser.LastName;
                    await _userManager.UpdateAsync(OriginalUser);
                }

                if (OriginalUser.UserName != appuser.UserName)
                {

                    OriginalUser.UserName = appuser.UserName;
                    await _userManager.UpdateAsync(OriginalUser);
                }

                if (OriginalUser.TeamId != appuser.TeamId)
                {

                    OriginalUser.TeamId = appuser.TeamId;
                    await _userManager.UpdateAsync(OriginalUser);
                }

                if (!string.IsNullOrEmpty(appuser.Password)) //Check to see if the password is being updated
                {
                    //Yes update it
                    string code = await _userManager.GeneratePasswordResetTokenAsync(OriginalUser);
                    var result = await _userManager.ResetPasswordAsync(OriginalUser, code, appuser.Password);

                    if (!result.Succeeded)
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, item.Description);
                        }
                        return View();
                    }

                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "TeamId", "Name", appuser.TeamId);
            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }


    }
}