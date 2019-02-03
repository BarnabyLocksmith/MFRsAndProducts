using ManufacturersAndTheirProductsMaintenanceApp.Data.Model;
using System.Collections.Generic;

namespace ManufacturersAndTheirProductsMaintenanceApp.Data
{
    public interface IMFRsAndProductsRepository
    {
        ManufacturerModel GetManufacturer(int id);

        IReadOnlyList<ManufacturerModel> GetManufacturers();

        void CreateManufacturer(ManufacturerModel newManufacturer);

        void CreateProduct(int mfrId, ProductModel newProduct);

        void UpdateManufacturer(ManufacturerModel updatedManufacturerData);

        void UpdateProduct(int mfrId, ProductModel updatedProductData);

        void DeleteManufacturer(int id);

        void DeleteProduct(int mfrId, int productId);
    }
}
