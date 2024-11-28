using Appointr.Enum;
using Appointr.Persistence.Entities;

namespace Appointr.ViewModel
{
    public class ActivityViewModel
    {
        public ActivityType Type { get; set; }

        public ActivityStatus Status { get; set; }

        public Guid Officer { get; set; }

        public DateTime From { get; set; } = DateTime.Now;

        public DateTime To { get; set; } = DateTime.Now;

        public List<Activity?> Activities {  get; set; }
    }
}
