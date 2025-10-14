using TurmaMaisA.Models;

namespace TurmaMaisA.Services.Courses.Dtos
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        public CourseDto() { }

        public CourseDto(Course entity)
        {
            Id = entity.Id;
            Name = entity.Name;
        }
    }
}
