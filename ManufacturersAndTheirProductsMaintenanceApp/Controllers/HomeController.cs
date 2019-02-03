using System;
using System.IO;
using ManufacturersAndTheirProductsMaintenanceApp.Data;
using ManufacturersAndTheirProductsMaintenanceApp.Data.Model;
using Microsoft.AspNetCore.Http;
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
        public IActionResult CreateManufacturer(string name, IFormFile logo)
        {
            byte[] logoBytes = fileToByteArray(logo);

            Repository.CreateManufacturer(name, logoBytes);

            return Redirect($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/home/create");
        }

        [HttpGet("update/{manufacturerId}")]
        public IActionResult UpdateManufacturer(int manufacturerId)
        {
            ViewBag.Title = $"Update manufacturer page";

            return View(Repository.GetManufacturer(manufacturerId));
        }

        [HttpPost("update/{manufacturerId}")]
        public IActionResult UpdateManufacturer(int manufacturerId, string name, IFormFile logo)
        {
            ViewBag.Title = $"Update manufacturer page";
            
            byte[] logoByteArray = Array.Empty<byte>();

            if (logo != null)
            {
                logoByteArray = fileToByteArray(logo);
            }
            
            Repository.UpdateManufacturer(manufacturerId, name, logoByteArray);

            return Redirect($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/home/index");
        }

        [HttpPost("delete/{id}")]
        public IActionResult DeleteManufacturer(int id)
        {
            Repository.DeleteManufacturer(id);
            Response.Redirect(Request.PathBase);
            
            return Redirect($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/home/index");
        }
        
        private static byte[] fileToByteArray(IFormFile logo)
        {
            using (var ms = new MemoryStream())
            {
                logo.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
