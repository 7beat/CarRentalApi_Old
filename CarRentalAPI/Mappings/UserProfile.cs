using AutoMapper;
using CarRentalAPI.Models.DTO;
using CarRentalAPI.Models.Identity;

namespace CarRentalAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Models.Identity.AppUser, Models.DTO.User>()
                .ReverseMap();

            CreateMap<UserRegistrationDto, AppUser>()
                .ReverseMap();
        }
    }
}
