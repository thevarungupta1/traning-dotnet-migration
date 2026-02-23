# Class Library
- a class library is a reusable unit of business logic with no UI and no entry point

# in modern .net architecture. class library are used to 
- isolate domain logic
- share code across CLI, Web, Desktop and services
- enable testability, maintainability reuse


# Clear architecture
UI (CLI / GUI) -> Application -> Domain -> Infrastructure



# Targeting Multiple Framework

Why multi-target
- support legacy app (.net 4.8)
- support modern app (.net 8)
- publish a single nuget package


<PropertyGroup>
	<TargetFrameworks>net48;net8.0</TargetFrameworks>
</PropertyGroup>


#if NET8_0
 // modern API
#else
 // legacy compatibility
#endif



# NuGet packaging and versioning
Why package class library
- share internally across teams
- enforce versioning discipline
- enable CI/CD consumption
- prepare for marketplace disributtion




1.2.3

1 - major
2 - minor
3 - path / bug fix




# Exercise 1:

Class library in .net 8 - src/MathToolKit
SDK style project 
targeting multiple framework 
nuget packaging and versioning 



# build the entire solution
> dotnet build

# run all unit test
> dotnet test

# run the console app
> dotnet run --project MathToolkit.ConsoleApp

# create the nuget package
> dotnet pack src/MathToolkit --configuration Release --output ./nupkg



# What is Local NuGet Feed?
A local nuget feed is simply a folder on your machine that nuget treats like a package repository

- internal / enterprise lib
- offline development
- training & demo



mkdir C:\LocalNuGetFeed


dotnet pack src\MathToolkit --configuration Release --output C:\LocalNuGetFeed




# register the local feed with nuget

dotnet nuget add source C:\LocalNuGetFeed --name LocalFeed

dotnet nuget list source


# add package from localfeed into project
donet add package MathToolKit --version 1.0.0 --source LocalFeed




