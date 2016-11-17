using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class DevelopmentToolController : Controller
    {
        private readonly Context _db = new Context();

        public ActionResult Index(ItemSearchCriteria searchCriteria)
        {
            var collection = _db.DevelopmentTools;
            IQueryable<DevelopmentTool> query = collection;

            if (searchCriteria == null)
            {
                return View(new DevelopmentToolsModel());
            }
            

            query = collection;

            return View(new DevelopmentToolsModel()
            {
                DevTools = query.ToList()
            });
        }

        // GET: /DevelopmentTool/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DevelopmentTool developmentTool = _db.DevelopmentTools.Find(id);
            if (developmentTool == null)
            {
                return HttpNotFound();
            }
            return View(developmentTool);
        }

        // GET: /DevelopmentTool/Create
        public ActionResult Create()
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            return View();
        }

        // POST: /DevelopmentTool/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Price,LastUpdate,Name,Description,Category")] DevelopmentTool developmentTool)
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            if (ModelState.IsValid)
            {
                developmentTool.Id = Guid.NewGuid();
                developmentTool.Company = _db.Companies.First();
                _db.DevelopmentTools.Add(developmentTool);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(developmentTool);
        }

        // GET: /DevelopmentTool/Edit/5
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
            DevelopmentTool developmentTool = _db.DevelopmentTools.Find(id);
            if (developmentTool == null)
            {
                return HttpNotFound();
            }
            return View(developmentTool);
        }

        // POST: /DevelopmentTool/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Price,LastUpdate,Name,Description,Category")] DevelopmentTool developmentTool)
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            if (ModelState.IsValid)
            {
                var original = _db.DevelopmentTools.Find(developmentTool.Id);
                original.Price = developmentTool.Price;
                original.LastUpdate = developmentTool.LastUpdate;
                original.Name = developmentTool.Name;
                original.Description = developmentTool.Description;

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(developmentTool);
        }

        // GET: /DevelopmentTool/Delete/5
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
            DevelopmentTool developmentTool = _db.DevelopmentTools.Find(id);
            if (developmentTool == null)
            {
                return HttpNotFound();
            }
            return View(developmentTool);
        }

        // POST: /DevelopmentTool/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            var conflictingStores = _db.Companies.Where(x => x.MostPopularDevelopmentTool.Id == id).ToList();
            foreach (var store in conflictingStores)
            {
                store.MostPopularDevelopmentTool = null;
                store.MostPopularComputerId = Guid.Empty;
            }

            var  item = _db.DevelopmentTools.Find(id);
            _db.DevelopmentTools.Remove(item);
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

    public class DevelopmentToolsModel
    {
        public List<DevelopmentTool> DevTools { get; set; }
    }

    public class ItemSearchCriteria
    {
        public int? PriceRangeFrom { get; set; }
        public int? PriceRangeTo { get; set; }
    }

}
