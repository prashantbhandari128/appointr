using Appointr.Enum;
using System.ComponentModel.DataAnnotations;

namespace Appointr.DTO
{
    public class PostDto
    {
        public Guid PostId { get; set; }
        [Required]
        [StringLength(20)]
        public string PostName { get; set; } = string.Empty;
        public Status Status { get; set; }
    }
}
