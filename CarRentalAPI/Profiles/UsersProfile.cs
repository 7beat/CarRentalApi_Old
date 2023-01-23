using AutoMapper;

namespace CarRentalAPI.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<Models.Domain.User, Models.DTO.User>()
                .ReverseMap();
        }
    }
}
