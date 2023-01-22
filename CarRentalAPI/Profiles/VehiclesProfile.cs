using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarRentalAPI.Profiles
{
    public class VehiclesProfile : Profile
    {
        public VehiclesProfile()
        {
            CreateMap<Models.Domain.Vehicle, Models.DTO.Vehicle>()
                .ReverseMap();
        }
    }
}
