namespace TurmaMaisA.Services.Enrollments.Dtos
{
    public class EnrollmentCreateDto
    {
        public required Guid StudentId { get; set; }
        public required List<Guid> CoursesIds { get; set; }
    }
}
