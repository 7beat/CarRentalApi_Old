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
            var query = await FindAllAsync(false);
            var result = await query.Include(x => x.User).Include(x => x.Vehicle).ThenInclude(x => x.Color).ToListAsync();
            return result;
        }

        public async Task<Rental> GetByIdAsync(int id)
        {
            var query = await FindByConditionAsync(x => x.Id.Equals(id), false);
            var result = await query.Include(x => x.Vehicle).ThenInclude(x => x.Color).SingleOrDefaultAsync();
            return result;
        }

        public async Task AddAsync(Rental rental) => await CreateAsync(rental);

        public async Task<Rental> UpdateAsync(int id, Rental rental)
        {
            //var existingRental = await _appDbContext.Rentals.FindAsync(id);

            //if (existingRental is null)
            //{
            //    return null;
            //}

            //existingRental.StartDate = rental.StartDate;
            //existingRental.EndDate= rental.EndDate;
            //await _appDbContext.SaveChangesAsync();
            var existingRental = await FindByConditionAsync(x => x.Id.Equals(id), true);
            var result = await existingRental.SingleOrDefaultAsync();

            result.StartDate = rental.StartDate;
            result.EndDate = rental.EndDate;

            await UpdateAsync(result);

            return result;
        }

        public async Task<Rental> DeleteAsync(int id)
        {
            var existingVehicle = await FindByConditionAsync(x => x.Id.Equals(id), true);
            var result = await existingVehicle.FirstOrDefaultAsync();

            if (result is not null)
            {
                await RemoveAsync(result);

                return result;
            }
            return null;
        }
    }
}
