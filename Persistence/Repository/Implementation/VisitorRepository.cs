using Appointr.Persistence.Context;
using Appointr.Persistence.Entities;
using Appointr.Persistence.Repository.Interface;

namespace Appointr.Persistence.Repository.Implementation
{
    public class VisitorRepository : Repository<Visitor>, IVisitorRepository
    {
        public VisitorRepository(AppDbContext context) : base(context)
        {

        }
    }
}
