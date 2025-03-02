using System;
using System.Collections.Specialized;
using System.Web.Configuration;
using CnSharp.Updater.Server.Infrastructure;
using NuGet;
using NuGet.Server;
using NuGet.Server.Core.Infrastructure;
using NuGet.Server.Core.Logging;
using NuGet.Server.Infrastructure;

namespace CnSharp.Updater.Server.Core
{
    public sealed class SharpUpdaterServiceResolver
        : IServiceResolver, IDisposable
    {
        private readonly NuGet.Server.Core.Logging.ILogger _logger;
        private readonly CryptoHashProvider _hashProvider;
        private readonly SharpUpdaterServerPackageRepository _packageRepository;
        private readonly PackageAuthenticationService _packageAuthenticationService;
        private readonly WebConfigSettingsProvider _settingsProvider;

        public SharpUpdaterServiceResolver() : this(
            PackageUtility.PackagePhysicalPath,
            WebConfigurationManager.AppSettings)
        {
        }

        public SharpUpdaterServiceResolver(string packagePath, NameValueCollection settings) : this(
            packagePath,
            settings,
            new TraceLogger())
        {
        }

        public SharpUpdaterServiceResolver(string packagePath, NameValueCollection settings, NuGet.Server.Core.Logging.ILogger logger)
        {
            _logger = logger;

            _hashProvider = new CryptoHashProvider(NuGet.Server.Core.Constants.HashAlgorithm);

            _settingsProvider = new WebConfigSettingsProvider(settings);

            _packageRepository = new SharpUpdaterServerPackageRepository(packagePath, _hashProvider, _settingsProvider, _logger);

            _packageAuthenticationService = new PackageAuthenticationService(settings);
        }

        public object Resolve(Type type)
        {
            if (type == typeof(NuGet.Server.Core.Logging.ILogger))
            {
                return _logger;
            }

            if (type == typeof(IHashProvider))
            {
                return _hashProvider;
            }

            if (type == typeof(IServerPackageRepository))
            {
                return _packageRepository;
            }

            if (type == typeof(IPackageAuthenticationService))
            {
                return _packageAuthenticationService;
            }

            if (type == typeof(ISettingsProvider))
            {
                return _settingsProvider;
            }

            return null;
        }

        public void Dispose()
        {
            _packageRepository.Dispose();
        }
    }
}