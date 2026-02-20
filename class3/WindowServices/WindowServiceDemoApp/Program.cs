// WindowsService - Windows-Specific Hosting Model (.net 8)

// Hosting Model: UseWindowsService()
// Integrates the .Net Generic Host with the Windows Control Manager
// When run from the SCM the app behave as a proper windows service; 
// when launched from the command line ir runs as a console app.
// making local development painless.

// Install / uninstall the service: (as an adminstrator)
// sc.exe create WindowsServuceDempApp binPath= "C:\publish\WindowsServiceDemoApp.exe"
// sc.exec start WindowsServuceDempApp
// sc.exe stop WindowsServuceDempApp
// sc.exe delete WindowsServuceDempApp

// Publish
// dotnet publish -c Release -r win-x64 --self-contained



using WindowServiceDemoApp;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
