using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BorderlandsStore.DATA.EF.Models;
using Microsoft.AspNetCore.Authorization;
using BorderlandsStore.UI.MVC.Utilities;
using System.Drawing;

namespace BorderlandsStore.UI.MVC.Controllers
{
    public class WeaponsController : Controller
    {
        private readonly BorderlandsStoreContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public WeaponsController(BorderlandsStoreContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Weapons
        public async Task<IActionResult> Index()
        {
            var borderlandsStoreContext = _context.Weapons.Include(w => w.Category).Include(w => w.Element).Include(w => w.Manufacturer);
            return View(await borderlandsStoreContext.ToListAsync());
        }
        public async Task<IActionResult> TiledProducts()
        {
            var products = _context.Weapons

                    .Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.WeaponStatus);
            return View(await products.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("WeaponId,Name,Description,CategoryId,ElementId,Price,ManufacturerId,WeaponImage,ImageFile")] Weapon weapon)
        {
            if (ModelState.IsValid)
            {
                #region File Upload - CREATE w/ Image Utility
                if (weapon.ImageFile != null)
                {
                    // process file

                    // Check the file type
                    // - retrieve the extension of the uploaded file
                    string ext = Path.GetExtension(weapon.ImageFile.FileName);

                    // - Create a list of valid extensions
                    string[] validExts = { ".jpg", ".jpeg", ".gif", ".png" };

                    // - Check the file extension against the list of valid extensions
                    if (validExts.Contains(ext.ToLower()) && weapon.ImageFile.Length < 4_194_303)
                    {
                        // Generate a unique filename
                        weapon.WeaponImage = Guid.NewGuid() + ext;

                        // Save our file to the web server (here it's saved to wwwroot/images
                        // - retrieve the path to the webroot
                        string webRootPath = _webHostEnvironment.WebRootPath;

                        // - create a variable for the full image path
                        string fullImagePath = webRootPath + "/asset/img/Weapon_Pics/";

                        // Create a MemoryStream to read the image into our web server's memory
                        using (var memoryStream = new MemoryStream())
                        {
                            await weapon.ImageFile.CopyToAsync(memoryStream);
                            using (var img = Image.FromStream(memoryStream))
                            {
                                // now, send the image to the ImageUtility for resizing and saving
                                // need 5 arguments for the utility to resize our image...
                                // 1) (int) maximum image size
                                // 2) (int) maximum thumbnamil size
                                // 3) (string) full path where the file will be saved
                                // 4) (Image) an image
                                // 5) (string) filename

                                int maxImageSize = 500;
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(fullImagePath, weapon.WeaponImage, img, maxImageSize, maxThumbSize);
                            }
                        }
                    }
                }
                else
                {
                    // assign a default image
                    // If no image was uploaded, assign a default filename
                    // Will also need to download a default image and name it 'noimage.png' -> place it in the wwroot/images
                    weapon.WeaponImage = "noimage.png";
                }
                #endregion

                _context.Add(weapon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TiledProducts));
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
        public async Task<IActionResult> Edit(int id, [Bind("WeaponId,Name,Description,CategoryId,ElementId,Price,ManufacturerId,WeaponImage,ImageFile")] Weapon weapon)
        {
            if (id != weapon.WeaponId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                #region File Upload - EDIT w/ Image Utility

                //retain old image file name so we can reuse if needed, or delete if a new file is uploaded
                string oldImageName = weapon.WeaponImage;

                //check if user uploaded a file
                if (weapon.ImageFile != null)
                {
                    // check the file extension
                    string ext = Path.GetExtension(weapon.ImageFile.FileName);

                    // list of valid extensions
                    string[] validExts = { ".jpg", ".jpeg", ".png", ".gif" };

                    // check the file extension against the valid extensions && check file size
                    if (validExts.Contains(ext.ToLower()) && weapon.ImageFile.Length < 4_194_303)
                    {
                        // generate a unique filename
                        weapon.WeaponImage = Guid.NewGuid() + ext;

                        // define our filepath to save the image
                        string fullPath = _webHostEnvironment.WebRootPath + "/assets/img/Weapon_Pics/";

                        // Delete the old image
                        if (oldImageName != "noimage.png" && oldImageName != null)
                        {
                            ImageUtility.Delete(fullPath, oldImageName);
                        }

                        // Save new image to webroot
                        using (var memoryStream = new MemoryStream())
                        {
                            await weapon.ImageFile.CopyToAsync(memoryStream);
                            using (var img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 500;
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(fullPath, weapon.WeaponImage, img, maxImageSize, maxThumbSize);
                            }
                        }
                    }
                }

                #endregion

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
                return RedirectToAction(nameof(TiledProducts));
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
