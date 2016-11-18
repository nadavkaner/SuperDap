using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class SearchController : Controller
    {
        private readonly Context _db = new Context();

        // GET: /SearchResults/
        public ActionResult Index(string searchRequest)
        {
            if (string.IsNullOrEmpty(searchRequest))
            {
                return View(new SearchResponseModel());
            }

            var companies = _db.Companies.Where(x => x.Name.Contains(searchRequest)).ToList();
            var devTools = _db.DevelopmentTools.Where(x => x.Name.Contains(searchRequest)).ToList();

            IList<AnswerModel> result = companies.Select(company => new AnswerModel()
            {
                Id = company.CompanyId,
                Type = AnswerModelType.Company,
                Name = company.Name
            }).ToList();

            foreach (var developmentTool in devTools)
            {
                result.Add(new AnswerModel()
                {
                    Id = developmentTool.Id,
                    Type = AnswerModelType.DevTools,
                    Name = developmentTool.Name
                });
            }

            return View(new SearchResponseModel() { Data = result });
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

    public class SearchResponseModel
    {
        public IEnumerable<AnswerModel> Data { get; set; }

        public SearchResponseModel()
        {
            Data = new List<AnswerModel>();
        }
    }

    public class AnswerModel
    {
        public Guid Id { get; set; }
        public AnswerModelType Type { get; set; }
        public string Name { get; set; }
    }

    public enum AnswerModelType
    {
        Company = 1,
        DevTools = 2
    }
}
