using System.ComponentModel.DataAnnotations;

namespace Appointr.DTO
{
    public class OfficerDto
    {
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

        public bool IsWorkTimeValid => WorkEndTime > WorkStartTime;
    }
}
