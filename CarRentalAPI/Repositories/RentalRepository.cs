using CarRentalAPI.Data;
using CarRentalAPI.Extensions;
using CarRentalAPI.Models.Domain;
using CarRentalAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDbContext _appDbContext;
        public RentalRepository(AppDbContext appContext)
        {
            _appDbContext = appContext;
        }

        public async Task<IEnumerable<Rental>> GetAllAsync()
        {
            return await _appDbContext.Rentals
                .Include(x => x.User)
                .Include(x => x.Vehicle)
                .ThenInclude(x => x.Color)
                .ToListAsync();
        }

        public async Task<Rental> GetByIdAsync(int id)
        {
            return await _appDbContext.Rentals
                .Include(x => x.Vehicle)
                .ThenInclude(x => x.Color)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Rental> AddAsync(Rental rental)
        {
            var addedRental = await _appDbContext.Rentals.AddAsync(rental);
            await _appDbContext.SaveChangesAsync();

            return await GetByIdAsync(rental.Id);
        }

        public async Task<Rental> UpdateAsync(int id, Rental rental)
        {
            var existingRental = await _appDbContext.Rentals.FindAsync(id);

            if (existingRental is null)
            {
                return null;
            }

            existingRental.StartDate = rental.StartDate;
            existingRental.EndDate= rental.EndDate;
            await _appDbContext.SaveChangesAsync();
            return existingRental;
        }
    }
}
