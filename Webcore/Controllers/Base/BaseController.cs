// <copyright file="BaseController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Business.Globalization;
    using Domain.Abstract;
    using Domain.Concrete;
    using Domain.Entities;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Represents the controller factory that is registered by default.
    /// </summary>
    public class BaseController : DefaultControllerFactory
    {
        /// <summary>
        /// Creates the specified controller by using the specified request context.
        /// </summary>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>The controller</returns>
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            Type controllerType = this.GetControllerType(requestContext, controllerName);

            IController controller;

            Language language = null;

            ISession session = MvcApplication.Container.Resolve<ISession>();

            LanguageManagement langrepo = new LanguageManagement(session, requestContext.HttpContext);

            if (requestContext.RouteData.Values.ContainsKey("culture"))
            {
                if (requestContext.RouteData.Values["culture"].ToString().ToLower() == "vs")
                {
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("vs", "true"));
                }

                language = langrepo.GetLanguage(requestContext.RouteData.Values["culture"].ToString());
            }
            else
            {
                language = langrepo.GetLanguageDefault();
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo(language.Culturename);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language.Culturename);

            if (controllerType != null)
            {
                controller = this.GetControllerInstance(requestContext, controllerType);
            }
            else
            {
                FriendlyurlRepository friendly = new FriendlyurlRepository(session);
                friendly.Entity.LanguageId = language.LanguageId;
                friendly.Entity.Friendlyurlid = requestContext.RouteData.Values["controller"].ToString();
                friendly.LoadByKey();

                if (friendly.Entity.Id != null)
                {
                    string queryString = this.CreateQueryString(HttpContext.Current.Request.QueryString);

                    if (friendly.Entity.Type == Friendlyurl.FriendlyType.Content)
                    {
                        HttpContext.Current.RewritePath("~/Contenido/Index/" + friendly.Entity.Id + "?lang=" + language.LanguageId + queryString);
                        controllerType = this.GetControllerType(requestContext, "Contenido");
                    }
                    else if (friendly.Entity.Type == Friendlyurl.FriendlyType.Idea)
                    {
                        HttpContext.Current.RewritePath("~/Idea/Index/" + friendly.Entity.Id + "?lang=" + language.LanguageId + queryString);
                        controllerType = this.GetControllerType(requestContext, "Idea");
                    }
                    else if (friendly.Entity.Type == Friendlyurl.FriendlyType.BlogEntry)
                    {
                        HttpContext.Current.RewritePath("~/Blog/Index/" + friendly.Entity.Id + "?lang=" + language.LanguageId + queryString);
                        controllerType = this.GetControllerType(requestContext, "Blog");
                    }
                    else if (friendly.Entity.Type == Friendlyurl.FriendlyType.Section)
                    {
                        HttpContext.Current.RewritePath("~/Seccion/Index/" + friendly.Entity.Id + "?lang=" + language.LanguageId + queryString);
                        controllerType = this.GetControllerType(requestContext, "Seccion");
                    }
                    else if (friendly.Entity.Type == Friendlyurl.FriendlyType.SuccessCase)
                    {
                        HttpContext.Current.RewritePath("~/SuccessCase/Index/" + friendly.Entity.Id + "?lang=" + language.LanguageId + queryString);
                        controllerType = this.GetControllerType(requestContext, "SuccessCase");
                    }

                    RouteData routeData = RouteTable.Routes.GetRouteData(requestContext.HttpContext);
                    foreach (KeyValuePair<string, object> routeElement in routeData.Values)
                    {
                        requestContext.RouteData.Values[routeElement.Key] = routeElement.Value;
                    }
                }
                else
                {
                    throw new HttpException(404, string.Empty);
                }

                if (session != null)
                {
                    session.Dispose();
                }

                controller = this.GetControllerInstance(requestContext, controllerType);
            }

            return controller;
        }

        /// <summary>
        /// gets the <c>URL</c> variables to path
        /// </summary>
        /// <param name="values">a collection of <c>URL</c> variables</param>
        /// <returns>returns a string</returns>
        private string CreateQueryString(NameValueCollection values)
        {
            var builder = new StringBuilder();
            Regex regEx = new Regex("[\\<>\"'()]");

            foreach (string key in values.Keys)
            {
                foreach (var value in values.GetValues(key))
                {
                    var safeValue = value;
                    if (regEx.IsMatch(value))
                    {
                        safeValue = regEx.Replace(value, string.Empty);
                    }

                    builder.AppendFormat("&{0}={1}", key, safeValue);
                }
            }

            return builder.ToString();
        }
    }
}