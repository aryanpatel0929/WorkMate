using Microsoft.AspNetCore.Identity;

namespace AttendanceManagement.IdentityEntities
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        public string? PersonName { get; set; }

    }
}