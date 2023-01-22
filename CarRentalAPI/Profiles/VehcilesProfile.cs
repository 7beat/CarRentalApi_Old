using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarRentalAPI.Profiles
{
    public class VehcilesProfile : Profile
    {
        public VehcilesProfile()
        {
            CreateMap<Models.Domain.Vehicle, Models.DTO.Vehicle>()
                .ReverseMap();
        }
    }
}
