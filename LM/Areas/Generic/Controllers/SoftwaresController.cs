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
            List<Software> softwares = new List<Software>();

            if (User.IsInRole("Admin"))
            {
                softwares = await _context.Softwares
                                        .Include(s => s.AppUser)
                                        .Include(s => s.TechArea)
                                        .Include(s => s.Tipi)
                                        .AsNoTracking().ToListAsync(); 
            }
            else
            {
                softwares = await _context.Softwares
                                        .Include(s => s.AppUser)
                                        .Include(s => s.TechArea)
                                        .Include(s => s.Tipi)
                                        .Where(s => s.AppUser == currentUser).AsNoTracking().ToListAsync();
            }
            return View(softwares);
        }

        // GET: Generic/Softwares/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            DetailsVM vm = new DetailsVM();

            var software = await _context.Softwares
                .Include(s => s.AppUser)
                .Include(s => s.TechArea)
                .Include(s => s.Tipi)
                .Include(s => s.Teams)
                .Include(s => s.ItOwner)
                .Include(s => s.SoftwareTeams)
                .Include(s => s.SoftwareBusinessOwnerTeams)
                .Include(s => s.Purpose)
                .FirstOrDefaultAsync(m => m.SoftwareId == id);

            var softwareTeams = _context.SoftwareTeams.Where(st => st.SoftwareId == id).ToList();
            List<Team> teams = new List<Team>();
            foreach (var st in softwareTeams)
            {
                var tempTeam = await _context.Teams.Where(t => t.TeamId == st.TeamId).FirstOrDefaultAsync();
                teams.Add(tempTeam);
            }
            //var teams = _context.Teams.Where(t => t.SoftwareTeams.FirstOrDefault().SoftwareId == softwareTeams.FirstOrDefault().SoftwareId).ToList();
            software.Teams = teams;


            var softwareBusinessOwnerTeams = _context.SoftwareBusinessOwnerTeams.Where(sbot => sbot.SoftwareId == id).ToList();
            List<Team> businessOwnerTeams = new List<Team>();
            foreach(var bot in softwareBusinessOwnerTeams)
            {
                var tempTeam = await _context.Teams.Where(t => t.TeamId == bot.BusinessOwnerTeamId).FirstOrDefaultAsync();
                businessOwnerTeams.Add(tempTeam);
            }
            software.BusinessOwnerTeams = businessOwnerTeams;

            if (software == null)
            {
                return NotFound();
            }


            vm.Software = software;
            return View(vm);
        }

        // GET: Generic/Softwares/Create
        public IActionResult Create()
        {
            var selectListTeams = _context.Teams;
            var selectListItOnly = _context.Teams.Where(t => t.Department.Name == "Infrastructure");
            var selectListBusinessOnly = _context.Teams.Where(t => t.Department.Name != "Infrastructure");
            var vm = new CreateVM();

            //Teams selectlists
            ViewData["TeamId"]= new SelectList(selectListTeams, "TeamId", "Name");
            ViewData["ItOwnerId"] = new SelectList(selectListTeams, "TeamId", "Name");
            ViewData["BusinessOwnerId"] = new SelectList(selectListTeams, "TeamId", "Name");
            ViewData["ResellerId"] = new SelectList(_context.Reseller, "ResellerId", "Name");
            ViewData["BusinessOwnerTeamId"] = new MultiSelectList(_context.Teams, "TeamId", "Name");
            ViewData["PurposeId"] = new SelectList(_context.Purposes, "PurposeId", "Name");
            ViewData["TechAreaId"] = new SelectList(_context.TechAreas, "TechAreaId", "Name");
            ViewData["TipiId"] = new SelectList(_context.Tipies, "TipiId", "Name");
            var software = new Software();
            var teams = _context.Teams.ToArray();
            software.Teams = teams;
            vm.Software = software;
            return View(vm);
        }

        // POST: Generic/Softwares/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SoftwareId,Name,Version,LicenseStart,LicenseEnd,UseCases,Description,TechAreaId,TipiId,Vendor,ItOwnerId,BusinessOwnerId,PurposeId,IsUsed,IsInternetFacing,IsMobile,TotalLicenses,LicensesInUse,ItOwnerName,ResellerId,Confidentiality,Integrity,Availability,Traceability")] Software software,
            [Bind("LicenseInfo")] bool LicenseInfo,
            [Bind("TeamId")] int[] teams,
            [Bind("BusinessOwnerTeamId")] int[] businessTeams,
            string ResellerBox
            )
        {
            var currentUser =_userManager.GetUserAsync(HttpContext.User).Result;
            var vm = new CreateVM();
            software.Standardized = DateTime.Now;
            software.AppUser = currentUser;
            software.ItOwner = await _context.Teams.Where(t => t.TeamId == software.ItOwnerId).FirstOrDefaultAsync();
            var selectListTeams = _context.Teams.Include(it => it.ItSoftwares);
            var selectListItOnly = _context.Teams.Where(t => t.Department.Name == "Infrastructure");
            var selectListBusinessOnly = _context.Teams.Where(t => t.Department.Name != "Infrastructure");



            if (LicenseInfo == false || software.LicenseStart == null || software.LicenseEnd == null)
            {
                software.LicenseStart = DateTime.Now.AddYears(-1);
                software.LicenseEnd = DateTime.Now.AddYears(1);
            }



            if ((software.LicenseStart > software.LicenseEnd) || (software.TotalLicenses < software.LicensesInUse))
            {
                ModelState.AddModelError(string.Empty, "End License Date cannot be smaller than Start License Date!" );
                ModelState.AddModelError(string.Empty, "Or Number of Licenses in Use cannot be bigger than Total Licenses!");

                software.TeamIds = new List<int>();
                software.BusinessOwnerTeamIds = new List<int>();
                foreach (var id in teams)
                {
                    software.TeamIds.Add(id);
                }

                foreach (var id in businessTeams)
                {
                    software.BusinessOwnerTeamIds.Add(id);
                }


                ViewData["TeamId"] = new MultiSelectList(selectListTeams, "TeamId", "Name", software.TeamIds);
                ViewData["BusinessOwnerTeamId"] = new MultiSelectList(_context.Teams, "TeamId", "Name", software.BusinessOwnerTeamIds);
                ViewData["TechAreaId"] = new SelectList(_context.TechAreas, "TechAreaId", "Name", software.TechAreaId);
                ViewData["ItOwnerId"] = new SelectList(selectListTeams, "TeamId", "Name", software.ItOwner.TeamId);
                ViewData["TipiId"] = new SelectList(_context.Tipies, "TipiId", "Name", software.TipiId);
                ViewData["PurposeId"] = new SelectList(_context.Purposes, "PurposeId", "Name", software.PurposeId);
                ViewData["ResellerId"] = new SelectList(_context.Reseller, "ResellerId", "Name", software.ResellerId);
                vm.Software = software;
                return View(vm);
            }

            if (ModelState.IsValid)
            {
                if (software.ResellerId == null && ResellerBox != null)
                {
                    var tempReseller = new Reseller
                    {
                        Name = ResellerBox
                    };
                    _context.Reseller.Add(tempReseller);
                    await _context.SaveChangesAsync();
                    software.Reseller = await _context.Reseller.LastAsync();
                }

                _context.Add(software);
                await _context.SaveChangesAsync();

                SoftwareTeam st = new SoftwareTeam();
                SoftwareBusinessOwnerTeam sbot = new SoftwareBusinessOwnerTeam();
                var currentSoftwareId = _context.Softwares.Last().SoftwareId;

                /*
                if (teams.Length == 0)
                {
                    //st.TeamId = currentUser.TeamId ?? 0; --> this gets the team of the current logged in user
                    st.TeamId = _context.Teams.Where(t => t.Name == "Unknown").FirstOrDefault().TeamId; //gets the 'Uknwon Team'
                    st.SoftwareId = currentSoftwareId;
                    _context.SoftwareTeams.Add(st);
                    await _context.SaveChangesAsync();
                }*/
                if (teams.FirstOrDefault() == -1)
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

                if (businessTeams.FirstOrDefault() == -1)
                {
                    var tempTeams = _context.Teams.ToArray();
                    foreach (var t in tempTeams)
                    {
                        sbot.BusinessOwnerTeamId= t.TeamId;
                        sbot.SoftwareId = currentSoftwareId;
                        _context.SoftwareBusinessOwnerTeams.Add(sbot);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    foreach (var t in businessTeams)
                    {
                        sbot.BusinessOwnerTeamId = t;
                        sbot.SoftwareId = currentSoftwareId;
                        _context.SoftwareBusinessOwnerTeams.Add(sbot);
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            software.TeamIds = new List<int>();
            software.BusinessOwnerTeamIds = new List<int>();
            foreach (var id in teams)
            {
                software.TeamIds.Add(id);
            }

            foreach (var id in businessTeams)
            {
                software.BusinessOwnerTeamIds.Add(id);
            }

            vm.Software = software;

            ViewData["TeamId"] = new MultiSelectList(selectListTeams, "TeamId", "Name", software.TeamIds);
            ViewData["BusinessOwnerTeamId"] = new MultiSelectList(_context.Teams, "TeamId", "Name", software.BusinessOwnerTeamIds);
            ViewData["ItOwnerId"] = new SelectList(selectListTeams, "TeamId", "Name", software.ItOwnerId);
            ViewData["TechAreaId"] = new SelectList(_context.TechAreas, "TechAreaId", "Name", software.TechAreaId);
            ViewData["TipiId"] = new SelectList(_context.Tipies, "TipiId", "Name", software.TipiId);
            ViewData["PurposeId"] = new SelectList(_context.Purposes, "PurposeId", "Name", software.PurposeId);
            ViewData["ResellerId"] = new SelectList(_context.Reseller, "ResellerId", "Name", software.ResellerId);
            return View(vm);
        }


        // GET: Generic/Softwares/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            var selectListTeams = _context.Teams;
            var vm = new EditVM();

            if (id == null)
            {
                return NotFound();
            }

            Software software = new Software();

            if (User.IsInRole("Admin"))
            {
                software = await _context.Softwares.Include(s => s.AppUser)
                                    .Include(t => t.Teams)
                                    .Include(it => it.ItOwner)
                                    .Include(r => r.Reseller)
                                    .Where(s => s.SoftwareId == id)
                                    .FirstOrDefaultAsync();
            }
            else
            {
                software = await _context.Softwares.Include(s => s.AppUser)
                                .Include(t => t.Teams)
                                .Include(it => it.ItOwner)
                                .Include(r => r.Reseller)
                                .Where(s => s.SoftwareId == id && s.AppUser == currentUser)
                                .FirstOrDefaultAsync();
            }

            
            if (software == null)
            {
                return NotFound();
            }

            List<SoftwareTeam> softwareTeams = _context.SoftwareTeams.Where(st => st.SoftwareId == id).ToList();
            List<SoftwareBusinessOwnerTeam> softwareBusinessOwnerTeams = _context.SoftwareBusinessOwnerTeams.Where(st => st.SoftwareId == id).ToList();

            List<int> teamIds = new List<int>();
            List<int> businessOwnerTeamIds = new List<int>();
            
            foreach (var item in softwareTeams)
            {
                teamIds.Add(item.TeamId);
            }

            foreach (var item in softwareBusinessOwnerTeams)
            {
                businessOwnerTeamIds.Add(item.BusinessOwnerTeamId);
            }

            software.ItOwnerId = software.ItOwner.TeamId;

            ViewData["TeamId"] = new MultiSelectList(selectListTeams, "TeamId", "Name", teamIds);
            ViewData["BusinessOwnerTeamId"] = new MultiSelectList(_context.Teams, "TeamId", "Name", businessOwnerTeamIds);
            ViewData["ItOwnerId"] = new SelectList(selectListTeams, "TeamId", "Name", software.ItOwnerId);
            ViewData["TechAreaId"] = new SelectList(_context.TechAreas, "TechAreaId", "Name", software.TechAreaId);
            ViewData["TipiId"] = new SelectList(_context.Tipies, "TipiId", "Name", software.TipiId);
            ViewData["PurposeId"] = new SelectList(_context.Purposes, "PurposeId", "Name", software.PurposeId);
            ViewData["ResellerId"] = new SelectList(_context.Reseller, "ResellerId", "Name", software.ResellerId);
            vm.Software = software;

            return View(vm);
        }

        // POST: Generic/Softwares/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SoftwareId,Name,Version,LicenseStart,LicenseEnd,UseCases,Description,TechAreaId,TipiId,AppUserId,Vendor,ItOwnerId,BusinessOwnerId,PurposeId,IsUsed,IsInternetFacing,IsMobile,TotalLicenses,LicensesInUse,ItOwnerName,ResellerId,Standardized,Confidentiality,Integrity,Availability,Traceability")] Software software,
            [Bind("LicenseInfo")] bool LicenseInfo,
            [Bind("TeamId")] int[] teams,
            [Bind("BusinessOwnerTeamId")] int[] businessTeams,
            string ResellerBox)
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
            var selectListTeams = _context.Teams;
            var vm = new EditVM();
            software.Review = DateTime.Now;
            software.ItOwner = await _context.Teams.Where(t => t.TeamId == software.ItOwnerId).FirstOrDefaultAsync();

            
            //Checks the dates, returns an error if dates are not good, fills the selected lists
            if (LicenseInfo == false || software.LicenseStart == null || software.LicenseEnd == null) {
                //Software thisSoftware = await _context.Softwares.FirstOrDefaultAsync(s => s == software);
                software.LicenseStart = DateTime.Now.AddYears(-1);
                software.LicenseEnd = DateTime.Now.AddYears(1);
            }
            

            if ( (software.LicenseStart > software.LicenseEnd) || (software.TotalLicenses < software.LicensesInUse))
            {
                ModelState.AddModelError(string.Empty, "End License Date cannot be smaller than Start License Date");
                ModelState.AddModelError(string.Empty, "Or Number of Licenses in Use cannot be bigger than Total Licenses!");

                software.TeamIds = new List<int>();
                software.BusinessOwnerTeamIds = new List<int>();
                foreach (var Id in teams)
                {
                    software.TeamIds.Add(Id);
                }

                foreach (var Id in businessTeams)
                {
                    software.BusinessOwnerTeamIds.Add(Id);
                }

                ViewData["TeamId"] = new MultiSelectList(selectListTeams, "TeamId", "Name", software.TeamIds);
                ViewData["BusinessOwnerTeamId"] = new MultiSelectList(selectListTeams, "TeamId", "Name", software.BusinessOwnerTeamIds);
                ViewData["ItOwnerId"] = new SelectList(selectListTeams, "TeamId", "Name", software.ItOwnerId);
                ViewData["TechAreaId"] = new SelectList(_context.TechAreas, "TechAreaId", "Name", software.TechAreaId);
                ViewData["TipiId"] = new SelectList(_context.Tipies, "TipiId", "Name", software.TipiId);
                ViewData["PurposeId"] = new SelectList(_context.Purposes, "PurposeId", "Name", software.PurposeId);
                ViewData["ResellerId"] = new SelectList(_context.Reseller, "ResellerId", "Name", software.ResellerId);
                vm.Software = software;
                return View(vm);
            }



            if (ModelState.IsValid)
            {
                try
                {
                    if (software.ResellerId == null && ResellerBox != null)
                    {
                        var tempReseller = new Reseller
                        {
                            Name = ResellerBox
                        };
                        _context.Reseller.Add(tempReseller);
                        await _context.SaveChangesAsync();
                        software.Reseller = await _context.Reseller.LastAsync();
                    }

                    
                    _context.Update(software);
                    await _context.SaveChangesAsync();

                    SoftwareTeam st = new SoftwareTeam();
                    SoftwareBusinessOwnerTeam sbot = new SoftwareBusinessOwnerTeam();
                    var currentSoftwareId = _context.Softwares.FirstOrDefault(s => s.SoftwareId == software.SoftwareId).SoftwareId;

                    //Delte old records
                    var oldSTs = _context.SoftwareTeams.Where(oldsts => oldsts.SoftwareId == id).ToList();

                    _context.SoftwareTeams.RemoveRange(oldSTs);
                    await _context.SaveChangesAsync();

                    var oldsbot = _context.SoftwareBusinessOwnerTeams.Where(oldsbots => oldsbots.SoftwareId == id).ToList();
                    _context.SoftwareBusinessOwnerTeams.RemoveRange(oldsbot);
                    /*
                    if (teams.Length == 0)
                    {
                        //st.TeamId = currentUser.TeamId ?? 0; --> this gets the team of the current logged in user
                        st.TeamId =  _context.Teams.Where(t => t.Name == "Unknown").FirstOrDefault().TeamId; //gets the 'Uknwon Team'
                        st.SoftwareId = currentSoftwareId;
                        _context.SoftwareTeams.Add(st);
                        await _context.SaveChangesAsync();
                    }*/

                    //SoftwareTeams
                    if (teams.FirstOrDefault() == -1)
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

                    //SoftwareBusinessOwnerTeams
                    if (businessTeams.FirstOrDefault() == -1)
                    {
                        var tempTeams = _context.Teams.ToArray();
                        foreach (var t in tempTeams)
                        {
                            sbot.BusinessOwnerTeamId = t.TeamId;
                            sbot.SoftwareId = currentSoftwareId;
                            _context.SoftwareBusinessOwnerTeams.Add(sbot);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        foreach (var t in businessTeams)
                        {
                            sbot.BusinessOwnerTeamId = t;
                            sbot.SoftwareId = currentSoftwareId;
                            _context.SoftwareBusinessOwnerTeams.Add(sbot);
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
            software.BusinessOwnerTeamIds = new List<int>();
            foreach (var Id in teams)
            {
                software.TeamIds.Add(Id);
            }
            foreach (var Id in businessTeams)
            {
                software.BusinessOwnerTeamIds.Add(Id);
            }

            vm.Software = software;

            ViewData["TeamId"] = new MultiSelectList(selectListTeams, "TeamId", "Name", software.TeamIds);
            ViewData["BusinessOwnerTeamId"] = new MultiSelectList(selectListTeams, "TeamId", "Name", software.BusinessOwnerTeamIds);
            ViewData["ItOwnerId"] = new SelectList(selectListTeams, "TeamId", "Name", software.ItOwnerId);
            ViewData["TechAreaId"] = new SelectList(_context.TechAreas, "TechAreaId", "Name", software.TechAreaId);
            ViewData["TipiId"] = new SelectList(_context.Tipies, "TipiId", "Name", software.TipiId);
            ViewData["PurposeId"] = new SelectList(_context.Purposes, "PurposeId", "Name", software.PurposeId);
            ViewData["ResellerId"] = new SelectList(_context.Reseller, "ResellerId", "Name", software.ResellerId);
            return View(vm);
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
