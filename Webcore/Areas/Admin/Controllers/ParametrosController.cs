// <copyright file="ParametrosController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System;
    using System.Configuration;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using Business.Services;
    using Domain.Entities;
    using Webcore.Areas.Admin.Models;
    
    /// <summary>
    /// Controller for parameters module
    /// </summary>
    [ModulAuthorize]
    public class ParametrosController : AdminController
    {
        /// <summary>
        /// gets the home of parameters module
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            Configuration myConfiguration = WebConfigurationManager.OpenWebConfiguration("~");

            return this.View(new Parametros()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CollValues = myConfiguration.AppSettings.Settings,
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// obtains the parameter detail
        /// </summary>
        /// <param name="key">key of parameter</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(string key)
        {
            return this.View(new Models.Parametros()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                Keyvalue = new KeyValue() { Key = key, Value = ConfigurationManager.AppSettings[key] },
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// updates a parameter
        /// </summary>
        /// <param name="model">information parameter</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Detail(Models.Parametros model)
        {
            try
            {
                Configuration myConfiguration = WebConfigurationManager.OpenWebConfiguration("~");
                string valueprevius = myConfiguration.AppSettings.Settings[model.Keyvalue.Key].Value;
                myConfiguration.AppSettings.Settings[model.Keyvalue.Key].Value = model.Keyvalue.Value;
                myConfiguration.Save();
                myConfiguration = null;

                this.InsertAudit(
                    "Update", 
                    model.Keyvalue.Key + " From '" + valueprevius + "' To '" + model.Keyvalue.Value + "'");
            }
            catch (Exception) 
            { 
            }

            return this.RedirectToAction("Index", "Parametros");
        }
    }
}
