using Appointr.Persistence.Context;
using Appointr.Persistence.Entities;
using Appointr.Persistence.Repository.Interface;

namespace Appointr.Persistence.Repository.Implementation
{
    public class WorkDayRepository : Repository<WorkDay>, IWorkDayRepository
    {
        public WorkDayRepository(AppDbContext context) : base(context)
        {

        }
    }
}
