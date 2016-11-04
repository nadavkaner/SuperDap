using System;
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
    public class StoreController : Controller
    {
        private readonly Context _db = new Context();

        // GET: /Store/
        public ActionResult Index(StoreSearchCriteria searchCriteria)
        {
            var collection = _db.Stores;
            IQueryable<Store> query = collection;

            if (searchCriteria == null)
            {
                return View(new StoresModel()
                {
                    Stores = query.ToList(),
                    AvailableLocations = _db.Stores.Select(x => x.Location).Distinct().ToList()
                });
            }

            if (searchCriteria.HasCrippleEntrance.HasValue &&
                searchCriteria.HasSecurity.HasValue &&
                !string.IsNullOrEmpty(searchCriteria.Location))
            {
                query = collection.Where(x => x.HasCrippleEntrance == searchCriteria.HasCrippleEntrance.Value &&
                                         x.HasSecurity == searchCriteria.HasSecurity.Value &&
                                         x.Location == searchCriteria.Location);
            }
            else if (searchCriteria.HasCrippleEntrance.HasValue &&
                     searchCriteria.HasSecurity.HasValue)
            {
                query = collection.Where(x => x.HasCrippleEntrance == searchCriteria.HasCrippleEntrance.Value &&
                                         x.HasSecurity == searchCriteria.HasSecurity.Value);
            }
            else if (searchCriteria.HasCrippleEntrance.HasValue &&
                     !string.IsNullOrEmpty(searchCriteria.Location))
            {
                query = collection.Where(x => x.HasCrippleEntrance == searchCriteria.HasCrippleEntrance.Value &&
                                              x.Location == searchCriteria.Location);
            }
            else if (searchCriteria.HasSecurity.HasValue &&
                     !string.IsNullOrEmpty(searchCriteria.Location))
            {
                query = collection.Where(x => x.HasSecurity == searchCriteria.HasSecurity.Value &&
                                              x.Location == searchCriteria.Location);
            }
            else if (searchCriteria.HasSecurity.HasValue)
            {
                query = collection.Where(x => x.HasSecurity == searchCriteria.HasSecurity.Value);
            }
            else if (!string.IsNullOrEmpty(searchCriteria.Location))
            {
                query = collection.Where(x => x.Location == searchCriteria.Location);
            }
            else if (searchCriteria.HasCrippleEntrance.HasValue)
            {
                query = collection.Where(x => x.HasCrippleEntrance == searchCriteria.HasCrippleEntrance.Value);
            }
            else
            {
                query = collection;
            }

            var storesModel = new StoresModel()
            {
                Stores = query.ToList(),
                CrippleEntranceFilter = searchCriteria.HasCrippleEntrance,
                LocationFilter = searchCriteria.Location,
                SecurityFilter = searchCriteria.HasSecurity,
                AvailableLocations = _db.Stores.Select(x => x.Location).Distinct().ToList()
            };

            return View(storesModel);
        }

        // GET: /Store/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = _db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // GET: /Store/Create
        public ActionResult Create()
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            return View();
        }

        // POST: /Store/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,Location,HasCrippleEntrance,HasSecurity")] Store store)
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            if (ModelState.IsValid)
            {
                store.StoreId = Guid.NewGuid();
                store.MostPopularComputer = _db.Computers.First();
                store.MostPopularComputerId = store.MostPopularComputer.ItemId;
                store.Coordinates = new Coordinates()
                {
                    Lat = 32.077263,
                    Long = 34.777505
                };
                _db.Stores.Add(store);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(store);
        }

        // GET: /Store/Edit/5
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
            Store store = _db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // POST: /Store/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StoreId,Name,Description,Location,HasCrippleEntrance,HasSecurity")] Store store)
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            if (ModelState.IsValid)
            {
                var original = _db.Stores.Find(store.StoreId);
                original.HasCrippleEntrance = store.HasCrippleEntrance;
                original.HasSecurity = store.HasSecurity;
                original.Description = store.Description;
                original.Location = store.Location;
                original.Name = store.Name;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(store);
        }

        // GET: /Store/Delete/5
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
            Store store = _db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // POST: /Store/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            var store = _db.Stores.Find(id);
            store.MostPopularComputer = null;
            store.MostPopularComputerId = Guid.Empty;
            _db.SaveChanges();

            store = _db.Stores.Find(id);
            _db.Stores.Remove(store);
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

    public class StoreSearchCriteria : ItemSearchCriteria
    {
        public bool? HasCrippleEntrance { get; set; }
        public bool? HasSecurity { get; set; }
        public string Location { get; set; }
    }

    public class StoresModel
    {
        public IEnumerable<Store> Stores { get; set; }
        public IEnumerable<string> AvailableLocations { get; set; }
        public bool? SecurityFilter { get; set; }
        public bool? CrippleEntranceFilter { get; set; }
        public string LocationFilter { get; set; }
    }
}
