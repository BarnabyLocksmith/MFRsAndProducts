using System;
using System.Collections.Generic;

namespace ManufacturersAndTheirProductsMaintenanceApp.Data.Entities
{
    public class Manufacturer
    {
        public string Name { get; set; }

        public string Logo { get; set; }

        public ICollection<Product> Products { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime LastChangedDateTime { get; set; }

        public Guid LastChangedBy { get; set; }
    }
}
