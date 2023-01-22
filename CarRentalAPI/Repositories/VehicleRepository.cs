using CarRentalAPI.Data;
using CarRentalAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext appDbContext;

        public VehicleRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await appDbContext.Vehicles
                .Include(x => x.Color)
                .ToListAsync();
        }

        public Task<Vehicle> GetByIdAsync(int id)
        {
            return appDbContext.Vehicles
                .Include(x => x.Color)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
