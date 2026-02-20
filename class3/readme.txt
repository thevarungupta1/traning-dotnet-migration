# Windows Services in .net 8
a windows services is a background process that runs without UI and start automatically with windows

In .net framework windows services used
 - serviceBase
- onStart() / onStop()
- manual threading
- no built-in DI or logging

In .Net 8 services use:
- Generic host
- Worker Service template
- dependency injection
- cross platform support



Worker service template
> dotnet new worker -n MyService

- Program.cs
- Worker.cs
- appsettings.json



# Hosting Models: Windows vs Cross-Platform


- Service Control Manager 
- automatic startup
- windows only



Cross Platform hosting
same code can run as 
- console app
- docker container
- Linux service
- Kubernetes pod




# WorkerServiceDemo
- BackgoundService + ExecuteAsync
- Generic Host + DI + ILogger
- Multiple Workers running in parallel
- PeriodicTimer + CancellatonToken

