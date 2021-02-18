using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FasTnT.Azure.AlwaysOn
{
    public class PingEpcisRepository
    {
        private readonly EpcisRepositoryService _repositoryService;
        private readonly ILogger<PingEpcisRepository> _logger;

        public PingEpcisRepository(EpcisRepositoryService repositoryService, ILogger<PingEpcisRepository> logger)
        {
            _repositoryService = repositoryService;
            _logger = logger;
        }

        [FunctionName(nameof(PingEpcisRepository))]
        public async Task Run([TimerTrigger("%PingEpcisRepository.Trigger.Cron%")] TimerInfo timer, CancellationToken cancellationToken)
        {
            var executionTime = DateTime.UtcNow;

            _logger.LogInformation($"Function {nameof(PingEpcisRepository)} executed at {executionTime}");
            _logger.LogInformation($"Next execution at {timer.Schedule.GetNextOccurrence(executionTime)}");

            await _repositoryService.PingAsync(cancellationToken);

            _logger.LogInformation("Request successful.");
        }
    }
}
