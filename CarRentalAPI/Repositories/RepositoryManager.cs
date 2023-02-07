using AutoMapper;
using CarRentalAPI.Data;
using CarRentalAPI.Models.Identity;
using CarRentalAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CarRentalAPI.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext dbContext;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IUserRepository _userRepository;
        private IUserAuthenticationRepository _userAuthenticationRepository;
        private UserManager<AppUser> _userManager;
        private IConfiguration _configuration;
        private IMapper mapper;

        public RepositoryManager(AppDbContext dbContext, UserManager<AppUser> userManager, IConfiguration configuration, IMapper mapper)
        {
            this.dbContext = dbContext;
            this._userManager = userManager;
            _configuration = configuration;
            this.mapper = mapper;
        }

        public IUserRepository Users => _userRepository ?? new UserRepository(_userManager);
        public IVehicleRepository Vehicles => _vehicleRepository ?? new VehicleRepository(dbContext);
        public IRentalRepository Rentals => _rentalRepository ?? new RentalRepository(dbContext);


        public IUserAuthenticationRepository UserAuthentication
        {
            get
            {
                if (_userAuthenticationRepository is null)
                    _userAuthenticationRepository = new UserAuthenticationRepository(_userManager, _configuration, mapper);
                return _userAuthenticationRepository;
            }
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
