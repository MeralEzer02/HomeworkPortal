using HomeworkPortal.API.Data;
using HomeworkPortal.API.Models;
namespace HomeworkPortal.API.Repositories
{
    public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(AppDbContext context) : base(context) { }
    }
}