using System;

namespace ManufacturersAndTheirProductsMaintenanceApp.Data.Entities
{
    public class Product
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public int ManufacturerId { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime LastChangedDateTime { get; set; }

        public Guid LastChangedBy { get; set; }
    }
}
