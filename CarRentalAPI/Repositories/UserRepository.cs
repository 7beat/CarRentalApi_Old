using CarRentalAPI.Data;
using CarRentalAPI.Models.Domain;
using CarRentalAPI.Repositories.Interfaces;
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

        public async Task<User> AddAsync(User user)
        {
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            //var test = _appDbContext;
            return await GetByIdAsync(user.Id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _appDbContext.Users
                .Include(x => x.Vehicles)
                .ThenInclude(x => x.Color)
                .ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _appDbContext.Users
                .Include(x => x.Vehicles)
                .ThenInclude(x => x.Color)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> UpdateAsync(int id, User user)
        {
            var existingUser = await _appDbContext.Users.FindAsync(id);

            if (existingUser is null)
                return null;

            existingUser.Username = user.Username;
            existingUser.FirstName= user.FirstName;
            existingUser.LastName= user.LastName;
            existingUser.BirthDay= user.BirthDay;

            await _appDbContext.SaveChangesAsync();
            return existingUser;
        }

        public async Task<User> DeleteAsync(int id)
        {
            var existingUser = await _appDbContext.Users.FindAsync(id);

            if (existingUser is not null)
            {
                _appDbContext.Users.Remove(existingUser);
                await _appDbContext.SaveChangesAsync();

                return existingUser;
            }

            return null;
        }

        //Testing
        public async Task<bool> IsEmailUnique(string email)
        {
            return !await _appDbContext.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> IsUsernameUnique(string username)
        {
            return !await _appDbContext.Users.AnyAsync(x => x.Username == username);
        }
    }
}
