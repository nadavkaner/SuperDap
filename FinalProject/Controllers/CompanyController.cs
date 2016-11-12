﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class CompanyController : Controller
    {
        private readonly Context _db = new Context();

        // GET: /Company/
        public ActionResult Index(CompanySearchCriteria searchCriteria)
        {
            var collection = _db.Companies;
            IQueryable<Company> query = collection;

            if (searchCriteria == null)
            {
                return View(new CompaniesModel()
                {
                    Companies = query.ToList(),
                    AvailableLocations = _db.Companies.Select(x => x.Location).Distinct().ToList()
                });
            }
            else if (!string.IsNullOrEmpty(searchCriteria.Location))
            {
                query = collection.Where(x => x.Location == searchCriteria.Location);
            }
            else
            {
                query = collection;
            }

            var companiesModel = new CompaniesModel()
            {
                Companies = query.ToList(),
                LocationFilter = searchCriteria.Location,
                AvailableLocations = _db.Companies.Select(x => x.Location).Distinct().ToList()
            };

            return View(companiesModel);
        }

        // GET: /Company/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = _db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: /Company/Create
        public ActionResult Create()
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            return View();
        }

        // POST: /Company/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,Location,HasCrippleEntrance,HasSecurity")] Company company)
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            if (ModelState.IsValid)
            {
                company.CompanyId = Guid.NewGuid();
                company.MostPopularComputer = _db.Computers.First();
                company.MostPopularComputerId = company.MostPopularComputer.ItemId;
                company.Coordinates = new Coordinates()
                {
                    Lat = 32.077263,
                    Long = 34.777505
                };
                _db.Companies.Add(company);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        // GET: /Company/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = _db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: /Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyId,Name,Description,Location,HasCrippleEntrance,HasSecurity")] Company company)
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            if (ModelState.IsValid)
            {
                var original = _db.Companies.Find(company.CompanyId);
                original.Description = company.Description;
                original.Location = company.Location;
                original.Name = company.Name;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        // GET: /Company/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = _db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: /Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            var store = _db.Companies.Find(id);
            store.MostPopularComputer = null;
            store.MostPopularComputerId = Guid.Empty;
            _db.SaveChanges();

            store = _db.Companies.Find(id);
            _db.Companies.Remove(store);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class CompanySearchCriteria : ItemSearchCriteria
    {
        public string Location { get; set; }
    }

    public class CompaniesModel
    {
        public IEnumerable<Company> Companies { get; set; }
        public IEnumerable<string> AvailableLocations { get; set; }
        public bool? SecurityFilter { get; set; }
        public bool? CrippleEntranceFilter { get; set; }
        public string LocationFilter { get; set; }
    }
}