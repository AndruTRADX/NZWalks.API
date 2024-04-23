using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionDTO
    {
        [Required(ErrorMessage = "Name of the region is mandatory")]
        [MaxLength(100, ErrorMessage = "Name cannot be larger than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Code of the region is mandatory")]
        [RegularExpression("^[a-zA-Z]{3}$", ErrorMessage = "Code must be exactly 3 alphabetic characters")]
        public string Code { get; set; } = string.Empty;

        [Url(ErrorMessage = "Region image URL is not valid")]
        public string? RegionImageUrl { get; set; } = string.Empty;
    }
}
