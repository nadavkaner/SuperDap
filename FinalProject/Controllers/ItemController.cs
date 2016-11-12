using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class ItemController : Controller
    {
        private readonly Context _db = new Context();

        public ActionResult Index(ComputerSearchCriteria searchCriteria)
        {
            var collection = _db.Computers;
            IQueryable<Computer> query = collection;

            if (searchCriteria == null)
            {
                return View(new ComputersModel()
                {
                    Computers = query.ToList()
                });
            }
            
            if (searchCriteria.HasTouchScreen.HasValue &&
                searchCriteria.GraphicsCardManufacturer.HasValue &&
                searchCriteria.ProcessorType.HasValue)
            {
                query = collection.Where(x => x.HasTouchScreen == searchCriteria.HasTouchScreen.Value &&
                                         x.GraphicsCardManufacturer == searchCriteria.GraphicsCardManufacturer.Value &&
                                         x.ProcessorType == searchCriteria.ProcessorType.Value);
            }
            else if (searchCriteria.HasTouchScreen.HasValue &&
                     searchCriteria.GraphicsCardManufacturer.HasValue)
            {
                query = collection.Where(x => x.HasTouchScreen == searchCriteria.HasTouchScreen.Value &&
                                         x.GraphicsCardManufacturer == searchCriteria.GraphicsCardManufacturer.Value);
            }
            else if (searchCriteria.HasTouchScreen.HasValue &&
                     searchCriteria.ProcessorType.HasValue)
            {
                query = collection.Where(x => x.HasTouchScreen == searchCriteria.HasTouchScreen.Value &&
                                              x.ProcessorType == searchCriteria.ProcessorType.Value);
            }
            else if (searchCriteria.GraphicsCardManufacturer.HasValue &&
                     searchCriteria.ProcessorType.HasValue)
            {
                query = collection.Where(x => x.GraphicsCardManufacturer == searchCriteria.GraphicsCardManufacturer.Value &&
                                              x.ProcessorType == searchCriteria.ProcessorType.Value);
            }
            else if (searchCriteria.GraphicsCardManufacturer.HasValue)
            {
                query = collection.Where(x => x.GraphicsCardManufacturer == searchCriteria.GraphicsCardManufacturer.Value);
            }
            else if (searchCriteria.ProcessorType.HasValue)
            {
                query = collection.Where(x => x.ProcessorType == searchCriteria.ProcessorType.Value);
            }
            else if (searchCriteria.HasTouchScreen.HasValue)
            {
                query = collection.Where(x => x.HasTouchScreen == searchCriteria.HasTouchScreen.Value);
            }
            else
            {
                query = collection;
            }

            return View(new ComputersModel()
            {
                Computers = query.ToList(),
                GraphicsCardManufacturerFilter = searchCriteria.GraphicsCardManufacturer,
                ProcessorTypeFilter = searchCriteria.ProcessorType,
                TouchScreenFilter = searchCriteria.HasTouchScreen,
            });
        }

        // GET: /Item/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _db.Computers.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: /Item/Create
        public ActionResult Create()
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            return View();
        }

        // POST: /Item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Price,AvailableFrom,Name,Description,Category")] Computer item)
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            if (ModelState.IsValid)
            {
                item.ItemId = Guid.NewGuid();
                item.Company = _db.Companies.First();
                item.StoreId = item.Company.CompanyId;
                _db.Computers.Add(item);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: /Item/Edit/5
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
            Item item = _db.Computers.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: /Item/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ItemId,Price,AvailableFrom,Name,Description,Category")] Item item)
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            if (ModelState.IsValid)
            {
                var original = _db.Computers.Find(item.ItemId);
                original.Price = item.Price;
                original.AvailableFrom = item.AvailableFrom;
                original.Name = item.Name;
                original.Description = item.Description;
                original.Category = item.Category;

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: /Item/Delete/5
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
            Item item = _db.Computers.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: /Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            var conflictingStores = _db.Companies.Where(x => x.MostPopularComputer.ItemId == id).ToList();
            foreach (var store in conflictingStores)
            {
                store.MostPopularComputer = null;
                store.MostPopularComputerId = Guid.Empty;
            }

            var  item = _db.Computers.Find(id);
            _db.Computers.Remove(item);
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

    public class ItemSearchCriteria
    {
        public int? PriceRangeFrom { get; set; }
        public int? PriceRangeTo { get; set; }
    }

    public class ComputerSearchCriteria : ItemSearchCriteria
    {
        public bool? HasTouchScreen { get; set; }
        public ProcessorType? ProcessorType { get; set; }
        public GraphicsCardManufacturer? GraphicsCardManufacturer { get; set; }
    }

}
