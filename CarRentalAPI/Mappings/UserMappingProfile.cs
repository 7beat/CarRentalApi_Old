using AutoMapper;
using CarRentalAPI.Models.DTO;
using CarRentalAPI.Models.Identity;

namespace CarRentalAPI.Profiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() 
        {
            CreateMap<UserRegistrationDto, AppUser>()
                .ReverseMap();
        }
    }
}
