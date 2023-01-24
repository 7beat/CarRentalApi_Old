using CarRentalAPI.Data;
using CarRentalAPI.Models.Domain;

namespace CarRentalAPI.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(int id, User user);
        Task<bool> IsEmailUnique(string email);
        Task<bool> IsUsernameUnique(string username);
    }
}
