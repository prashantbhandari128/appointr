using Appointr.DTO;
using Appointr.Enum;
using Appointr.Persistence.Entities;
using Appointr.Persistence.UnitOfWork.Interface;
using Appointr.Service.Interface;
using Appointr.Service.Result;
using Appointr.ViewModel;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Appointr.Service.Implementation
{
    public class OfficerService : IOfficerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkDayService _workDayService;
        private readonly IMapper _mapper;

        public OfficerService(IUnitOfWork unitOfWork, IWorkDayService workDayService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _workDayService = workDayService;
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
            try
            {
                Officer? officer = await GetOfficerByIdAsync(officerid);
                if (officer == null)
                {
                    return new ProcessResult(false, "Officer not found.");
                }
                officer.Status = Status.Active;
                _unitOfWork.Officers.Update(officer);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    return new ProcessResult(true, "Officer activated successfully.");
                }
                return new ProcessResult(false, "Officer failed to activate.");
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
                    return new ProcessResult(true, "Officer deactivated successfully.");
                }
                return new ProcessResult(false, "Officer failed to deactivate.");
            }
            catch (Exception ex)
            {
                return new ProcessResult(false, $"Exception Occurred : {ex.Message}.");
            }
        }
    }
}
