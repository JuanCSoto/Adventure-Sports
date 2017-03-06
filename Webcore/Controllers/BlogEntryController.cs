// <copyright file="BlogEntryController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Webcore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Xml;
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;
    using Webcore.Areas.Admin.Controllers;
    using Webcore.Areas.Admin.Models;

    /// <summary>
    /// Controller for BlogEntry module
    /// </summary>    
    [Authorize]
    public class BlogEntryController : AdminController
    {
        /// <summary>
        /// gets the home of BlogEntry module
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            SetLabel();
            Business.Services.CustomPrincipal currentUserInfo = (Business.Services.CustomPrincipal)User;
            if (Utils.IsBlogAdmin(currentUserInfo.UserId))
            {
                int mod = 54;
                int sectionId = 34;

                ContentManagement objcontentman = new ContentManagement(this.SessionCustom, HttpContext);
                SectionRepository objsection = new SectionRepository(this.SessionCustom);
                TemplateRepository objtemplate = new TemplateRepository(this.SessionCustom);
                objtemplate.Entity.Type = 0;

                return this.View(new BlogEntryModel()
                {
                    UserPrincipal = this.CustomUser,
                    Module = this.Module,
                    ColModul = CustomMemberShipProvider.GetModuls(this.CustomUser.UserId, this.SessionCustom, HttpContext),
                    Templates = objtemplate.GetAll().Select(t => t.TemplateId),
                    DeepFollower = Business.Utils.GetDeepFollower(objsection.GetAll(), sectionId),
                    IContent = new Domain.Entities.Content()
                    {
                        ModulId = mod,
                        SectionId = sectionId
                    },
                    CurrentLanguage = this.CurrentLanguage
                });
            }

            return null;
        }

        /// <summary>
        /// obtains the challenge detail
        /// </summary>        
        /// <param name="id">identifier of section</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(int id)
        {
            Business.Services.CustomPrincipal currentUserInfo = (Business.Services.CustomPrincipal)User;
            if (Utils.IsBlogAdmin(currentUserInfo.UserId))
            {
                ContentManagement objcontentman = new ContentManagement(SessionCustom, HttpContext);
                ContentRepository objcontent = new ContentRepository(SessionCustom);
                BlogEntryRepository objblogentry = new BlogEntryRepository(SessionCustom);
                FileattachRepository objfiles = new FileattachRepository(SessionCustom);
                TagRepository objtag = new TagRepository(SessionCustom);
                SectionRepository objsection = new SectionRepository(SessionCustom);
                TemplateRepository objtemplate = new TemplateRepository(SessionCustom);
                CommentRepository objcomment = new CommentRepository(SessionCustom);

                objtemplate.Entity.Type = 0;

                objblogentry.Entity.ContentId =
                    objfiles.Entity.ContentId =
                    objcomment.Entity.ContentId =
                    objcontent.Entity.ContentId = id;

                objblogentry.LoadByKey();
                objcontent.LoadByKey();

                int totalComments = 0;
                List<CommentsPaging> comments = objcomment.CommentsPagingContent(0, 50, out totalComments, id);
                ViewBag.TotalComments = totalComments;

                IEnumerable<Tag> SelectedTags = objtag.GetTagbycontent(id);
                this.ViewBag.SelectedTags = string.Join("|", SelectedTags.Select(t => t.TagId));
                this.ViewBag.NewsTags = string.Empty;

                return this.View(
                    "Index",
                    new BlogEntryModel()
                    {
                        UserPrincipal = this.CustomUser,
                        ColModul = CustomMemberShipProvider.GetModuls(this.CustomUser.UserId, this.SessionCustom, HttpContext),
                        Module = this.Module,
                        ListFiles = objfiles.GetAllReadOnly(),
                        BlogEntry = objblogentry.Entity,
                        IContent = objcontent.Entity,
                        Templates = objtemplate.GetAll().Select(t => t.TemplateId),
                        ListContent = objcontent.GetContentRelation(CurrentLanguage.LanguageId.Value),
                        ListTags = SelectedTags,
                        DeepFollower = Business.Utils.GetDeepFollower(objsection.GetAll(), objcontent.Entity.SectionId.Value),
                        CurrentLanguage = this.CurrentLanguage,
                        ListComments = comments
                    });
            }

            return null;
        }

        /// <summary>
        /// inserts or updates a item blog entry
        /// </summary>
        /// <param name="model">identifier of module</param>
        /// <param name="contentImage">represents a upload content image</param>
        /// <param name="videoyoutube">represents a list of videos</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(BlogEntryModel model, string contentImage, List<string> videoyoutube)
        {
            Business.Services.CustomPrincipal currentUserInfo = (Business.Services.CustomPrincipal)User;
            if (Utils.IsBlogAdmin(currentUserInfo.UserId))
            {
                BlogEntryRepository objblogentry = new BlogEntryRepository(this.SessionCustom);
                ContentManagement objcontent = new ContentManagement(this.SessionCustom, HttpContext);

                try
                {
                    model.IContent.Template = "BlogEntry";
                    model.IContent.Widget = false;
                    model.IContent.Private = false;
                    model.IContent.Active = true;
                    model.IContent.Image = contentImage;
                                        
                    objcontent.CollVideos = videoyoutube;
                    this.SessionCustom.Begin();

                    model.IContent.LanguageId = 2;
                    objcontent.ContentInsert(model.IContent, 4);
                    objblogentry.Entity = model.BlogEntry;
                    if (objblogentry.Entity.ContentId != null)
                    {
                        objblogentry.Update();
                        this.InsertAudit("Update", this.Module.Name + " -> " + model.IContent.Name);
                    }
                    else
                    {
                        objblogentry.Entity.ContentId = objcontent.ObjContent.ContentId;
                        objblogentry.Insert();

                        this.InsertAudit("Insert", this.Module.Name + " -> " + model.IContent.Name);
                    }

                    string serverMap = Server.MapPath("~");
                    string origin = serverMap + @"\resources\temporal\blog\" + contentImage;
                    if (System.IO.File.Exists(origin))
                    {
                        if (!System.IO.Directory.Exists(serverMap + @"\files\" + objblogentry.Entity.ContentId))
                        {
                            System.IO.Directory.CreateDirectory(serverMap + @"\files\" + objblogentry.Entity.ContentId);
                        }

                        System.IO.File.Move(origin, serverMap + @"\files\" + objblogentry.Entity.ContentId + @"\" + contentImage);
                    }

                    this.SessionCustom.Commit();
                    ViewBag.Result = true;
                }
                catch (Exception ex)
                {
                    SessionCustom.RollBack();
                    Utils.InsertLog(
                        this.SessionCustom,
                        "Error" + this.Module.Name,
                        ex.Message + " " + ex.StackTrace);
                    ViewBag.Result = false;
                }

                return this.View("index", model);
            }

            return null;
        }

        /// <summary>
        /// "deletes" a blog entry
        /// </summary>
        /// <param name="id">blog id</param>
        /// <returns>A JSON object indicating the result</returns>
        [Authorize]
        public JsonResult Delete(int id)
        {
            bool result = false;
            if (Utils.IsBlogAdmin(((Business.Services.CustomPrincipal)User).UserId))
            {
                ContentRepository content = new ContentRepository(SessionCustom);
                content.Entity.ContentId = id;
                content.LoadByKey();
                content.Entity.Active = false;
                content.Update();

                result = true;
            }

            return this.Json(new { result = result });
        }

        /// <summary>
        /// "deletes" a comment blog entry
        /// </summary>
        /// <param name="id">comment id</param>
        /// <returns>A JSON object indicating the result</returns>
        [Authorize]
        public JsonResult DeleteComment(int id)
        {
            bool result = false;
            int userId = ((Business.Services.CustomPrincipal)User).UserId;
            CommentRepository comment = new CommentRepository(SessionCustom);
            comment.Entity.CommentId = id;
            comment.LoadByKey();
            if (comment.Entity.ContentId.HasValue)
            {
                ContentRepository content = new ContentRepository(SessionCustom);
                content.Entity.ContentId = comment.Entity.ContentId;
                content.LoadByKey();

                if ((Utils.IsBlogAdmin(userId) && content.Entity.UserId == userId) || comment.Entity.UserId == userId)
                {
                    comment.Entity.Active = false;
                    comment.Update();

                    result = true;
                }
            }

            return this.Json(new { result = result });
        }

        /// <summary>
        /// Show a form to upload an image for a blog entry
        /// </summary>
        /// <param name="edit">indicates whether is edition or creation</param>
        /// <param name="ideaId">idea id if editing</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        public ActionResult UploadBlogImage(bool? edit, int? ideaId)
        {
            if (Utils.IsBlogAdmin(((Business.Services.CustomPrincipal)User).UserId))
            {
                ViewBag.Edit = edit;
                ViewBag.IdeaId = ideaId;
                return this.View("_UploadBlogImage");
            }

            return null;
        }

        /// <summary>
        /// Receive a form to upload an image for a blog entry
        /// </summary>
        /// <param name="frameFile">the image file</param>
        /// <param name="edit">indicates whether is edition or creation</param>
        /// <param name="ideaId">idea id if editing</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        [HttpPost]
        public ActionResult UploadBlogImage(HttpPostedFileBase frameFile, bool? edit, int? ideaId)
        {
            if (Utils.IsBlogAdmin(((Business.Services.CustomPrincipal)User).UserId))
            {
                ViewBag.Edit = edit;
                ViewBag.IdeaId = ideaId;
                string serverMap = Server.MapPath("~");
                string fileName = Business.Utils.UploadFile(frameFile, serverMap, @"resources\temporal\blog\", string.Empty);

                return this.View("_UploadBlogImage", (object)fileName);
            }

            return null;
        }
              /// <summary>
        /// Set label vistas
        /// </summary>
        private void SetLabel()
        {
            LabelManagement objlabel = new LabelManagement(SessionCustom, HttpContext);
            ViewBag.TXTESCAQUICON = objlabel.GetLabelByName("TXTESCAQUICON", CurrentLanguage.LanguageId.Value);
        }
    }
}
