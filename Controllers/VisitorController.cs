using Appointr.DTO;
using Appointr.Helper.Data.Toastr;
using Appointr.Helper.Interface;
using Appointr.Persistence.Entities;
using Appointr.Service.Implementation;
using Appointr.Service.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Appointr.Controllers
{
    public class VisitorController : Controller
    {
        private readonly IToastrHelper _toastrHelper;
        private readonly IVisitorService _visitorService;
        private readonly IMapper _mapper;

        public VisitorController(IToastrHelper toastrHelper, IVisitorService visitorService, IMapper mapper)
        {
            _toastrHelper = toastrHelper;
            _visitorService = visitorService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Visitor> visitors = await _visitorService.GetAllVisitorsAsync();
            return View(visitors);
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(VisitorDto visitorDto)
        {
            if (!ModelState.IsValid)
            {
                return View(visitorDto);
            }
            var result = await _visitorService.AddVisitorAsync(visitorDto);
            if (result.Status == true)
            {
                _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Success);
                return RedirectToAction("Index");
            }
            _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Error);
            ModelState.AddModelError("", result.Message);
            return View(visitorDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Visitor? visitor = await _visitorService.GetVisitorByIdAsync(id);
            VisitorDto visitorDto = _mapper.Map<VisitorDto>(visitor);
            return View(visitorDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, VisitorDto visitorDto)
        {
            if (!ModelState.IsValid)
            {
                return View(visitorDto);
            }
            visitorDto.VisitorId = id;
            var result = await _visitorService.UpdateVisitorAsync(visitorDto);
            if (result.Status == true)
            {
                _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Success);
                return RedirectToAction("Index");
            }
            _toastrHelper.SendMessage(this, "Appointr", result.Message, MessageType.Error);
            ModelState.AddModelError("", result.Message);
            return View(visitorDto);
        }


        [HttpGet]
        public async Task<IActionResult> Activate(Guid id)
        {
            var result = await _visitorService.ActivateVisitorAsync(id);
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
            var result = await _visitorService.DeactivateVisitorAsync(id);
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
