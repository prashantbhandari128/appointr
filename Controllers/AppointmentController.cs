using Appointr.DTO;
using Appointr.Helper.Data.Toastr;
using Appointr.Helper.Interface;
using Appointr.Persistence.Entities;
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
            if (!ModelState.IsValid || !appointmentDto.IsTimeDurationValid || !appointmentDto.IsDateValid || appointmentDto.OfficerId == Guid.Empty || appointmentDto.VisitorId == Guid.Empty)
            {
                if (!appointmentDto.IsTimeDurationValid)
                {
                    _toastrHelper.AddMessage("Appointr", "Time difference not valid.", MessageType.Warning);
                    ModelState.AddModelError("", "Time difference not valid.");
                }
                if (!appointmentDto.IsDateValid)
                {
                    _toastrHelper.AddMessage("Appointr", "Past date not valid.", MessageType.Warning);
                    ModelState.AddModelError("date", "Past date not valid.");
                }
                if (appointmentDto.OfficerId == Guid.Empty)
                {
                    _toastrHelper.AddMessage("Appointr", "Please select officer.", MessageType.Warning);
                    ModelState["OfficerId"].Errors.Clear();
                    ModelState.AddModelError("OfficerId", "Please select officer.");
                }
                if (appointmentDto.VisitorId == Guid.Empty)
                {
                    _toastrHelper.AddMessage("Appointr", "Please select visitor.", MessageType.Warning);
                    ModelState["VisitorId"].Errors.Clear();
                    ModelState.AddModelError("VisitorId", "Please select visitor.");
                }
                ViewBag.Officers = await _officerService.GetOfficersSelectAsync();
                ViewBag.Visitors = await _visitorService.GetVisitorsSelectAsync();
                _toastrHelper.Send(this);
                return View(appointmentDto);
            }
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
            if (!ModelState.IsValid || !appointmentDto.IsTimeDurationValid || !appointmentDto.IsDateValid || appointmentDto.OfficerId == Guid.Empty || appointmentDto.VisitorId == Guid.Empty)
            {
                if (!appointmentDto.IsTimeDurationValid)
                {
                    _toastrHelper.AddMessage("Appointr", "Time difference not valid.", MessageType.Warning);
                    ModelState.AddModelError("", "Time difference not valid.");
                }
                if (!appointmentDto.IsDateValid)
                {
                    _toastrHelper.AddMessage("Appointr", "Past date not valid.", MessageType.Warning);
                    ModelState.AddModelError("date", "Past date not valid.");
                }
                if (appointmentDto.OfficerId == Guid.Empty)
                {
                    _toastrHelper.AddMessage("Appointr", "Please select officer.", MessageType.Warning);
                    ModelState["OfficerId"].Errors.Clear();
                    ModelState.AddModelError("OfficerId", "Please select officer.");
                }
                if (appointmentDto.VisitorId == Guid.Empty)
                {
                    _toastrHelper.AddMessage("Appointr", "Please select visitor.", MessageType.Warning);
                    ModelState["VisitorId"].Errors.Clear();
                    ModelState.AddModelError("VisitorId", "Please select visitor.");
                }
                ViewBag.Officers = await _officerService.GetOfficersSelectAsync();
                ViewBag.Visitors = await _visitorService.GetVisitorsSelectAsync();
                _toastrHelper.Send(this);
                return View(appointmentDto);
            }
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
    }
}
