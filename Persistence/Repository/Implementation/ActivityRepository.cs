using Appointr.Persistence.Context;
using Appointr.Persistence.Entities;
using Appointr.Persistence.Repository.Interface;

namespace Appointr.Persistence.Repository.Implementation
{
    public class ActivityRepository : Repository<Activity>, IActivityRepository
    {
        public ActivityRepository(AppDbContext context) : base(context)
        {

        }
    }
}
