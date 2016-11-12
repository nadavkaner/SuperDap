﻿using System.Linq;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context _db = new Context();

        public ActionResult Index()
        {
            var items = _db.Computers.Take(10).OrderByDescending(x => x.NumberOfPurchases).ToList();
            return View(items);
        }

        public ActionResult About()
        {
            ViewBag.Message = "This is an aggregate of items from all around the nation. Use this to compare prices from different stores and more.";

            return View();
        }
    }
}