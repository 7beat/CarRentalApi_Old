using CarRentalAPI.Models.DTO;
using CarRentalAPI.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace CarRentalAPI.Repositories.Interfaces
{
    public interface IUserAuthenticationRepository
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto user);
        Task<bool> ValidateUserAsync(UserLoginDto loginDto);
        Task<string> CreateTokenAsync();
        Task<(string token, int userId)> CreateEmailCredentials();
        Task<IdentityResult> ConfirmUserEmail(AppUser user, string token);
    }
}
