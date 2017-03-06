// <copyright file="AuditoriaController.cs" company="Dasigno">
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
    /// Controller for audit module
    /// </summary>
    [ModulAuthorize]
    public class AuditoriaController : AdminController
    {
        /// <summary>
        /// gets the home of audit module
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            return this.View(new Auditoria()
            {
                UserPrincipal = this.CustomUser,
                Module = this.Module,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, this.SessionCustom, HttpContext),
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// export the audit information to excel
        /// </summary>
        public void ExportAudit()
        {
            AuditRepository objaudit = new AuditRepository(SessionCustom);

            ConvertListExcel<AuditComp> objConvert = new
                ConvertListExcel<AuditComp>(HttpContext);

            objConvert.CollList = objaudit.GetList();
            objConvert.SheetName = this.Module.Name;
            objConvert.ConvertToExcel();
        }

        /// <summary>
        /// clean all information to audit
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult CleanAudit()
        {
            AuditRepository objaudit = new AuditRepository(SessionCustom);
            objaudit.Delete();
            ViewBag.Result = true;
            return this.RedirectToAction("Index", "Auditoria");
        }
    }
}