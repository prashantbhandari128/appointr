using Appointr.Persistence.Repository.Interface;
using Microsoft.EntityFrameworkCore.Storage;

namespace Appointr.Persistence.UnitOfWork.Interface
{
    public interface IUnitOfWork
    {
        // Repositories
        IActivityRepository Activities { get; }
        IAppointmentRepository Appointments { get; }
        IOfficerRepository Officers { get; }
        IPostRepository Posts { get; }
        IVisitorRepository Visitors { get; }
        IWorkDayRepository WorkDays { get; }

        // Save changes
        int Complete();
        Task<int> CompleteAsync();

        // Transaction
        IDbContextTransaction BeginTransaction();
    }
}
