using Appointr.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Appointr.Persistence.Entities
{
    public class Activity
    {
        [Key]
        public Guid ActivityId { get; set; }

        [Required]
        public Guid OfficerId { get; set; }

        [Required]
        public ActivityType Type { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        public ActivityStatus Status { get; set; }

        [ForeignKey("OfficerId")]
        public Officer? Officer { get; set; }
    }
}
