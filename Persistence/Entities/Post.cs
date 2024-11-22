using Appointr.Enum;
using System.ComponentModel.DataAnnotations;

namespace Appointr.Persistence.Entities
{
    public class Post
    {
        [Key]
        public Guid PostId { get; set; }
        [Required]
        [StringLength(20)]
        public string PostName { get; set; } = string.Empty;
        public Status Status { get; set; } = Status.Active;
    }
}
