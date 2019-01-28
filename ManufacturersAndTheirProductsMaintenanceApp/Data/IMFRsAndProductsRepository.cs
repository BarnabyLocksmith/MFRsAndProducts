using ManufacturersAndTheirProductsMaintenanceApp.Data.Model;
using System.Collections.Generic;

namespace ManufacturersAndTheirProductsMaintenanceApp.Data
{
    public interface IMFRsAndProductsRepository
    {
        IReadOnlyList<ManufacturerModel> GetManufacturers();
    }
}
