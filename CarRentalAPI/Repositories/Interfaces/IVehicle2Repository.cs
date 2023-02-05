using CarRentalAPI.Models.Domain;

namespace CarRentalAPI.Repositories.Interfaces
{
    public interface IVehicle2Repository
    {
        Task<Vehicle> GetVehicle(int id, bool trackChanges);
        Task<IEnumerable<Vehicle>> GetAllVehicles(bool trackChanges);
        Task CreateVehicle(Vehicle vehicle);
        Task DeleteVehicle(Vehicle vehicle);
        Task UpdateVehicle(Vehicle vehicle);
    }
}
