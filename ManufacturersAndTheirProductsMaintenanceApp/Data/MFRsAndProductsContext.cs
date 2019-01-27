using ManufacturersAndTheirProductsMaintenanceApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ManufacturersAndTheirProductsMaintenanceApp.Data
{
    public class MFRsAndProductsContext : DbContext
    {
        public MFRsAndProductsContext(DbContextOptions<MFRsAndProductsContext> options) : base(options)
        {
        }

        public DbSet<Manufacturer> Manufacturers {get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
