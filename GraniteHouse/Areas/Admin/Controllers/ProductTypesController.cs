using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Models;
using GraniteHouse.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraniteHouse.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminUser)]
    [Area("Admin")]
    public class ProductTypesController : Controller
    {

        private readonly ApplicationDbContext _db;
        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }
        // Get Create
        public IActionResult Create()
        {
            return View();
        }

        //post Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes value)
        {
            if (ModelState.IsValid)
            {
                _db.Add(value);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(value);
        }

        // Get Edit
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var product = await _db.productTypes.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? Id,ProductTypes value)
        {

            if (Id != value.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(value);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(value);
        }


        // Get Delete
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var product = await _db.productTypes.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //post Delete
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int Id)
        {

            var productTypeDeleted = await _db.productTypes.FindAsync(Id);
            _db.productTypes.Remove(productTypeDeleted);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Get Details
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var product = await _db.productTypes.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        public IActionResult Index()
        {
            return View(_db.productTypes.ToList());
        }
    }
}