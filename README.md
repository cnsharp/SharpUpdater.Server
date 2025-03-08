# SharpUpdater.Server

The home of the SharpUpdater.server package, a lightweight standalone SharpUpdater server extended from [Nuget.Server](https://github.com/NuGet/NuGet.Server).

## Client Tools

This project is a part of SharpUpdater Suite.

The others are:
* SharpUpdater.Core Library

Client Tools:
* SharpUpdater.CLI
* Visual Studio extension
* Updaters (updater.exe)

You can find them in [SharpUpdater Home Repository](https://github.com/cnsharp/SharpUpdater).

## Installation
1. Create a empty ASP.NET Web Application (.NET Framework) project in Visual Studio. Select the framework of '.NET Framework 4.6.2' or higher.
Note that the project template not default installed in VS2022, 
so you meight need to install it manually by searching the option '.NET Framework project and item templates' of workload 'ASP.NET and web development' in
Visual Studio Installer.
1. Install the SharpUpdater.Server NuGet package into your project.
	```csharp
	Install-Package SharpUpdater.Server
	```
1. Set `apiKey` in web.config.
1. Deploy it to your IIS.
1. Then you can push your packages by [SharpUpdater.CLI](https://www.nuget.org/packages/SharpUpdater.CLI/) or [Visual Studio extension](https://marketplace.visualstudio.com/items?itemName=CnSharpStudio.SharpUpdater).

## Feedback

If you're having trouble with the SharpUpdater.Server, file a bug on the [SharpUpdater.Server Issue Tracker](https://github.com/cnsharp/SharpUpdater.Server/issues). 

If you're having trouble with the SharpUpdater client tools (the Visual Studio extension, SharpUpdater.CLI, etc.), file a bug on [SharpUpdater Issue Tacker](https://github.com/cnsharp/SharpUpdater/issues).