using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServiceDemo.Services
{
    public class DataService : IDataService
    {
        private readonly ILogger<DataService> _logger;
        private readonly List<string> _queue = [];

        public DataService(ILogger<DataService> logger)
        {
            _logger = logger;
        }
        public Task<HealthStatus> CheckHealthAsync(CancellationToken cancellationToken)
        {
            var status = new HealthStatus(
                IsHealthy: true,
                Description: $"DataService operational - {_queue.Count} items pending ",
                CheckedAt: DateTime.UtcNow
            );
            return Task.FromResult(status);
        }

        public Task<IReadOnlyList<string>> GetPendingItemsAsync(CancellationToken cancellationToken)
        {
            IReadOnlyList<string> pendingItems = _queue.AsReadOnly();
            return Task.FromResult(pendingItems);
        }

        public Task ProcessItemAsync(string item, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing item: {Item}", item);
            _queue.Remove(item);
            return Task.CompletedTask;
        }
        private void SeedDemoData()
        {
            for(var i = 1; i <= 10; i++)
            {
                _queue.Add($"WorkerItem-{i:D3}");
            }
        }
    }
}
