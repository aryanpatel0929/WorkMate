using System.Threading.Tasks;
using AttendanceManagement.IdentityEntities;
using AttendanceManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AttendanceManagement.Controllers.LoginControllers
{
    [Authorize("NotAuthorized")]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginController> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public LoginController(IConfiguration configuration, IMemoryCache memoryCache, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<LoginController> logger, RoleManager<ApplicationRole> roleManager)
        {
            _configuration = configuration;
            _memoryCache = memoryCache;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize("NotAuthorized")]
        public IActionResult Index(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> UserLogin(LoginClass loginClass, string? ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginClass.Email);
                if (user.UserName!=null && await _userManager.CheckPasswordAsync(user, loginClass.Password))
                {
                    var result = await _userManager.GetUsersInRoleAsync("Admin");
                    foreach (var item in result)
                    {
                        if (item.Id == user.Id)
                        {
                            _memoryCache.Set("Admin", user);
                            break;
                        }
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User {Email} logged in successfully.", loginClass.Email);
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        // Redirect to default page
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    _logger.LogWarning("Invalid login attempt for user {Email}.", loginClass.Email);
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            else
            {
                _logger.LogWarning("Model state is invalid for user {Email}.", loginClass.Email);
            }
            return View("Index", loginClass);
        }
    }
}