// <copyright file="Global.asax.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Domain.Abstract;
    using Microsoft.Practices.Unity;
    using Webcore.Controllers;
    
    /// <summary>
    /// Defines the methods, properties, and events that are common to all application
    /// objects in an ASP.NET application
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Gets or sets a simple, extensible dependency injection container.
        /// </summary>
        public static UnityContainer Container { get; set; }
        
        /// <summary>
        /// <para></para>
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Container = new UnityContainer();
            Container.RegisterType<ISession, SqlSession>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));

            BaseController controllerFactory = new BaseController();
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}