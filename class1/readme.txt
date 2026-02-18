# .Net Framework vs Dotnet Core

- Cross Platform Compatibility
- Open Source
- Performance and Modernization
- LTS


# .Net Core Characteristics
- High Performance 
- Open source
- Cross platform
- Lightweight and Modular
- Build-in Dependency Injection
- Cloud Ready
- Middleware



# What the ASP.Net core doesnot have?
- Global.asax file -> Programe.cs file
- Web.config -> appsettings.json (appsettings.{environment}.json)
- HTTP Handlers and HTTP Modules -> Middleware components in asp.net core request pipeline





# .Net Framework vs .Net Core Framework


.Net Framework (4, 4.5) / Lagecy
- designed exclusively for windows
- Monolithic, including all features by default, leading to larger application
- does not match the performance optimization of .net core sue to its older architecture
- it is not open source
- ideal for app tighly integrated with windows ecosystem


.Net Core  / Dotnet Core (1-10)
- cross platform
- modular, allow developer to include only necessary packages viw NuGet
- Optimized for high performance
- open source
- building modern web app, microservices, app required cross platyform fucntinality (MAUI / Blazer)





# CLI Improvements in .Net Core 8/9

dotnet new console [template] --name [project name]		- create project
							
dotnet build							- compile

dotnet run							- run 

dotnet watch run						- hot reload

dotnet publish							- create deployment package

dotnet restore							- restore nuget packages

# Runtime enhancement 
- Faster startup
- reduced memory usage
- native AOT (Ahead-of time compiler)
- better disgnostics
- Improved threading and GC



# Old (.net framework)

.csproj

<Project ToolVersion="">

</Project>


# New SDK style

<Project Sdk="Microsoft.Net.Sdk">

	<PropertyGroup>
		<TargetFramework>net8.0</TargetFramework>
	</PropertyGroup>
	
</Project>




# Exercise 1: Enterprise level logging & log handling implementation for .net core 8 console app

- Structured logging (JSON)
- Multiple sinks (console + files)
- log level by environment
- correlation ID support
- dependency injection
- config based logging
- middleware
- clear architecture friendly


  <PackageReference Include="Microsoft.Extensions.Hosting" Version="8.0.0" />
  <PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />
  <PackageReference Include="Serilog.Enrichers.Environment" Version="3.0.1" />
  <PackageReference Include="Serilog.Enrichers.Process" Version="3.0.0" />
  <PackageReference Include="Serilog.Enrichers.Thread" Version="4.0.0" />
  <PackageReference Include="Serilog.Settings.Configuration" Version="10.0.0" />
  <PackageReference Include="Serilog.Sinks.Console" Version="6.1.1" />
  <PackageReference Include="Serilog.Sinks.File" Version="7.0.0" />


Step 1: Create .Net 8 Console App

dotnet new console -n EnterpriseLoggingApp
cd EnterpriseLoggingApp
code .


Step 2: Install Required Packages

dotnet add package Microsoft.Extensions.Hosting
dotnet add package Serilog.AspNetCore



{timestamp:HH:mm:ss} - {log level} - {message} {}



Step 3: Add appsettings.json

appsettings.json

{
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Miscosoft": "Warning",
        "System": "Warning"
      }
    },
    "Enrich": [
      "FromLogContext",
      "WithMachineName",
      "WithThreadId",
      "WithProcessId"
    ],
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3} {Message:lj} {Properties:j}{NewLine}{Exception}]"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.json",
          "rollingInterval": "Day",
          "formatter": "Serilog.Formatting.Json.JsonFormatter, Serilog"
        }
      }
    ],
    "Properties": {
      "Application": "EnterpriseLoggingApp",
      "Environment": "Development"
    }
  }
}


Step 4: Configure Host + Serilog (Program.cs)






appsettings.Production.json

{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Warning"
    }
  }
}





dotnet run

dotnet run --environment Production




https://learn.microsoft.com/en-us/dotnet/core/porting/upgrade-assistant-overview


> dotnet tool install -g upgrade-assistant


dotnet tool 		// using dotnet cli tool mamangement
install 		// install a tool
-g 			// global
upgrade-assistant 	// tool name


> upgrade-assistant --version  or  upgrade-assistant

> dotnet tool update -g upgrade-assistant


> upgrade-assistant upgrade MyProject.csproj


1. detect project type
2. ask target framework (eg .net 8)
3. suggest upgrade steps
4. execute steps one by one


- Backup project
- convert project files
- update packages
- update target framework




> upgrade-assistant upgrade MyProject.sln

> upgrade-assistant upgrade MyProject.sln --non-interactive

> upgrade-assistant upgrade MyProject.sln --target net8.0


> upgrade-assistant upgrade SampleConsoleApp.csproj




# Option 2



Step 1: Create .net 8 console app

> dotnet new console -n ModernSampleConsoleApp


Step 2: Add modern packages
> dotnet add package Microsoft.Extensions.Hosting
> dotnet add package Microsoft.Extensions.Configuration.Json



ModernSampleConsoleApp
|--- Progam.cs
|--- Services/
|--- appsettings.json






















