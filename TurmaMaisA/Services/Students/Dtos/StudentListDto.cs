using TurmaMaisA.Models;

namespace TurmaMaisA.Services.Students.Dtos
{
    public class StudentListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string RA { get; set; }
        public string Cpf { get; set; }

        public StudentListDto() { }

        public StudentListDto(Student entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            RA = entity.RA;
            Cpf = entity.Cpf;
        }
    }
}
