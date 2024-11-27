using Appointr.Enum;
using Appointr.Persistence.Entities;
using Appointr.Persistence.UnitOfWork.Interface;
using Appointr.Service.Interface;
using Appointr.Service.Result;
using Microsoft.EntityFrameworkCore;

namespace Appointr.Service.Implementation
{
    public class WorkDayService : IWorkDayService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkDayService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<Days>> GetOfficerWorkDaysAsync(Guid officerid)
        {
            return _unitOfWork.WorkDays.GetQueryable().Where(x => x.OfficerId == officerid).Select(x => x.DayOfWeek).ToListAsync();
        }

        public async Task<ProcessResult> SaveWorkDaysAsync(Guid officerid, List<Days> WorkDays)
        {
            try
            {
                List<WorkDay> workdays = WorkDays.Select(x => new WorkDay
                {
                    OfficerId = officerid,
                    DayOfWeek = x
                }).ToList();
                await _unitOfWork.WorkDays.InsertRangeAsync(workdays);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == workdays.Count)
                {
                    return new ProcessResult(true, $"Workday saved successfully.");
                }
                return new ProcessResult(false, $"Workday failed to save.");
            }
            catch (Exception ex)
            {
                return new ProcessResult(false, $"Exception Occurred : {ex.Message}.");
            }
        }
        public async Task<ProcessResult> DeletePreviousWorkDaysAsync(Guid officerid)
        {
            try
            {
                List<WorkDay> previousworkdays = await _unitOfWork.WorkDays.GetQueryable().Where(x => x.OfficerId == officerid).ToListAsync();
                _unitOfWork.WorkDays.DeleteRange(previousworkdays);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == previousworkdays.Count)
                {
                    return new ProcessResult(true, $"Workdays delected successfully.");
                }
                return new ProcessResult(false, $"Workdays failed to delete.");
            }
            catch (Exception ex)
            {
                return new ProcessResult(false, $"Exception Occurred : {ex.Message}.");
            }
        }
    }
}
