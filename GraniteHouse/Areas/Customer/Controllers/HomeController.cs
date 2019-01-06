using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GraniteHouse.Models;
using GraniteHouse.Data;
using Microsoft.EntityFrameworkCore;
using GraniteHouse.Extensions;

namespace GraniteHouse.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _db.products.Include(m => m.ProductTypes).Include(m => m.SpecialTags).ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.products.Include(m => m.ProductTypes).Include(m => m.SpecialTags).Where(m=>m.Id==id).FirstOrDefaultAsync();

            return View(product);
        }

        [HttpPost,ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsPost(int id)
        {
            List<int> lstCart = HttpContext.Session.Get<List<int>>("ssShopingCart");
            if (lstCart == null)
            {
                lstCart = new List<int>();
            }

            lstCart.Add(id);
            HttpContext.Session.Set("ssShopingCart",lstCart);
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        public IActionResult Remove(int id)
        {
            List<int> lstCart = HttpContext.Session.Get<List<int>>("ssShopingCart");
            if (lstCart.Count > 0)
            {
                if (lstCart.Contains(id))
                {
                    lstCart.Remove(id);
                }
            }
            HttpContext.Session.Set("ssShopingCart", lstCart);
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
