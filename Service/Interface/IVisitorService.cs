using Appointr.DTO;
using Appointr.Enum;
using Appointr.Persistence.Entities;
using Appointr.Service.Result;
using Appointr.ViewModel;

namespace Appointr.Service.Interface
{
    public interface IVisitorService
    {
        Task<List<SelectModel>> GetVisitorsSelectAsync();
        Task<List<Visitor>> GetAllVisitorsAsync();
        Task<Visitor?> GetVisitorByIdAsync(Guid visitorid);
        Task<Status> GetVisitorStatusByIdAsync(Guid visitorid);
        Task<OperationResult<Visitor>> AddVisitorAsync(VisitorDto visitorDto);
        Task<OperationResult<Visitor>> UpdateVisitorAsync(VisitorDto visitorDto);
        Task<ProcessResult> ActivateVisitorAsync(Guid visitorid);
        Task<ProcessResult> DeactivateVisitorAsync(Guid visitorid);
        Task<bool> VisitorHasAppointmentAsync(Guid visitorid, DateOnly date);
    }
}
