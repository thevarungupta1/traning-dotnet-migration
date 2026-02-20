using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerServiceDemo.Services;

namespace WorkerServiceDemo.Workers
{
    public sealed class FileWatcherWorker : BackgroundService
    {
        private readonly ILogger<HealthCheckWorker> _logger;
        private readonly string _watchPath;
        private FileSystemWatcher _watcher;
        public FileWatcherWorker(ILogger<HealthCheckWorker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _watchPath = configuration.GetValue<string>("FileWatcher:Path")
                ?? Path.GetTempFileName();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
           _logger.LogInformation("FileWatcherWorker started, watching path: {Path}", _watchPath);
            if(!Directory.Exists(_watchPath))
            {   
                Directory.CreateDirectory(_watchPath);
                _logger.LogInformation("Created directory to watch: {Path}", _watchPath);
            }
            _watcher = new FileSystemWatcher(_watchPath)
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                Filter = "*.*",
                EnableRaisingEvents = true
            };

            _watcher.Created += (_, e) => 
            _logger.LogInformation("File created: {FileName} at {Time}", e.FullPath, DateTimeOffset.Now);
            
            _watcher.Changed += (_, e) =>
                _logger.LogInformation("File changed: {FileName} at {Time}", e.FullPath, DateTimeOffset.Now);

            _watcher.Deleted += (_, e) =>
                _logger.LogInformation("File deleted: {FileName} at {Time}", e.FullPath, DateTimeOffset.Now);

            _watcher.Error += (_, e) =>
                _logger.LogError(e.GetException(), "File watcher error at {Time}", DateTimeOffset.Now);

            stoppingToken.Register(() =>
            {
                _logger.LogInformation("FileWatcherWorker stopping, disposing watcher at {Time}", DateTimeOffset.Now);
                _watcher.Dispose();
            });

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _watcher?.Dispose();
            base.Dispose();
        }
    }
}
