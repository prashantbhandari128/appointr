using Appointr.DTO;
using Appointr.Persistence.Entities;
using Appointr.Persistence.UnitOfWork.Interface;
using Appointr.Service.Interface;
using Appointr.Service.Result;
using AutoMapper;

namespace Appointr.Service.Implementation
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<List<Post>> GetAllPostsAsync()
        {
            return _unitOfWork.Posts.ListAsync();
        }

        public Task<Post?> GetPostByIdAsync(Guid postid)
        {
            return _unitOfWork.Posts.FindAsync(postid);
        }

        public async Task<OperationResult<Post>> AddPostAsync(PostDto postDto)
        {
            try
            {
                Post post = _mapper.Map<Post>(postDto);
                await _unitOfWork.Posts.InsertAsync(post);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    return new OperationResult<Post>(true, "Post saved successfully.", rowsaffected, post);
                }
                return new OperationResult<Post>(false, "Post failed to save.", rowsaffected, null);
            }
            catch (Exception ex)
            {
                return new OperationResult<Post>(false, $"Exception Occurred : {ex.Message}", 0, null);
            }
        }

        public async Task<OperationResult<Post>> UpdatePostAsync(PostDto postDto)
        {
            try
            {
                Post post = _mapper.Map<Post>(postDto);
                _unitOfWork.Posts.Update(post);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    return new OperationResult<Post>(true, "Post updated successfully.", rowsaffected, post);
                }
                return new OperationResult<Post>(false, "Post failed to update.", rowsaffected, null);
            }
            catch (Exception ex)
            {
                return new OperationResult<Post>(false, $"Exception Occurred : {ex.Message}", 0, null);
            }
        }

        public async Task<ProcessResult> ActivatePostAsync(Guid postid)
        {
            try
            {
                Post? post = await GetPostByIdAsync(postid);
                if (post == null)
                {
                    return new ProcessResult(false, $"{postid} : Post not found.");
                }
                post.Status = Enum.Status.Active;
                _unitOfWork.Posts.Update(post);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    return new ProcessResult(true, $"{post.PostName} : Post activated successfully.");
                }
                return new ProcessResult(false, $"{post.PostName} : Post failed to activate.");
            }
            catch (Exception ex)
            {
                return new ProcessResult(false, $"Exception Occurred : {ex.Message}.");
            }
        }

        public async Task<ProcessResult> DeactivatePostAsync(Guid postid)
        {
            try
            {
                Post? post = await GetPostByIdAsync(postid);
                if (post == null)
                {
                    return new ProcessResult(false, "Post not found.");
                }
                post.Status = Enum.Status.Inactive;
                _unitOfWork.Posts.Update(post);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    return new ProcessResult(true, $"{post.PostName} : Post deactivated successfully.");
                }
                return new ProcessResult(false, $"{post.PostName} : Post failed to deactivate.");
            }
            catch (Exception ex)
            {
                return new ProcessResult(false, $"Exception Occurred : {ex.Message}.");
            }
        }
    }
}
