using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            IEnumerable<Product> allProducts = _context.Product.Include(p => p.Category);
            return View(allProducts);
        }


        // GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            ProductVM prodVM = new ProductVM();

            Product prod = (id.HasValue ? _context.Product.Find(id.Value) : new Product());
            if (prod != null) {
                prodVM.Product = prod;

                IEnumerable<SelectListItem> CategoryDropDown = _context.Category.Select(cat => new SelectListItem { Text = cat.Name, Value = cat.Id.ToString() });
                prodVM.CategorySelectList = CategoryDropDown;

                IEnumerable<SelectListItem> ApplicationTypeDropDown = _context.ApplicationType.Select(at => new SelectListItem { Text = at.Name, Value = at.Id.ToString() });
                prodVM.ApplicationTypeSelectList = ApplicationTypeDropDown;

                return View(prodVM);
            }
            else
                return NotFound();

        }



        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM prodVM)
        {
            if (ModelState.IsValid)
            {

                var files = HttpContext.Request.Form.Files;

                string webRootPath = _webHostEnvironment.WebRootPath;
                string uploadPath = Path.Combine(webRootPath, WebConstants.ImagePath);


                if (prodVM.Product.Id == 0)
                {
                    //Creating

                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using(var fileStream = new FileStream(Path.Combine(uploadPath,fileName+extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    prodVM.Product.Image = fileName + extension;

                    _context.Product.Add(prodVM.Product);
                }
                else
                {
                    //Updating
                    _context.Product.Update(prodVM.Product);
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View();
        }
    }
}
