using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LM.Data;
using LM.Models.LM;
using Microsoft.AspNetCore.Authorization;

namespace LM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Admin")]
    public class TipisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Tipis
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tipies.ToListAsync());
        }

        // GET: Admin/Tipis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipi = await _context.Tipies
                .FirstOrDefaultAsync(m => m.TipiId == id);
            if (tipi == null)
            {
                return NotFound();
            }

            return View(tipi);
        }

        // GET: Admin/Tipis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Tipis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipiId,Name,Description,Rate")] Tipi tipi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipi);
        }

        // GET: Admin/Tipis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipi = await _context.Tipies.FindAsync(id);
            if (tipi == null)
            {
                return NotFound();
            }
            return View(tipi);
        }

        // POST: Admin/Tipis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipiId,Name,Description,Rate")] Tipi tipi)
        {
            if (id != tipi.TipiId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipiExists(tipi.TipiId))
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
            return View(tipi);
        }

        // GET: Admin/Tipis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipi = await _context.Tipies
                .FirstOrDefaultAsync(m => m.TipiId == id);
            if (tipi == null)
            {
                return NotFound();
            }

            return View(tipi);
        }

        // POST: Admin/Tipis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipi = await _context.Tipies.FindAsync(id);
            _context.Tipies.Remove(tipi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipiExists(int id)
        {
            return _context.Tipies.Any(e => e.TipiId == id);
        }
    }
}
