// <copyright file="WebApiConfig.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore
{
    using System.Web.Http;
    
    /// <summary>
    /// register a routes application
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// register a routes application
        /// </summary>
        /// <param name="config">Configuration of System.Web.Http.HttpServer instances.</param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: 
                new 
                { 
                    id = RouteParameter.Optional 
                });
        }
    }
}
