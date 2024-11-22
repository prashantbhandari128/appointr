using Appointr.Persistence.Repository.Interface;
using Microsoft.EntityFrameworkCore.Storage;

namespace Appointr.Persistence.UnitOfWork.Interface
{
    public interface IUnitOfWork
    {
        // Repositories
        IPostRepository Posts { get; }

        // Save changes
        int Complete();
        Task<int> CompleteAsync();

        // Transaction
        IDbContextTransaction BeginTransaction();
    }
}
