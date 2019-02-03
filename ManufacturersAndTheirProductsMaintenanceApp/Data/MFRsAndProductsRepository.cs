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
        
        public void UpdateManufacturer(ManufacturerModel updatedManufacturerData)
        {
            var updatableManufacturer = GetManufacturerWithIncludes(updatedManufacturerData.Id);
            
            updatableManufacturer.Name = updatedManufacturerData.Name;
            updatableManufacturer.Logo = updatedManufacturerData.Logo;
            updatableManufacturer.LastChangedBy = Startup.UserGuid;
            updatableManufacturer.LastChangedDateTime = DateTime.Now;

            Context.Manufacturers.Update(updatableManufacturer);
            Context.Entry(updatableManufacturer).State = EntityState.Modified;

            Context.SaveChanges();
        }

        public void UpdateProduct(int mfrId, ProductModel updatedProductData)
        {
            var manufacturer = GetManufacturerWithIncludes(mfrId);

            var manufacturerItem = manufacturer.Items.Where(mfrItem => mfrItem.Product.Id == updatedProductData.Id).Single();
            var oldProduct = manufacturerItem.Product;

            Context.Products.Remove(oldProduct);
            Context.Entry(manufacturerItem.Product).State = EntityState.Deleted;
            Context.SaveChanges();

            var product = new Product
            {
                Name = updatedProductData.Name,
                Image = updatedProductData.Image,
                CreatedBy = oldProduct.CreatedBy,
                CreatedDateTime = oldProduct.CreatedDateTime,
                LastChangedBy = Startup.UserGuid,
                LastChangedDateTime = DateTime.Now
            };


            manufacturer.Items.Remove(manufacturerItem);
            Context.Entry(manufacturerItem).State = EntityState.Deleted;

            manufacturer.Items.Add(new ManufacturerItem
            {
                Manufacturer = manufacturer,
                Product = product
            });
            
            Context.SaveChanges();
        }

        public void DeleteManufacturer(int id)
        {
            try
            {
                var deletableManufacturer = GetManufacturerWithIncludes(id);

                var deletableManufacturerItems = deletableManufacturer.Items.ToList();

                var deleatableProducts = deletableManufacturer.Items.Select(x => x.Product);

                Context.Products.RemoveRange(deleatableProducts);

                deletableManufacturer.Items.Clear();

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
            var manufacturer = GetManufacturerWithIncludes(mfrId);

            var deletableProduct = manufacturer.Items.Where(x => x.Product.Id == productId).Single();

            manufacturer.Items.Remove(deletableProduct);

            Context.Products.Remove(deletableProduct.Product);
            Context.Entry(deletableProduct).State = EntityState.Deleted;

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
