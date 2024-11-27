using Appointr.DTO;
using Appointr.Enum;
using Appointr.Persistence.Entities;
using Appointr.Service.Result;
using Appointr.ViewModel;

namespace Appointr.Service.Interface
{
    public interface IOfficerService
    {
        Task<List<SelectModel>> GetOfficersSelectAsync();
        Task<List<Officer>> GetAllOfficersAsync();
        Task<Officer?> GetOfficerByIdAsync(Guid officerid);
        Task<Status> GetOfficerStatusByIdAsync(Guid officerid);
        Task<OperationResult<Officer>> AddOfficerAsync(OfficerDto officerDto);
        Task<OperationResult<Officer>> UpdateOfficerAsync(OfficerDto officerDto);
        Task<ProcessResult> ActivateOfficerAsync(Guid officerid);
        Task<ProcessResult> DeactivateOfficerAsync(Guid officerid);
    }
}
