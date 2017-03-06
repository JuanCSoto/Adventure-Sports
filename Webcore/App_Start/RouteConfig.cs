// <copyright file="RouteConfig.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Business.Globalization;
    
    /// <summary>
    /// set the routes configuration
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// register the routes application
        /// </summary>
        /// <param name="routes">collection of routes for ASP.NET routing.</param>
        /// <History>
        /// Modificado por      :   Juan Carlos Soto Cruz (JCS)
        /// Descripción cambio  :   Se asigna el UrlParameter.Optional al id del objeto defaultRouteValueDictionary.
        /// Fecha               :   2015/11/06 
        /// 
        /// Modificado por      :   Juan Carlos Soto Cruz (JCS)
        /// Descripción cambio  :   Se vuelve a asignar 31 quemado al id del objeto defaultRouteValueDictionary pues de lo contrario se produce un error.
        /// Fecha               :   2015/11/09
        /// 
        /// Modificado por      :   Ferney Osorio
        /// Descripción cambio  :   Se deja que la pagina de inicio del sitio sea la de retos.
        /// Fecha               :   2016/08/16
        /// </History>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                namespaces: new string[] { "Webcore.Controllers" },
                name: "sitemap",
                url: "sitemap.xml",
                defaults: new { controller = "Home", action = "SiteMap" });

            RouteValueDictionary defaultRouteValueDictionary = new RouteValueDictionary(
                new
                {
                    controller = "Home",
                    action = "Index",
                    id = 51 //// Tabla Section Id de Retos.                    
                });

            Route defRoute = new Route("{controller}/{action}/{id}", defaultRouteValueDictionary, new MvcRouteHandler());

            if (defRoute.DataTokens == null)
            {
                defRoute.DataTokens = new RouteValueDictionary();
            }

            defRoute.DataTokens.Add("Namespaces", new string[] { "Webcore.Controllers" });

            Route defaultGlobalized = new Route(
                "{culture}/{controller}/{action}/{id}",
                defaultRouteValueDictionary,
                new RouteValueDictionary(new { culture = new CultureRoute() }),
                new GlobalizationRouteHandler());

            if (defaultGlobalized.DataTokens == null)
            {
                defaultGlobalized.DataTokens = new RouteValueDictionary();
            }

            defaultGlobalized.DataTokens.Add("Namespaces", new string[] { "Webcore.Controllers" });

            routes.Add("DefaultGlobalised", defaultGlobalized);
            routes.Add("Default", defRoute);            
        }
    }
}