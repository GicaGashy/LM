using LM.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Models.LM
{
    public class TopNavbarViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        // You can inject anything you want here
        public TopNavbarViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var appUser = await _userManager.FindByNameAsync(
                HttpContext.User.Identity.Name);

            var vm = new TopNavbarViewModel
            {
                // Just as an example... perhaps you have additional 
                // property like FirstName and LastName in your IdentityUser.
                FirstName = appUser?.FirstName,
                LastName = appUser?.PhoneNumber,
                Email = appUser?.Email,
            };

            return View(vm);
        }
    }
}
