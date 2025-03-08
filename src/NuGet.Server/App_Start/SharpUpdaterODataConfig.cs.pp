using System.Diagnostics;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using CnSharp.Updater.Server.Controllers;
using CnSharp.Updater.Server.Core;
using CnSharp.Updater.Server.V2;
using NuGet.Server.Infrastructure;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.SharpUpdaterODataConfig), "Start")]

namespace $rootnamespace$.App_Start 
{
    public static class SharpUpdaterODataConfig 
    {
        public static void Start()
        {
            ServiceResolverHolder.SetServiceResolver(new SharpUpdaterServiceResolver());

            Initialize(GlobalConfiguration.Configuration, "SharpUpdaterPackagesOData");
        }

        public static void Initialize(HttpConfiguration config, string controllerName)
        {
            SharpUpdaterV2WebApiEnabler.UseSharpUpdaterV2WebApiFeed(
                config,
                "SharpUpdaterDefault",
                "sp",
                controllerName,
                enableLegacyPushRoute: true);

            config.Services.Replace(typeof(IExceptionLogger), new TraceExceptionLogger());

            // Trace.Listeners.Add(new TextWriterTraceListener(HostingEnvironment.MapPath("~/SharpUpdater.Server.log")));
            // Trace.AutoFlush = true;

            config.Routes.MapHttpRoute(
                name: "SharpUpdaterDefault_ClearCache",
                routeTemplate: "sp/clear-cache",
                defaults: new { controller = controllerName, action = nameof(SharpUpdaterPackagesODataController.ClearCache) },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
            );
        }
    }
}
