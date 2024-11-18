using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors
{
    public class RequestLogger<TRequest>(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger = logger;

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = currentUserService.UserId;

            _logger.LogInformation("Request: {Name}, UserId: {@UserId}, {@Request}", requestName, userId, request);

            await Task.CompletedTask;
        }
    }
}