using ManufacturersAndTheirProductsMaintenanceApp.Data;
using ManufacturersAndTheirProductsMaintenanceApp.Data.Entities;
using ManufacturersAndTheirProductsMaintenanceApp.Data.Model;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ManufacturersAndTheirProductsMaintenanceApp.Controllers
{
    [Route("home/create/")]
    public class ManufacturerController : Controller
    {
        private readonly MFRsAndProductsContext Context;
        private readonly IMFRsAndProductsRepository Repository;

        public ManufacturerController(MFRsAndProductsContext context, IMFRsAndProductsRepository repository)
        {
            this.Context = context;
            this.Repository = repository;
        }

        [HttpGet]
        public IActionResult CreateManufacturer()
        {
            ViewBag.Title = "Create new manufacturer";
            return View();
        }

        [HttpPost()]
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
            return View("manufacturerAdded");
        }
    }
}
