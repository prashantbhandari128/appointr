using Appointr.DTO;
using Appointr.Enum;
using Appointr.Persistence.Entities;
using Appointr.Persistence.UnitOfWork.Interface;
using Appointr.Service.Interface;
using Appointr.Service.Result;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Appointr.Service.Implementation
{
    public class ActivityService : IActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ActivityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<List<Activity>> GetAllActivityAsync()
        {
            return _unitOfWork.Activities.GetQueryable().Include(x => x.Officer).ToListAsync();
        }

        public async Task<OperationResult<Activity>> CreateActivityAsync(ActivityDto activityDto)
        {
            try
            {
                Activity activity = _mapper.Map<Activity>(activityDto);
                activity.Status = ActivityStatus.Active;
                await _unitOfWork.Activities.InsertAsync(activity);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    return new OperationResult<Activity>(true, "Activity saved successfully.", rowsaffected, activity);
                }
                return new OperationResult<Activity>(false, "Activity failed to save.", rowsaffected, null);
            }
            catch (Exception ex)
            {
                return new OperationResult<Activity>(false, $"Exception Occurred : {ex.Message}", 0, null);
            }
        }

        public async Task<List<Activity>> FilterActivityAsync(ActivityType type, ActivityStatus status, Guid officer, DateTime from, DateTime to)
        {
            return await _unitOfWork.Activities.GetQueryable().Include(x => x.Officer).Where(record =>
                record.Type == type &&
                record.Status == status &&
                record.OfficerId == officer &&
                record.StartDateTime >= from &&
                record.EndDateTime <= to
            ).ToListAsync();
        }
    }
}
