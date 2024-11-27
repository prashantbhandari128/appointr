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
    public class VisitorService : IVisitorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VisitorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<SelectModel>> GetVisitorsSelectAsync()
        {
            return await _unitOfWork.Visitors.GetQueryable()
                .Select(x => new SelectModel
                {
                    Value = x.VisitorId,
                    Option = x.Name
                }).ToListAsync();
        }

        public Task<List<Visitor>> GetAllVisitorsAsync()
        {
            return _unitOfWork.Visitors.ListAsync();
        }

        public Task<Visitor?> GetVisitorByIdAsync(Guid visitorid)
        {
            return _unitOfWork.Visitors.FindAsync(visitorid);
        }

        public async Task<Status> GetVisitorStatusByIdAsync(Guid visitorid)
        {
            return await _unitOfWork.Visitors.GetQueryable().AsNoTracking().Where(x => x.VisitorId == visitorid).Select(x => x.Status).SingleAsync();
        }

        public async Task<OperationResult<Visitor>> AddVisitorAsync(VisitorDto visitorDto)
        {
            try
            {
                Visitor visitor = _mapper.Map<Visitor>(visitorDto);
                visitor.Status = Status.Active;
                await _unitOfWork.Visitors.InsertAsync(visitor);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    return new OperationResult<Visitor>(true, "Visitor saved successfully.", rowsaffected, visitor);
                }
                return new OperationResult<Visitor>(false, "Visitor failed to save.", rowsaffected, null);
            }
            catch (Exception ex)
            {
                return new OperationResult<Visitor>(false, $"Exception Occurred : {ex.Message}", 0, null);
            }
        }

        public async Task<OperationResult<Visitor>> UpdateVisitorAsync(VisitorDto visitorDto)
        {
            try
            {
                Visitor visitor = _mapper.Map<Visitor>(visitorDto);
                visitor.Status = await GetVisitorStatusByIdAsync(visitorDto.VisitorId);
                _unitOfWork.Visitors.Update(visitor);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    return new OperationResult<Visitor>(true, "Visitor updated successfully.", rowsaffected, visitor);
                }
                return new OperationResult<Visitor>(false, "Visitor failed to update.", rowsaffected, null);
            }
            catch (Exception ex)
            {
                return new OperationResult<Visitor>(false, $"Exception Occurred : {ex.Message}", 0, null);
            }
        }

        public async Task<ProcessResult> ActivateVisitorAsync(Guid visitorid)
        {
            try
            {
                Visitor? visitor = await GetVisitorByIdAsync(visitorid);
                if (visitor == null)
                {
                    return new ProcessResult(false, "Visitor not found.");
                }
                visitor.Status = Status.Active;
                _unitOfWork.Visitors.Update(visitor);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    return new ProcessResult(true, "Visitor activated successfully.");
                }
                return new ProcessResult(false, "Visitor failed to activate.");
            }
            catch (Exception ex)
            {
                return new ProcessResult(false, $"Exception Occurred : {ex.Message}.");
            }
        }

        public async Task<ProcessResult> DeactivateVisitorAsync(Guid visitorid)
        {
            try
            {
                Visitor? visitor = await GetVisitorByIdAsync(visitorid);
                if (visitor == null)
                {
                    return new ProcessResult(false, "Visitor not found.");
                }
                visitor.Status = Status.Inactive;
                _unitOfWork.Visitors.Update(visitor);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    return new ProcessResult(true, "Visitor deactivated successfully.");
                }
                return new ProcessResult(false, "Visitor failed to deactivate.");
            }
            catch (Exception ex)
            {
                return new ProcessResult(false, $"Exception Occurred : {ex.Message}.");
            }
        }
    }
}
