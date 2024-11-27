using Appointr.Persistence.Context;
using Appointr.Persistence.Entities;
using Appointr.Persistence.Repository.Interface;

namespace Appointr.Persistence.Repository.Implementation
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context) : base(context)
        {

        }
    }
}
