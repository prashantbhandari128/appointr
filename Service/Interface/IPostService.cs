using Appointr.DTO;
using Appointr.Enum;
using Appointr.Persistence.Entities;
using Appointr.Service.Result;
using Appointr.ViewModel;

namespace Appointr.Service.Interface
{
    public interface IPostService
    {
        Task<List<SelectModel>> GetPostsSelectAsync();
        Task<List<Post>> GetAllPostsAsync();
        Task<Post?> GetPostByIdAsync(Guid postid);
        Task<Status> GetPostStatusByIdAsync(Guid postid);
        Task<OperationResult<Post>> AddPostAsync(PostDto postDto);
        Task<OperationResult<Post>> UpdatePostAsync(PostDto postDto);
        Task<ProcessResult> ActivatePostAsync(Guid postid);
        Task<ProcessResult> DeactivatePostAsync(Guid postid);
        Task<bool> PostHasOfficerAsync(Guid postid);
    }
}
