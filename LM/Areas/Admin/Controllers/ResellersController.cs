using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LM.Data;
using LM.Models.LM;

namespace LM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ResellersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResellersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Resellers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reseller.ToListAsync());
        }

        // GET: Admin/Resellers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reseller = await _context.Reseller
                .FirstOrDefaultAsync(m => m.ResellerId == id);
            if (reseller == null)
            {
                return NotFound();
            }

            return View(reseller);
        }

        // GET: Admin/Resellers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Resellers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResellerId,Name,Country")] Reseller reseller)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reseller);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reseller);
        }

        // GET: Admin/Resellers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reseller = await _context.Reseller.FindAsync(id);
            if (reseller == null)
            {
                return NotFound();
            }
            return View(reseller);
        }

        // POST: Admin/Resellers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResellerId,Name,Country")] Reseller reseller)
        {
            if (id != reseller.ResellerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reseller);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResellerExists(reseller.ResellerId))
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
            return View(reseller);
        }

        // GET: Admin/Resellers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reseller = await _context.Reseller
                .FirstOrDefaultAsync(m => m.ResellerId == id);
            if (reseller == null)
            {
                return NotFound();
            }

            return View(reseller);
        }

        // POST: Admin/Resellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reseller = await _context.Reseller.FindAsync(id);
            _context.Reseller.Remove(reseller);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResellerExists(int id)
        {
            return _context.Reseller.Any(e => e.ResellerId == id);
        }
    }
}
