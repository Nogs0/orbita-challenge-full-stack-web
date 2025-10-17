using System.Diagnostics.CodeAnalysis;
using TurmaMaisA.Models;

namespace TurmaMaisA.Services.Students.Dtos
{
    public class StudentListDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string RA { get; set; }
        public required string Cpf { get; set; }

        public StudentListDto() { }

        [SetsRequiredMembers]
        public StudentListDto(Student entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            RA = entity.RA;
            Cpf = entity.Cpf;
        }
    }
}
