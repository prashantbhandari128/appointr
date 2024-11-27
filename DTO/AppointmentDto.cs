using System.ComponentModel.DataAnnotations;

namespace Appointr.DTO
{
    public class AppointmentDto
    {
        public Guid AppointmentId { get; set; }

        [Required]
        public string Name { get; set; } = String.Empty;

        [Required]
        public Guid OfficerId { get; set; }

        [Required]
        public Guid VisitorId { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        public bool IsTimeDurationValid => EndTime > StartTime;

        public bool IsDateValid => Date >= DateOnly.FromDateTime(DateTime.Now);
    }
}
