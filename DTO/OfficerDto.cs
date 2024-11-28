using Appointr.Enum;
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
        public TimeOnly WorkStartTime { get; set; }

        [Required]
        public TimeOnly WorkEndTime { get; set; }

        public List<Days>? Days { get; set; }

        public bool IsWorkTimeValid => WorkEndTime > WorkStartTime;
    }
}
