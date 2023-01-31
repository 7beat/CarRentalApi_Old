﻿using CarRentalAPI.Data;
using CarRentalAPI.Models.Domain;
using CarRentalAPI.Models.Identity;
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

        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            return await _appDbContext.AppUsers
                .Include(x => x.Vehicles)
                .ThenInclude(x => x.Color)
                .ToListAsync();
        }

        public async Task<AppUser> GetByIdAsync(int id)
        {
            return await _appDbContext.AppUsers
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

        public async Task<AppUser> DeleteAsync(int id)
        {
            var existingUser = await _appDbContext.AppUsers.Include(x => x.Vehicles).FirstOrDefaultAsync(x => x.Id == id);

            if (existingUser is not null)
            {
                existingUser.Vehicles.Clear();
                _appDbContext.AppUsers.Remove(existingUser);
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
