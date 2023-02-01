using AutoMapper;

namespace CarRentalAPI.Mappings
{
    public class RentalProfile : Profile
    {
        public RentalProfile()
        {
            CreateMap<Models.Domain.Rental, Models.DTO.Rental>()
                .ReverseMap();
        }
    }
}
