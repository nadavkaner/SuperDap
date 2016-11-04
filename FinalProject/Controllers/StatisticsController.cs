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

            var processorTypes = _db.Computers.GroupBy(x => x.ProcessorType).Select(x => new
            {
                ProcessorType = x.Key,
                NumberOfComputers = x.Count()
            }).ToList();
            var graphicCards = _db.Computers.GroupBy(x => x.GraphicsCardManufacturer).Select(x => new
            {
                GraphicsCard = x.Key,
                NumberOfComputers = x.Count()
            }).ToList();
            
            return View(new StatisticsModel()
            {
                ProcessorTypeComputersStatistics = processorTypes.Select(x => new ComputerProcessorTypeStatistics()
                {
                    ProcessorType = ProcessorTypeToText(x.ProcessorType),
                    NumberOfComputers = x.NumberOfComputers
                }),
                GraphicsCardComputersStatistics = graphicCards.Select(x => new ComputerGraphicsCardStatistics()
                {
                    GraphicsCard = GraphicsCardToText(x.GraphicsCard),
                    NumberOfComputers = x.NumberOfComputers
                })
            });
        }

        private string GraphicsCardToText(GraphicsCardManufacturer graphicsCard)
        {
            switch (graphicsCard)
            {
                case GraphicsCardManufacturer.Intel:
                    return "Intel";
                case GraphicsCardManufacturer.Nvidia:
                    return "Nvidia";
                case GraphicsCardManufacturer.Amd:
                    return "AMD";
                default:
                    throw new ArgumentOutOfRangeException("graphicsCard");
            }
        }

        private string ProcessorTypeToText(ProcessorType key)
        {
            switch (key)
            {
                case ProcessorType.i7:
                    return "i7";
                case ProcessorType.i5:
                    return "i5";
                case ProcessorType.i3:
                    return "i3";
                default:
                    throw new ArgumentOutOfRangeException("key");
            }
        }
    }
}