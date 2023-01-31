using CarRentalAPI.Data;
using CarRentalAPI.Models.Domain;
using CarRentalAPI.Models.Identity;

namespace CarRentalAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllAsync();
        Task<AppUser> GetByIdAsync(int id);
        Task<User> UpdateAsync(int id, User user);
        Task<User> DeleteAsync(int id);
        Task<bool> IsEmailUnique(string email);
        Task<bool> IsUsernameUnique(string username);
    }
}
