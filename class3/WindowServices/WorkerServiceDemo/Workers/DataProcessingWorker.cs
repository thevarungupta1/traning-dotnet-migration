using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerServiceDemo.Services;

namespace WorkerServiceDemo.Workers
{
    // Processes queued work item on a schedule
    // Demonstrate the worker service pattern: multiple Background service implementation
    // register in DI, each running independenlty - something that was cumbersome in 
    // .net framwork where you had a single serviceBase with onStart/onStop manging
    // all thread manually
    public sealed class DataProcessingWorker : BackgroundService
    {
        private readonly ILogger<DataProcessingWorker> _logger;
        private readonly IDataService _dataService;
        private readonly TimeSpan _pollingInterva = TimeSpan.FromSeconds(10);

        public DataProcessingWorker(ILogger<DataProcessingWorker> logger, IDataService dataService)
        {
            _logger = logger;
            _dataService = dataService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DataProcessingWorker started at : {Time}", DateTimeOffset.Now);
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var items = await _dataService.GetPendingItemsAsync(stoppingToken);
                    if (items.Count > 0)
                    {
                        _logger.LogInformation("Found {Count} pending items to process", items.Count);
                        foreach (var item in items)
                        {
                            if (stoppingToken.IsCancellationRequested) break;
                            await _dataService.ProcessItemAsync(item, stoppingToken);

                            // Simulate Work
                            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                        }
                    }
                    else
                    {
                        _logger.LogInformation("No pending items found at {Time}", DateTimeOffset.Now);
                    }

                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break; // Graceful shutdown
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing items");
                }
                await Task.Delay(_pollingInterva, stoppingToken);
            }
            _logger.LogInformation("DataProcessingWorker stopping at : {Time}", DateTimeOffset.Now);
        }
    }
}
