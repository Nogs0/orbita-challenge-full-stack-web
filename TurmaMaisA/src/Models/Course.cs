using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TurmaMaisA.Models.Shared;

namespace TurmaMaisA.Models
{
    public class Course : BaseEntity, IMustHaveOrganizationId, ISoftDelete
    {
        [MaxLength(128)]
        public required string Name { get; set; }
        public Guid OrganizationId { get; set; }
        public Organization? Organization { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
