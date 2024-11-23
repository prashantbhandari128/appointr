using Appointr.Helper.Data.Toastr;
using Microsoft.AspNetCore.Mvc;

namespace Appointr.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
