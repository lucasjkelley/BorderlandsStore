using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BorderlandsStore.DATA.EF.Models;

namespace BorderlandsStore.UI.MVC.Controllers
{
    public class WeaponsController : Controller
    {
        private readonly BorderlandsStoreContext _context;

        public WeaponsController(BorderlandsStoreContext context)
        {
            _context = context;
        }

        // GET: Weapons
        public async Task<IActionResult> Index()
        {
            var borderlandsStoreContext = _context.Weapons.Include(w => w.Category).Include(w => w.Element).Include(w => w.Manufacturer);
            return View(await borderlandsStoreContext.ToListAsync());
        }

        // GET: Weapons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Weapons == null)
            {
                return NotFound();
            }

            var weapon = await _context.Weapons
                .Include(w => w.Category)
                .Include(w => w.Element)
                .Include(w => w.Manufacturer)
                .FirstOrDefaultAsync(m => m.WeaponId == id);
            if (weapon == null)
            {
                return NotFound();
            }

            return View(weapon);
        }

        // GET: Weapons/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Category1");
            ViewData["ElementId"] = new SelectList(_context.Elements, "ElementId", "ElementType");
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "Location");
            return View();
        }

        // POST: Weapons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WeaponId,Name,Description,CategoryId,ElementId,Price,ManufacturerId,WeaponImage")] Weapon weapon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(weapon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Category1", weapon.CategoryId);
            ViewData["ElementId"] = new SelectList(_context.Elements, "ElementId", "ElementType", weapon.ElementId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "Location", weapon.ManufacturerId);
            return View(weapon);
        }

        // GET: Weapons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Weapons == null)
            {
                return NotFound();
            }

            var weapon = await _context.Weapons.FindAsync(id);
            if (weapon == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Category1", weapon.CategoryId);
            ViewData["ElementId"] = new SelectList(_context.Elements, "ElementId", "ElementType", weapon.ElementId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "Location", weapon.ManufacturerId);
            return View(weapon);
        }

        // POST: Weapons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WeaponId,Name,Description,CategoryId,ElementId,Price,ManufacturerId,WeaponImage")] Weapon weapon)
        {
            if (id != weapon.WeaponId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weapon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeaponExists(weapon.WeaponId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Category1", weapon.CategoryId);
            ViewData["ElementId"] = new SelectList(_context.Elements, "ElementId", "ElementType", weapon.ElementId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "Location", weapon.ManufacturerId);
            return View(weapon);
        }

        // GET: Weapons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Weapons == null)
            {
                return NotFound();
            }

            var weapon = await _context.Weapons
                .Include(w => w.Category)
                .Include(w => w.Element)
                .Include(w => w.Manufacturer)
                .FirstOrDefaultAsync(m => m.WeaponId == id);
            if (weapon == null)
            {
                return NotFound();
            }

            return View(weapon);
        }

        // POST: Weapons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Weapons == null)
            {
                return Problem("Entity set 'BorderlandsStoreContext.Weapons'  is null.");
            }
            var weapon = await _context.Weapons.FindAsync(id);
            if (weapon != null)
            {
                _context.Weapons.Remove(weapon);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeaponExists(int id)
        {
          return (_context.Weapons?.Any(e => e.WeaponId == id)).GetValueOrDefault();
        }
    }
}
