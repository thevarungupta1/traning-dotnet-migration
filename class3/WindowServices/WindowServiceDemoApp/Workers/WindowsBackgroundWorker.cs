using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using WindowServiceDemoApp.Services;

namespace WindowServiceDemoApp.Workers
{
    public sealed class WindowsBackgroundWorker : BackgroundService
    {
        private readonly ILogger<WindowsBackgroundWorker> _logger;
        private readonly IEventLogService _eventLogService;
        private readonly IWindowsSystemInfoService _windowsSystemInfoService;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(10);

        public WindowsBackgroundWorker(
            ILogger<WindowsBackgroundWorker> logger,
            IEventLogService eventLogService,
            IWindowsSystemInfoService windowsSystemInfoService)
        {
            _logger = logger;
            _eventLogService = eventLogService;
            _windowsSystemInfoService = windowsSystemInfoService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var osVersion = _windowsSystemInfoService.GetOSVersion();
            var machineName = _windowsSystemInfoService.GetMachineName();

            _logger.LogInformation("WindowsBackgroundWorker starting on machine {MachineName} with OS {OSVersion}",
                machineName, osVersion);

            _logger.LogInformation("WindowsBackgroundWorker will log system uptime every {Interval} seconds",
                _interval.TotalSeconds);

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_interval);
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var uptimeMs = _windowsSystemInfoService.GetSystemUpTimeMs();
                    var uptime = TimeSpan.FromMilliseconds(uptimeMs);

                    _logger.LogInformation("System uptime: {Days}d {Hours}h {Minutes}m", uptime.Days, uptime.Hours, uptime.Minutes);

                    _eventLogService.WriteInformation(
                        $"Heartbeat: System uptime is {uptime.Days}d {uptime.Hours}h {uptime.Minutes}m");
                }
                catch(OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    // expected during shutdown, safe to ignore
                    break;
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while logging system uptime");
                    _eventLogService.WriteError($"Error in WindowsBackgroundWorker: {ex.Message}");
                }
                await timer.WaitForNextTickAsync(stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
            {
                _logger.LogInformation("WindowsBackgroundWorker is stopping.");
            _eventLogService.WriteInformation("WindowsBackgroundWorker is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
