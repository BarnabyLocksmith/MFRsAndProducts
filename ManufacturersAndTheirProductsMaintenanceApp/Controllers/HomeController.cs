using System;
using ManufacturersAndTheirProductsMaintenanceApp.Data;
using ManufacturersAndTheirProductsMaintenanceApp.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ManufacturersAndTheirProductsMaintenanceApp.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {
        private readonly MFRsAndProductsContext Context;
        private readonly IMFRsAndProductsRepository Repository;

        public HomeController(MFRsAndProductsContext context, IMFRsAndProductsRepository repository)
        {
            this.Context = context;
            this.Repository = repository;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            ViewBag.Title = "List of manufacturers";
            var manufacturers = Repository.GetManufacturers();
            return View(manufacturers);
        }

        [HttpGet("create")]
        public IActionResult CreateManufacturer()
        {
            ViewBag.Title = "Create new manufacturer";
            return View();
        }

        [HttpPost("create")]
        public IActionResult CreateManufacturer(Manufacturer newManufacturer)
        {
            Context.Manufacturers.Add(new Manufacturer()
            {
                CreatedBy = Startup.UserGuid,
                CreatedDateTime = DateTime.Now,
                LastChangedBy = Startup.UserGuid,
                LastChangedDateTime = DateTime.Now,
                Logo = newManufacturer.Logo,
                Name = newManufacturer.Name
            });

            Context.SaveChanges();

            ViewBag.Title = "Manufacturer has been added!";
            return View("Result");
        }

        [HttpPost("delete/{id}")]
        public IActionResult DeleteManufacturer(int id)
        {
            Repository.DeleteManufacturer(id);
            Response.Redirect(Request.PathBase);
            
            ViewBag.Title = "Manufacturer has been deleted!";
            return View("Result");
        }
    }
}
