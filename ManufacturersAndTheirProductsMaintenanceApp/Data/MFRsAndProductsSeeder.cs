using ManufacturersAndTheirProductsMaintenanceApp.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ManufacturersAndTheirProductsMaintenanceApp.Data
{
    public class MFRsAndProductsSeeder
    {
        private readonly MFRsAndProductsContext Context;
        private readonly IHostingEnvironment Hosting;

        public MFRsAndProductsSeeder(MFRsAndProductsContext context, IHostingEnvironment hostingEnvironment)
        {
            this.Context = context;
            this.Hosting = hostingEnvironment;
        }

        public void Seed()
        {
            Context.Database.EnsureCreated();

            if (!Context.Manufacturers.Any())
            {
                SetSeedData();
            }
        }

        private void SetSeedData()
        {
            SetManufacturers();
            SetProducts();

            var manufacturer = Context.Manufacturers
                .Where(manufact => manufact.Id == 1)
                .FirstOrDefault();


            if (manufacturer != null)
            {

                var manufacturerItemList = new List<ManufacturerItem>();

                foreach (var product in Context.Products)
                {
                    manufacturerItemList.Add(
                        new ManufacturerItem
                        {
                            Product = product,
                            Manufacturer = manufacturer
                        });
                }

                manufacturer.Products = manufacturerItemList;
            }

            Context.SaveChanges();
        }

        private void SetProducts()
        {
            var userGuid = Startup.UserGuid;

            var filePath = Path.Combine(Hosting.ContentRootPath, "Data/SeedDataFiles/products.json");
            var json = File.ReadAllText(filePath);
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json).ToList();

            foreach (var product in products)
            {
                product.CreatedBy = userGuid;
                product.CreatedDateTime = DateTime.Now;
                product.LastChangedBy = userGuid;
                product.LastChangedDateTime = DateTime.Now;
            }

            Context.Products.AddRange(products);
            Context.SaveChanges();
        }

        private void SetManufacturers()
        {
            var userGuid = Startup.UserGuid;

            var filePath = Path.Combine(Hosting.ContentRootPath, "Data/SeedDataFiles/manufacturers.json");
            var json = File.ReadAllText(filePath);
            var manufacturers = JsonConvert.DeserializeObject<IEnumerable<Manufacturer>>(json).ToList();

            foreach (var manufacturerItem in manufacturers)
            {
                manufacturerItem.CreatedBy = userGuid;
                manufacturerItem.CreatedDateTime = DateTime.Now;
                manufacturerItem.LastChangedBy = userGuid;
                manufacturerItem.LastChangedDateTime = DateTime.Now;
            }

            Context.Manufacturers.AddRange(manufacturers);
            Context.SaveChanges();
        }
    }
}
