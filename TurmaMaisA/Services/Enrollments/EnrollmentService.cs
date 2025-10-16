using TurmaMaisA.Models;
using TurmaMaisA.Persistence.Interfaces;
using TurmaMaisA.Persistence.Repositories.Courses;
using TurmaMaisA.Persistence.Repositories.Enrollments;
using TurmaMaisA.Persistence.Repositories.Students;
using TurmaMaisA.Services.Enrollments.Dtos;
using TurmaMaisA.Utils.Exceptions;

namespace TurmaMaisA.Services.Enrollments
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _repository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUnitOfWork _uow;

        public EnrollmentService(IEnrollmentRepository repository,
            IUnitOfWork uow,
            IStudentRepository studentRepository,
            ICourseRepository courseRepository)
        {
            _repository = repository;
            _uow = uow;
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }

        public async Task SetEnrolllmentsAsync(EnrollmentCreateDto dto)
        {
            var student = await _studentRepository.GetByIdAsync(dto.StudentId) ??
                throw new NotFoundException("Student", dto.StudentId);

            if (dto.CoursesIds == null || dto.CoursesIds.Count == 0)
                return;

            var coursesToEnroll = await _courseRepository.GetByIdsAsync(dto.CoursesIds);

            var notFoundCourses = dto.CoursesIds.Where(dtoCId => !coursesToEnroll.Select(c => c.Id).Contains(dtoCId));
            if (notFoundCourses.Any())
                throw new NotFoundException($"Os seguintes IDs de curso não foram encontrados: {string.Join(", ", notFoundCourses)}");

            student.Enrollments ??= new List<Enrollment>();
            
            student.Enrollments = student.Enrollments.Where(e => coursesToEnroll.Select(cte => cte.Id).Contains(e.CourseId)).ToList();

            foreach (var course in coursesToEnroll)
            {
                if (student.Enrollments.Any(e => e.CourseId == course.Id))
                    continue;

                student.Enrollments.Add(new Enrollment
                {
                    Student = student,
                    Course = course
                });
            }

            await _uow.SaveChangesAsync();
        }

        public async Task<List<EnrollmentDto>> GetByStudentIdAsync(Guid studentId)
        {
            var enrollments = await _repository.GetAllByStudentId(studentId);
            return enrollments.Select(e => new EnrollmentDto(e)).ToList();
        }
    }
}
