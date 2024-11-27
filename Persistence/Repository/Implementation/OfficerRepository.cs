using Appointr.Persistence.Context;
using Appointr.Persistence.Entities;
using Appointr.Persistence.Repository.Interface;

namespace Appointr.Persistence.Repository.Implementation
{
    public class OfficerRepository : Repository<Officer>, IOfficerRepository
    {
        public OfficerRepository(AppDbContext context) : base(context)
        {

        }
    }
}
