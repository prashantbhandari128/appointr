using Appointr.DTO;
using Appointr.Enum;
using Appointr.Persistence.Entities;
using Appointr.Persistence.UnitOfWork.Interface;
using Appointr.Service.Interface;
using Appointr.Service.Result;
using Appointr.ViewModel;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;

namespace Appointr.Service.Implementation
{
    public class OfficerService : IOfficerService
    {
        private readonly IWorkDayService _workDayService;
        private readonly IPostService _postService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OfficerService(IWorkDayService workDayService, IPostService postService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _workDayService = workDayService;
            _postService = postService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<SelectModel>> GetOfficersSelectAsync()
        {
            return await _unitOfWork.Officers.GetQueryable()
                .Select(x => new SelectModel
                {
                    Value = x.OfficerId,
                    Option = x.Name
                }).ToListAsync();
        }

        public Task<List<Officer>> GetAllOfficersAsync()
        {
            return _unitOfWork.Officers.GetQueryable().Include(x => x.Post).ToListAsync();
        }

        public Task<Officer?> GetOfficerByIdAsync(Guid officerid)
        {
            return _unitOfWork.Officers.FindAsync(officerid);
        }

        public async Task<Status> GetOfficerStatusByIdAsync(Guid officerid)
        {
            return await _unitOfWork.Officers.GetQueryable()
                .AsNoTracking()
                .Where(x => x.OfficerId == officerid)
                .Select(x => x.Status)
                .SingleAsync();
        }

        public async Task<OperationResult<Officer>> AddOfficerAsync(OfficerDto officerDto)
        {
            try
            {
                Officer officer = _mapper.Map<Officer>(officerDto);
                officer.Status = Status.Active;
                await _unitOfWork.Officers.InsertAsync(officer);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    await _workDayService.SaveWorkDaysAsync(officer.OfficerId, officerDto.Days);
                    return new OperationResult<Officer>(true, "Officer saved successfully.", rowsaffected, officer);
                }
                return new OperationResult<Officer>(false, "Officer failed to save.", rowsaffected, null);
            }
            catch (Exception ex)
            {
                return new OperationResult<Officer>(false, $"Exception Occurred : {ex.Message}", 0, null);
            }
        }

        public async Task<OperationResult<Officer>> UpdateOfficerAsync(OfficerDto officerDto)
        {
            try
            {
                Officer officer = _mapper.Map<Officer>(officerDto);
                officer.Status = await GetOfficerStatusByIdAsync(officerDto.OfficerId);
                _unitOfWork.Officers.Update(officer);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    var deleteprevious = await _workDayService.DeletePreviousWorkDaysAsync(officer.OfficerId);
                    if (deleteprevious.Status)
                    {
                        await _workDayService.SaveWorkDaysAsync(officer.OfficerId, officerDto.Days);
                    }
                    return new OperationResult<Officer>(true, "Officer updated successfully.", rowsaffected, officer);
                }
                return new OperationResult<Officer>(false, "Officer failed to update.", rowsaffected, null);
            }
            catch (Exception ex)
            {
                return new OperationResult<Officer>(false, $"Exception Occurred : {ex.Message}", 0, null);
            }
        }

        public async Task<ProcessResult> ActivateOfficerAsync(Guid officerid)
        {
            Officer? officer = await GetOfficerByIdAsync(officerid);
            try
            {
                if(await _postService.GetPostStatusByIdAsync(officer.PostId) == Status.Inactive)
                {
                    return new ProcessResult(false, "Officer`s post is inactive.");
                }
                else
                {
                    if (officer == null)
                    {
                        return new ProcessResult(false, "Officer not found.");
                    }
                    officer.Status = Status.Active;
                    _unitOfWork.Officers.Update(officer);
                    int rowsaffected = await _unitOfWork.CompleteAsync();
                    if (rowsaffected == 1)
                    {
                        await _unitOfWork.Appointments.GetEntitySet().Include(x => x.Visitor).Where(x => x.OfficerId == officerid && x.Status == AppointmentStatus.Deactivated && x.Date >= DateOnly.FromDateTime(DateTime.Now) && x.Visitor.Status == Status.Active)
                            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Status, AppointmentStatus.Active));
                        return new ProcessResult(true, "Officer activated successfully.");
                    }
                    return new ProcessResult(false, "Officer failed to activate.");
                }
            }
            catch (Exception ex)
            {
                return new ProcessResult(false, $"Exception Occurred : {ex.Message}.");
            }
        }

        public async Task<ProcessResult> DeactivateOfficerAsync(Guid officerid)
        {
            try
            {
                Officer? officer = await GetOfficerByIdAsync(officerid);
                if (officer == null)
                {
                    return new ProcessResult(false, "Officer not found.");
                }
                officer.Status = Status.Inactive;
                _unitOfWork.Officers.Update(officer);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    await _unitOfWork.Appointments.GetEntitySet().Where(x => x.OfficerId == officerid && x.Status == AppointmentStatus.Active && x.Date >= DateOnly.FromDateTime(DateTime.Now))
                        .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Status, AppointmentStatus.Deactivated));
                    return new ProcessResult(true, "Officer deactivated successfully.");
                }
                return new ProcessResult(false, "Officer failed to deactivate.");
            }
            catch (Exception ex)
            {
                return new ProcessResult(false, $"Exception Occurred : {ex.Message}.");
            }
        }

        public async Task<bool> IsOfficerBusyAsync(Guid officerid, DateOnly date)
        {
            return await _unitOfWork.Activities
               .GetQueryable()
               .AnyAsync(x => x.OfficerId == officerid &&
                   DateOnly.FromDateTime(x.StartDateTime) <= date &&
                   DateOnly.FromDateTime(x.EndDateTime) >= date &&
                   x.Status == ActivityStatus.Active);
        }

        public async Task<bool> CheckOfficerWorkDaysAsync(Guid officerId, int day)
        {
            var workdays = await _workDayService.GetOfficerWorkDaysAsync(officerId);
            return workdays.Any(x => (int)x == day);
        }

        public async Task<bool> CheckOfficerStartEndTime(Guid officerid, TimeOnly starttime, TimeOnly endtime)
        {
            Officer? officer = await GetOfficerByIdAsync(officerid);
            return FallsBetween(officer.WorkStartTime, officer.WorkEndTime, starttime, endtime);
        }

        public static bool FallsBetween(TimeOnly startTime1, TimeOnly endTime1, TimeOnly startTime2, TimeOnly endTime2)
        {
            // Ensure valid time ranges
            if (startTime1 > endTime1 || startTime2 > endTime2)
            {
                return false;
            }

            // Check if the second range is completely within the first range
            return startTime2 >= startTime1 && endTime2 <= endTime1;
        }
    }
}
