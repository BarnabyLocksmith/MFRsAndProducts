using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManufacturersAndTheirProductsMaintenanceApp.Data.Entities
{
    public class ManufacturerProduct
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        public Manufacturer Manufacturer { get; set; }
    }
}
