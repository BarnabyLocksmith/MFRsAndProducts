using System;
using ManufacturersAndTheirProductsMaintenanceApp.Data;
using ManufacturersAndTheirProductsMaintenanceApp.Data.Model;
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
        public IActionResult CreateManufacturer(ManufacturerModel newManufacturer)
        {
            Repository.CreateManufacturer(newManufacturer);

            return Redirect($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/home/create");
        }

        [HttpPost("delete/{id}")]
        public IActionResult DeleteManufacturer(int id)
        {
            Repository.DeleteManufacturer(id);
            Response.Redirect(Request.PathBase);
            
            return Redirect($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/home/index");
        }
    }
}
