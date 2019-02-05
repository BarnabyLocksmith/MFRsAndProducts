using System;
using System.IO;
using ManufacturersAndTheirProductsMaintenanceApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

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

            if (manufacturers == null)
            {
                return NotFound();
            }

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
            // If product creation is allowed without image, then null check for image is not required.
            if (name == null || logo == null)
            {
                return BadRequest($"Failed to create manufacturer, parameters are missing.");
            }

            byte[] logoBytes = CreateResizedLogoByteArray(logo);
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
                logoByteArray = CreateResizedLogoByteArray(logo);
            }

            var currentManufacturer = Repository.GetManufacturer(manufacturerId);

            if (currentManufacturer == null)
            {
                return NotFound();
            }

            Repository.UpdateManufacturer(manufacturerId, name, logoByteArray);

            return Redirect($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/home/index");
        }

        [HttpPost("delete/{id}")]
        public IActionResult DeleteManufacturer(int id)
        {
            var manufacturer = Repository.GetManufacturer(id);

            if (manufacturer == null)
            {
                return NotFound();
            }

            Repository.DeleteManufacturer(id);
            Response.Redirect(Request.PathBase);
            
            return Redirect($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/home/index");
        }

        private static byte[] CreateResizedLogoByteArray(IFormFile logo)
        {
            int newSize = 256;
            byte[] logoBytes = FileToByteArray(logo);

            using (var outputStream = new MemoryStream())
            {
                using (var image = Image.Load(logoBytes))
                {
                    double newImageHeight = newSize;
                    double newImageWidth = newSize;

                    if (image.Width >= image.Height)
                    {
                        double resolutionRate = (double) image.Width / image.Height;
                        newImageHeight = newSize / resolutionRate;
                    }
                    else
                    {
                        double resolutionRate = (double) image.Height / image.Width;
                        newImageWidth = newSize / resolutionRate;
                    }
                    image.Mutate(ctx => ctx.Resize(Convert.ToInt32(newImageWidth), Convert.ToInt32(newImageHeight)));
                    image.SaveAsPng(outputStream);
                }

                logoBytes = outputStream.ToArray();
            }

            return logoBytes;
        }

        private static byte[] FileToByteArray(IFormFile logo)
        {
            using (var ms = new MemoryStream())
            {
                logo.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
