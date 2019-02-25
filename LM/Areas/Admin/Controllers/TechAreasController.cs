using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LM.Data;
using LM.Models.LM;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace LM.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class TechAreasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TechAreasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/TechAreas
        public async Task<IActionResult> Index()
        {
            return View(await _context.TechAreas.ToListAsync());
        }

        // GET: Admin/TechAreas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var techArea = await _context.TechAreas
                .FirstOrDefaultAsync(m => m.TechAreaId == id);
            if (techArea == null)
            {
                return NotFound();
            }

            return View(techArea);
        }

        // GET: Admin/TechAreas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/TechAreas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TechAreaId,Name,Description,Image")] TechArea techArea, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {

                if (ImageFile != null)
                {
                    var FileName = Path.GetRandomFileName() + Path.GetExtension(ImageFile.FileName);
                    var FileDirectory = "wwwroot/images/techarea";

                    if (!Directory.Exists(FileDirectory))
                    {
                        Directory.CreateDirectory(FileDirectory);
                    }

                    var FilePath = FileDirectory + "/" + FileName;

                    using (var Stream = new FileStream(FilePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(Stream);
                    }

                    techArea.Image = FileName;
                }

                techArea.Image = "def.jpg";
                _context.Add(techArea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(techArea);
        }

        // GET: Admin/TechAreas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var techArea = await _context.TechAreas.FindAsync(id);
            if (techArea == null)
            {
                return NotFound();
            }
            return View(techArea);
        }

        // POST: Admin/TechAreas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TechAreaId,Name,Description,Image")] TechArea techArea, IFormFile ImageFile)
        {
            if (id != techArea.TechAreaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (ImageFile != null)
                {
                    var FileName = Path.GetRandomFileName() + Path.GetExtension(ImageFile.FileName);
                    var FileDirectory = "wwwroot/images/techarea";

                    if (!Directory.Exists(FileDirectory))
                    {
                        Directory.CreateDirectory(FileDirectory);
                    }

                    var FilePath = FileDirectory + "/" + FileName;

                    using (var Stream = new FileStream(FilePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(Stream);
                    }

                    techArea.Image = FileName;
                }

                try
                {
                    _context.Update(techArea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TechAreaExists(techArea.TechAreaId))
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
            return View(techArea);
        }

        // GET: Admin/TechAreas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var techArea = await _context.TechAreas
                .FirstOrDefaultAsync(m => m.TechAreaId == id);
            if (techArea == null)
            {
                return NotFound();
            }

            return View(techArea);
        }

        // POST: Admin/TechAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var techArea = await _context.TechAreas.FindAsync(id);
            _context.TechAreas.Remove(techArea);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TechAreaExists(int id)
        {
            return _context.TechAreas.Any(e => e.TechAreaId == id);
        }
    }
}
