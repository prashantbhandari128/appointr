using Appointr.DTO;
using Appointr.Enum;
using Appointr.Persistence.Entities;
using Appointr.Service.Result;

namespace Appointr.Service.Interface
{
    public interface IActivityService
    {
        Task<List<Activity>> GetAllActivityAsync();
        Task<OperationResult<Activity>> CreateActivityAsync(ActivityDto activityDto);
        Task<List<Activity>> FilterActivityAsync(ActivityType type, ActivityStatus status, Guid officer, DateTime from, DateTime to);
    }
}
