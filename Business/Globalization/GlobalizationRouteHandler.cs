// <copyright file="GlobalizationRouteHandler.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business.Globalization
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Creates an object that implements the IHttpHandler interface and passes the request context to it.
    /// </summary>
    public class GlobalizationRouteHandler : MvcRouteHandler
    {
        /// <summary>
        /// Initializes a new instance of the GlobalizationRouteHandler class.
        /// </summary>
        public GlobalizationRouteHandler()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the GlobalizationRouteHandler class.
        /// </summary>
        /// <param name="controllerFactory">Defines the methods that are required for a controller factory.</param>
        public GlobalizationRouteHandler(IControllerFactory controllerFactory)
            : base(controllerFactory)
        {
        }

        /// <summary>
        /// Gets the culture value
        /// </summary>
        private string CultureValue
        {
            get
            {
                return (string)this.RouteDataValues["culture"];
            }
        }

        /// <summary>
        /// Gets or sets case-insensitive collection of key/value pairs that you use
        /// in various places in the routing framework
        /// </summary>
        private RouteValueDictionary RouteDataValues { get; set; }

        /// <summary>
        /// Returns the HTTP handler by using the specified HTTP context.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <returns>The HTTP handler.</returns>
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            this.RouteDataValues = requestContext.RouteData.Values;
            return base.GetHttpHandler(requestContext);
        }
    }
}