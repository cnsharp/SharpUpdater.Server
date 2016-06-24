using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using NuGet;
using NuGet.Resources;

namespace CnSharp.Updater.Server.Infrastructure
{

    public class SharpExpandedPackageRepository : PackageRepositoryBase, IPackageLookup
    {
        private readonly IFileSystem _fileSystem;
        private readonly IHashProvider _hashProvider;

        public SharpExpandedPackageRepository(IFileSystem fileSystem)
            : this(fileSystem, new CryptoHashProvider())
        {
        }

        public SharpExpandedPackageRepository(
            IFileSystem fileSystem,
            IHashProvider hashProvider)
        {
            _fileSystem = fileSystem;
            _hashProvider = hashProvider;

            Logger = fileSystem.Logger;
        }

        public override string Source
        {
            get { return _fileSystem.Root; }
        }

        public override bool SupportsPrereleasePackages
        {
            get { return true; }
        }

        public override void AddPackage(IPackage package)
        {
            var packagePath = GetPackageRoot(package.Id, package.Version);
            var nupkgPath = Path.Combine(packagePath, package.Id + "." + package.Version.ToNormalizedString() + Strings.PackageExt);

            using (var stream = package.GetStream())
            {
                _fileSystem.AddFile(nupkgPath, stream);
            }

            var hashBytes = Encoding.UTF8.GetBytes(package.GetHash(_hashProvider));
            var hashFilePath = Path.ChangeExtension(nupkgPath, Constants.HashFileExtension);
            _fileSystem.AddFile(hashFilePath, hashFileStream => { hashFileStream.Write(hashBytes, 0, hashBytes.Length); });

            using (var stream = package.GetStream())
            {
                using (var manifestStream = SharpPackage.GetManifestStream(stream))
                {
                    var manifestPath = Path.Combine(packagePath, package.Id + Manifest.ManifestExt);
                    _fileSystem.AddFile(manifestPath, manifestStream);
                }
            }
        }

        public override void RemovePackage(IPackage package)
        {
            if (Exists(package.Id, package.Version))
            {
                var packagePath = GetPackageRoot(package.Id, package.Version);
                _fileSystem.DeleteDirectorySafe(packagePath, recursive: true);
            }
        }

        public bool Exists(string packageId, SemanticVersion version)
        {
            var hashFilePath = Path.ChangeExtension(GetPackagePath(packageId, version), Constants.HashFileExtension);
            return _fileSystem.FileExists(hashFilePath);
        }

        public IPackage FindPackage(string packageId, SemanticVersion version)
        {
            if (!Exists(packageId, version))
            {
                return null;
            }

            return GetPackageInternal(packageId, version);
        }

        public IEnumerable<IPackage> FindPackagesById(string packageId)
        {
            foreach (var versionDirectory in _fileSystem.GetDirectoriesSafe(packageId))
            {
                var versionDirectoryName = Path.GetFileName(versionDirectory);
                SemanticVersion version;
                if (SemanticVersion.TryParse(versionDirectoryName, out version) &&
                    Exists(packageId, version))
                {
                    IPackage package = null;

                    try
                    {
                        package = GetPackageInternal(packageId, version);
                    }
                    catch (XmlException ex)
                    {
                        Logger.Log(MessageLevel.Warning, ex.Message);
                        Logger.Log(
                            MessageLevel.Warning,
                            NuGetResources.Manifest_NotFound,
                            string.Format("{0}/{1}", packageId, version));
                        continue;
                    }
                    catch (IOException ex)
                    {
                        Logger.Log(MessageLevel.Warning, ex.Message);
                        Logger.Log(
                            MessageLevel.Warning,
                            NuGetResources.Manifest_NotFound,
                            string.Format("{0}/{1}", packageId, version));
                        continue;
                    }

                    yield return package;
                }
            }
        }

        public override IQueryable<IPackage> GetPackages()
        {
            return _fileSystem.GetDirectoriesSafe(path: string.Empty)
                .SelectMany(packageDirectory =>
                {
                    var packageId = Path.GetFileName(packageDirectory);
                    return FindPackagesById(packageId);
                }).AsQueryable();
        }

        private static string GetPackageRoot(string packageId, SemanticVersion version)
        {
            return Path.Combine(packageId, version.ToNormalizedString());
        }

        private IPackage GetPackageInternal(string packageId, SemanticVersion version)
        {
            var packagePath = GetPackagePath(packageId, version);
            var manifestPath = Path.Combine(GetPackageRoot(packageId, version), packageId + Manifest.ManifestExt);
            return new SharpPackage(() => _fileSystem.OpenFile(packagePath), () => _fileSystem.OpenFile(manifestPath));
        }

        private static string GetPackagePath(string packageId, SemanticVersion version)
        {
            return Path.Combine(
                GetPackageRoot(packageId, version),
                packageId + "." + version.ToNormalizedString() + Constants.PackageExtension);
        }
    }
}