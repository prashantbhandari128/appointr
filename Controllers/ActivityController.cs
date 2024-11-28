using Appointr.DTO;
using Appointr.Enum;
using Appointr.Helper.Data.Toastr;
using Appointr.Helper.Interface;
using Appointr.Persistence.Entities;
using Appointr.Service.Interface;
using Appointr.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Appointr.Controllers
{
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly IOfficerService _officerService;
        private readonly IToastrHelper _toastrHelper;
        private readonly IMapper _mapper;

        public ActivityController(IActivityService activityService, IOfficerService officerService, IToastrHelper toastrHelper, IMapper mapper)
        {
            _activityService = activityService;
            _officerService = officerService;
            _toastrHelper = toastrHelper;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ActivityViewModel activityViewModel = new ActivityViewModel();
            ViewBag.Officers = await _officerService.GetOfficersSelectAsync();
            activityViewModel.Activities = await _activityService.GetAllActivityAsync();
            return View(activityViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ActivityViewModel activityViewModel)
        {
            ViewBag.Officers = await _officerService.GetOfficersSelectAsync();
            activityViewModel.Activities = await _activityService.FilterActivityAsync(activityViewModel.Type, activityViewModel.Status, activityViewModel.Officer, activityViewModel.From, activityViewModel.To);
            return View(activityViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Officers = await _officerService.GetOfficersSelectAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ActivityDto activityDto)
        {
            if (!ModelState.IsValid ||activityDto.Type == ActivityType.Appointment || activityDto.OfficerId == Guid.Empty || !activityDto.IsActivityDurationValid)
            {
                if (activityDto.Type == ActivityType.Appointment)
                {
                    _toastrHelper.AddMessage("Appointr", "Appointment type cannot be created by user.", MessageType.Warning);
                    ModelState.AddModelError("Type", "Appointment type cannot be created by user.");
                }
                if (activityDto.OfficerId == Guid.Empty)
                {
                    _toastrHelper.AddMessage("Appointr", "Please select officer.", MessageType.Warning);
                    ModelState["OfficerId"].Errors.Clear();
                    ModelState.AddModelError("OfficerId", "Please select officer.");
                }
                if(!activityDto.IsActivityDurationValid)
                {
                    _toastrHelper.AddMessage("Appointr", "Activity duration not valid.", MessageType.Warning);
                    ModelState.AddModelError("", "Activity duration not valid.");
                }
                ViewBag.Officers = await _officerService.GetOfficersSelectAsync();
                _toastrHelper.Send(this);
                return View(activityDto);
            }
            var result = await _activityService.CreateActivityAsync(activityDto);
            if (result.Status == true)
            {
                _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Success);
                return RedirectToAction("Index");
            }
            _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Error);
            ModelState.AddModelError("", result.Message);
            ViewBag.Officers = await _officerService.GetOfficersSelectAsync();
            return View(activityDto);
        }
    }
}
