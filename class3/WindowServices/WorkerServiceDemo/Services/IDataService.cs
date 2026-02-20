using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServiceDemo.Services
{
    public interface IDataService
    {
        Task<IReadOnlyList<string>> GetPendingItemsAsync(CancellationToken cancellationToken);
        Task ProcessItemAsync(string item, CancellationToken cancellationToken);
        Task<HealthStatus> CheckHealthAsync(CancellationToken cancellationToken);
    }

    public record HealthStatus(bool IsHealthy, string Description, DateTime CheckedAt);
}
