using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Utility;
using GraniteHouse.ViewModel;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraniteHouse.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly HostingEnvironment _he;
        [BindProperty]
        public ProductsVM productsVM { get; set; }
        public ProductsController(ApplicationDbContext db, HostingEnvironment he)
        {
            _db = db;
            _he = he;
            productsVM = new ProductsVM()
            {
                ProductTypes = _db.productTypes.ToList(),
                specialTags = _db.specialTags.ToList(),
                products = new Models.Products()
            };
        }

        //Get Create
        public IActionResult Create()
        {
            return View(productsVM);
        }
        //post
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostProducts()
        {
            if (!ModelState.IsValid)
            {
                return View(productsVM);
            }
            _db.products.Add(productsVM.products);
            await _db.SaveChangesAsync();

            //Handle Image Save
            string webRootPath = _he.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var dbProduct = _db.products.Find(productsVM.products.Id);
            if (files.Count != 0)
            {
                //img uploaded
                var uploads = Path.Combine(webRootPath,SD.ImageFolder);
                var extention = Path.GetExtension(files[0].FileName);
                using(var filestream= new FileStream(Path.Combine(uploads, productsVM.products.Id + extention), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                dbProduct.Image = @"\" + SD.ImageFolder + @"\" + productsVM.products.Id + extention;
            }
            else
            {
                //img not uploaded
                var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);
                System.IO.File.Copy(uploads,webRootPath+@"\"+SD.ImageFolder+@"\"+productsVM.products.Id+".png");
                dbProduct.Image = @"\" + SD.ImageFolder + @"\" + productsVM.products.Id + ".png";
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Index()
        {
            var product = _db.products.Include(m => m.ProductTypes).Include(m => m.SpecialTags);
            return View(await product.ToListAsync());
        }
    }
}