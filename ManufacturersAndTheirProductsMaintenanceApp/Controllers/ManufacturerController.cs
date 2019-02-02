using System;
using ManufacturersAndTheirProductsMaintenanceApp.Data;
using ManufacturersAndTheirProductsMaintenanceApp.Data.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ManufacturersAndTheirProductsMaintenanceApp.Controllers
{
    [Route("manufacturer/{id}")]
    public class ManufacturerController : Controller
    {
        private readonly MFRsAndProductsContext Context;
        private readonly IMFRsAndProductsRepository Repository;

        public ManufacturerController(MFRsAndProductsContext context, IMFRsAndProductsRepository repository)
        {
            this.Context = context;
            this.Repository = repository;
        }

        public IActionResult Index(int id)
        {
            var manufacturer = Repository.GetManufacturer(id);

            ViewBag.Title = $"Info of {manufacturer.Name}";
            return View(manufacturer);
        }

        [HttpGet("create")]
        public IActionResult CreateProduct()
        {
            ViewBag.Title = $"Create new product";

            return View();
        }

        [HttpPost("create")]
        public IActionResult CreateProduct(ProductModel newProduct)
        {
            var currentMfrId = Convert.ToInt32(this.RouteData.Values["id"]);
            ViewBag.Title = $"Create new product";

            Repository.CreateProduct(currentMfrId, newProduct);

            return Redirect($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/manufacturer/{currentMfrId}/create");
        }

        [HttpPost("delete/{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            var currentMfrId = Convert.ToInt32(this.RouteData.Values["id"]);
            Repository.DeleteProduct(currentMfrId, productId);

            return Redirect($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/manufacturer/{currentMfrId}");
        }
    }
}
