using Microsoft.Extensions.Logging;

namespace JancyExpressSample.Infrastructure
{
    public interface IJancyLogger
    {
        void LogInfo(string message, string category, string identifier);
        void LogError(string message, string category, string traceIdentifier);
    }

    public class JancyLogger : IJancyLogger
    {
        private readonly ILogger<JancyLogger> _logger;

        public JancyLogger(ILogger<JancyLogger> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message, string category, string identifier)
        {
            _logger.LogInformation($"[{category}] - {identifier} - {message}");
        }

        public void LogError(string message, string category, string identifier)
        {
            _logger.LogError($"[{category}] - {identifier} - {message}");
        }
    }
}