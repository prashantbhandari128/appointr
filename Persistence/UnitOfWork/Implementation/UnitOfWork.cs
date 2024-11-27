using Appointr.Persistence.Context;
using Appointr.Persistence.Repository.Implementation;
using Appointr.Persistence.Repository.Interface;
using Appointr.Persistence.UnitOfWork.Interface;
using Microsoft.EntityFrameworkCore.Storage;

namespace Appointr.Persistence.UnitOfWork.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private bool _disposed = false;

        //-----------[ Add repositories here ]----------
        public IActivityRepository Activities { get; }
        public IAppointmentRepository Appointments { get; }
        public IOfficerRepository Officers { get; }
        public IPostRepository Posts { get; }
        public IVisitorRepository Visitors { get; }
        public IWorkDayRepository WorkDays { get; }
        //----------------------------------------------

        public UnitOfWork(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            //----------[ Initialize repositories here ]----------
            Activities = new ActivityRepository(_context);
            Appointments = new AppointmentRepository(_context);
            Officers = new OfficerRepository(_context);
            Posts = new PostRepository(_context);
            Visitors = new VisitorRepository(_context);
            WorkDays = new WorkDayRepository(_context);
            //----------------------------------------------------
        }

        public int Complete() => _context.SaveChanges();

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public IDbContextTransaction BeginTransaction() => _context.Database.BeginTransaction();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
