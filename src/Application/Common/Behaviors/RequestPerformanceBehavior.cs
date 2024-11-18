using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors
{
    public class RequestPerformanceBehaviour<TRequest, TResponse>(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer = new();

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if(elapsedMilliseconds > 1000)
            {
                var requestName = typeof(TRequest).Name;
                var userId = currentUserService.UserId;

                logger.LogWarning("Long Running Request detected: {Name} ({ElapsedMilliseconds} milliseconds), UserId: {@UserId}, Request: {@Request}",
                    requestName, elapsedMilliseconds, userId, request);
            }

            return response;
        }
    }
}