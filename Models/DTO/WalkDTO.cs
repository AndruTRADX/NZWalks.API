﻿
namespace NZWalks.API.Models.DTO
{
    public class WalkDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        public DifficultyDTO Difficulty { get; set; }
        public RegionDTO Region { get; set; }
    }
}
