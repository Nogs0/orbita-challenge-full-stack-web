using System.ComponentModel;
using TurmaMaisA.Models.Shared;

namespace TurmaMaisA.Models
{
    public class Enrollment : BaseEntity, IMustHaveOrganizationId, ISoftDelete
    {
        public Student? Student { get; set; }
        public Guid StudentId { get; set; }
        public Course? Course { get; set; }
        public Guid CourseId { get; set; }

        public Guid OrganizationId { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
