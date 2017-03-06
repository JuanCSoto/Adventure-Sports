// <copyright file="NotificationSettingsController.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Xml;
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using OfficeOpenXml;
    using Webcore.Areas.Admin.Models;

    /// <summary>
    /// Controller for Ally module
    /// </summary>
    [ModulAuthorize]
    public class NotificationSettingsController : AdminController
    {
        /// <summary>
        /// gets the home of report module
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            ContentManagement objcontentman = new ContentManagement(this.SessionCustom, HttpContext);
            SectionRepository objsection = new SectionRepository(this.SessionCustom);
            TemplateRepository objtemplate = new TemplateRepository(this.SessionCustom);
            objtemplate.Entity.Type = 0;

            ContentRepository content = new ContentRepository(SessionCustom);
            DataTable pulsesTable = content.ReportPulses();
            List<SelectListItem> pulses = new List<SelectListItem>();
            foreach (DataRow dr in pulsesTable.Rows)
            {
                pulses.Add(new SelectListItem() { Value = dr["ContentId"].ToString(), Text = dr["Nombre"].ToString() });
            }

            pulsesTable.Dispose();
            ViewBag.Pulses = pulses;

            return this.View(new NotificationSettingsModel()
            {
                UserPrincipal = this.CustomUser,
                Module = this.Module,
                ColModul = CustomMemberShipProvider.GetModuls(this.CustomUser.UserId, this.SessionCustom, HttpContext),
                Templates = objtemplate.GetAll().Select(t => t.TemplateId),
                
                CurrentLanguage = this.CurrentLanguage
            });
        }        
    }
}
