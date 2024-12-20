﻿using Appointr.DTO;
using Appointr.Enum;
using Appointr.Persistence.Entities;
using Appointr.Persistence.UnitOfWork.Interface;
using Appointr.Service.Interface;
using Appointr.Service.Result;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Appointr.Service.Implementation
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IActivityService _activityService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AppointmentService(IActivityService activityService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _activityService = activityService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return _unitOfWork.Appointments.GetQueryable().Include(x => x.Officer).Include(x => x.Visitor).ToListAsync();
        }
        public Task<Appointment?> GetAppointmentByIdAsync(Guid appointmentid)
        {
            return _unitOfWork.Appointments.FindAsync(appointmentid);
        }

        public async Task<DateTime> GetAppointmentAddedDateByIdAsync(Guid appointmentid)
        {
            return await _unitOfWork.Appointments.GetQueryable()
                .AsNoTracking()
                .Where(x => x.AppointmentId == appointmentid)
                .Select(x => x.AddedOn)
                .SingleAsync();
        }

        public async Task<AppointmentStatus> GetAppointmentStatusByIdAsync(Guid appointmentid)
        {
            return await _unitOfWork.Appointments.GetQueryable()
                .AsNoTracking()
                .Where(x => x.AppointmentId == appointmentid)
                .Select(x => x.Status)
                .SingleAsync();
        }
        public async Task<OperationResult<Appointment>> AddAppointmentAsync(AppointmentDto appointmentDto)
        {
            await _unitOfWork.Appointments.GetEntitySet().Include(x => x.Visitor).Where(x => x.OfficerId == appointmentDto.OfficerId && x.Status == AppointmentStatus.Deactivated && x.Date == appointmentDto.Date && x.StartTime == appointmentDto.StartTime && x.EndTime == appointmentDto.EndTime)
                            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Status, AppointmentStatus.Cancelled));
            try
            {
                Appointment appointment = _mapper.Map<Appointment>(appointmentDto);
                appointment.Status = AppointmentStatus.Active;
                appointment.AddedOn = DateTime.Now;
                appointment.LastUpdatedOn = DateTime.Now;
                await _unitOfWork.Appointments.InsertAsync(appointment);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    ActivityDto activityDto = new ActivityDto();
                    activityDto.OfficerId = appointmentDto.OfficerId;
                    activityDto.Type = ActivityType.Appointment;
                    activityDto.StartDateTime = appointment.Date.ToDateTime(appointment.StartTime);
                    activityDto.EndDateTime = appointment.Date.ToDateTime(appointment.EndTime);
                    await _activityService.CreateActivityAsync(activityDto);
                    return new OperationResult<Appointment>(true, "Appointmant saved successfully.", rowsaffected, appointment);
                }
                return new OperationResult<Appointment>(false, "Appointmant failed to save.", rowsaffected, null);
            }
            catch (Exception ex)
            {
                return new OperationResult<Appointment>(false, $"Exception Occurred : {ex.Message}", 0, null);
            }
        }
        public async Task<OperationResult<Appointment>> UpdateAppointmentAsync(AppointmentDto appointmentDto)
        {
            try
            {
                Appointment appointment = _mapper.Map<Appointment>(appointmentDto);
                appointment.Status = await GetAppointmentStatusByIdAsync(appointmentDto.AppointmentId);
                appointment.AddedOn = await GetAppointmentAddedDateByIdAsync(appointmentDto.AppointmentId);
                appointment.LastUpdatedOn = DateTime.Now;
                _unitOfWork.Appointments.Update(appointment);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    return new OperationResult<Appointment>(true, "Appointmant Updated successfully.", rowsaffected, appointment);
                }
                return new OperationResult<Appointment>(false, "Appointmant failed to update.", rowsaffected, null);
            }
            catch (Exception ex)
            {
                return new OperationResult<Appointment>(false, $"Exception Occurred : {ex.Message}", 0, null);
            }
        }
        public async Task<ProcessResult> CancelAppointmentAsync(Guid appointmentid)
        {
            try
            {
                Appointment? appointment = await GetAppointmentByIdAsync(appointmentid);
                if (appointment == null)
                {
                    return new ProcessResult(false, "Appointment not found.");
                }
                if(appointment.Status == AppointmentStatus.Active || appointment.Status == AppointmentStatus.Deactivated)
                {
                    appointment.Status = AppointmentStatus.Cancelled;
                    _unitOfWork.Appointments.Update(appointment);
                    int rowsaffected = await _unitOfWork.CompleteAsync();
                    if (rowsaffected == 1)
                    {
                        return new ProcessResult(true, $"Appointment canceled successfully.");
                    }
                    return new ProcessResult(false, $"Appointment Failed to Cancel.");
                }
                else
                {
                    return new ProcessResult(true, $"Appointment cannot be Canceled.");
                }
            }
            catch (Exception ex)
            {
                return new ProcessResult(false, $"Exception Occurred : {ex.Message}.");
            }
        }
    }
}
