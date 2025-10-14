using System.ComponentModel.DataAnnotations;
using TurmaMaisA.Models.Shared;

namespace TurmaMaisA.Models
{
    public class Student : BaseEntity, IMustHaveOrganizationId
    {
        [MaxLength(128)]
        public required string Name { get; set; }

        [MaxLength(128)]
        public required string Email { get; set; }

        [MaxLength(9)]
        public required string RA { get; set; }

        [MaxLength(14)]
        public required string Cpf { get; set; }

        public Guid OrganizationId { get; set; }
        public Organization? Organization { get; set; }
    }
}
