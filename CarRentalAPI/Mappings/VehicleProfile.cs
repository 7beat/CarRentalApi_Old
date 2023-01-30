using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarRentalAPI.Profiles
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<Models.Domain.Vehicle, Models.DTO.Vehicle>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src =>
                src.Color.Name))
                .ReverseMap();
        }
    }
}
