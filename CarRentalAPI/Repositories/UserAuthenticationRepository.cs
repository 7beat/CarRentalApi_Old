using AutoMapper;
using CarRentalAPI.Models.Domain;
using CarRentalAPI.Models.DTO;
using CarRentalAPI.Models.Identity;
using CarRentalAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CarRentalAPI.Repositories
{
    internal sealed class UserAuthenticationRepository : IUserAuthenticationRepository
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserAuthenticationRepository(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistration)
        {
            var user = _mapper.Map<AppUser>(userRegistration);
            var result = await _userManager.CreateAsync(user, userRegistration.Password);
            return result;
        }

        public async Task<bool> ValidateUserAsync(UserLoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            var result = user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password);
            return result;
        }

        public Task<string> CreateTokenAsync()
        {
            throw new NotImplementedException();
        }

        //public Task<IdentityResult> RegisterUserAsync(AppUser userRegistration)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
