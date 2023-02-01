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
            if (!ValidateRentalDate(rental))
                return null;

            var addedRental = await _appDbContext.Rentals.AddAsync(rental);
            await _appDbContext.SaveChangesAsync();

            return await GetByIdAsync(rental.Id);
        }

        private bool ValidateRentalDate(Rental newRental)
        {
            foreach (var item in _appDbContext.Rentals.Where(x => x.Vehicle.Id == newRental.VehicleId)) //daje wszystkie rentale danego auta
            {
                //Czy start nowego rentala znajduje się pomiędzy Startem i endem jakiegoś innego? jeśli nie to true
                if (!newRental.StartDate.IsInRange(item.StartDate, item.EndDate))
                {
                    //Nie koliduje
                    return true;
                }
            }

            //Koliduje z czymś
            return false;
        }
    }
}
