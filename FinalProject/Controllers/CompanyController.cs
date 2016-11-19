using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public ActionResult Create(Company company)
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            if (ModelState.IsValid)
            {
                company.CompanyId = Guid.NewGuid();
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

            ViewBag.devProducts = company.DevelopmentTools.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList();

            CompanyViewModel companyVm = new CompanyViewModel()
            {
                Name = company.Name,
                MostPopularDevelopmentTool = company.MostPopularDevelopmentTool != null ? company.MostPopularDevelopmentTool.Id.ToString() : "",
                CompanyId = company.CompanyId,
                DevelopmentTools = company.DevelopmentTools,
                Coordinates = company.Coordinates,
                Description = company.Description,
                Location = company.Location,
                RevenuePerYears = company.RevenuePerYears,
                TotalRevenue = company.TotalRevenue,
                ImagePath = company.ImagePath
            };
            return View(companyVm);
        }

        // POST: /Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyViewModel company)
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
                original.Coordinates = company.Coordinates;
                original.Name = company.Name;
                original.MostPopularDevelopmentTool = !string.IsNullOrEmpty(company.MostPopularDevelopmentTool) ?
                    _db.DevelopmentTools.Find(Guid.Parse(company.MostPopularDevelopmentTool)) : null;
                original.TotalRevenue = company.TotalRevenue;
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

            var company = _db.Companies.Find(id);
            company.MostPopularDevelopmentTool = null;
            _db.SaveChanges();

            company = _db.Companies.Find(id);
            foreach (var year in company.RevenuePerYears.ToList())
            {
                company.RevenuePerYears.Remove(year);
            }

            foreach (var developmentTool in company.DevelopmentTools)
            {
                developmentTool.Comments.Clear();
            }

            foreach (var developmentTool in company.DevelopmentTools.ToList())
            {
                _db.DevelopmentTools.Remove(developmentTool);
            }
            _db.SaveChanges();
            _db.Companies.Remove(company);
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

    public class CompanyViewModel
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string MostPopularDevelopmentTool { get; set; }
        public ICollection<DevelopmentTool> DevelopmentTools { get; set; }
        public Coordinates Coordinates { get; set; }
        public double TotalRevenue { get; set; }
        public ICollection<RevenueForYear> RevenuePerYears { get; set; }
        public string ImagePath { get; set; }
    }

    public class MostPopularDevelopmentToolViewModel
    {
        public string Id { get; set; }
        
    }
}
