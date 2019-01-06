using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Models;
using GraniteHouse.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraniteHouse.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminUser)]
    [Area("Admin")]
    public class UsersMgmtController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UsersMgmtController(ApplicationDbContext db)
        {
            _db = db;
        }


        //get Edit
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var dbUser = await _db.users.FindAsync(id);
            if (dbUser == null)
            {
                return NotFound();
            }
            return View(dbUser);
        }

        //post edit
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(string id)
        {
            Users dbUser = _db.users.Where(u => u.Id == id).FirstOrDefault();
            dbUser.LockoutEnd =DateTime.Now.AddYears(10);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        //get Edit
        public async Task<IActionResult> Edit(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var dbUser = await _db.users.FindAsync(id);
            if (dbUser==null)
            {
                return NotFound();
            }
            return View(dbUser);
        }

        //post edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, Users value)
        {
            if (id != value.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                Users dbUser = _db.users.Where(u => u.Id == id).FirstOrDefault();
                dbUser.Name = value.Name;
                dbUser.PhoneNumber = value.PhoneNumber;

                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(value);
        }

        public IActionResult Index()
        {
            return View(_db.users.ToList());
        }
    }
}