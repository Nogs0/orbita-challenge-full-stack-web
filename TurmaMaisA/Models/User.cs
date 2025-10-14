using Microsoft.AspNetCore.Identity;
using TurmaMaisA.Models.Shared;

namespace TurmaMaisA.Models
{
    public class User : IdentityUser, IMustHaveOrganizationId
    {
        public Guid OrganizationId { get; set; }
        public Organization? Organization { get; set; }
    }
}
