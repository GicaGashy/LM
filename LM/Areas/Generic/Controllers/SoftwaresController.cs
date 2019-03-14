using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LM.Data;
using LM.Models.LM;
using LM.Areas.Generic.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace LM.Areas.Generic.Controllers
{
    [Area("Generic")]
    [Authorize(Roles = "Generic, Admin")]
    public class SoftwaresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public SoftwaresController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Generic/Softwares
        public async Task<IActionResult> Index()
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var applicationDbContext = _context.Softwares
                                        .Include(s => s.AppUser)
                                        .Include(s => s.TechArea)
                                        .Include(s => s.Tipi)
                                        .Where(s => s.AppUser == currentUser).AsNoTracking();
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Generic/Softwares/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var software = await _context.Softwares
                .Include(s => s.AppUser)
                .Include(s => s.TechArea)
                .Include(s => s.Tipi)
                .Include(s => s.Teams)
                .Include(s => s.SoftwareTeams)
                .FirstOrDefaultAsync(m => m.SoftwareId == id);

            var softwareTeams = _context.SoftwareTeams.Where(st => st.SoftwareId == id).ToList();
            var teams = _context.Teams.Where(t => t.SoftwareTeams.FirstOrDefault().SoftwareId == softwareTeams.FirstOrDefault().SoftwareId).ToList();

            software.Teams = teams;
            if (software == null)
            {
                return NotFound();
            }

            return View(software);
        }

        // GET: Generic/Softwares/Create
        public IActionResult Create()
        {
            ViewData["TeamId"]= new SelectList(_context.Teams, "TeamId", "Name");
            ViewData["TechAreaId"] = new SelectList(_context.TechAreas, "TechAreaId", "Name");
            ViewData["TipiId"] = new SelectList(_context.Tipies, "TipiId", "Name");
            var vm = new Software();
            var teams = _context.Teams.ToArray();
            vm.Teams = teams;
            
            return View(vm);
        }

        // POST: Generic/Softwares/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SoftwareId,Name,Version,LicenseStart,LicenseEnd,UseCases,Description,TechAreaId,TipiId,Producer")] Software software,
            [Bind("TeamId")] int[] teams)
        {
            var currentUser =_userManager.GetUserAsync(HttpContext.User).Result;
            software.AppUser = currentUser;

            if (software.LicenseStart > software.LicenseEnd)
            {
                ModelState.AddModelError(string.Empty, "End License Date cannot be smaller than Start License Date" );

                software.TeamIds = new List<int>();
                foreach (var id in teams)
                {
                    software.TeamIds.Add(id);
                }
                

                ViewData["TeamId"] = new MultiSelectList(_context.Teams, "TeamId", "Name", software.TeamIds);
                ViewData["TechAreaId"] = new SelectList(_context.TechAreas, "TechAreaId", "Name", software.TechAreaId);
                ViewData["TipiId"] = new SelectList(_context.Tipies, "TipiId", "Name", software.TipiId);
                return View(software);
            }

            if (ModelState.IsValid)
            {

                _context.Add(software);
                await _context.SaveChangesAsync();

                SoftwareTeam st = new SoftwareTeam();
                var currentSoftwareId = _context.Softwares.Last().SoftwareId;

                if (teams.Length == 0)
                {
                    st.TeamId = currentUser.TeamId ?? 0;
                    st.SoftwareId = currentSoftwareId;
                    _context.SoftwareTeams.Add(st);
                    await _context.SaveChangesAsync();
                }
                else if (teams.FirstOrDefault() == -1)
                {
                    var tempTeams = _context.Teams.ToArray();
                    foreach (var t in tempTeams)
                    {
                        st.TeamId = t.TeamId;
                        st.SoftwareId = currentSoftwareId;
                        _context.SoftwareTeams.Add(st);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    foreach (var t in teams)
                    {
                        st.TeamId = t;
                        st.SoftwareId = currentSoftwareId;
                        _context.SoftwareTeams.Add(st);
                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            software.TeamIds = new List<int>();
            foreach (var id in teams)
            {
                software.TeamIds.Add(id);
            }
            ViewData["TeamId"] = new MultiSelectList(_context.Teams, "TeamId", "Name", software.TeamIds);
            ViewData["TechAreaId"] = new SelectList(_context.TechAreas, "TechAreaId", "Name", software.TechAreaId);
            ViewData["TipiId"] = new SelectList(_context.Tipies, "TipiId", "Name", software.TipiId);
            return View(software);
        }

        // GET: Generic/Softwares/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;

            if (id == null)
            {
                return NotFound();
            }

            var software = await _context.Softwares.Include(s => s.AppUser)
                                .Where(s => s.SoftwareId == id && s.AppUser == currentUser)
                                .FirstOrDefaultAsync();
            if (software == null)
            {
                return NotFound();
            }

            List<SoftwareTeam> softwareTeams = _context.SoftwareTeams.Where(st => st.SoftwareId == id).ToList();
            List<int> teamIds = new List<int>();

            foreach (var item in softwareTeams)
            {
                teamIds.Add(item.TeamId);
            }
            
            ViewData["TeamId"] = new MultiSelectList(_context.Teams, "TeamId", "Name", teamIds);
            ViewData["TechAreaId"] = new SelectList(_context.TechAreas, "TechAreaId", "Name", software.TechAreaId);
            ViewData["TipiId"] = new SelectList(_context.Tipies, "TipiId", "Name", software.TipiId);
            return View(software);
        }

        // POST: Generic/Softwares/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SoftwareId,Name,Version,LicenseStart,LicenseEnd,UseCases,Description,TechAreaId,TipiId,AppUserId,Producer")] Software software,
            [Bind("TeamId")] int[] teams)
        {
            if (id != software.SoftwareId)
            {
                return NotFound();
            }

            //gets current user's id
            /*
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            software.AppUser = currentUser;
            */
            var currentUser = _userManager.FindByIdAsync(software.AppUserId).Result;
            //Checks the dates, returns an error if dates are not good, fills the selected lists
            if (software.LicenseStart > software.LicenseEnd)
            {
                ModelState.AddModelError(string.Empty, "End License Date cannot be smaller than Start License Date");

                software.TeamIds = new List<int>();
                foreach (var Id in teams)
                {
                    software.TeamIds.Add(Id);
                }
                ViewData["TeamId"] = new MultiSelectList(_context.Teams, "TeamId", "Name", software.TeamIds);
                ViewData["TechAreaId"] = new SelectList(_context.TechAreas, "TechAreaId", "Name", software.TechAreaId);
                ViewData["TipiId"] = new SelectList(_context.Tipies, "TipiId", "Name", software.TipiId);
                return View(software);
            }



            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(software);
                    await _context.SaveChangesAsync();

                    SoftwareTeam st = new SoftwareTeam();
                    var currentSoftwareId = _context.Softwares.FirstOrDefault(s => s.SoftwareId == software.SoftwareId).SoftwareId;

                    //Delte old records
                    var oldSTs = _context.SoftwareTeams.Where(oldsts => oldsts.SoftwareId == id).ToList();

                    _context.SoftwareTeams.RemoveRange(oldSTs);
                    await _context.SaveChangesAsync();

                    if (teams.Length == 0)
                    {
                        st.TeamId = currentUser.TeamId ?? 0;
                        st.SoftwareId = currentSoftwareId;
                        _context.SoftwareTeams.Add(st);
                        await _context.SaveChangesAsync();
                    }
                    else if (teams.FirstOrDefault() == -1)
                    {
                        var tempTeams = _context.Teams.ToArray();
                        foreach (var t in tempTeams)
                        {
                            st.TeamId = t.TeamId;
                            st.SoftwareId = currentSoftwareId;
                            _context.SoftwareTeams.Add(st);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        foreach (var t in teams)
                        {
                            st.TeamId = t;
                            st.SoftwareId = currentSoftwareId;
                            _context.SoftwareTeams.Add(st);
                            await _context.SaveChangesAsync();
                        }
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoftwareExists(software.SoftwareId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            software.TeamIds = new List<int>();
            foreach (var Id in teams)
            {
                software.TeamIds.Add(Id);
            }
            ViewData["TeamId"] = new MultiSelectList(_context.Teams, "TeamId", "Name", software.TeamIds);
            ViewData["TechAreaId"] = new SelectList(_context.TechAreas, "TechAreaId", "Name", software.TechAreaId);
            ViewData["TipiId"] = new SelectList(_context.Tipies, "TipiId", "Name", software.TipiId);
            return View(software);
        }

        // GET: Generic/Softwares/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var software = await _context.Softwares
                .Include(s => s.AppUser)
                .Include(s => s.TechArea)
                .Include(s => s.Tipi)
                .FirstOrDefaultAsync(m => m.SoftwareId == id);
            if (software == null)
            {
                return NotFound();
            }

            return View(software);
        }

        // POST: Generic/Softwares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var software = await _context.Softwares.FindAsync(id);
            _context.Softwares.Remove(software);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoftwareExists(int id)
        {
            return _context.Softwares.Any(e => e.SoftwareId == id);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext, DateTime start, DateTime end)
        {
            if (start > end)
            {
                yield return new ValidationResult("Rent date must be prior to return date", new[] { "RentDate" });
            }
        }
    }
}
