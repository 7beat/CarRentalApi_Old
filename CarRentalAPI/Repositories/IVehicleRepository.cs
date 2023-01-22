using CarRentalAPI.Models.Domain;

namespace CarRentalAPI.Repositories
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle> GetByIdAsync(int id);
    }
}
