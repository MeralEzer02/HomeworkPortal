using HomeworkPortal.API.Data;
using HomeworkPortal.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace HomeworkPortal.API.Services
{
    public class ProgressBackgroundWorker : BackgroundService
    {
        private readonly IProgressUpdateQueue _taskQueue;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ProgressBackgroundWorker> _logger;

        public ProgressBackgroundWorker(IProgressUpdateQueue taskQueue, IServiceProvider serviceProvider, ILogger<ProgressBackgroundWorker> logger)
        {
            _taskQueue = taskQueue;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🚀 Progress İşçisi (Background Worker) göreve başladı.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var message = await _taskQueue.DequeueAsync(stoppingToken);

                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    if (message.ActionType == "NEW_ASSIGNMENT")
                    {
                        await HandleNewAssignmentAsync(dbContext, message.CourseId);
                    }
                    else if (message.ActionType == "SUBMITTED")
                    {
                        await HandleSubmissionAsync(dbContext, message.CourseId, message.StudentId!);
                    }
                    else if (message.ActionType == "GRADED")
                    {
                        await HandleGradedAsync(dbContext, message.CourseId, message.StudentId!);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Arka planda metrikler güncellenirken kritik hata oluştu.");
                }
            }
        }

        private async Task HandleNewAssignmentAsync(AppDbContext context, int courseId)
        {
            var course = await context.Courses.Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null) return;

            foreach (var student in course.Students)
            {
                var progress = await context.UserCourseProgresses
                    .FirstOrDefaultAsync(p => p.CourseId == courseId && p.UserId == student.Id);

                if (progress == null)
                {
                    progress = new UserCourseProgress { UserId = student.Id, CourseId = courseId, TotalAssignments = 1 };
                    context.UserCourseProgresses.Add(progress);
                }
                else
                {
                    progress.TotalAssignments += 1;
                    progress.LastUpdated = DateTime.UtcNow;
                }
            }
            await context.SaveChangesAsync();
            _logger.LogInformation($"✅ {course.Name} kursundaki öğrencilerin toplam ödev sayıları güncellendi.");
        }

        private async Task HandleSubmissionAsync(AppDbContext context, int courseId, string studentId)
        {
            var progress = await context.UserCourseProgresses
                .FirstOrDefaultAsync(p => p.CourseId == courseId && p.UserId == studentId);

            if (progress != null)
            {
                progress.SubmittedAssignments += 1;
                progress.LastUpdated = DateTime.UtcNow;
                await context.SaveChangesAsync();
                _logger.LogInformation($"✅ {studentId} id'li öğrencinin teslim istatistiği artırıldı.");
            }
        }

        private async Task HandleGradedAsync(AppDbContext context, int courseId, string studentId)
        {
            var progress = await context.UserCourseProgresses
                .FirstOrDefaultAsync(p => p.CourseId == courseId && p.UserId == studentId);

            if (progress != null)
            {
                progress.SubmittedAssignments -= 1;
                progress.GradedAssignments += 1;   
                progress.LastUpdated = DateTime.UtcNow;
                await context.SaveChangesAsync();
                _logger.LogInformation($"✅ {studentId} id'li öğrencinin notlanma istatistiği güncellendi.");
            }
        }
    }
}