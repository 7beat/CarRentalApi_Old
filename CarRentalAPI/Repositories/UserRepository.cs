using CarRentalAPI.Data;
using CarRentalAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _appDbContext.Users
                .Include(x => x.Vehicles)
                .ThenInclude(x => x.Color)
                .ToListAsync();
        }
    }
}
