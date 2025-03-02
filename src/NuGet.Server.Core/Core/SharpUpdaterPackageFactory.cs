using CnSharp.Updater.Server.Packages;
using System;
using System.IO;

namespace NuGet.Server.Core
{
    public static class SharpUpdaterPackageFactory
    {
        public static IPackage Open(string fullPackagePath)
        {
            if (string.IsNullOrEmpty(fullPackagePath))
            {
                throw new ArgumentNullException(nameof(fullPackagePath));
            }

            var directoryName = Path.GetDirectoryName(fullPackagePath);
            var fileName = Path.GetFileName(fullPackagePath);

            var fileSystem = new PhysicalFileSystem(directoryName);

            try
            {
                return new SharpUpdaterPackage(
                    fileSystem,
                    fileName,
                    NullFileSystem.Instance);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}