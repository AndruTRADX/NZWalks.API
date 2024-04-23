using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddWalkDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(3, ErrorMessage = "Name cannot be shorter than 3 characters")]
        [MaxLength(100, ErrorMessage = "Name cannot be larger than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [MinLength(3, ErrorMessage = "Description cannot be shorter than 3 characters")]
        [MaxLength(550, ErrorMessage = "Description cannot be larger than 550 characters")]
        public string Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Length must be a non-negative number")]
        public double LengthInKm { get; set; }

        [Display(Name = "Walk Image URL")]
        [Url(ErrorMessage = "Walk image URL is not valid")]
        public string? WalkImageUrl {  get; set; }

        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }
    }
}
