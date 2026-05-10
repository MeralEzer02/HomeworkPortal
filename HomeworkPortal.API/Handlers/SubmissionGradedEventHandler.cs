using MediatR;
using HomeworkPortal.API.Events;
using HomeworkPortal.API.Services;

namespace HomeworkPortal.API.Handlers
{
    public class SubmissionGradedEventHandler : INotificationHandler<SubmissionGradedEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly IActionLogService _actionLogService;
        private readonly IProgressUpdateQueue _progressQueue;

        public SubmissionGradedEventHandler(
            INotificationService notificationService,
            IActionLogService actionLogService,
            IProgressUpdateQueue progressQueue)
        {
            _notificationService = notificationService;
            _actionLogService = actionLogService;
            _progressQueue = progressQueue;
        }

        public async Task Handle(SubmissionGradedEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.CreateNotificationAsync(
                notification.StudentId,
                $"{notification.CourseName} dersindeki '{notification.AssignmentTitle}' ödeviniz notlandırıldı. Notunuz: {notification.Grade}"            );

            await _actionLogService.LogActionAsync(
                notification.TeacherId,
                "SUBMISSION_GRADED",
                $"Bir öğrenci teslimi notlandırıldı. Verilen Not: {notification.Grade}",
                "Submissions",
                notification.SubmissionId
            );

            await _progressQueue.QueueWorkItemAsync(new ProgressMessage
            {
                CourseId = notification.CourseId,
                ActionType = "GRADED",
                StudentId = notification.StudentId
            });
        }
    }
}