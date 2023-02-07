﻿using AutoMapper;
using CarRentalAPI.Data;
using CarRentalAPI.Models.Identity;
using CarRentalAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CarRentalAPI.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext dbContext;
        private readonly IVehicle2Repository _vehicle2Repository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRentalRepository _rentalRepository;
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

        //public IVehicle2Repository Vehicle
        //{
        //    get
        //    {
        //        if (_vehicle2Repository is null)
        //            _vehicle2Repository = new Vehicle2Repository(dbContext);
        //        return _vehicle2Repository;
        //    }
        //}
        public IVehicle2Repository Vehicle2 => _vehicle2Repository ?? new Vehicle2Repository(dbContext);

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
