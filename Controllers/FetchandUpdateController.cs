using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AttendanceManagement.Controllers
{
    public class FetchandUpdateController : Controller
    {
        private readonly IConfiguration _configuration;
        public FetchandUpdateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FetchUserDetails(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("The provided ID is null or empty.");
                return RedirectToAction("Index");
            }

            UserDetails? userDetails = null;

            try
            {
                SQLConnection connection = new SQLConnection(_configuration);

                // Fetch user details from the database
                userDetails = await connection.FetchData(id);

                if (userDetails == null)
                {
                    Console.WriteLine($"No user details found for ID: {id}");
                    // You can redirect to an error page or a different view if no data is found
                    return RedirectToAction("Error");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while fetching user details: {ex.Message}");
                // Optionally redirect to an error page
                return RedirectToAction("Error");
            }

            // Return the UserDetailsView if data is found
            return View("UserDetailsView", userDetails);
        }

        public IActionResult DisplayUserDetails()
        {
            return View();
        }
    }

}
