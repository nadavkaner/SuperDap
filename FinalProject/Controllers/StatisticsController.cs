using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly Context _db = new Context();

        //
        // GET: /Statistics/
        public ActionResult Index()
        {
            if (User == null || !User.IsInRole("Admin"))
            {
                return Redirect("/Home/Index");
            }

            var companiesRevenue = _db.Companies.Select(x => new
            {
                x.Name,
                x.TotalRevenue,
                x.RevenuePerYears
            });
            
            return View(new StatisticsModel
            {
                CompanyRevenues = companiesRevenue.Select(x => new CompanyRevenue
                {
                    Company = x.Name,
                    Revenue = x.TotalRevenue,
                    RevenueForYear = x.RevenuePerYears
                })
            });
        }
    }
}
