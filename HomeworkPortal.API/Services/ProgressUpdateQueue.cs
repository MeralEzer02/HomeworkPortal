using System.Threading.Channels;

namespace HomeworkPortal.API.Services
{
    public class ProgressMessage
    {
        public int CourseId { get; set; }
        public string ActionType { get; set; } = string.Empty; 
        public string? StudentId { get; set; }
    }

    public interface IProgressUpdateQueue
    {
        ValueTask QueueWorkItemAsync(ProgressMessage message);
        ValueTask<ProgressMessage> DequeueAsync(CancellationToken cancellationToken);
    }

    public class ProgressUpdateQueue : IProgressUpdateQueue
    {
        private readonly Channel<ProgressMessage> _queue;

        public ProgressUpdateQueue()
        {
            var options = new BoundedChannelOptions(1000) { FullMode = BoundedChannelFullMode.Wait };
            _queue = Channel.CreateBounded<ProgressMessage>(options);
        }

        public async ValueTask QueueWorkItemAsync(ProgressMessage message)
        {
            await _queue.Writer.WriteAsync(message);
        }

        public async ValueTask<ProgressMessage> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}