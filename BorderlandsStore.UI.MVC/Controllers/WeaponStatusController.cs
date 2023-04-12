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
    public class WeaponStatusController : Controller
    {
        private readonly BorderlandsStoreContext _context;

        public WeaponStatusController(BorderlandsStoreContext context)
        {
            _context = context;
        }

        // GET: WeaponStatus
        public async Task<IActionResult> Index()
        {
            var borderlandsStoreContext = _context.WeaponStatuses.Include(w => w.Weapon);
            return View(await borderlandsStoreContext.ToListAsync());
        }

        // GET: WeaponStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.WeaponStatuses == null)
            {
                return NotFound();
            }

            var weaponStatus = await _context.WeaponStatuses
                .Include(w => w.Weapon)
                .FirstOrDefaultAsync(m => m.WeaponId == id);
            if (weaponStatus == null)
            {
                return NotFound();
            }

            return View(weaponStatus);
        }

        // GET: WeaponStatus/Create
        public IActionResult Create()
        {
            ViewData["WeaponId"] = new SelectList(_context.Weapons, "WeaponId", "Description");
            return View();
        }

        // POST: WeaponStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WeaponId,InStock,OutOfStock,OnOrder,Discontinued")] WeaponStatus weaponStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(weaponStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WeaponId"] = new SelectList(_context.Weapons, "WeaponId", "Description", weaponStatus.WeaponId);
            return View(weaponStatus);
        }

        // GET: WeaponStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WeaponStatuses == null)
            {
                return NotFound();
            }

            var weaponStatus = await _context.WeaponStatuses.FindAsync(id);
            if (weaponStatus == null)
            {
                return NotFound();
            }
            ViewData["WeaponId"] = new SelectList(_context.Weapons, "WeaponId", "Description", weaponStatus.WeaponId);
            return View(weaponStatus);
        }

        // POST: WeaponStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WeaponId,InStock,OutOfStock,OnOrder,Discontinued")] WeaponStatus weaponStatus)
        {
            if (id != weaponStatus.WeaponId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weaponStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeaponStatusExists(weaponStatus.WeaponId))
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
            ViewData["WeaponId"] = new SelectList(_context.Weapons, "WeaponId", "Description", weaponStatus.WeaponId);
            return View(weaponStatus);
        }

        // GET: WeaponStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.WeaponStatuses == null)
            {
                return NotFound();
            }

            var weaponStatus = await _context.WeaponStatuses
                .Include(w => w.Weapon)
                .FirstOrDefaultAsync(m => m.WeaponId == id);
            if (weaponStatus == null)
            {
                return NotFound();
            }

            return View(weaponStatus);
        }

        // POST: WeaponStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.WeaponStatuses == null)
            {
                return Problem("Entity set 'BorderlandsStoreContext.WeaponStatuses'  is null.");
            }
            var weaponStatus = await _context.WeaponStatuses.FindAsync(id);
            if (weaponStatus != null)
            {
                _context.WeaponStatuses.Remove(weaponStatus);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeaponStatusExists(int id)
        {
          return (_context.WeaponStatuses?.Any(e => e.WeaponId == id)).GetValueOrDefault();
        }
    }
}
