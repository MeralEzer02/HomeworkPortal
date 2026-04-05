using HomeworkPortal.API.Data;
using HomeworkPortal.API.Models;
namespace HomeworkPortal.API.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext context) : base(context) { }
    }
}