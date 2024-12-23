using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagement.Controllers.ProductsControllers
{
    public class GenerateReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
