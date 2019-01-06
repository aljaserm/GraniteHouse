using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Extensions;
using GraniteHouse.Models;
using GraniteHouse.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraniteHouse.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ShoppingCartVM shoppingCartVM { get; set; }
        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
            shoppingCartVM = new ShoppingCartVM()
            {
                lstProducts = new List<Models.Products>(),
            };
        }

        // Get
        public IActionResult AppointmentConfirmation(int id)
        {
            shoppingCartVM.appointments = _db.appointments.Where(a => a.Id == id).FirstOrDefault();
            List<AppointmentsProducts> lstap = _db.appointmentsProducts.Where(p => p.appointmentId == id).ToList();
            foreach(AppointmentsProducts ap in lstap)
            {
                shoppingCartVM.lstProducts.Add(_db.products.Include(p=>p.ProductTypes).Include(p=>p.SpecialTags).Where(p=>p.Id==ap.ProductId).FirstOrDefault());
            }
            return View(shoppingCartVM);
        }

        //Post Remove from Cart
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
            return RedirectToAction(nameof(Index));
        }

        // Get ShoppingCart Index
        public async Task<IActionResult> Index()
        {
            List<int> lstCart = HttpContext.Session.Get<List<int>>("ssShopingCart");
            if (lstCart!=null)
            {
                foreach (int i in lstCart)
                {
                    Products products = _db.products.Include(p=>p.SpecialTags).Include(p=>p.ProductTypes).Where(p => p.Id == i).FirstOrDefault();
                    shoppingCartVM.lstProducts.Add(products);
                }
            }
            return View(shoppingCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult PostIndex()
        {
            List<int> lstCart = HttpContext.Session.Get<List<int>>("ssShopingCart");
            shoppingCartVM.appointments.AppointmentDate = shoppingCartVM.appointments.AppointmentDate.AddHours(shoppingCartVM.appointments.AppointmentTime.Hour).AddMinutes(shoppingCartVM.appointments.AppointmentTime.Minute);
            Appointments appointments = shoppingCartVM.appointments;
            _db.appointments.Add(appointments);
            _db.SaveChanges();
            int appointmentID = appointments.Id;
            foreach (int p in lstCart)
            {
                AppointmentsProducts ap = new AppointmentsProducts()
                {
                    appointmentId = appointmentID,
                    ProductId = p
                };
                _db.appointmentsProducts.Add(ap);
            }
            _db.SaveChanges();
            lstCart = new List<int>();
            HttpContext.Session.Set("ssShopingCart", lstCart);
            return RedirectToAction("AppointmentConfirmation","ShoppingCart",new { Id=appointmentID});


        }
    }
}