using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TurmaMaisA.Models.Organizations;

namespace TurmaMaisA.Models.Users
{
    public class User : IdentityUser
    {

        [MaxLength(128)]
        public required string FullName { get; set; }
        public Guid OrganizationId { get; set; }
        public Organization? Organization { get; set; }
    }
}
