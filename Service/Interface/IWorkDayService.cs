using Appointr.Enum;
using Appointr.Service.Result;

namespace Appointr.Service.Interface
{
    public interface IWorkDayService
    {
        Task<List<Days>> GetOfficerWorkDaysAsync(Guid officerid);
        Task<ProcessResult> SaveWorkDaysAsync(Guid officerid,List<Days> WorkDays);
        Task<ProcessResult> DeletePreviousWorkDaysAsync(Guid officerid);
    }
}
