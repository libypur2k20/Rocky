using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Category> categories = _context.Category.ToList();
            return View(categories);
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Category.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }
        }


        //GET - EDIT
        public IActionResult Edit(int id)
        {
            Category cat = ( id > 0 ? _context.Category.Find(id) : null);

            return (cat != null ? View(cat) : NotFound());

        }


        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category cat)
        {
            if (ModelState.IsValid)
            {
                _context.Category.Update(cat);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(cat);
            }
        }
    }
}
