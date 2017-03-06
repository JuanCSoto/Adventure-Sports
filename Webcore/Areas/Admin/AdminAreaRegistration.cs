// <copyright file="AdminAreaRegistration.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin
{
    using System.Web.Mvc;
    
    /// <summary>
    /// Provides a way to register one or more areas in an ASP.NET MVC application.
    /// </summary>
    public class AdminAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Gets the area name
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new 
                { 
                    controller = "Home", 
                    action = "Index", 
                    id = UrlParameter.Optional 
                });
        }
    }
}
