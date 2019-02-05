using System;
using System.IO;
using ManufacturersAndTheirProductsMaintenanceApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

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
            try
            {
                var manufacturer = Repository.GetManufacturer(id);

                if (manufacturer == null)
                {
                    return NotFound();
                }

                ViewBag.Title = $"Products of {manufacturer.Name}";
                return View(manufacturer);
            }
            catch (Exception e)
            {
                return BadRequest($"Failed to get manufacturer with the following id: {id}. {e}");
            }
        }

        [HttpGet("create")]
        public IActionResult CreateProduct()
        {
            var currentMfrId = Convert.ToInt32(this.RouteData.Values["id"]);
            var manufacturer = Repository.GetManufacturer(currentMfrId);

            if (manufacturer == null)
            {
                return NotFound();
            }

            ViewBag.Title = $"Create new product for {manufacturer.Name}";

            return View();
        }

        [HttpPost("create")]
        public IActionResult CreateProduct(string name, IFormFile image)
        {
            // If product creation is allowed without image, then null check for image is not required.
            if (name == null || image == null)
            {
                return BadRequest($"Failed to create product, parameters are missing.");
            }

            var currentMfrId = Convert.ToInt32(this.RouteData.Values["id"]);
            ViewBag.Title = "Create new product";

            var logoByteArray = CreateResizedImageByteArray(image);

            Repository.CreateProduct(currentMfrId, name, logoByteArray);

            return Redirect($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/manufacturer/{currentMfrId}/create");
        }

        [HttpGet("update")]
        public IActionResult UpdateProduct()
        {
            var currentMfrId = Convert.ToInt32(this.RouteData.Values["id"]);

            ViewBag.Title = $"Update product page";

            var currentManufacturer = Repository.GetManufacturer(currentMfrId);

            if (currentManufacturer == null)
            {
                return NotFound();
            }

            return View(currentManufacturer);
        }

        [HttpPost("update/{productId}")]
        public IActionResult UpdateProduct(int productId, string name, IFormFile image)
        {
            if (!Repository.IsProductExists(productId))
            {
                return NotFound();
            }

            if (name == null)
            {
                return BadRequest();
            }

            var currentMfrId = Convert.ToInt32(this.RouteData.Values["id"]);
            byte[] imageByteArray = Array.Empty<byte>();
            
            ViewBag.Title = $"Update product page";

            if (image != null)
            {
                imageByteArray = CreateResizedImageByteArray(image);
            }

            Repository.UpdateProduct(currentMfrId, productId, name, imageByteArray);

            return Redirect($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/manufacturer/{currentMfrId}/update");
        }

        [HttpPost("delete/{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            if (!Repository.IsProductExists(productId))
            {
                return NotFound();
            }

            var currentMfrId = Convert.ToInt32(this.RouteData.Values["id"]);
            Repository.DeleteProduct(currentMfrId, productId);

            return Redirect($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/manufacturer/{currentMfrId}");
        }


        private static byte[] CreateResizedImageByteArray(IFormFile image)
        {
            int newSize = 256;
            byte[] imageBytes = FileToByteArray(image);

            using (var outputStream = new MemoryStream())
            {
                using (var tempImage = Image.Load(imageBytes))
                {
                    double newImageHeight = newSize;
                    double newImageWidth = newSize;

                    if (tempImage.Width >= tempImage.Height)
                    {
                        double resolutionRate = (double)tempImage.Width / tempImage.Height;
                        newImageHeight = newSize / resolutionRate;
                    }
                    else
                    {
                        double resolutionRate = (double)tempImage.Height / tempImage.Width;
                        newImageWidth = newSize / resolutionRate;
                    }
                    tempImage.Mutate(ctx => ctx.Resize(Convert.ToInt32(newImageWidth), Convert.ToInt32(newImageHeight)));
                    tempImage.SaveAsPng(outputStream);
                }

                imageBytes = outputStream.ToArray();
            }

            return imageBytes;
        }

        private static byte[] FileToByteArray(IFormFile image)
        {
            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
