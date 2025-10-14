using System.ComponentModel.DataAnnotations;
using TurmaMaisA.Models.Shared;

namespace TurmaMaisA.Models
{
    public class Course : BaseEntity, IMustHaveOrganizationId
    {
        [MaxLength(128)]
        public required string Name { get; set; }
        public Guid OrganizationId { get; set; }
        public Organization? Organization { get; set; }
    }
}
