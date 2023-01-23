using CarRentalAPI.Models.Domain;

namespace CarRentalAPI.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
    }
}
