using System;
using System.Collections.Generic;
using System.Linq;
using ManufacturersAndTheirProductsMaintenanceApp.Data.Entities;
using ManufacturersAndTheirProductsMaintenanceApp.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace ManufacturersAndTheirProductsMaintenanceApp.Data
{
    public class MFRsAndProductsRepository : IMFRsAndProductsRepository
    {
        private readonly MFRsAndProductsContext Context;

        public MFRsAndProductsRepository(MFRsAndProductsContext Context)
        {
            this.Context = Context;
        }

        public void CreateProduct(int mfrId, ProductModel newProduct)
        {
            Manufacturer manufacturer = GetManufacturerWithIncludes(mfrId);

            manufacturer.Items.Add(new ManufacturerItem()
            {
                Manufacturer = manufacturer,
                Product = new Product
                {
                    Name = newProduct.Name,
                    Image = newProduct.Image,
                    CreatedBy = Startup.UserGuid,
                    CreatedDateTime = DateTime.Now,
                    LastChangedBy = Startup.UserGuid,
                    LastChangedDateTime = DateTime.Now
                }
            });

            Context.SaveChanges();
        }

        public void CreateManufacturer(ManufacturerModel newManufacturer)
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
        }

        public void DeleteManufacturer(int id)
        {
            try
            {
                var deletableManufacturer = Context.Manufacturers.Where(manufacturer => manufacturer.Id == id).Single();

                Context.Manufacturers.Remove(deletableManufacturer);
                Context.SaveChanges();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public void DeleteProduct(int mfrId, int productId)
        {
            var manufacturer = Context.Manufacturers
                .Include(mfr => mfr.Items)
                .ThenInclude(mfrItem => mfrItem.Product)
                .Where(mfr => mfr.Id == mfrId)
                .Single();

            var deletableProduct = manufacturer.Items.Where(x => x.Product.Id == productId).Single();

            manufacturer.Items.Remove(deletableProduct);

            Context.SaveChanges();
        }

        public ManufacturerModel GetManufacturer(int id)
        {
            return Context.Manufacturers.Where(manufacturer => manufacturer.Id == id)
                .Include(mfr => mfr.Items)
                .ThenInclude(mfrItem => mfrItem.Product)
                .Select(manufacturer => new ManufacturerModel
                {
                    Id = manufacturer.Id,
                    Name = manufacturer.Name,
                    Logo = manufacturer.Logo,
                    CreatedBy = manufacturer.CreatedBy,
                    CreatedDateTime = manufacturer.CreatedDateTime,
                    LastChangedBy = manufacturer.LastChangedBy,
                    LastChangedDateTime = manufacturer.LastChangedDateTime,
                    Products = manufacturer.Items.Select(x => x.Product)
                })
                .Single();
        }

        public IReadOnlyList<ManufacturerModel> GetManufacturers()
        {
            var manufacturers = Context.Manufacturers
                .Select(manufacturer => new ManufacturerModel()
                {
                    Id = manufacturer.Id,
                    Name = manufacturer.Name,
                    Logo = manufacturer.Logo,
                    CreatedBy = manufacturer.CreatedBy,
                    CreatedDateTime = manufacturer.CreatedDateTime,
                    LastChangedBy = manufacturer.LastChangedBy,
                    LastChangedDateTime = manufacturer.LastChangedDateTime
                });

            return manufacturers.ToList();
        }
        
        private Manufacturer GetManufacturerWithIncludes(int mfrId)
        {
            return Context.Manufacturers
                .Include(mfr => mfr.Items)
                .ThenInclude(mfrItem => mfrItem.Product)
                .Where(mfr => mfr.Id == mfrId)
                .Single();
        }
    }
}
