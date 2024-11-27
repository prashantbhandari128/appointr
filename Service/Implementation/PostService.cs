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
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<SelectModel>> GetPostsSelectAsync()
        {
            return await _unitOfWork.Posts.GetQueryable()
                .Select(x => new SelectModel
                {
                    Value = x.PostId,
                    Option = x.Name
                }).ToListAsync();
        }

        public Task<List<Post>> GetAllPostsAsync()
        {
            return _unitOfWork.Posts.ListAsync();
        }

        public Task<Post?> GetPostByIdAsync(Guid postid)
        {
            return _unitOfWork.Posts.FindAsync(postid);
        }

        public async Task<Status> GetPostStatusByIdAsync(Guid postid)
        {
            return await _unitOfWork.Posts.GetQueryable()
                .AsNoTracking()
                .Where(x => x.PostId == postid)
                .Select(x => x.Status)
                .SingleAsync();
        }

        public async Task<OperationResult<Post>> AddPostAsync(PostDto postDto)
        {
            try
            {
                Post post = _mapper.Map<Post>(postDto);
                post.Status = Status.Active;
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
                post.Status = await GetPostStatusByIdAsync(postDto.PostId);
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
                post.Status = Status.Active;
                _unitOfWork.Posts.Update(post);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    return new ProcessResult(true, $"Post activated successfully.");
                }
                return new ProcessResult(false, $"Post failed to activate.");
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
                post.Status = Status.Inactive;
                _unitOfWork.Posts.Update(post);
                int rowsaffected = await _unitOfWork.CompleteAsync();
                if (rowsaffected == 1)
                {
                    return new ProcessResult(true, $"Post deactivated successfully.");
                }
                return new ProcessResult(false, $"Post failed to deactivate.");
            }
            catch (Exception ex)
            {
                return new ProcessResult(false, $"Exception Occurred : {ex.Message}.");
            }
        }
    }
}
