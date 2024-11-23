using Appointr.DTO;
using Appointr.Persistence.Entities;
using Appointr.Service.Result;

namespace Appointr.Service.Interface
{
    public interface IPostService
    {
        Task<List<Post>> GetAllPostsAsync();
        Task<Post?> GetPostByIdAsync(Guid postid);
        Task<OperationResult<Post>> AddPostAsync(PostDto postDto);
        Task<OperationResult<Post>> UpdatePostAsync(PostDto postDto);
        Task<ProcessResult> ActivatePostAsync(Guid postid);
        Task<ProcessResult> DeactivatePostAsync(Guid postid);
    }
}
