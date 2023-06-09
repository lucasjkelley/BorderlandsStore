﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BorderlandsStore.DATA.EF.Models;

namespace BorderlandsStore.UI.MVC.Controllers
{
    public class ElementsController : Controller
    {
        private readonly BorderlandsStoreContext _context;

        public ElementsController(BorderlandsStoreContext context)
        {
            _context = context;
        }

        // GET: Elements
        public async Task<IActionResult> Index()
        {
              return _context.Elements != null ? 
                          View(await _context.Elements.ToListAsync()) :
                          Problem("Entity set 'BorderlandsStoreContext.Elements'  is null.");
        }

        // GET: Elements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Elements == null)
            {
                return NotFound();
            }

            var element = await _context.Elements
                .FirstOrDefaultAsync(m => m.ElementId == id);
            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // GET: Elements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Elements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElementId,ElementType")] Element element)
        {
            if (ModelState.IsValid)
            {
                _context.Add(element);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(element);
        }

        // GET: Elements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Elements == null)
            {
                return NotFound();
            }

            var element = await _context.Elements.FindAsync(id);
            if (element == null)
            {
                return NotFound();
            }
            return View(element);
        }

        // POST: Elements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ElementId,ElementType")] Element element)
        {
            if (id != element.ElementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(element);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElementExists(element.ElementId))
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
            return View(element);
        }

        // GET: Elements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Elements == null)
            {
                return NotFound();
            }

            var element = await _context.Elements
                .FirstOrDefaultAsync(m => m.ElementId == id);
            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // POST: Elements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Elements == null)
            {
                return Problem("Entity set 'BorderlandsStoreContext.Elements'  is null.");
            }
            var element = await _context.Elements.FindAsync(id);
            if (element != null)
            {
                _context.Elements.Remove(element);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElementExists(int id)
        {
          return (_context.Elements?.Any(e => e.ElementId == id)).GetValueOrDefault();
        }
    }
}
