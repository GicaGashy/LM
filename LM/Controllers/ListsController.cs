using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LM.Data;
using LM.Models.Graphs;
using LM.Models.LM;
using LM.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LM.Controllers
{
    [Authorize]
    public class ListsController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListTa()
        {
            var vm = new ListsViewModel
            {
                Softwares = await _context.Softwares.Include(t => t.Tipi).ToListAsync(),
                TechAreas = await _context.TechAreas.AsNoTracking().ToListAsync()
            };
            return View(vm);
        }

        public async Task<IActionResult> ListDe()
        {
            var vm = new ListsViewModel
            {
                Softwares = await _context.Softwares.Include(s => s.AppUser)
                                                    .ThenInclude(u => u.Team)
                                                    .ThenInclude(d => d.Department)
                                                    .Include(t => t.Tipi)
                                                    .Include(it => it.ItOwner)
                                                    .ToListAsync(),
                Departments = await _context.Departments.ToListAsync()
            };
            return View(vm);
        }

        public async Task<IActionResult> ListTe()
        {
            /*
            var Softwares = await _context.Softwares
                .Include(s => s.AppUser)
                .Include(s => s.TechArea)
                .Include(s => s.Tipi)
                .Include(s => s.Teams)
                .Include(s => s.SoftwareTeams).ToListAsync(); */
            var Softwares = await _context.Softwares.Include(s => s.Tipi).ToListAsync();
            var SoftwareTeams = await _context.SoftwareTeams.ToListAsync();
            var Teams = await _context.Teams.ToListAsync();

            

            var vm = new ListsViewModel()
            {
                Softwares = Softwares,
                SoftwareTeams = SoftwareTeams,
                Teams = Teams
            };


            return View(vm);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var Software = await _context.Softwares.Include(t => t.Tipi)
                .Include(u => u.AppUser).ThenInclude(t => t.Team).ThenInclude(d => d.Department)
                .Include(ta => ta.TechArea)
                .Include(t => t.Tipi)
                .Include(it => it.ItOwner)
                .Include(p => p.Purpose)
                .Include(R => R.Reseller)
                .Include(sbot => sbot.SoftwareBusinessOwnerTeams)
                .FirstOrDefaultAsync(s => s.SoftwareId == id);

            var softwareBusinessOwnerTeams = _context.SoftwareBusinessOwnerTeams.Where(sbot => sbot.SoftwareId == id).ToList();
            List<Team> businessOwnerTeams = new List<Team>();
            foreach (var bot in softwareBusinessOwnerTeams)
            {
                var tempTeam = await _context.Teams.Where(t => t.TeamId == bot.BusinessOwnerTeamId).FirstOrDefaultAsync();
                businessOwnerTeams.Add(tempTeam);
            }
            Software.BusinessOwnerTeams = businessOwnerTeams;

            if (Software.Reseller == null)
            {

                Software.Reseller = new Reseller
                {
                    Name = ""
                };
            }

            var vm = new SoftwareDetailsViewModel();

            //Color
            vm.Software = Software;
            if (Software.DaysLeft() < 30) {
                vm.BackgroundColor = "bg-red";
                vm.FontColor = "col-deep-orange";
            } else if (Software.DaysLeft() > 30 && Software.DaysLeft() < 60) {
                vm.BackgroundColor = "bg-orange";
                vm.FontColor = "col-blue-grey";
            }
            else
            {
                vm.BackgroundColor = "bg-teal";
                vm.FontColor = "col-teal";
            }

            List<Software> softwareToBeRemoved = _context.Softwares.Where(s => s.SoftwareId == id).ToList();
            //See also
            var SeeAlsoSoftwares = await _context.Softwares
                .Include(t => t.Tipi)
                .Where(s => s.TipiId == Software.TipiId).Except(softwareToBeRemoved).Take(5).ToListAsync();
            
            vm.SeeAlso = SeeAlsoSoftwares;

            var Graph = new TestGraph();
            Graph.DaysLeft = Software.DaysLeft();
            Graph.DaysPassed = Software.DaysPassed();
            Graph.TotalDays = Software.TotalDays();
            //Percentage stuff
            vm.Graph = Graph;
            
            return View(vm);
        }

        private string GetColor(double days)
        {
            if (days < 30)
            {
                return "bg-red";
            }
            else if (days > 30 && days < 60)
            {
                return  "bg-orange";
            }
            else
            {
                return "bg-teal";
            }
        }
    }
}