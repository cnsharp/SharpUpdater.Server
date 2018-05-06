# SharpUpdater.Server

The home of the SharpUpdater.Server package, a lightweight standalone SharpUpdater server.

Latest version based on [NuGet.Server](https://github.com/NuGet/NuGet.Server) 3.1.2.

### Client Tools
##### Visual Studio extension 
* [VSIX Install](https://visualstudiogallery.msdn.microsoft.com/7235fee1-a830-466b-b7be-022dc6f98aa9)
* [Source](https://github.com/cnsharp/SharpUpdater/tree/master/src/VSIX/PackingTool)

##### Updater.exe
* [Source](https://github.com/cnsharp/SharpUpdater/tree/master/src/Client/SharpUpdater)

### Files changed

* package file extension: 

  * .nupkg -> .sp
  * NuGet.Server\App_Start\NuGetODataConfig.cs
  * NuGet.Server\Core\Helpers.cs
  * NuGet.Server\Default.aspx
  * NuGet.Server\Web.config
  * NuGet.Server.Core\Core\Constants.cs

* extarcted from NuGet.Core

  * NuGet.Server.Core\Core\MemoryCache.cs
  * NuGet.Server.Core\Core\UriUtility.cs
  * NuGet.Server.Core\Core\ZipPackageAssemblyReference.cs
  * NuGet.Server.Core\Core\ZipPackageFile.cs
  * NuGet.Server.Core\Infrastructure\FileSystemExtensions.cs

* expanded

  * NuGet.Server.Core\Core\ManifestExtensions.cs
  * NuGet.Server.Core\Core\SharpPackage.cs
  * NuGet.Server.Core\Infrastructure\SharpExpandedPackageRepository.cs