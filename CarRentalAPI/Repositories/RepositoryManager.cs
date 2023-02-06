using CarRentalAPI.Data;
using CarRentalAPI.Repositories.Interfaces;

namespace CarRentalAPI.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext dbContext;
        private readonly IVehicle2Repository _vehicle2Repository;
        private readonly IVehicleRepository _vehicleRepository;

        public RepositoryManager(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //public IVehicle2Repository Vehicle
        //{
        //    get
        //    {
        //        if (_vehicle2Repository is null)
        //            _vehicle2Repository = new Vehicle2Repository(dbContext);
        //        return _vehicle2Repository;
        //    }
        //}
        public IVehicle2Repository Vehicle2 => _vehicle2Repository ?? new Vehicle2Repository(dbContext);

        public IVehicleRepository Vehicles => _vehicleRepository ?? new VehicleRepository(dbContext);

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
