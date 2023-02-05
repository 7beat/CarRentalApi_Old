using CarRentalAPI.Data;
using CarRentalAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Repositories.Interfaces
{
    public class Vehicle2Repository : RepositoryBase<Vehicle>, IVehicle2Repository
    {
        public Vehicle2Repository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        //public async Task<Vehicle> GetVehicle(int id)
        //    => await FindByConditionAsync(e => e.TeacherId.Equals(teacherId) && e.Id.Equals(studentId), trackChanges).Result.SingleOrDefaultAsync();

        public async Task<Vehicle> GetVehicle(int id, bool trackChanges)
            => await FindByConditionAsync(e => e.Id.Equals(id), trackChanges).Result.Include(x => x.Color).SingleOrDefaultAsync();

        public async Task<IEnumerable<Vehicle>> GetAllVehicles(bool trackChanges)
        {
            //var vehicles = await FindAllAsync(trackChanges).Result.Include(x => x.Color);
            return await FindAllAsync(trackChanges).Result.Include(x => x.Color).ToListAsync();
        }

        public async Task CreateVehicle(Vehicle vehicle) => await CreateAsync(vehicle);

        public async Task DeleteVehicle(Vehicle vehicle) => await RemoveAsync(vehicle);

        public async Task UpdateVehicle(Vehicle vehicle) => await UpdateAsync(vehicle);

    }
}
