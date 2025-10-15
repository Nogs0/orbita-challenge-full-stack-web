using Microsoft.AspNetCore.Identity;

namespace TurmaMaisA.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public Guid OrganizationId { get; set; }
        public Organization? Organization { get; set; }
    }
}
