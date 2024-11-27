using Appointr.DTO;
using Appointr.Helper.Data.Toastr;
using Appointr.Helper.Interface;
using Appointr.Persistence.Entities;
using Appointr.Service.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Appointr.Controllers
{
    public class OfficerController : Controller
    {
        private readonly IOfficerService _officerService;
        private readonly IPostService _postService;
        private readonly IToastrHelper _toastrHelper;
        private readonly IMapper _mapper;

        public OfficerController(IOfficerService officerService, IPostService postService, IToastrHelper toastrHelper, IMapper mapper)
        {
            _officerService = officerService;
            _postService = postService;
            _toastrHelper = toastrHelper;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Officer> officers = await _officerService.GetAllOfficersAsync();
            return View(officers);
        }

        public async Task<IActionResult> Add()
        {
            ViewBag.Posts = await _postService.GetPostsSelectAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(OfficerDto officerDto)
        {
            if (!ModelState.IsValid || !officerDto.IsWorkTimeValid || officerDto.PostId == Guid.Empty)
            {
                if (!officerDto.IsWorkTimeValid)
                {
                    _toastrHelper.AddMessage("Appointr", "Work time difference not valid.", MessageType.Warning);
                    ModelState.AddModelError("", "Work time difference not valid.");
                }
                if (officerDto.PostId == Guid.Empty)
                {
                    _toastrHelper.AddMessage("Appointr", "Please select post.", MessageType.Warning);
                    ModelState["PostId"].Errors.Clear();
                    ModelState.AddModelError("PostId", "Please select post.");
                }
                ViewBag.Posts = await _postService.GetPostsSelectAsync();
                _toastrHelper.Send(this);
                return View(officerDto);
            }
            var result = await _officerService.AddOfficerAsync(officerDto);
            if (result.Status == true)
            {
                _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Success);
                return RedirectToAction("Index");
            }
            _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Error);
            ModelState.AddModelError("", result.Message);
            ViewBag.Posts = await _postService.GetPostsSelectAsync();
            return View(officerDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Officer? office = await _officerService.GetOfficerByIdAsync(id);
            OfficerDto officerDto = _mapper.Map<OfficerDto>(office);
            ViewBag.Posts = await _postService.GetPostsSelectAsync();
            return View(officerDto);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, OfficerDto officerDto)
        {
            if (!ModelState.IsValid || !officerDto.IsWorkTimeValid || officerDto.PostId == Guid.Empty)
            {
                if (!officerDto.IsWorkTimeValid)
                {
                    _toastrHelper.AddMessage("Appointr", "Work time difference not valid.", MessageType.Warning);
                    ModelState.AddModelError("", "Work time difference not valid.");
                }
                if (officerDto.PostId == Guid.Empty)
                {
                    _toastrHelper.AddMessage("Appointr", "Please select post.", MessageType.Warning);
                    ModelState["PostId"].Errors.Clear();
                    ModelState.AddModelError("PostId", "Please select post.");
                }
                ViewBag.Posts = await _postService.GetPostsSelectAsync();
                _toastrHelper.Send(this);
                return View(officerDto);
            }
            officerDto.OfficerId = id;
            var result = await _officerService.UpdateOfficerAsync(officerDto);
            if (result.Status == true)
            {
                _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Success);
                return RedirectToAction("Index");
            }
            _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Error);
            ModelState.AddModelError("", result.Message);
            ViewBag.Posts = await _postService.GetPostsSelectAsync();
            return View(officerDto);
        }

        [HttpGet]
        public async Task<IActionResult> Activate(Guid id)
        {
            var result = await _officerService.ActivateOfficerAsync(id);
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
            var result = await _officerService.DeactivateOfficerAsync(id);
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
