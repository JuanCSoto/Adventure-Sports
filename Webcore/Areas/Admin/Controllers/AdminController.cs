// <copyright file="AdminController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
    using Business;
    using Business.Services;
    using Domain.Abstract;
    using Domain.Concrete;
    using Domain.Entities;
    using Microsoft.Practices.Unity;
    
    /// <summary>
    /// controller administrator base
    /// </summary>
    public class AdminController : Controller
    {
        /// <summary>
        /// Gets or sets a framework that establishes communication between the application and the database
        /// </summary>
        [Dependency]
        public ISession SessionCustom { get; set; }

        /// <summary>
        /// Gets or sets the object module
        /// </summary>
        protected Modul Module { get; set; }

        /// <summary>
        /// Gets or sets the object user principal
        /// </summary>
        protected CustomPrincipal CustomUser { get; set; }

        /// <summary>
        /// Gets or sets the language thread
        /// </summary>
        protected Language CurrentLanguage { get; set; }

        /// <summary>
        /// Creates an action invoker.
        /// </summary>
        /// <returns>An action invoker.</returns>
        protected override IActionInvoker CreateActionInvoker()
        {
            string controller = HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string cookieName = FormsAuthentication.FormsCookieName;
                HttpCookie authCookie = HttpContext.Request.Cookies[cookieName];
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                string[] data = authTicket.UserData.Split('|');

                FormsIdentity id = new FormsIdentity(authTicket);
                HttpContext.User = this.CustomUser = new CustomPrincipal(id, data);

                if (HttpContext.Session["lang"] != null)
                {
                    this.CurrentLanguage = (Language)HttpContext.Session["lang"];
                }
                else
                {
                    LanguageRepository languagerepo = new LanguageRepository(this.SessionCustom);
                    languagerepo.GetByUser(this.CustomUser.UserId);
                    this.CurrentLanguage = languagerepo.Entity;
                    HttpContext.Session.Add("lang", this.CurrentLanguage);
                }

                Thread.CurrentThread.CurrentCulture = new CultureInfo(this.CurrentLanguage.Culturename);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(this.CurrentLanguage.Culturename);
            }
            else
            {
                this.CustomUser = new CustomPrincipal(HttpContext.User.Identity);
                LanguageRepository languagerepo = new LanguageRepository(this.SessionCustom);
                languagerepo.Entity.IsDefault = true;
                languagerepo.Load();
               
            }

            if (controller != null)
            {
                ModulRepository modulrepo = new ModulRepository(this.SessionCustom);
                modulrepo.Entity.Controller = controller;
                if (this.CurrentLanguage != null)
                {
                    modulrepo.Entity.LanguageId = this.CurrentLanguage.LanguageId;
                }

                modulrepo.Load();
                this.Module = modulrepo.Entity;
            }

            return base.CreateActionInvoker();
        }

        /// <summary>
        /// insert a new audit item
        /// </summary>
        /// <param name="message">message audit</param>
        /// <param name="description">description audit</param>
        protected void InsertAudit(string message, string description)
        {
            Utils.InsertAudit(
                this.SessionCustom, 
                new Domain.Entities.Audit()
            {
                Joindate = DateTime.Now,
                Description = description,
                Username = this.CustomUser.UserId,
                Auditaction = message
            });
        }

        /// <summary>
        /// Called when an unhandled exception occurs in the action.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            if (!(filterContext.Exception is System.Data.Common.DbException))
            {
                Utils.InsertLog(this.SessionCustom, filterContext.Exception.Message, filterContext.Exception.StackTrace);
            }

            filterContext.ExceptionHandled = true;
            filterContext.Result = this.View("~/Areas/Admin/Views/Shared/Error.cshtml", filterContext.Exception);
            
            base.OnException(filterContext);
        }

        /// <summary>
        /// Redirects to the specified action using the action name.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller</param>
        /// <param name="routeValues">The parameters for a route.</param>
        /// <returns>The redirect result object.</returns>
        protected override RedirectToRouteResult RedirectToAction(string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues)
        {
            if (routeValues != null)
            {
                routeValues.Add("Area", "Admin");
            }
            else
            {
                routeValues = new RouteValueDictionary();
                routeValues.Add("Area", "Admin");
            }

            return base.RedirectToAction(actionName, controllerName, routeValues);
        }

        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            this.SessionCustom.Dispose();
            base.Dispose(disposing);
        }
    }
}