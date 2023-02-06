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

            CreateMap<Models.DTO.UpdateVehicleRequest, Models.Domain.Vehicle>()
                .ForMember(dest => dest.Color, opt => opt.Ignore())
                                //.ForMember(opt =>)
                .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src =>
                src.Color))

                .ReverseMap();
        }
    }
}
