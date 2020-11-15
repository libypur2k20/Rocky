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


        //GET - EDIT
        public IActionResult Edit(int id)
        {
            ApplicationType appType = (id > 0 ? _dbContext.ApplicationType.Find(id) : null);

            return (appType != null ? View(appType) : NotFound());
        }


        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType appType)
        {
            if(appType != null)
            {
                if (ModelState.IsValid)
                {
                    _dbContext.ApplicationType.Update(appType);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(appType);
                }
            }

            return NotFound();
        }


        //GET - DELETE
        public IActionResult Delete(int id)
        {
            ApplicationType appType = (id > 0 ? _dbContext.ApplicationType.Find(id) : null);
            return (appType != null ? View(appType) : NotFound());
        }


        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ApplicationType appType)
        {
            if(appType != null)
            {
                _dbContext.ApplicationType.Remove(appType);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
