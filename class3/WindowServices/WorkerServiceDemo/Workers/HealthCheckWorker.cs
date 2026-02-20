using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerServiceDemo.Services;

namespace WorkerServiceDemo.Workers
{
    // Periodic health check worker
    // Key concept: BackgroundService (the base of the worker service in .Net 8)
    // .Net Framework: you inherit from SystemProcess.ServiceBase and override OnStart/OnStop, managing threads manually
    // .Net core 8: you inherit from BackgroundService and override ExecuteAsync, using async/await and Task-based programming model
    // the host manage the lifetime for you via cancellation tokens, 
    public sealed class HealthCheckWorker : BackgroundService
    {
        private readonly ILogger<HealthCheckWorker> _logger;
        private readonly IDataService _dataService;
        private readonly TimeSpan _healthCheckInterval = TimeSpan.FromSeconds(30);

        public HealthCheckWorker(ILogger<HealthCheckWorker> logger, IDataService dataService)
        {
            _logger = logger;
            _dataService = dataService;   
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("HealthCheckWorker started at : {Time}", DateTimeOffset.Now);

            // PeriodicTimer is th modern .Net 8 replacement for thread.Sleep / Timer Callback
            using var timer = new PeriodicTimer(_healthCheckInterval);
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var health = await _dataService.CheckHealthAsync(stoppingToken);
                    if(health.IsHealthy)
                    {
                        _logger.LogInformation("Health Check Passed: {Description} at {CheckedAt}", health.Description, health.CheckedAt);
                    }
                    else
                    {
                        _logger.LogWarning("Health Check Failed: {Description} at {CheckedAt}", health.Description, health.CheckedAt);
                    }
                }
                catch(OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break; // Graceful shutdown
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during health check");
                }
        }
    }
}
