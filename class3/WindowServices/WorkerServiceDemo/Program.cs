// WorkerServiceDemo - Pure Worker Service Tenmplate (.Net 8)

// Framrwork vs Core - Service Model changes 

// .Net Framework                           .Net Core (8)
// Inherit ServiceBase                      Inherit BackgroundService
// Override OnStart()                       Override ExecuteAsync()
// Manual thread management                     Use async/await and Task-based programming
// ServiceInstallers for installation         Use Worker Service template and host builder
// Tight coupling with Windows services        Cross-platform support (Windows, Linux, macOS)
// Global.aspx-style configuration           Use appsettings.json and dependency injection
// No Built-in DI                   Built-in Dependency Injection (DI) support
// System.Diagnostics for logging          Use Microsoft.Extensions.Logging for structured logging

using WorkerServiceDemo.Services;
using WorkerServiceDemo.Workers;

try
{
    var builder = Host.CreateDefaultBuilder(args)
        .ConfigureServices(services =>
        {
            services.AddSingleton<IDataService, DataService>();
            services.AddHostedService<HealthCheckWorker>();
            services.AddHostedService<DataProcessingWorker>();
            services.AddHostedService<FileWatcherWorker>();
        });

    var host = builder.Build();
    host.Run();

}
catch(Exception ex)
{
    Console.WriteLine("Fatal error: {0}", ex);
}
finally
{
       Console.WriteLine("Worker service is shutting down.");
}