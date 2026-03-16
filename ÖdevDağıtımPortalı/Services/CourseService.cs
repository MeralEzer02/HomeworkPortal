using AutoMapper;
using ÖdevDağıtım.API.DTOs;
using ÖdevDağıtım.API.Models;
using ÖdevDağıtım.API.Repositories;

namespace ÖdevDağıtım.API.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<CourseReadDto> CreateCourseAsync(CourseCreateDto dto)
        {
            var currentUserId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(currentUserId))
                throw new UnauthorizedAccessException("Giriş yapmalısınız.");

            var newCourse = _mapper.Map<Course>(dto);
            // Öğretmen ID'sini doğrudan giriş yapan kişiden alıyoruz
            newCourse.TeacherId = currentUserId;

            // UnitOfWork üzerinden CourseRepo'ya erişim (Senin yapına uygun hali)
            await _unitOfWork.Courses.AddAsync(newCourse);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CourseReadDto>(newCourse);
        }

        public async Task DeleteCourseAsync(int courseId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null) throw new Exception("Ders bulunamadı.");

            // SAHİPLİK KONTROLÜ
            if (course.TeacherId != _currentUserService.UserId)
                throw new UnauthorizedAccessException("Sadece kendi açtığınız dersleri silebilirsiniz.");

            course.IsDeleted = true;
            await _unitOfWork.CompleteAsync();
        }

        // --- INTERFACE'TEN GELEN EKSİK METOTLAR ---

        public Task<IEnumerable<CourseReadDto>> GetAllCoursesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CourseReadDto> GetCourseByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCourseAsync(int id, CourseUpdateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task AssignTeacherAsync(int courseId, string teacherId)
        {
            throw new NotImplementedException();
        }

        public Task EnrollStudentAsync(int courseId, string studentId)
        {
            throw new NotImplementedException();
        }
    }
}