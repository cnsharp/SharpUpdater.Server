// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using CnSharp.Updater.Server.Infrastructure;
using CnSharp.Updater.Server.Logging;
using CnSharp.Updater.Server.Publishing;
using NuGet;

namespace CnSharp.Updater.Server
{
    public class DefaultServiceResolver
        : IServiceResolver
    {
        private readonly IHashProvider _hashProvider;
        private readonly IPackageService _packageService;
        private readonly IServerPackageRepository _packageRepository;

        public DefaultServiceResolver()
        {
            _hashProvider = new CryptoHashProvider(Constants.HashAlgorithm);

            _packageRepository = new ServerPackageRepository(PackageUtility.PackagePhysicalPath,  _hashProvider, new TraceLogger());

            _packageService = new PackageService(_packageRepository, new PackageAuthenticationService());
        }

        public object Resolve(Type type)
        {
            if (type == typeof(IHashProvider))
            {
                return _hashProvider;
            }

            if (type == typeof(IPackageService))
            {
                return _packageService;
            }

            if (type == typeof(IServerPackageRepository))
            {
                return _packageRepository;
            }

            return null;
        }
    }
}