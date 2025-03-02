<%@ Page Language="C#" %>
<%@ Import Namespace="CnSharp.Updater.Server.App_Start" %>
<%@ Import Namespace="CnSharp.Updater.Server.Core" %>
<%@ Import Namespace="NuGet.Server" %>
<%@ Import Namespace="NuGet.Server.Infrastructure" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SharpUpdater Private Repository</title>
    <style>
        body { font-family: Calibri; }
    </style>
</head>
<body>
    <div>
        <h2>You are running <%= typeof(SharpUpdaterODataConfig).Assembly.GetName().Name %> v<%= typeof(SharpUpdaterODataConfig).Assembly.GetName().Version %></h2>
        <p>
            Click <a href="<%= VirtualPathUtility.ToAbsolute("~/sp/Packages") %>">here</a> to view your packages.
        </p>
        <fieldset style="width:800px">
            <legend><strong>Repository URLs</strong></legend>
            In the package manager settings, add the following URL to the list of 
            Package Sources:
            <blockquote>
                <strong><%=
                // Request Validation is ON by default in Asp.NET: https://learn.microsoft.com/en-us/previous-versions/aspnet/hh882339(v=vs.110) & https://learn.microsoft.com/en-us/aspnet/whitepapers/request-validation
                // CodeQL [SM02175] False Positive: Url is validated
                // CodeQL [SM00430] False Positive: Url is validated
    SharpUpdaterHelpers.GetRepositoryUrl(Request.Url, Request.ApplicationPath) %></strong>
            </blockquote>
            <% if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["apiKey"])) { %>
            To enable pushing packages to this feed using the <a href="https://www.nuget.org/downloads">SharpUpdater CLI</a> (dotnet tool 'su'), set the <code>apiKey</code> appSetting in web.config.
            <% } else { %>
            Use the command below to push packages to this feed using the <a href="https://www.nuget.org/downloads">SharpUpdater CLI</a> (dotnet tool 'su').
            <blockquote>
                <strong>su push -p {package file} -k {apikey} -s <%= 
                // Request Validation is ON by default in Asp.NET: https://learn.microsoft.com/en-us/previous-versions/aspnet/hh882339(v=vs.110) & https://learn.microsoft.com/en-us/aspnet/whitepapers/request-validation
                // CodeQL [SM02175] False Positive: Url is validated
                // CodeQL [SM00430] False Positive: Url is validated
    SharpUpdaterHelpers.GetPushUrl(Request.Url, Request.ApplicationPath) %></strong>
            </blockquote>
            <% } %> 
        </fieldset>

        <% if (Request.IsLocal || ServiceResolverHolder.Current.Resolve<NuGet.Server.Core.Infrastructure.ISettingsProvider>().GetBoolSetting("allowRemoteCacheManagement", false)) { %>
        <fieldset style="width:800px">
            <legend><strong>Adding packages</strong></legend>

            To add packages to the feed put package files (.sp files) in the folder
            <code><% = PackageUtility.PackagePhysicalPath %></code><br/><br/>

            Click <a href="<%= VirtualPathUtility.ToAbsolute("~/sp/clear-cache") %>">here</a> to clear the package cache.
        </fieldset>
        <% } %>
    </div>
</body>
</html>
