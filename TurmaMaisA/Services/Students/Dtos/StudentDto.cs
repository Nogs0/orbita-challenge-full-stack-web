using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TurmaMaisA.Models;

namespace TurmaMaisA.Services.Students.Dtos
{
    public class StudentDto
    {
        public Guid Id { get; set; }

        [MaxLength(128)]
        public required string Name { get; set; }

        [MaxLength(128)]
        public required string Email { get; set; }

        [MaxLength(9)]
        public required string RA { get; set; }

        [MaxLength(14)]
        public required string Cpf { get; set; }

        public StudentDto() { }

        [SetsRequiredMembers]
        public StudentDto(Student entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Email = entity.Email;
            RA = entity.RA;
            Cpf = entity.Cpf;
        }
    }
}
