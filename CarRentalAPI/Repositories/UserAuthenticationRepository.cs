using AutoMapper;
using CarRentalAPI.Models.Domain;
using CarRentalAPI.Models.DTO;
using CarRentalAPI.Models.Identity;
using CarRentalAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarRentalAPI.Repositories
{
    internal sealed class UserAuthenticationRepository : IUserAuthenticationRepository
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private AppUser? _user;

        public UserAuthenticationRepository(UserManager<AppUser> userManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistration)
        {
            var user = _mapper.Map<AppUser>(userRegistration);
            var result = await _userManager.CreateAsync(user, userRegistration.Password);
            return result;
        }

        public async Task<bool> ValidateUserAsync(UserLoginDto loginDto)
        {
            _user = await _userManager.FindByNameAsync(loginDto.Username);
            var result = _user != null && await _userManager.CheckPasswordAsync(_user, loginDto.Password);
            return result;
        }

        public async Task<string> CreateTokenAsync()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);       
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtConfig = _configuration.GetSection("Jwt"); //Rename to Jwt
            var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }

    }
}
