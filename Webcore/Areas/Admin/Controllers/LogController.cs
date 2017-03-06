// <copyright file="LogController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;
    using Webcore.Areas.Admin.Models;
    
    /// <summary>
    /// Controller for <c>log</c> module
    /// </summary>
    [ModulAuthorize]
    public class LogController : AdminController
    {
        /// <summary>
        /// gets the home of log module
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            return this.View(new Logs()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// export the log information to excel file
        /// </summary>
        public void ExportLog()
        {
            LogRepository objlog = new LogRepository(SessionCustom);
                        
            ConvertListExcel<Log> objConvert = new 
                ConvertListExcel<Log>(HttpContext);

            objConvert.CollList = objlog.GetAll();
            objConvert.SheetName = this.Module.Name;
            objConvert.ConvertToExcel();
        }

        /// <summary>
        /// clean the log information
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult CleanLog()
        {
            LogRepository objlog = new LogRepository(SessionCustom);
            objlog.Delete();
            return this.RedirectToAction("Index", "Log");
        }
    }
}
