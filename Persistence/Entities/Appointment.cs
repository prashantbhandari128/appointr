using Appointr.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Appointr.Persistence.Entities
{
    public class Appointment
    {
        [Key]
        public Guid AppointmentId { get; set; }
        
        [Required]
        public string Name { get; set;} = String.Empty;

        [Required]
        public Guid OfficerId { get; set; }

        [Required]
        public Guid VisitorId { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set;}

        [Required]
        public DateTime AddedOn { get; set; }

        [Required]
        public DateTime LastUpdatedOn { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; }

        [ForeignKey("OfficerId")]
        public Officer? Officer { get; set; }

        [ForeignKey("VisitorId")]
        public Visitor? Visitor { get; set; }

        [NotMapped]
        public bool IsTimeDurationValid => EndTime > StartTime;
    }
}
