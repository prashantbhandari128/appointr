using Appointr.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Appointr.Persistence.Entities
{
    public class Officer
    {
        [Key]
        public Guid OfficerId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Guid PostId { get; set; }

        [Required]
        public TimeSpan WorkStartTime { get; set; }

        [Required]
        public TimeSpan WorkEndTime { get; set; }

        public Status Status { get; set; } = Status.Active;
        
        [ForeignKey("PostId")]
        public Post? Post { get; set; }
    }
}
