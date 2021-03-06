﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Data.Services;
using System.ServiceModel.Activation;
using System.Web.Routing;
using CnSharp.Updater.Server.Publishing;
using RouteMagic;


namespace CnSharp.Updater.Server.DataServices
{
    public static class SharpUpdaterRoutes
    {
        public static void Start()
        {
            ServiceResolver.SetServiceResolver(new DefaultServiceResolver());
            RouteTable.Routes.Clear();
            MapRoutes(RouteTable.Routes);
        }

   

        private static void MapRoutes(RouteCollection routes)
        {
            // Route to create a new package
            routes.MapDelegate("CreatePackage-Root",
                               "",
                               new { httpMethod = new HttpMethodConstraint("PUT") },
                               context => CreatePackageService().CreatePackage(context.HttpContext));

            routes.MapDelegate("CreatePackage",
                               "api/v2/package",
                               new { httpMethod = new HttpMethodConstraint("PUT") },
                               context => CreatePackageService().CreatePackage(context.HttpContext));
            
            // Route to delete packages
            routes.MapDelegate("DeletePackage-Root",
                                           "{packageId}/{version}",
                                           new { httpMethod = new HttpMethodConstraint("DELETE") },
                                           context => CreatePackageService().DeletePackage(context.HttpContext));
            
            routes.MapDelegate("DeletePackage",
                               "api/v2/package/{packageId}/{version}",
                               new { httpMethod = new HttpMethodConstraint("DELETE") },
                               context => CreatePackageService().DeletePackage(context.HttpContext));

            // Route to get packages
            routes.MapDelegate("DownloadPackage",
                               "api/v2/package/{packageId}/{version}",
                               new { httpMethod = new HttpMethodConstraint("GET") },
                               context => CreatePackageService().DownloadPackage(context.HttpContext));

            // Route to clear package cache
            routes.MapDelegate("ClearPackageCache",
                               Constants.UriId+"/api/clear-cache",
                               new { httpMethod = new HttpMethodConstraint("GET") },
                               context => CreatePackageService().ClearCache(context.HttpContext));

#if DEBUG
            // Route to create a new package(http://{root}/sp)
            routes.MapDelegate("CreatePackage"+Constants.ProductName,
                                Constants.UriId,
                               new { httpMethod = new HttpMethodConstraint("PUT") },
                               context => CreatePackageService().CreatePackage(context.HttpContext));

            // The default route is http://{root}/sp/Packages
            var factory = new DataServiceHostFactory();
            var serviceRoute = new ServiceRoute(Constants.UriId, factory, typeof(Packages));
            serviceRoute.Defaults = new RouteValueDictionary { { "serviceType", "odata" } };
            serviceRoute.Constraints = new RouteValueDictionary { { "serviceType", "odata" } };
            routes.Add(Constants.UriId, serviceRoute);
#endif

        }

        private static IPackageService CreatePackageService()
        {
            return ServiceResolver.Resolve<IPackageService>();
        }
    }
}
