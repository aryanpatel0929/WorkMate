using AttendanceManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AttendanceManagement.Controllers.LoginControllers
{
    public class LoginController : Controller
    {
        public readonly IConfiguration configuration;
        public readonly IMemoryCache memoryCache;
        public LoginController(IConfiguration configuration, IMemoryCache memoryCache)
        {
            this.configuration = configuration;
            this.memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserLogin(LoginClass loginClass)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index","Login");
            }
            if(loginClass.userEmail == "aryanbbkup74@gmail.com" && loginClass.password == "testlogin")
            {
                memoryCache.Set("userLogin", "true");
                return RedirectToAction("Index", "Home");
            }
            return new StatusCodeResult(200);
        }
    }
}
