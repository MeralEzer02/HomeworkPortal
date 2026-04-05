using Polly;
using Polly.Retry;
using Microsoft.Extensions.Logging;

namespace HomeworkPortal.API.Helpers
{
    public static class RetryHelper
    {
        public static AsyncRetryPolicy CreateRetryPolicy<T>(ILogger<T> logger)
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3, 
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        logger.LogWarning($"[TEKRAR DENENİYOR] İşlem başarısız oldu: {exception.Message}. {timeSpan.TotalSeconds} saniye beklendi. Deneme sayısı: {retryCount}");
                    });
        }
    }
}