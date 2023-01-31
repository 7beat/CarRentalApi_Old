using AutoMapper;

namespace CarRentalAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Models.Identity.AppUser, Models.DTO.User>()
                .ReverseMap();
        }
    }
}
