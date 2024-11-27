using Appointr.Enum;
using System.ComponentModel.DataAnnotations;

namespace Appointr.DTO
{
    public class ActivityDto
    {
        public Guid ActivityId { get; set; }

        [Required]
        public Guid OfficerId { get; set; }

        [Required]
        public ActivityType Type { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        public bool IsActivityDurationValid => EndDateTime > StartDateTime;
    }
}
