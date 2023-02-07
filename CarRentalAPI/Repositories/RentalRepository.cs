using CarRentalAPI.Data;
using CarRentalAPI.Extensions;
using CarRentalAPI.Models.Domain;
using CarRentalAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Repositories
{
    public class RentalRepository : RepositoryBase<Rental>, IRentalRepository
    {
        private readonly AppDbContext _appDbContext;
        public RentalRepository(AppDbContext appContext) : base(appContext)
        {
            //_appDbContext = appContext;
        }

        public async Task<IEnumerable<Rental>> GetAllAsync()
        {
            //return await _appDbContext.Rentals
            //    .Include(x => x.User)
            //    .Include(x => x.Vehicle)
            //    .ThenInclude(x => x.Color)
            //    .ToListAsync();
            var query = await FindAllAsync(false);
            var result = await query.Include(x => x.User).Include(x => x.Vehicle).ThenInclude(x => x.Color).ToListAsync();
            return result;
        }

        public async Task<Rental> GetByIdAsync(int id)
        {
            //return await _appDbContext.Rentals
            //    .Include(x => x.Vehicle)
            //    .ThenInclude(x => x.Color)
            //    .FirstOrDefaultAsync(x => x.Id == id);
            var query = await FindByConditionAsync(x => x.Id.Equals(id), false);
            var result = await query.Include(x => x.Vehicle).ThenInclude(x => x.Color).SingleOrDefaultAsync();
            return result;
        }

        public async Task AddAsync(Rental rental) => await CreateAsync(rental);

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

        public async Task<Rental> DeleteAsync(int id)
        {
            var existingRental = await _appDbContext.Rentals.FindAsync(id);

            if (existingRental is not null)
            {
                _appDbContext.Rentals.Remove(existingRental);
                await _appDbContext.SaveChangesAsync();

                return existingRental;
            }
            return null;
        }
    }
}
