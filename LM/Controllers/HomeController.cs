using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LM.Models;
using LM.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using LM.Models.LM;
using LM.Areas.Generic.ViewModels;
using Microsoft.EntityFrameworkCore;
using LM.ViewModels;

namespace LM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int Entries)
        {
            HomeViewModel vm = new HomeViewModel
            {
                Softwares = await _context.Softwares.Include(u => u.AppUser)
                                              .Include(p => p.Purpose)
                                              .Include(t => t.TechArea)
                                              .Include(ty => ty.Tipi)
                                              .Include(teams => teams.Teams)
                                              .Include(it => it.ItOwner)
                                              .Include(sbot => sbot.SoftwareBusinessOwnerTeams)
                                              .Include(st => st.SoftwareTeams).ToListAsync(),
                Entries = Entries
            };

            foreach (var software in vm.Softwares)
            {
                var softwareBusinessOwnerTeams = await _context.SoftwareBusinessOwnerTeams.Where(s => s.SoftwareId == software.SoftwareId).ToListAsync();
                List<Team> businessOwnerTeams = new List<Team>();
                foreach(var bot in softwareBusinessOwnerTeams)
                {
                    var tempTeam = await _context.Teams.Where(t => t.TeamId == bot.BusinessOwnerTeamId).FirstOrDefaultAsync();
                    businessOwnerTeams.Add(tempTeam);
                }
                software.BusinessOwnerTeams = businessOwnerTeams;
            }
           
            return View(vm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
