using System.Linq;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    { 
        private readonly Context _db = new Context();

        public ActionResult Index()
        {
            var companies = _db.Companies.Take(8).OrderByDescending(x => x.TotalRevenue).ToList();
            return View(companies);
        }
    }
}