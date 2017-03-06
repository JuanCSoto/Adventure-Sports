// <copyright file="ContentController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;
    using Webcore.Areas.Admin.Models;

    /// <summary>
    /// Controller for content module
    /// </summary>
    public class ContentController : AdminController
    {
        /// <summary>
        /// gets the home of content module
        /// </summary>
        /// <param name="mod">identifier of module</param>
        /// <returns>returns the result to action</returns>
        [ModulAuthorize]
        public ActionResult Index(int mod)
        {
            ContentRepository content = new ContentRepository(SessionCustom);
            ModulRepository modul = new ModulRepository(SessionCustom);
            SectionRepository objsection = new SectionRepository(SessionCustom);

            PaginInfo paginInfo = new PaginInfo()
            {
                PageIndex = 1
            };

            content.Entity.ModulId = mod;
            modul.Entity.ModulId = content.Entity.ModulId = mod;
            content.Entity.LanguageId = modul.Entity.LanguageId = CurrentLanguage.LanguageId;
            modul.Load();

            ////objsection.Entity.Active = true;
            objsection.Entity.LanguageId = CurrentLanguage.LanguageId;

            if (HttpContext.Session["CollClones"] != null)
            {
                ViewBag.CollClone = (List<int>)HttpContext.Session["CollClones"];
            }

            return this.View(new Models.Content()
            {
                UserPrincipal = CustomUser,
                Module = modul.Entity,
                CollSection = objsection.GetAll().Where(t => t.ParentId == null),
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CollContent = content.GetAllPaging(null, paginInfo),
                Total = paginInfo.TotalCount,
                Controller = modul.Entity.Controller,
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// Obtains a string information to render item content list
        /// </summary>
        /// <param name="mod">identifier module</param>
        /// <param name="page">page index</param>
        /// <param name="sectionId">identifier section</param>
        /// <param name="name">criteria search</param>
        /// <param name="filter">order filter</param>
        /// <param name="active">content active</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public JsonResult GetContent(int mod, int page, int? sectionId, string name, short? filter, bool? active)
        {
            StringBuilder strbl = new StringBuilder();

            ContentRepository content = new ContentRepository(SessionCustom);
            PaginInfo paginInfo = new PaginInfo()
            {
                PageIndex = page
            };

            content.Entity.Name = name;
            content.Entity.Active = active;
            content.Entity.SectionId = sectionId;
            content.Entity.ModulId = mod;
            content.Entity.LanguageId = CurrentLanguage.LanguageId;

            IEnumerable<Domain.Entities.Content> contents = content.GetAllPaging(filter, paginInfo);
            foreach (Domain.Entities.Content item in contents)
            {
                strbl.AppendLine("<li id=\"li" + item.ContentId + "\" ondblclick=\"ctnback.editcontent(" + item.ContentId + ")\" onclick=\"if(ctnback.clicOk) { ctnback.contentselect(this, " + item.ContentId + "); } else { ctnback.clicOk = true; }\">");
                ////strbl.AppendLine("<input style=\"float:right;\" onclick=\"ctnback.clicOk=false;\" type=\"checkbox\" value=\"" + item.ContentId + "\" />");
                strbl.AppendLine("<img order=\"" + item.Orderliness + "\" id=\"" + item.ContentId + "\" class=\"handle\" src=\"" + Business.Utils.GetImageContent(item.Image, item.ContentId.Value, 44, 44) + "\" width=\"44\" height=\"44\" />");
                strbl.AppendLine("<div class=\"info-content\"><span title=\"Arrastre hacia una sección para cambiarla.\" class=\"sptitle cursor\">" + Business.Utils.TruncateWord(item.Name, 85) + "</span><br />");
                strbl.AppendLine("<span class=\"spdate\">" + item.Joindate.Value.ToString("F") + "</span></div><div class=\"clear\"></div>");
                strbl.AppendLine("</li>");
            }

            return this.Json(new { html = strbl.ToString(), count = contents.Count(), total = paginInfo.TotalCount }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// obtains the content detail
        /// </summary>
        /// <param name="id">identifier content</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        public ActionResult ViewDetail(int id)
        {
            ContentRepository objcont = new ContentRepository(SessionCustom);
            UserRepository objuser = new UserRepository(SessionCustom);
            SectionRepository objSection = new SectionRepository(SessionCustom);
            ContentrelationRepository objrel = new ContentrelationRepository(SessionCustom);
            objrel.Entity.ContentId = id;

            objcont.Entity.ContentId = id;
            objcont.Load();

            objuser.Entity.UserId = objcont.Entity.UserId;
            objuser.Load();

            return this.View(new InfoContent()
            {
                Content = objcont.Entity,
                Autor = objuser.Entity.Names,
                DeepFollower = Business.Utils.GetDeepFollower(objSection.GetAll(), objcont.Entity.SectionId.Value),
                CountContent = objrel.GetAllReadOnly().Count
            });
        }

        /// <summary>
        /// delete a content item
        /// </summary>
        /// <param name="listContentId">list of id content</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        [Authorize]
        public JsonResult DeleteContent(string listContentId)
        {
            string[] listofIds = listContentId.Split(',');

            ContentManagement objcontent = new ContentManagement(SessionCustom, HttpContext);
            objcontent.DeleteContent(listofIds);
            return this.Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// displays the content relation page
        /// </summary>
        /// <param name="contentId">identifier content</param>
        /// <param name="page">index page</param>
        /// <param name="name">criteria search</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        public ActionResult ContentRelation(int contentId, int? page, string name)
        {
            ContentRepository objcontent = new ContentRepository(SessionCustom);
            PaginInfo paginInfo = new PaginInfo()
            {
                Size = 7,
                PageIndex = page != null ? page.Value : 1
            };

            objcontent.Entity.ContentId = contentId;
            objcontent.Entity.Name = name;

            return this.View(new ContentsRelations()
            {
                CollContentNoRel = objcontent.GetContentNoRelation(paginInfo, CurrentLanguage.LanguageId.Value),
                CollContentRel = objcontent.GetContentRelation(CurrentLanguage.LanguageId.Value),
                Pagininfo = paginInfo
            });
        }

        /// <summary>
        /// attach content to other content
        /// </summary>
        /// <param name="id">identifier of content to attach</param>
        /// <param name="contentId">identifier of primary content</param>
        /// <param name="name">criteria search</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        public ActionResult Attach(int id, int contentId, string name)
        {
            ContentManagement objcontent = new ContentManagement(SessionCustom, HttpContext);
            objcontent.AttachDettachContent(contentId, id, true);

            return this.RedirectToAction(
                "ContentRelation",
                new
                {
                    ContentId = contentId,
                    name = name
                });
        }

        /// <summary>
        /// detach content from other content
        /// </summary>
        /// <param name="id">identifier of content detach</param>
        /// <param name="contentId">identifier of primary content</param>
        /// <param name="name">criteria search</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        public ActionResult Dettach(int id, int contentId, string name)
        {
            ContentManagement objcontent = new ContentManagement(SessionCustom, HttpContext);
            objcontent.AttachDettachContent(contentId, id, false);

            return this.RedirectToAction(
                "ContentRelation",
                new
                {
                    ContentId = contentId,
                    name = name
                });
        }

        /// <summary>
        /// detach content from other content
        /// </summary>
        /// <param name="id">identifier of content detach</param>
        /// <param name="contentId">identifier of primary content</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        [HttpPost]
        public JsonResult DettachAjax(int id, int contentId)
        {
            ContentManagement objcontent = new ContentManagement(SessionCustom, HttpContext);
            return this.Json(
                new
                {
                    result = objcontent.AttachDettachContent(contentId, id, false)
                });
        }

        /// <summary>
        /// delete attach file
        /// </summary>
        /// <param name="id">identifier of file</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        [HttpPost]
        public JsonResult Deletefile(int id)
        {
            ContentManagement objcontent = new ContentManagement(SessionCustom, HttpContext);
            return this.Json(new
            {
                result = objcontent.DeleteFileAttach(id)
            });
        }

        /// <summary>
        /// change order to contents
        /// </summary>
        /// <param name="contentId">identifier of content</param>
        /// <param name="prevId">identifier of previous content</param>
        /// <param name="limit">limit of content</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        [HttpPost]
        public JsonResult ChangeOrder(int contentId, int prevId, bool limit)
        {
            ContentManagement objcontent = new ContentManagement(SessionCustom, HttpContext);
            return this.Json(new
            {
                result = objcontent.ChangeOrder(contentId, prevId, limit)
            });
        }

        /// <summary>
        /// change order content files
        /// </summary>
        /// <param name="fileattachId">identifier of file</param>
        /// <param name="prevId">identifier of previous file</param>
        /// <param name="limit">limit of file</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        [HttpPost]
        public JsonResult ChangeOrderFile(int fileattachId, int prevId, bool limit)
        {
            ContentManagement objcontent = new ContentManagement(SessionCustom, HttpContext);
            return this.Json(new
            {
                result = objcontent.ChangeOrderFile(fileattachId, prevId, limit)
            });
        }

        /// <summary>
        /// change section to content
        /// </summary>
        /// <param name="contentId">identifier of content</param>
        /// <param name="sectionId">identifier of section</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        [HttpPost]
        public JsonResult ChangeSection(int contentId, int sectionId)
        {
            ContentManagement objcontent = new ContentManagement(SessionCustom, HttpContext);
            return this.Json(new
            {
                result = objcontent.ChangeSection(contentId, sectionId)
            });
        }

        /// <summary>
        /// update the file name
        /// </summary>
        /// <param name="fileattachId">identifier of file</param>
        /// <param name="name">name of file</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        [HttpPost]
        public JsonResult UpdateFile(int fileattachId, string name)
        {
            ContentManagement objcontent = new ContentManagement(SessionCustom, HttpContext);
            return this.Json(new
            {
                result = objcontent.UpdateFile(fileattachId, name)
            });
        }

        /// <summary>
        /// upload content image to server
        /// </summary>
        /// <param name="id">identifier of content</param>
        /// <param name="imageName">represents the upload file</param>
        [HttpPost]
        public void UploadImage(int? id, HttpPostedFileBase imageName)
        {
            if (imageName.ContentLength > 0)
            {
                string strfile = DateTime.Now.ToString("ddmmyyyyhhmmss") + Path.GetExtension(imageName.FileName);
                string fullPath = Path.Combine(Server.MapPath("~"), @"Files/" + (id != null ? id.ToString() : "Images") + "/" + strfile);
                string path = System.IO.Path.GetDirectoryName(fullPath);
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                imageName.SaveAs(fullPath);
                Response.Write(string.Concat("<div id='image'>", System.Configuration.ConfigurationManager.AppSettings["PathHost"].TrimEnd('/'), "/Files/", id != null ? id.ToString() : "Images", "/", strfile, "</div>"));
            }
        }

        /// <summary>
        /// add a content item to collection to clone contents
        /// </summary>
        /// <param name="id">identifier of content</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        [HttpPost]
        public JsonResult AddClone(int id)
        {
            try
            {
                List<int> contentIds = new List<int>();
                if (HttpContext.Session["CollClones"] != null)
                {
                    contentIds = (List<int>)HttpContext.Session["CollClones"];
                }

                if (!contentIds.Contains(id))
                {
                    contentIds.Add(id);
                }

                HttpContext.Session["CollClones"] = contentIds;

                return this.Json(new
                {
                    result = true,
                    numclone = contentIds.Count
                });
            }
            catch (Exception)
            {
                return this.Json(new
                {
                    result = false
                });
            }
        }

        /// <summary>
        /// creates copies of a content
        /// </summary>
        /// <param name="sectionId">identifier of section</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        [HttpPost]
        public JsonResult CloneContent(int sectionId)
        {
            ContentManagement objcontent = new ContentManagement(SessionCustom, HttpContext);
            List<int> contentIds = new List<int>();
            if (HttpContext.Session["CollClones"] != null)
            {
                contentIds = (List<int>)HttpContext.Session["CollClones"];
                foreach (int item in contentIds)
                {
                    objcontent.Clone(item, CurrentLanguage.LanguageId.Value, sectionId);
                }

                HttpContext.Session.Remove("CollClones");
            }

            return this.Json(new
            {
                result = true
            });
        }
    }
}