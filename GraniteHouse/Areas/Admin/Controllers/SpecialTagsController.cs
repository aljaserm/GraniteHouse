using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Models;
using Microsoft.AspNetCore.Mvc;

namespace GraniteHouse.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SpecialTagsController(ApplicationDbContext db)
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
        public async Task<IActionResult> Create(SpecialTags value)
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
            var tags = await _db.specialTags.FindAsync(Id);
            if (tags == null)
            {
                return NotFound();
            }
            return View(tags);
        }

        //post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? Id, SpecialTags value)
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
            var tags = await _db.specialTags.FindAsync(Id);
            if (tags == null)
            {
                return NotFound();
            }
            return View(tags);
        }

        //post Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int Id)
        {

            var tagDeleted = await _db.specialTags.FindAsync(Id);
            _db.specialTags.Remove(tagDeleted);
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
            var tags = await _db.specialTags.FindAsync(Id);
            if (tags == null)
            {
                return NotFound();
            }
            return View(tags);
        }
        public IActionResult Index()
        {
            return View(_db.specialTags.ToList());
        }
    }
}