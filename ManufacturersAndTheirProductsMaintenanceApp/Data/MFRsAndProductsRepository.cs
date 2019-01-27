namespace ManufacturersAndTheirProductsMaintenanceApp.Data
{
    public class MFRsAndProductsRepository : IMFRsAndProductsRepository
    {
        private readonly MFRsAndProductsContext Context;

        public MFRsAndProductsRepository(MFRsAndProductsContext Context)
        {
            this.Context = Context;
        }
    }
}
