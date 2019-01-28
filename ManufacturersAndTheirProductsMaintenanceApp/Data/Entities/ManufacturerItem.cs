namespace ManufacturersAndTheirProductsMaintenanceApp.Data.Entities
{
    public class ManufacturerItem
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        public Manufacturer Manufacturer { get; set; }
    }
}
