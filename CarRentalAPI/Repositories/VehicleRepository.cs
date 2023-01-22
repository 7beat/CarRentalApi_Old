using CarRentalAPI.Data;
using CarRentalAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _appDbContext;

        public VehicleRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Vehicle> AddAsync(Vehicle vehicle)
        {
            var addedVehicle = await _appDbContext.Vehicles.AddAsync(vehicle);
            await _appDbContext.SaveChangesAsync();
            
            //WIP
            return await GetByIdAsync(vehicle.Id);
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _appDbContext.Vehicles
                .Include(x => x.Color)
                .ToListAsync();
        }

        public Task<Vehicle> GetByIdAsync(int id)
        {
            return _appDbContext.Vehicles
                .Include(x => x.Color)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
