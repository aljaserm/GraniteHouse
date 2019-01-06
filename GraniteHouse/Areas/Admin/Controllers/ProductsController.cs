using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Models;
using GraniteHouse.Utility;
using GraniteHouse.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraniteHouse.Controllers
{
    [Authorize(Roles = SD.SuperAdminUser)]
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

        //GET Delete Product
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            productsVM.products = await _db.products.Include(m => m.SpecialTags).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);
            if (productsVM.products == null)
            {
                return NotFound();
            }

            return View(productsVM);
        }

        //Post Delete
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string webPathRoot = _he.WebRootPath;
            Products products = await _db.products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            else
            {
                var uploads = Path.Combine(webPathRoot,SD.ImageFolder);
                var extension = Path.GetExtension(products.Image);
                if (System.IO.File.Exists(Path.Combine(uploads, products.Id + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, products.Id + extension));
                }
                _db.products.Remove(products);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        //GET Details Product
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            productsVM.products = await _db.products.Include(m => m.SpecialTags).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);
            if (productsVM.products == null)
            {
                return NotFound();
            }

            return View(productsVM);
        }

        //GET Edit Product
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            productsVM.products = await _db.products.Include(m => m.SpecialTags).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);
            if (productsVM.products == null)
            {
                return NotFound();
            }

            return View(productsVM);
        }

        // Post Edit Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _he.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                var dbProduct = _db.products.Where(m => m.Id == productsVM.products.Id).FirstOrDefault();
                if (files.Count > 0 && files[0]!=null)
                {
                    //if there is new image
                    var uploads = Path.Combine(webRootPath,SD.ImageFolder);
                    var newExtention = Path.GetExtension(files[0].FileName);
                    var oldExtention = Path.GetExtension(dbProduct.Image);
                    if (System.IO.File.Exists(Path.Combine(uploads, productsVM.products.Id + oldExtention)))
                    {
                        System.IO.File.Delete(Path.Combine(uploads, productsVM.products.Id + oldExtention));
                    }
                    using (var filestream = new FileStream(Path.Combine(uploads, productsVM.products.Id + newExtention), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    productsVM.products.Image = @"\" + SD.ImageFolder + @"\" + productsVM.products.Id + newExtention;
                }
                if(productsVM.products.Image != null)
                {
                    dbProduct.Image = productsVM.products.Image;
                }
                dbProduct.Name = productsVM.products.Name;
                dbProduct.Price = productsVM.products.Price;
                dbProduct.Available = productsVM.products.Available;
                dbProduct.ProductTypeId = productsVM.products.ProductTypeId;
                dbProduct.SpecialTagId = productsVM.products.SpecialTagId;
                dbProduct.ProductColor = productsVM.products.ProductColor;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index
));
            }
            else
            {
                return View(productsVM);
            }
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