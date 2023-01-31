using CarRentalAPI.Data;
using CarRentalAPI.Models.Domain;
using CarRentalAPI.Models.Identity;

namespace CarRentalAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllAsync();
        Task<AppUser> GetByIdAsync(int id);
        Task<AppUser> UpdateAsync(int id, UserUpdateDto user);
        Task<AppUser> DeleteAsync(int id);
    }
}
