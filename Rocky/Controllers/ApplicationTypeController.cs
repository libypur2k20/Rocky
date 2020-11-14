using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ApplicationTypeController(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public IActionResult Index()
        {
            List<ApplicationType> appTypes = _dbContext.ApplicationType.ToList();
            return View(appTypes);
        }


        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }


        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType appType)
        {
            if (ModelState.IsValid)
            {
                _dbContext.ApplicationType.Add(appType);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(appType);
            }
        }
    }
}
