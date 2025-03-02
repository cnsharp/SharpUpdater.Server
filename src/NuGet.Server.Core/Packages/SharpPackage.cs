using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Xml;
using CnSharp.Updater.Util;
using NuGet;
using NuGet.Resources;
using MemoryCache = CnSharp.Updater.Server.Utility.MemoryCache;
using PackageBuilder = CnSharp.Updater.Packaging.PackageBuilder;

namespace CnSharp.Updater.Server.Packages
{
    public class SharpPackage : OptimizedZipPackage
    {
        private const string CacheKeyFormat = "SHARPUPDATER_ZIP_PACKAGE_{0}_{1}{2}";
        private const string AssembliesCacheKey = "ASSEMBLIES";
        private const string FilesCacheKey = "FILES";
        private static readonly TimeSpan CacheTimeout = TimeSpan.FromSeconds(15);
        private readonly bool _enableCaching;
        private static readonly string[] ExcludePaths = new[] { "_rels", "package" };
        private readonly Func<Stream> _streamFactory;
        public override Stream GetStream()
        {
            return _streamFactory();
        }

        public SharpPackage(string filePath) : base(filePath)
        {
            _streamFactory = () => File.OpenRead(filePath);
            using (var stream = _streamFactory())
            {
                EnsureManifest(() => GetManifestStream(stream));
            }
        }

        public SharpPackage(IFileSystem fileSystem, string filePath, IFileSystem expandedFileSystem) : base(fileSystem, filePath, expandedFileSystem)
        {
            _streamFactory = () => File.OpenRead(filePath);
            using (var stream = _streamFactory())
            {
                EnsureManifest(() => GetManifestStream(stream));
            }
        }

        //public SharpPackage(Func<Stream> packageStreamFactory, Func<Stream> manifestStreamFactory)
        //{
        //    if (packageStreamFactory == null)
        //    {
        //        throw new ArgumentNullException("packageStreamFactory");
        //    }

        //    if (manifestStreamFactory == null)
        //    {
        //        throw new ArgumentNullException("manifestStreamFactory");
        //    }

        //    _enableCaching = false;
        //    _streamFactory = packageStreamFactory;
        //    EnsureManifest(manifestStreamFactory);
        //}

        //public SharpPackage(Stream stream) 
        //{
        //    if (stream == null)
        //    {
        //        throw new ArgumentNullException("stream");
        //    }
        //    _enableCaching = false;
        //    _streamFactory = stream.ToStreamFactory();
        //    using (stream = _streamFactory())
        //    {
        //        EnsureManifest(() => GetManifestStream(stream));
        //    }
        //}

        //private SharpPackage(string filePath, bool enableCaching)
        //{
        //    if (String.IsNullOrEmpty(filePath))
        //    {
        //        throw new ArgumentException("filePath");
        //    }
        //    _enableCaching = enableCaching;
        //    _streamFactory = () => File.OpenRead(filePath);
        //    using (var stream = _streamFactory())
        //    {
        //        EnsureManifest(() => GetManifestStream(stream));
        //    }
        //}

        //internal SharpPackage(Func<Stream> streamFactory, bool enableCaching)
        //{
        //    if (streamFactory == null)
        //    {
        //        throw new ArgumentNullException("streamFactory");
        //    }
        //    _enableCaching = enableCaching;
        //    _streamFactory = streamFactory;
        //    using (var stream = _streamFactory())
        //    {
        //        EnsureManifest(() => GetManifestStream(stream));
        //    }
        //}

        public static Stream GetManifestStream(Stream packageStream) 
        {
            Package package = Package.Open(packageStream);

            return GetManifestStream(package);
        }

        public static Stream GetManifestStream(Package package)
        {

            PackageRelationship relationshipType = package.GetRelationshipsByType(PackageBuilder.PackageRelationshipNamespace + PackageBuilder.ManifestRelationType).SingleOrDefault();

            if (relationshipType == null)
            {
                throw new InvalidOperationException(NuGetResources.PackageDoesNotContainManifest);
            }

            PackagePart manifestPart = package.GetPart(relationshipType.TargetUri);

            if (manifestPart == null)
            {
                throw new InvalidOperationException(NuGetResources.PackageDoesNotContainManifest);
            }

            return manifestPart.GetStream();
        }


        private void EnsureManifest(Func<Stream> manifestStreamFactory)
        {
            using (Stream manifestStream = manifestStreamFactory())
            {
                ReadMyManifest(manifestStream);
                //todo:validate manifest
            }
        }

        protected void ReadMyManifest(Stream manifestStream)
        {
            //Manifest manifest = Manifest.ReadFrom(manifestStream, validateSchema: false);

            var bytes = manifestStream.ReadAllBytes();
            var xml = Encoding.UTF8.GetString(bytes);
            var metadata = XmlSerializerHelper.LoadObjectFromXmlString<Manifest>(xml);
          

            Id = metadata.Id;
            Version = new SemanticVersion(metadata.Version);
            Title = metadata.AppName;
            Authors = new [] { metadata.Owner};
            Owners = new [] { metadata.Owner };
            if(!string.IsNullOrWhiteSpace(metadata.IconUrl))
                IconUrl = new Uri(metadata.IconUrl);
            //LicenseUrl = metadata.LicenseUrl;
            //ProjectUrl = metadata.ProjectUrl;
            //RequireLicenseAcceptance = metadata.RequireLicenseAcceptance;
            //DevelopmentDependency = metadata.DevelopmentDependency;
            Description = metadata.Description;
            //Summary = metadata.Summary;
            ReleaseNotes = metadata.ReleaseNotes;
            Language = metadata.Language;
            Tags = metadata.Tags;
            DependencySets = metadata.DependencySets;
            //FrameworkAssemblies = metadata.FrameworkAssemblies;
            Copyright = metadata.Copyright;
            //PackageAssemblyReferences = metadata.PackageAssemblyReferences;
            //MinClientVersion = metadata.MinClientVersion;

            // Ensure tags start and end with an empty " " so we can do contains filtering reliably
            if (!String.IsNullOrEmpty(Tags))
            {
                Tags = " " + Tags + " ";
            }
        }


        public override void ExtractContents(IFileSystem fileSystem, string extractPath)
        {
            using (Stream stream = _streamFactory())
            {
                var package = Package.Open(stream);
                var packageId = GetPackageIdentifier(package);

                foreach (var part in package.GetParts()
                    .Where(p => IsPackageFile(p, packageId)))
                {
                    var relativePath = UriUtility.GetPath(part.Uri);

                    var targetPath = Path.Combine(extractPath, relativePath);
                    using (var partStream = part.GetStream())
                    {
                        fileSystem.AddFile(targetPath, partStream);
                    }
                }
            }
        }

        public static string GetPackageIdentifier(Package package)
        {
            try
            {
                // PackageProperties can throw an XmlException when the content of an XML
                // tag in the properties file is not properly encoded.
                // If that's the case, then just return null for the package identifier
                return package.PackageProperties.Identifier;
            }
            catch (XmlException)
            {
                return null;
            }
        }

        internal static bool IsPackageFile(PackagePart part, string packageId)
        {
            string path = UriUtility.GetPath(part.Uri);
            string directory = Path.GetDirectoryName(path);

            // We exclude any opc files and the auto-generated package manifest file ({packageId}.nuspec)
            return !ExcludePaths.Any(p => directory.StartsWith(p, StringComparison.OrdinalIgnoreCase)) &&
                   !PackageHelper.IsPackageManifest(path, packageId);
        }


        protected override IEnumerable<IPackageFile> GetFilesBase()
        {
            if (_enableCaching)
            {
                return MemoryCache.Instance.GetOrAdd(GetFilesCacheKey(), GetFilesNoCache, CacheTimeout);
            }
            return GetFilesNoCache();
        }

        private List<IPackageFile> GetFilesNoCache()
        {
            using (Stream stream = _streamFactory())
            {
                Package package = Package.Open(stream);
                var packageId = GetPackageIdentifier(package);

                return (from part in package.GetParts()
                        where IsPackageFile(part, packageId)
                        select (IPackageFile)new ZipPackageFile(part)).ToList();
            }
        }

        private string GetFilesCacheKey()
        {
            return String.Format(CultureInfo.InvariantCulture, CacheKeyFormat, FilesCacheKey, Id, Version);
        }


        protected override IEnumerable<IPackageAssemblyReference> GetAssemblyReferencesCore()
        {
            if (_enableCaching)
            {
                return MemoryCache.Instance.GetOrAdd(GetAssembliesCacheKey(), GetAssembliesNoCache, CacheTimeout);
            }

            return GetAssembliesNoCache();
        }

        private string GetAssembliesCacheKey()
        {
            return String.Format(CultureInfo.InvariantCulture, CacheKeyFormat, AssembliesCacheKey, Id, Version);
        }


        private List<IPackageAssemblyReference> GetAssembliesNoCache()
        {
            return (from file in GetFiles()
                    where IsAssemblyReference(file.Path)
                    select (IPackageAssemblyReference)new ZipPackageAssemblyReference(file)).ToList();
        }
    }
}