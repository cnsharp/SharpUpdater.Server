// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 
namespace NuGet.Server.Core
{
    public static class Constants
    {
        public const string HashAlgorithm = "SHA512";

        public const string PackageExtension = ".sp";

        public const string PackageFilter = "*.sp";

        public const string UrlSegment = "sp";

        /// <summary>
        /// Represents the ".sp.sha512" extension.
        /// </summary>
        public static readonly string HashFileExtension = PackageExtension + ".sha512";
    }
}