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
    }
}
