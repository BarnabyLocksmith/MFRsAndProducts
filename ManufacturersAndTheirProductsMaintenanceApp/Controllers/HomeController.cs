using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManufacturersAndTheirProductsMaintenanceApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace ManufacturersAndTheirProductsMaintenanceApp.Controllers
{
    [Route("home/index/")]
    public class HomeController : Controller
    {
        private readonly IMFRsAndProductsRepository Repository;

        public HomeController(IMFRsAndProductsRepository repository)
        {
            this.Repository = repository;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "List of manufacturers";
            var manufacturers = Repository.GetManufacturers();
            return View(manufacturers);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
