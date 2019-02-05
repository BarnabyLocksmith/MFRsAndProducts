using ManufacturersAndTheirProductsMaintenanceApp.Data.Model;
using System.Collections.Generic;

namespace ManufacturersAndTheirProductsMaintenanceApp.Data
{
    public interface IMFRsAndProductsRepository
    {
        ManufacturerModel GetManufacturer(int id);

        IReadOnlyList<ManufacturerModel> GetManufacturers();

        void CreateManufacturer(string name, byte[] logo);

        void CreateProduct(int mfrId, string name, byte[] image);

        void UpdateManufacturer(int id, string name, byte[] logo);
        
        void UpdateProduct(int mfrId, int productId, string name, byte[] image);

        void DeleteManufacturer(int id);

        void DeleteProduct(int mfrId, int productId);

        bool IsProductExists(int id);
    }
}
