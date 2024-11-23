using Appointr.Helper.Implementation;
using Appointr.Helper.Interface;

namespace Appointr.Middlewares
{
    public class ConcurrentRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private static int _requestCount = 0;
        private static readonly object _lock = new object();
        private readonly int _maxRequests;
        private readonly IConsoleHelper _consoleHelper;
        private readonly ILogger<ConcurrentRequestMiddleware> _logger;

        public ConcurrentRequestMiddleware(RequestDelegate next, int maxRequests, IConsoleHelper consoleHelper, ILogger<ConcurrentRequestMiddleware> logger)
        {
            _next = next;
            _maxRequests = maxRequests;
            _consoleHelper = consoleHelper;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            bool isRequestAllowed = false;

            lock (_lock)
            {
                if (_requestCount < _maxRequests)
                {
                    _requestCount++;
                    isRequestAllowed = true;
                }
            }

            if (isRequestAllowed)
            {
                try
                {
                    await _next(context);
                }
                finally
                {
                    lock (_lock)
                    {
                        _requestCount--;
                    }
                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                LogRequestLimitWarning("[+] The maximum number of concurrent requests has been reached.");
                await context.Response.WriteAsync("The maximum number of concurrent requests has been reached. Please try again later.");
            }
        }

        private void LogRequestLimitWarning(string message)
        {
            var formattedMessage = _consoleHelper.Decorate(ConsoleHelper.BOLD, ConsoleHelper.BACKGROUND_COLORS["black"], ConsoleHelper.COLORS["red"], message);
            _logger.LogWarning(formattedMessage);
        }
    }
}
