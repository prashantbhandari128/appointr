using Appointr.DTO;
using Appointr.Persistence.Entities;
using Appointr.Service.Result;

namespace Appointr.Service.Interface
{
    public interface IActivityService
    {
        Task<List<Activity>> GetAllActivityAsync();
        Task<OperationResult<Activity>> CreateActivityAsync(ActivityDto activityDto);
    }
}
