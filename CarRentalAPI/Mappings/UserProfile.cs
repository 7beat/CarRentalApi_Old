﻿using AutoMapper;

namespace CarRentalAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Models.Domain.User, Models.DTO.User>()
                .ReverseMap();
        }
    }
}