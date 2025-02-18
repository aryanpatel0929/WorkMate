using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagement.Controllers.LoginControllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
