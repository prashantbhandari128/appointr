using System.ComponentModel.DataAnnotations;

namespace Appointr.DTO
{
    public class VisitorDto
    {
        public Guid VisitorId { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Mobile { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
