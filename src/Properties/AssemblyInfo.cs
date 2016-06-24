// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Reflection;
using System.Runtime.CompilerServices;
using CnSharp.Updater.Server.DataServices;

[assembly: AssemblyTitle("SharpUpdater.Server")]
[assembly: AssemblyDescription("Web Application used to host a simple SharpUpdater feed")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("CnSharp")]
[assembly: AssemblyProduct("SharpUpdater")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("4.0.0.0")]
[assembly: AssemblyFileVersion("4.0.0.0")]


[assembly: InternalsVisibleTo("SharpUpdater.Server.Tests")]

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SharpUpdaterRoutes), "Start")]

