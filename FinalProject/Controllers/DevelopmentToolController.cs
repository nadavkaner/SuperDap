using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FinalProject.Models;
using Microsoft.AspNet.Identity;

namespace FinalProject.Controllers
{
    public class DevelopmentToolController : Controller
    {
        private readonly Context _db = new Context();

        public ActionResult Index(DevToolSearchCriteria searchCriteria)
        {
            var collection = _db.DevelopmentTools;
            IQueryable<DevelopmentTool> query = collection;

            if (searchCriteria == null)
            {
                return View(new DevelopmentToolsModel());
            }
            if (!string.IsNullOrWhiteSpace(searchCriteria.SourceCodeLicenses))
            {
                var sourceCodeLicenses = (SourceCodeLicense)Enum.Parse(typeof(SourceCodeLicense), searchCriteria.SourceCodeLicenses);
                query = query.Where(x => x.SourceCodeLicense == sourceCodeLicenses);
            }
            if (!string.IsNullOrWhiteSpace(searchCriteria.CompanyName))
            {
                query = query.Where(x => x.Company.Name == searchCriteria.CompanyName);
            }
            if (searchCriteria.PriceRange != PriceRange.None)
            {
                query = FilterDevToolsByPriceRange(searchCriteria, query);
            }

            DevelopmentToolsModel developmentToolsModel = new DevelopmentToolsModel()
            {
                AvailableSourceCodeLicenses = Enum.GetNames(typeof(SourceCodeLicense)),
                AvailableCompanyNames = _db.DevelopmentTools.Select(x => x.Company.Name).Distinct().ToList(),
                CompanyNameFilter = searchCriteria.CompanyName,
                SourceCodeLicenseFilter = searchCriteria.SourceCodeLicenses,
                PriceFilter = searchCriteria.PriceRange,
                DevTools = query.ToList(),
                DevToolsGroupedByCompany = GetDevToolsGroupedByCompany(query)
            };
            return View(developmentToolsModel);
        }

        private static IQueryable<DevelopmentTool> FilterDevToolsByPriceRange(DevToolSearchCriteria searchCriteria, IQueryable<DevelopmentTool> collection)
        {
            switch (searchCriteria.PriceRange)
            {
                case PriceRange.ZeroToHundred:
                    return collection.Where(x => x.Price >= 0 && x.Price < 100);
                case PriceRange.HundredToThousand:
                    return collection.Where(x => x.Price >= 100 && x.Price < 1000);
                case PriceRange.ThousandToTenThousand:
                    return collection.Where(x => x.Price >= 1000 && x.Price < 10000);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Dictionary<Company, List<DevelopmentTool>> GetDevToolsGroupedByCompany(IQueryable<DevelopmentTool> query)
        {
            return query.GroupBy(x => x.Company).ToDictionary(x => x.Key, x => x.ToList());
        }

        // GET: /DevelopmentTool/GroupByCompany
        public ActionResult GroupByCompany(DevToolSearchCriteria searchCriteria)
        {
            var collection = _db.DevelopmentTools;
            IQueryable<DevelopmentTool> query = collection;

            if (searchCriteria == null)
            {
                return View(new DevelopmentToolsModel());
            }


            query = collection;

            DevelopmentToolsModel developmentToolsModel = new DevelopmentToolsModel()
            {
                ShowGroupedBy = true, DevTools = query.ToList(), DevToolsGroupedByCompany = GetDevToolsGroupedByCompany(query)
            };
            return View(developmentToolsModel);
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
        public ActionResult Edit([Bind(Include = "Id,Price,LastUpdate,Name,Description,Category")] DevelopmentTool developmentTool)
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

        // GET: /DevelopmentTool/DeleteComment/5
        public ActionResult DeleteComment(Guid? id)
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userIsAdmin = User.IsInRole("Admin");
            var developmentTools = _db.DevelopmentTools.Where(x => x.Comments.Any()).ToList();
            var devToolcomment = developmentTools.SelectMany(x => x.Comments.Select(y => new {devtool = x, comment = y})).FirstOrDefault(z => z.comment.Id == id);
            if (devToolcomment == null || (!userIsAdmin && devToolcomment.comment.User.Id != User.Identity.GetUserId()))
            {
                return HttpNotFound();
            }
            devToolcomment.devtool.Comments.Remove(devToolcomment.comment);
            _db.SaveChanges();
            return RedirectToAction("Details", new {id = devToolcomment.devtool.Id});
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

            var companies = _db.Companies.Where(x => x.MostPopularDevelopmentTool.Id == id).ToList();
            foreach (var company in companies)
            {
                company.MostPopularDevelopmentTool = null;
            }

            var developmentTool = _db.DevelopmentTools.Find(id);
            foreach (var comments in developmentTool.Comments.ToList())
            {
                developmentTool.Comments.Remove(comments);
            }
            _db.DevelopmentTools.Remove(developmentTool);
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

        public ActionResult AddComment(Guid? id)
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var devtool = _db.DevelopmentTools.FirstOrDefault(x => x.Id == id);
            if (devtool == null)
            {
                return HttpNotFound();
            }
            return View(new LeaveACommentModel {DevToolId = devtool.Id, DevToolName = devtool.Name});
        }


        // POST: /DevelopmentTool/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment([Bind(Include = "Text, DevToolId, DevToolName")] LeaveACommentModel leaveACommentModel)
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Index");
            }
            var applicationUser = _db.Users.Find(User.Identity.GetUserId());
            if (applicationUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (leaveACommentModel == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!string.IsNullOrEmpty(leaveACommentModel.Text))
            {
                var devtool = _db.DevelopmentTools.FirstOrDefault(x => x.Id == leaveACommentModel.DevToolId);
                if (devtool == null)
                {
                    return HttpNotFound();
                }
                devtool.Comments.Add(new Comment {Id = Guid.NewGuid(), Date = DateTime.Now, Text = leaveACommentModel.Text, User = applicationUser});
                _db.SaveChanges();
                return RedirectToAction("Details", new {id = leaveACommentModel.DevToolId});
            }
            else
            {
                ModelState.AddModelError("Text", "Enter text for a comment");
                return View(leaveACommentModel);
            }
        }
    }

    public class DevelopmentToolsModel
    {
        public bool ShowGroupedBy { get; set; }
        public List<DevelopmentTool> DevTools { get; set; }
        public Dictionary<Company, List<DevelopmentTool>> DevToolsGroupedByCompany { get; set; }
        public IEnumerable<string> AvailableSourceCodeLicenses { get; set; }
        public IEnumerable<string> AvailableCompanyNames { get; set; }
        public IEnumerable<string> AvailablePrices { get; set; }
        public string SourceCodeLicenseFilter { get; set; }
        public string CompanyNameFilter { get; set; }
        public PriceRange PriceFilter { get; set; }
    }

    public class LeaveACommentModel
    {
        public string DevToolName { get; set; }
        public Guid DevToolId { get; set; }
        public string Text { get; set; }
    }

    public class DevToolSearchCriteria
    {
        public string SourceCodeLicenses { get; set; }
        public string CompanyName { get; set; }
        public PriceRange PriceRange { get; set; }
    }

    public enum PriceRange
    {
        None,
        ZeroToHundred,
        HundredToThousand,
        ThousandToTenThousand
    }
}
