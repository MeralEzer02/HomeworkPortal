using MediatR;
using HomeworkPortal.API.Events;
using HomeworkPortal.API.Services;

namespace HomeworkPortal.API.Handlers
{
    public class SubmissionCreatedEventHandler : INotificationHandler<SubmissionCreatedEvent>
    {
        private readonly IActionLogService _actionLogService;
        private readonly IProgressUpdateQueue _progressQueue;
        private readonly IBadgeService _badgeService;

        public SubmissionCreatedEventHandler(
            IActionLogService actionLogService,
            IProgressUpdateQueue progressQueue,
            IBadgeService badgeService)
        {
            _actionLogService = actionLogService;
            _progressQueue = progressQueue;
            _badgeService = badgeService;
        }

        public async Task Handle(SubmissionCreatedEvent notification, CancellationToken cancellationToken)
        {
            await _actionLogService.LogActionAsync(
                notification.StudentId,
                "SUBMISSION_CREATED",
                $"'{notification.AssignmentTitle}' isimli ödev için yeni bir teslim dosyası yüklendi.",
                "Submissions",
                notification.SubmissionId
            );

            await _progressQueue.QueueWorkItemAsync(new ProgressMessage
            {
                CourseId = notification.CourseId,
                ActionType = "SUBMITTED",
                StudentId = notification.StudentId
            });

            await _badgeService.CheckAndAwardBadgesAsync(notification.StudentId);
        }
    }
}