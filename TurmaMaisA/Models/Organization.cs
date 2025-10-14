using System.ComponentModel.DataAnnotations;
using TurmaMaisA.Models.Shared;

namespace TurmaMaisA.Models
{
    public class Organization : BaseEntity
    {
        [MaxLength(128)]
        public required string Name { get; set; }
    }
}
