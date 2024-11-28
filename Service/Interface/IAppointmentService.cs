using Appointr.DTO;
using Appointr.Enum;
using Appointr.Persistence.Entities;
using Appointr.Service.Result;

namespace Appointr.Service.Interface
{
    public interface IAppointmentService
    {
        Task<List<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment?> GetAppointmentByIdAsync(Guid appointmentid);
        Task<DateTime> GetAppointmentAddedDateByIdAsync(Guid appointmentid);
        Task<AppointmentStatus> GetAppointmentStatusByIdAsync(Guid appointmentid);
        Task<OperationResult<Appointment>> AddAppointmentAsync(AppointmentDto appointmentDto);
        Task<OperationResult<Appointment>> UpdateAppointmentAsync(AppointmentDto appointmentDto);
        Task<ProcessResult> CancelAppointmentAsync(Guid appointmentid);
    }
}
