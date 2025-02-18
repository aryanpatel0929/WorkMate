using Microsoft.AspNetCore.Identity;

namespace AttendanceManagement.IdentityEntities
{
    public class ApplicationRole: IdentityRole<Guid>
    {
        public string? RoleDescription { get; set; }
    }
}