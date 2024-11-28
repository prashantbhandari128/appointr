using Appointr.DTO;
using Appointr.Enum;
using Appointr.Helper.Data.Toastr;
using Appointr.Helper.Interface;
using Appointr.Persistence.Entities;
using Appointr.Service.Implementation;
using Appointr.Service.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Appointr.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IOfficerService _officerService;
        private readonly IVisitorService _visitorService;
        private readonly IToastrHelper _toastrHelper;
        private readonly IMapper _mapper;

        public AppointmentController(IAppointmentService appointmentService, IOfficerService officerService, IVisitorService visitorService, IToastrHelper toastrHelper, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _officerService = officerService;
            _visitorService = visitorService;
            _toastrHelper = toastrHelper;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Appointment> appointments = await _appointmentService.GetAllAppointmentsAsync();
            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Officers = await _officerService.GetOfficersSelectAsync();
            ViewBag.Visitors = await _visitorService.GetVisitorsSelectAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AppointmentDto appointmentDto)
        {
            if (!ModelState.IsValid || !appointmentDto.IsTimeDurationValid || !appointmentDto.IsDateValid || appointmentDto.OfficerId == Guid.Empty || appointmentDto.VisitorId == Guid.Empty || appointmentDto.OfficerId != Guid.Empty || appointmentDto.VisitorId != Guid.Empty)
            {
                var count = 0;
                if (!appointmentDto.IsTimeDurationValid)
                {
                    _toastrHelper.AddMessage("Appointr", "Time difference not valid.", MessageType.Warning);
                    ModelState.AddModelError("", "Time difference not valid.");
                    count++;
                }
                if (!appointmentDto.IsDateValid)
                {
                    _toastrHelper.AddMessage("Appointr", "Past date not valid.", MessageType.Warning);
                    ModelState.AddModelError("date", "Past date not valid.");
                    count++;
                }
                if (appointmentDto.OfficerId == Guid.Empty)
                {
                    _toastrHelper.AddMessage("Appointr", "Please select officer.", MessageType.Warning);
                    ModelState["OfficerId"].Errors.Clear();
                    ModelState.AddModelError("OfficerId", "Please select officer.");
                    count++;
                }
                else
                {
                    // Validate OfficerId
                    if (await _officerService.GetOfficerStatusByIdAsync(appointmentDto.OfficerId) == Status.Inactive)
                    {
                        _toastrHelper.AddMessage("Appointr", "Selected officer is inactive.", MessageType.Warning);
                        ModelState["OfficerId"].Errors.Clear();
                        ModelState.AddModelError("OfficerId", "Selected officer is inactive.");
                        count++;
                    }
                    if (await _officerService.IsOfficerBusyAsync(appointmentDto.OfficerId, appointmentDto.Date))
                    {
                        _toastrHelper.AddMessage("Appointr", "Selected officer is busy on requested date.", MessageType.Warning);
                        ModelState.AddModelError("", "Selected officer is busy on requested date.");
                        count++;
                    }
                    if (!await _officerService.CheckOfficerWorkDaysAsync(appointmentDto.OfficerId, (int)appointmentDto.Date.DayOfWeek))
                    {
                        _toastrHelper.AddMessage("Appointr", "Requested date doesn't fall in officer's workdays.", MessageType.Warning);
                        ModelState.AddModelError("", "Requested date doesn't fall in officer's workdays.");
                        count++;
                    }
                    if (!await _officerService.CheckOfficerStartEndTime(appointmentDto.OfficerId, appointmentDto.StartTime, appointmentDto.EndTime))
                    {
                        _toastrHelper.AddMessage("Appointr", "Requested time doesn't fall in officer's workhours.", MessageType.Warning);
                        ModelState.AddModelError("", "Requested time doesn't fall in officer's workhours.");
                        count++;
                    }
                }

                if (appointmentDto.VisitorId == Guid.Empty)
                {
                    _toastrHelper.AddMessage("Appointr", "Please select visitor.", MessageType.Warning);
                    ModelState["VisitorId"].Errors.Clear();
                    ModelState.AddModelError("VisitorId", "Please select visitor.");
                    count++;
                }
                else
                {
                    // Validate VisitorId
                    if (await _visitorService.GetVisitorStatusByIdAsync(appointmentDto.VisitorId) == Status.Inactive)
                    {
                        _toastrHelper.AddMessage("Appointr", "Selected visitor is inactive.", MessageType.Warning);
                        ModelState["VisitorId"].Errors.Clear();
                        ModelState.AddModelError("VisitorId", "Selected visitor is inactive.");
                        count++;
                    }
                    if (await _visitorService.VisitorHasAppointmentAsync(appointmentDto.VisitorId, appointmentDto.Date))
                    {
                        _toastrHelper.AddMessage("Appointr", "Selected visitor already has an appointment on requested date.", MessageType.Warning);
                        ModelState.AddModelError("", "Selected visitor already has an appointment on requested date.");
                        count++;
                    }
                }
                if(count == 0)
                {
                    goto Action;
                }
                ViewBag.Officers = await _officerService.GetOfficersSelectAsync();
                ViewBag.Visitors = await _visitorService.GetVisitorsSelectAsync();
                _toastrHelper.Send(this);
                return View(appointmentDto);
            }
            Action:
            var result = await _appointmentService.AddAppointmentAsync(appointmentDto);
            if (result.Status == true)
            {
                _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Success);
                return RedirectToAction("Index");
            }
            _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Error);
            ModelState.AddModelError("", result.Message);
            ViewBag.Officers = await _officerService.GetOfficersSelectAsync();
            ViewBag.Visitors = await _visitorService.GetVisitorsSelectAsync();
            return View(appointmentDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Appointment? appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            AppointmentDto appointmentDto = _mapper.Map<AppointmentDto>(appointment);
            ViewBag.Officers = await _officerService.GetOfficersSelectAsync();
            ViewBag.Visitors = await _visitorService.GetVisitorsSelectAsync();
            return View(appointmentDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, AppointmentDto appointmentDto)
        {
            if (!ModelState.IsValid || !appointmentDto.IsTimeDurationValid || !appointmentDto.IsDateValid || appointmentDto.OfficerId == Guid.Empty || appointmentDto.VisitorId == Guid.Empty || appointmentDto.OfficerId != Guid.Empty || appointmentDto.VisitorId != Guid.Empty)
            {
                var count = 0;
                if (!appointmentDto.IsTimeDurationValid)
                {
                    _toastrHelper.AddMessage("Appointr", "Time difference not valid.", MessageType.Warning);
                    ModelState.AddModelError("", "Time difference not valid.");
                    count++;
                }
                if (!appointmentDto.IsDateValid)
                {
                    _toastrHelper.AddMessage("Appointr", "Past date not valid.", MessageType.Warning);
                    ModelState.AddModelError("date", "Past date not valid.");
                    count++;
                }
                if (appointmentDto.OfficerId == Guid.Empty)
                {
                    _toastrHelper.AddMessage("Appointr", "Please select officer.", MessageType.Warning);
                    ModelState["OfficerId"].Errors.Clear();
                    ModelState.AddModelError("OfficerId", "Please select officer.");
                    count++;
                }
                else
                {
                    if (await _officerService.IsOfficerBusyAsync(appointmentDto.OfficerId, appointmentDto.Date))
                    {
                        _toastrHelper.AddMessage("Appointr", "Selected officer is busy on requested date.", MessageType.Warning);
                        ModelState.AddModelError("", "Selected officer is busy on requested date.");
                        count++;
                    }
                    if (!await _officerService.CheckOfficerWorkDaysAsync(appointmentDto.OfficerId, (int)appointmentDto.Date.DayOfWeek))
                    {
                        _toastrHelper.AddMessage("Appointr", "Requested date doesn't fall in officer's workdays.", MessageType.Warning);
                        ModelState.AddModelError("", "Requested date doesn't fall in officer's workdays.");
                        count++;
                    }
                    if (!await _officerService.CheckOfficerStartEndTime(appointmentDto.OfficerId, appointmentDto.StartTime, appointmentDto.EndTime))
                    {
                        _toastrHelper.AddMessage("Appointr", "Requested time doesn't fall in officer's workhours.", MessageType.Warning);
                        ModelState.AddModelError("", "Requested time doesn't fall in officer's workhours.");
                        count++;
                    }
                }

                if (appointmentDto.VisitorId == Guid.Empty)
                {
                    _toastrHelper.AddMessage("Appointr", "Please select visitor.", MessageType.Warning);
                    ModelState["VisitorId"].Errors.Clear();
                    ModelState.AddModelError("VisitorId", "Please select visitor.");
                    count++;
                }
                else
                {
                    if (await _visitorService.VisitorHasAppointmentAsync(appointmentDto.VisitorId, appointmentDto.Date))
                    {
                        _toastrHelper.AddMessage("Appointr", "Selected visitor already has an appointment on requested date.", MessageType.Warning);
                        ModelState.AddModelError("", "Selected visitor already has an appointment on requested date.");
                        count++;
                    }
                }
                if (count == 0)
                {
                    goto Action;
                }
                ViewBag.Officers = await _officerService.GetOfficersSelectAsync();
                ViewBag.Visitors = await _visitorService.GetVisitorsSelectAsync();
                _toastrHelper.Send(this);
                return View(appointmentDto);
            }
            Action:
            appointmentDto.AppointmentId = id;
            var result = await _appointmentService.UpdateAppointmentAsync(appointmentDto);
            if (result.Status == true)
            {
                _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Success);
                return RedirectToAction("Index");
            }
            _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Error);
            ModelState.AddModelError("", result.Message);
            ViewBag.Officers = await _officerService.GetOfficersSelectAsync();
            ViewBag.Visitors = await _visitorService.GetVisitorsSelectAsync();
            return View(appointmentDto);
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _appointmentService.CancelAppointmentAsync(id);
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