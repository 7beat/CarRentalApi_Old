using CarRentalAPI.Models.Domain;

namespace CarRentalAPI.Repositories.Interfaces
{
    public interface IRentalRepository
    {
        Task<IEnumerable<Rental>> GetAllAsync();
        Task<Rental> GetByIdAsync(int id);
        Task AddAsync(Rental rental);
        Task<Rental> UpdateAsync(int id, Rental rental);
        Task<Rental> DeleteAsync(int id);
    }
}
