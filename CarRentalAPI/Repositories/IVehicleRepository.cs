using CarRentalAPI.Models.Domain;

namespace CarRentalAPI.Repositories
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle> GetByIdAsync(int id);
        Task<Vehicle> AddAsync(Vehicle vehicle);
        Task<Vehicle> UpdateAsync(int id, Vehicle vehicle);
    }
}
