using Appointr.DTO;
using Appointr.Helper.Data.Toastr;
using Appointr.Helper.Interface;
using Appointr.Persistence.Entities;
using Appointr.Service.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Appointr.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IToastrHelper _toastrHelper;
        private readonly IMapper _mapper;

        public PostController(IPostService postService, IToastrHelper toastrHelper, IMapper mapper)
        {
            _postService = postService;
            _toastrHelper = toastrHelper;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Post> posts = await _postService.GetAllPostsAsync();
            return View(posts);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return View(postDto);
            }
            var result = await _postService.AddPostAsync(postDto);
            if (result.Status == true)
            {
                _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Success);
                return RedirectToAction("Index");
            }
            _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Error);
            ModelState.AddModelError("", "Failed to add Post");
            return View(postDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Post? post = await _postService.GetPostByIdAsync(id);
            PostDto postDto = _mapper.Map<PostDto>(post);
            return View(postDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return View(postDto);
            }
            postDto.PostId = id;
            var result = await _postService.UpdatePostAsync(postDto);
            if (result.Status == true)
            {
                _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Success);
                return RedirectToAction("Index");
            }
            _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Error);
            ModelState.AddModelError("", "Failed to update Post");
            return View(postDto);
        }

        [HttpGet]
        public async Task<IActionResult> Activate(Guid id)
        {
            var result = await _postService.ActivatePostAsync(id);
            if (result.Status == true)
            {
                _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Success);
            }
            else
            {
                _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Error);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var result = await _postService.DeactivatePostAsync(id);
            if (result.Status == true)
            {
                _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Success);
            }
            else
            {
                _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Error);
            }
            return RedirectToAction("Index");
        }
    }
}
