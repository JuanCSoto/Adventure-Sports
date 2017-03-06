// <copyright file="IdeaReportController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Xml;
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;
    using Webcore.Areas.Admin.Models;

    /// <summary>
    /// Controller for IdeaReport module
    /// </summary>
    [ModulAuthorize]
    public class IdeaReportController : AdminController
    {
        /// <summary>
        /// gets the index of the idea report module
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            IdeaReportRepository ideaReport = new IdeaReportRepository(SessionCustom);
            ModulRepository modul = new ModulRepository(SessionCustom);

            modul.Entity.ModulId = 56;
            modul.Entity.LanguageId = CurrentLanguage.LanguageId;
            modul.Load();

            PaginInfo paginInfo = new PaginInfo()
            {
                PageIndex = 1
            };

            return this.View(new Models.IdeaReport()
            {
                UserPrincipal = CustomUser,
                Module = modul.Entity,
                CollIdeaReport = ideaReport.GetAllPaging(null, paginInfo),
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                Total = paginInfo.TotalCount,
                Controller = modul.Entity.Controller,
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// Obtains a string information to render ideaReport list
        /// </summary>
        /// <param name="mod">identifier module</param>
        /// <param name="page">page index</param>
        /// <param name="text">criteria search</param>
        /// <param name="filter">order filter</param>
        /// <param name="active">content active</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public JsonResult GetIdeaReports(int mod, int page, string text, short? filter, bool? active)
        {
            StringBuilder strbl = new StringBuilder();

            IdeaReportRepository objideaReport = new IdeaReportRepository(SessionCustom);
            PaginInfo paginInfo = new PaginInfo()
            {
                PageIndex = page
            };

            objideaReport.Entity.Text = text;
            objideaReport.Entity.Status = active;

            IEnumerable<Domain.Entities.FrontEnd.IdeaReportPaging> ideaReports = objideaReport.GetAllPaging(filter, paginInfo);
            foreach (Domain.Entities.FrontEnd.IdeaReportPaging ideaReport in ideaReports)
            {
                string ideaActive = ideaReport.IdeaActive.Value ? "block" : "none";
                string ideaInActive = !ideaReport.IdeaActive.Value ? "block" : "none";
                string ideaUserActive = ideaReport.IdeaUserActive.Value ? "none" : "block";
                string ideaUserInActive = !ideaReport.IdeaUserActive.Value ? "none" : "block";
                strbl.AppendLine("<tr id=\"li" + ideaReport.IdeaReportId + "\" >");
                strbl.AppendLine("<td onclick=\"if(ctnback.clicOk) { ctnback.contentselect(this, " + ideaReport.IdeaReportId + "); } else { ctnback.clicOk = true; }\">" + Business.Utils.TruncateWord(ideaReport.IdeaText, 45) + "</td>");
                strbl.AppendLine("<td onclick=\"if(ctnback.clicOk) { ctnback.contentselect(this, " + ideaReport.IdeaReportId + "); } else { ctnback.clicOk = true; }\">" + ideaReport.Motive + "</td>");
                strbl.AppendLine("<td onclick=\"if(ctnback.clicOk) { ctnback.contentselect(this, " + ideaReport.IdeaReportId + "); } else { ctnback.clicOk = true; }\">" + Business.Utils.TruncateWord(ideaReport.Text, 45) + "</td>");
                strbl.AppendLine("<td>");
                if (!ideaReport.Status.Value)
                {
                    strbl.AppendLine("<a class=\"check-report\" data-id=\"" + ideaReport.IdeaReportId + "\">" + Resources.Global.Messages.CHECKED + "</a>");
                }
                else
                {
                    strbl.AppendLine("<img src=\"" + Url.Content("~/resources/images/25check.png") + "\" />");
                }

                strbl.AppendLine("</td>");
                strbl.AppendLine("<td data-id=\"" + ideaReport.IdeaId + "\">");
                strbl.AppendLine("<a class=\"block-idea\" style=\"display:" + ideaActive + ";\">" + Resources.Extend.Messages.BLOCK_IDEA + "</a>");
                strbl.AppendLine("<a class=\"unblock-idea\" style=\"display:" + ideaInActive + ";\">" + Resources.Extend.Messages.UNBLOCK_IDEA + "</a>");
                strbl.AppendLine("</td>");
                strbl.AppendLine("<td data-id=\"" + ideaReport.IdeaUserId + "\">");
                strbl.AppendLine("<a class=\"block-idea-user\" style=\"display:" + ideaUserActive + ";\">" + Resources.Extend.Messages.BLOCK_USER + "</a>");
                strbl.AppendLine("<a class=\"unblock-idea-user\" style=\"display:" + ideaUserInActive + ";\">" + Resources.Extend.Messages.UNBLOCK_USER + "</a>");
                strbl.AppendLine("</td></tr>");
            }

            return this.Json(new { html = strbl.ToString(), count = ideaReports.Count(), total = paginInfo.TotalCount }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// obtains the content detail
        /// </summary>
        /// <param name="id">identifier content</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        public ActionResult ViewDetail(int id)
        {
            IdeaReportRepository objideaReport = new IdeaReportRepository(SessionCustom);
            UserRepository objuser = new UserRepository(SessionCustom);
            SectionRepository objSection = new SectionRepository(SessionCustom);

            objideaReport.Entity.IdeaReportId = id;
            objideaReport.Load();

            objuser.Entity.UserId = objideaReport.Entity.UserId;
            objuser.Load();

            return this.View(new InfoIdeaReport()
            {
                IdeaReportPaging = objideaReport.GetIdeaReportPagingById(id),
                Autor = objuser.Entity.Names
            });
        }

        /// <summary>
        /// block a user in the site
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>a JSON object with the result of the action</returns>
        [HttpPost]
        [Authorize]
        public JsonResult BlockUser(int id)
        {
            UserRepository objuser = new UserRepository(this.SessionCustom);
            objuser.Entity.UserId = id;
            objuser.LoadByKey();
            objuser.Entity.Active = false;
            objuser.Update();

            this.InsertAudit("Update", this.Module.Name + " -> Block User" + objuser.Entity.Names);
            return this.Json(new { result = true });
        }

        /// <summary>
        /// unblock a user in the site
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>a JSON object with the result of the action</returns>
        [HttpPost]
        [Authorize]
        public JsonResult UnBlockUser(int id)
        {
            UserRepository objuser = new UserRepository(this.SessionCustom);
            objuser.Entity.UserId = id;
            objuser.LoadByKey();
            objuser.Entity.Active = true;
            objuser.Update();

            this.InsertAudit("Update", this.Module.Name + " -> Unblock User" + objuser.Entity.Names);
            return this.Json(new { result = true });
        }

        /// <summary>
        /// block an idea in the site
        /// </summary>
        /// <param name="id">idea id</param>
        /// <returns>a JSON object with the result of the action</returns>
        [HttpPost]
        [Authorize]
        public JsonResult BlockIdea(int id)
        {
            IdeaRepository objidea = new IdeaRepository(this.SessionCustom);
            objidea.Entity.IdeaId = id;
            objidea.LoadByKey();
            objidea.Entity.Active = false;
            objidea.Update();

            this.InsertAudit("Update", this.Module.Name + " -> Block Idea" + id + " " + objidea.Entity.Text);
            return this.Json(new { result = true });
        }

        /// <summary>
        /// unblock an idea in the site
        /// </summary>
        /// <param name="id">idea id</param>
        /// <returns>a JSON object with the result of the action</returns>
        [HttpPost]
        [Authorize]
        public JsonResult UnBlockIdea(int id)
        {
            IdeaRepository objidea = new IdeaRepository(this.SessionCustom);
            objidea.Entity.IdeaId = id;
            objidea.LoadByKey();
            objidea.Entity.Active = true;
            objidea.Update();

            this.InsertAudit("Update", this.Module.Name + " -> Unblock Idea" + id + " " + objidea.Entity.Text);
            return this.Json(new { result = true });
        }

        /// <summary>
        /// mark an idea report as resolved
        /// </summary>
        /// <param name="id">idea report id</param>
        /// <returns>a JSON object with the result of the action</returns>
        [HttpPost]
        [Authorize]
        public JsonResult IdeaReportChecked(int id)
        {
            IdeaReportRepository objideareport = new IdeaReportRepository(this.SessionCustom);
            objideareport.Entity.IdeaReportId = id;
            objideareport.LoadByKey();
            objideareport.Entity.Status = true;
            objideareport.Update();

            this.InsertAudit("Update", this.Module.Name + " -> Report Checked" + id + " " + objideareport.Entity.Text);
            return this.Json(new { result = true });
        }
    }
}
