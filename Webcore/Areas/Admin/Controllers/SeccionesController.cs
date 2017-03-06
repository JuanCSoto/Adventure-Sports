// <copyright file="SeccionesController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;
    using Webcore.Areas.Admin.Models;
    
    /// <summary>
    /// Controller for sections module
    /// </summary>
    [ModulAuthorize]
    public class SeccionesController : AdminController
    {
        /// <summary>
        /// gets the home of section module
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            SectionManagement objSection = new SectionManagement(SessionCustom, HttpContext);

            return this.View(new Secciones()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                CollSections = objSection.GetSectionsParentNull(CurrentLanguage.LanguageId.Value),
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CollLayouts = objSection.GetLayouts(),
                CollTemplates = objSection.GetTemplates(),
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// obtains the section detail
        /// </summary>
        /// <param name="id">identifier of section</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public JsonResult Detail(int id)
        {
            SectionManagement objSection = new SectionManagement(SessionCustom, HttpContext);

            return this.Json(objSection.GetSection(id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// update the item section
        /// </summary>
        /// <param name="objsec">information section</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public JsonResult SaveSection(Section objsec)
        {
            SectionManagement objSection = new SectionManagement(SessionCustom, HttpContext);
            objsec.LanguageId = CurrentLanguage.LanguageId;
            HttpContext.Cache.Remove("sectionsactive");
            return this.Json(new { result = objSection.SaveSection(objsec) });
        }

        /// <summary>
        /// gets a section childs
        /// </summary>
        /// <param name="id">identifier of section</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public JsonResult GetChild(int id)
        {
            UrlHelper url = new UrlHelper(Request.RequestContext);
            SectionRepository objsection = new SectionRepository(SessionCustom);
            objsection.Entity.ParentId = id;
            objsection.Entity.LanguageId = CurrentLanguage.LanguageId;

            IList<Section> coll = objsection.GetAllReadOnly();
            if (coll.Count > 0)
            {
                StringBuilder strb = new StringBuilder();
                strb.AppendLine("<ul style=\"display:none;\">");
                foreach (Section item in coll)
                {
                    strb.AppendLine("<li><div id='" + item.SectionId + "'><nobr><img onclick=\"expand(this, " + item.SectionId + ")\" height=\"15px\" width=\"15px\" src=\"" + url.Content("~/resources/images/25add.gif") + "\" />");
                    strb.AppendLine("<span onclick=\"ctnback.binddetail(" + item.SectionId + ")\">" + item.Name + "</span></nobr></div></li>");
                }

                strb.AppendLine("</ul>");
                return this.Json(new { Iscontain = true, html = strb.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json(new { Iscontain = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// change the section parent
        /// </summary>
        /// <param name="sectionId">identifier of section</param>
        /// <param name="parentId">identifier of parent section</param>
        /// <returns>returns the result to action</returns>
        public JsonResult ChangeParent(int sectionId, int parentId)
        {
            SectionRepository objsection = new SectionRepository(SessionCustom);
            SectionManagement objman = new SectionManagement(SessionCustom, HttpContext);

            objsection.Entity.SectionId = sectionId;
            objsection.LoadByKey();

            if (parentId != 0)
            {
                objsection.Entity.ParentId = parentId;
            }
            else
            {
                objsection.Entity.ParentId = null;
            }

            objsection.Update();
            objsection.Entity = new Section();

            objman.CreateTreeView(sectionId, objsection.GetAll());

            return this.Json(new { result = true, html = objman.Tree });
        }

        /// <summary>
        /// delete section
        /// </summary>
        /// <param name="sectionId">identifier of section</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public JsonResult DeleteSection(int sectionId)
        {
            SectionManagement objSection = new SectionManagement(SessionCustom, HttpContext);

            return this.Json(new { result = objSection.DeleteSection(sectionId) });
        }
    }
}