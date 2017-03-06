// <copyright file="FrontEndController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Controllers
{
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Business;
    using Business.Globalization;
    using Business.Services;
    using Domain.Abstract;
    using Domain.Concrete;
    using Domain.Entities;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// controller front end base
    /// </summary>
    public class FrontEndController : Controller
    {
        /// <summary>
        /// Gets or sets a framework that establishes communication between the application and the database
        /// </summary>
        [Dependency]
        public ISession SessionCustom { get; set; }

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
            LanguageManagement langman = new LanguageManagement(this.SessionCustom, HttpContext);
            //this.CurrentLanguage = langman.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
            UpdateLanguage();
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string cookieName = FormsAuthentication.FormsCookieName;
                HttpCookie authCookie = HttpContext.Request.Cookies[cookieName];
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                string[] data = authTicket.UserData.Split('|');

                FormsIdentity id = new FormsIdentity(authTicket);
                HttpContext.User = this.CustomUser = new CustomPrincipal(id, data);
            }
            else
            {
                HttpContext.User = this.CustomUser = new CustomPrincipal(HttpContext.User.Identity);
            }

            return base.CreateActionInvoker();
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
            filterContext.Result = this.View("~/Views/Shared/Error.cshtml", filterContext.Exception);

            base.OnException(filterContext);
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

        private void UpdateLanguage()
        {
            if (HttpContext.Session["lang"] != null)
            {
                this.CurrentLanguage = (Language)HttpContext.Session["lang"];
            }
            else
            {
                LanguageRepository languagerepo = new LanguageRepository(this.SessionCustom);
               
                if (this.CurrentLanguage != null)
                {
                    HttpContext.Session.Add("lang", this.CurrentLanguage);
                }
                else
                {
                    languagerepo.GetLanguageDefault();
                   this.CurrentLanguage = languagerepo.Entity;
                    HttpContext.Session.Add("lang", this.CurrentLanguage);
                }

            }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(this.CurrentLanguage.Culturename);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(this.CurrentLanguage.Culturename);

        }
    }
}