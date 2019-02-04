using System;
using System.IO;
using ManufacturersAndTheirProductsMaintenanceApp.Data;
using ManufacturersAndTheirProductsMaintenanceApp.Data.Model;
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
            var manufacturer = Repository.GetManufacturer(id);

            ViewBag.Title = $"Products of {manufacturer.Name}";
            return View(manufacturer);
        }

        [HttpGet("create")]
        public IActionResult CreateProduct()
        {
            var currentMfrId = Convert.ToInt32(this.RouteData.Values["id"]);
            var manufacturerName = Repository.GetManufacturer(currentMfrId).Name;

            ViewBag.Title = $"Create new product for {manufacturerName}";

            return View();
        }

        [HttpPost("create")]
        public IActionResult CreateProduct(string name, IFormFile image)
        {
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

            return View(Repository.GetManufacturer(currentMfrId));
        }

        [HttpPost("update/{productId}")]
        public IActionResult UpdateProduct(int productId, string name, IFormFile image)
        {
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
