using CarRentalAPI.Models.Domain;

namespace CarRentalAPI.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle> GetByIdAsync(int id);
        Task AddAsync(Vehicle vehicle);
        Task<Vehicle> UpdateAsync(int id, Vehicle vehicle);
        Task<Vehicle> DeteleAsync(int id);
    }
}
