using CarRentalAPI.Data;
using CarRentalAPI.Repositories.Interfaces;

namespace CarRentalAPI.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext dbContext;
        private IVehicle2Repository _vehicle2Repository;

        public RepositoryManager(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IVehicle2Repository Vehicle
        {
            get
            {
                if (_vehicle2Repository is null)
                    _vehicle2Repository = new Vehicle2Repository(dbContext);
                return _vehicle2Repository;
            }
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
