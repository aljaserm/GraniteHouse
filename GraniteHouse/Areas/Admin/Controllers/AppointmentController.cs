using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Models;
using GraniteHouse.Utility;
using GraniteHouse.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraniteHouse.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminUser + "," + SD.AdminUser)]
    [Area("Admin")]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AppointmentController(ApplicationDbContext db)
        {
            _db = db;
        }

        //Get Delete
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                NotFound();
            }
            var lstProducts = (IEnumerable<Products>)(from p in _db.products
                                                      join a in _db.appointmentsProducts
                                                      on p.Id equals a.ProductId
                                                      where a.appointmentId == id
                                                      select p).Include("ProductTypes");
            AppointmentsDetailsVM advm = new AppointmentsDetailsVM()
            {
                appointments = _db.appointments.Include(a => a.SalesPerson).Where(a => a.Id == id).FirstOrDefault(),
                lstUsers = _db.users.Where(u => u.LockoutEnd == null || u.LockoutEnd < DateTime.Now).ToList(),
                lstproducts = lstProducts.ToList()
            };
            return View(advm);
        }

        //Post Delete

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var app = await _db.appointments.FindAsync(id);
            _db.appointments.Remove(app);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Get Edit
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                NotFound();
            }
            var lstProducts = (IEnumerable<Products>)(from p in _db.products
                                                      join a in _db.appointmentsProducts
                                                      on p.Id equals a.ProductId
                                                      where a.appointmentId == id
                                                      select p).Include("ProductTypes");
            AppointmentsDetailsVM advm = new AppointmentsDetailsVM()
            {
                appointments = _db.appointments.Include(a => a.SalesPerson).Where(a => a.Id == id).FirstOrDefault(),
                lstUsers = _db.users.Where(u => u.LockoutEnd == null || u.LockoutEnd < DateTime.Now).ToList(),
                lstproducts = lstProducts.ToList()
            };
            return View(advm);
        }


        //Get Edit
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                NotFound();
            }
            var lstProducts = (IEnumerable<Products>)(from p in _db.products
                                                   join a in _db.appointmentsProducts
                                                   on p.Id equals a.ProductId
                                                   where a.appointmentId == id
                                                   select p).Include("ProductTypes");
            AppointmentsDetailsVM advm = new AppointmentsDetailsVM()
            {
                appointments = _db.appointments.Include(a => a.SalesPerson).Where(a => a.Id == id).FirstOrDefault(),
                lstUsers = _db.users.Where(u=>u.LockoutEnd==null || u.LockoutEnd<DateTime.Now).ToList(),
                lstproducts=lstProducts.ToList()
            };
            return View(advm);
        }

        //Post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppointmentsDetailsVM value)
        {
            if (ModelState.IsValid)
            {
                value.appointments.AppointmentDate = value.appointments.AppointmentDate.AddHours(value.appointments.AppointmentTime.Hour).AddMinutes(value.appointments.AppointmentTime.Minute);
                var dbappointment = _db.appointments.Where(a => a.Id == value.appointments.Id).FirstOrDefault();
                dbappointment.CustomerName = value.appointments.CustomerName;
                dbappointment.CustomerEmail = value.appointments.CustomerEmail;
                dbappointment.CustomerPhone = value.appointments.CustomerPhone;
                dbappointment.AppointmentDate = value.appointments.AppointmentDate;
                dbappointment.Confirmed = value.appointments.Confirmed;
                if (User.IsInRole(SD.SuperAdminUser))
                {
                    dbappointment.SalesPersonID = value.appointments.SalesPersonID;
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(value);
        }


        public async Task<IActionResult> Index(string searchName=null, string searchPhone=null, string searchEmail=null,string SearchDate=null)
        {
            ClaimsPrincipal claimsUser = this.User;
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claims=claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            AppointmentsVM appointmentsVM = new AppointmentsVM()
            {
                appointments=new List<Models.Appointments>()
            };

            

            appointmentsVM.appointments = _db.appointments.Include(a=>a.SalesPerson).ToList();
            if (User.IsInRole(SD.AdminUser))
            {
                appointmentsVM.appointments = appointmentsVM.appointments.Where(a => a.SalesPersonID == claims.Value).ToList();
            }

            if (searchName != null)
            {
                appointmentsVM.appointments = appointmentsVM.appointments.Where(a => a.CustomerName.ToLower().Contains(searchName.ToLower())).ToList();

            }
            if (searchEmail != null)
            {
                appointmentsVM.appointments = appointmentsVM.appointments.Where(a => a.CustomerEmail.ToLower().Contains(searchEmail.ToLower())).ToList();

            }
            if (searchPhone != null)
            {
                appointmentsVM.appointments = appointmentsVM.appointments.Where(a => a.CustomerPhone.ToLower().Contains(searchPhone.ToLower())).ToList();

            }
            if (SearchDate != null)
            {
                try
                {
                    DateTime searchAppDate = Convert.ToDateTime(SearchDate);
                    appointmentsVM.appointments = appointmentsVM.appointments.Where(a => a.AppointmentDate.ToShortDateString().Equals(searchAppDate.ToShortDateString())).ToList();
                }
                catch(Exception ex)
                {

                }

            }

            return View(appointmentsVM);
        }
    }
}