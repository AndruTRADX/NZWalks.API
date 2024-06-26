﻿using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<AddRegionDTO, Region>().ReverseMap();

            CreateMap<Walk, WalkDTO>().ReverseMap();
            CreateMap<AddWalkDTO, Walk>().ReverseMap();

            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();
        }
    }
}
