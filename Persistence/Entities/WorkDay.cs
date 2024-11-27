using Appointr.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Appointr.Persistence.Entities
{
    public class WorkDay
    {
        [Key]
        public Guid WorkDayId { get; set; }

        [Required]
        public Guid OfficerId { get; set; }

        [Required]
        public Days DayOfWeek { get; set; }

        [ForeignKey("PostId")]
        public Officer? Officer { get; set; }
    }
}
