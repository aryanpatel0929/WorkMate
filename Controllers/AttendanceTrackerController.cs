using AttendanceManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagement.Controllers
{
    public class AttendanceTrackerController : Controller
    {
        public IActionResult Attendance()
        {
            UserAttendanceDetailsModel model = new UserAttendanceDetailsModel();
            return View(model);
        }
    }
}
