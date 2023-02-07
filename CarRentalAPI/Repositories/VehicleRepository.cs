using CarRentalAPI.Data;
using CarRentalAPI.Models.Domain;
using CarRentalAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Repositories
{
    public class VehicleRepository : RepositoryBase<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task AddAsync(Vehicle vehicle) => await CreateAsync(vehicle);

        public async Task<Vehicle> DeteleAsync(int id)
        {
            var existingVehicle = await FindByConditionAsync(x => x.Id == id, true);
            var result = await existingVehicle.FirstOrDefaultAsync();

            if (result is not null)
            {
                await RemoveAsync(result);

                return result;
            }
            return null;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            var result = await FindAllAsync(false);
            return await result.Include(x => x.Color).ToListAsync();
        }

        public async Task<Vehicle> GetByIdAsync(int id)
        {
            var query = await FindByConditionAsync(x => x.Id.Equals(id), false);
            var result = await query.Include(x => x.Color).FirstOrDefaultAsync();

            return result;
        }

        public async Task<Vehicle> UpdateAsync(int id, Vehicle vehicle)
        {
            var existingVehicle = await FindByConditionAsync(x => x.Id.Equals(id), true);
            var result = await existingVehicle.FirstOrDefaultAsync();

            if (result is null)
                return null;

            // Modification
            result.ColorId = vehicle.ColorId;
            result.Model = vehicle.Model;

            await UpdateAsync(result);

            return result;
        }
    }
}
