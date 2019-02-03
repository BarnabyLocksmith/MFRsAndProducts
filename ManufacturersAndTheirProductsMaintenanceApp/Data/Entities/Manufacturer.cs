using System;
using System.Collections.Generic;

namespace ManufacturersAndTheirProductsMaintenanceApp.Data.Entities
{
    public class Manufacturer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Logo { get; set; }

        public ICollection<ManufacturerItem> Items { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime LastChangedDateTime { get; set; }

        public Guid LastChangedBy { get; set; }
    }
}
