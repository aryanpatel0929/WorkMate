namespace AttendanceManagement.Controllers.AccountControllers
{
    using AttendanceManagement.IdentityEntities;
    using AttendanceManagement.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    public class AccountController : Controller
    {
        public readonly IConfiguration configuration;
        public readonly IMemoryCache memoryCache;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        public AccountController(IConfiguration configuration, IMemoryCache memoryCache, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            this.configuration = configuration;
            this.memoryCache = memoryCache;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }



       [Authorize("NotAuthorized")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [Authorize("NotAuthorized")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterClass registerClass)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    PersonName = registerClass.PersonName,
                    UserName = registerClass.Email,
                    Email = registerClass.Email,
                    PhoneNumber = registerClass.Phone
                };
                if(registerClass.Password != registerClass.ConfirmPassword || registerClass.Password == null || registerClass.ConfirmPassword == null)
                {
                    ModelState.AddModelError("Index", "Password and Confirm Password should be same");
                    return View("Index", registerClass);
                }
                var result = await userManager.CreateAsync(user, registerClass.Password);
                if (result.Succeeded)
                {
                    if(registerClass.Role == "Admin")
                    {
                        if(await roleManager.FindByNameAsync("Admin") == null)
                        {
                            await roleManager.CreateAsync(new ApplicationRole { Name = "Admin" });
                            await userManager.AddToRoleAsync(user, "Admin");
                        }
                        else
                        {
                            await userManager.AddToRoleAsync(user, "Admin");
                        }
                    }
                    else
                    {
                        if(await roleManager.FindByNameAsync("User") == null)
                        {
                            await roleManager.CreateAsync(new ApplicationRole { Name = "User" });
                            await userManager.AddToRoleAsync(user, "User");
                        }
                        else
                        {
                            await userManager.AddToRoleAsync(user, "User");
                        }
                    }
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Index", error.Description);
                }
            }
            return View("Index", registerClass);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }

        [AllowAnonymous]
        public async Task<IActionResult> IsEmailAlreadyAvailable(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return Json(true);
            }
            return Json(false);
        }   
    }
}