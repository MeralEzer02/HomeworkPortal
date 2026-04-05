using HomeworkPortal.API.Data;
using HomeworkPortal.API.Models;
namespace HomeworkPortal.API.Repositories
{
    public class SubmissionRepository : GenericRepository<Submission>, ISubmissionRepository
    {
        public SubmissionRepository(AppDbContext context) : base(context) { }
    }
}