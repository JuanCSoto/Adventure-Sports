// <copyright file="SystemNotificationController.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>
namespace Webcore.Areas.Admin.Controllers
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
    using Core.Facades;
    using Domain.Concrete;
    using Domain.Entities;
    using Webcore.Areas.Admin.Models;

    /// <summary>
    /// Controller for SystemNotification module
    /// </summary>
    [ModulAuthorize]
    public class SystemNotificationController : AdminController
    {
        /// <summary>
        /// gets the home of SystemNotification module
        /// </summary>
        /// <param name="mod">identifier of module</param>
        /// <param name="sectionId">identifier of section</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Index(int mod, int? sectionId)
        {
            ContentManagement objcontentman = new ContentManagement(this.SessionCustom, HttpContext);
            SectionRepository objsection = new SectionRepository(this.SessionCustom);
            TemplateRepository objtemplate = new TemplateRepository(this.SessionCustom);
            TagFacade tagFacade = new TagFacade();
            objtemplate.Entity.Type = 0;

            return this.View(new SystemNotificationTemplateModel()
            {
                UserPrincipal = this.CustomUser,
                Module = this.Module,
                ColModul = CustomMemberShipProvider.GetModuls(this.CustomUser.UserId, this.SessionCustom, HttpContext),
                Templates = objtemplate.GetAll().Select(t => t.TemplateId),
                DeepFollower = sectionId != null ? Business.Utils.GetDeepFollower(objsection.GetAll(), sectionId.Value) : null,
                IContent = new Domain.Entities.Content()
                {
                    ModulId = mod,
                    SectionId = sectionId
                },
                CurrentLanguage = this.CurrentLanguage,
                Tags = tagFacade.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.TagId.ToString() })
            });
        }

        /// <summary>
        /// obtains the SystemNotification detail
        /// </summary>
        /// <param name="mod">identifier of module</param>
        /// <param name="id">identifier of section</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(int mod, int id)
        {
            ContentManagement objcontentman = new ContentManagement(SessionCustom, HttpContext);
            ContentRepository objcontent = new ContentRepository(SessionCustom);
            SystemNotificationTemplateRepository objnotification = new SystemNotificationTemplateRepository(SessionCustom);
            FileattachRepository objfiles = new FileattachRepository(SessionCustom);
            TagRepository objtag = new TagRepository(SessionCustom);
            SectionRepository objsection = new SectionRepository(SessionCustom);
            TemplateRepository objtemplate = new TemplateRepository(SessionCustom);
            TagFacade tagFacade = new TagFacade();

            objtemplate.Entity.Type = 0;

            objnotification.Entity.ContentId =
                objfiles.Entity.ContentId =
                objcontent.Entity.ContentId = id;

            objnotification.LoadByKey();
            objcontent.LoadByKey();

            IEnumerable<Tag> SelectedTags = objtag.GetTagbycontent(id);
            this.ViewBag.SelectedTags = string.Join("|", SelectedTags.Select(t => t.TagId));
            this.ViewBag.NewsTags = string.Empty;

            return this.View(
                "Index",
                new SystemNotificationTemplateModel()
                {
                    UserPrincipal = this.CustomUser,
                    ColModul = CustomMemberShipProvider.GetModuls(this.CustomUser.UserId, this.SessionCustom, HttpContext),
                    Module = this.Module,
                    ListFiles = objfiles.GetAllReadOnly(),
                    SystemNotificationTemplate = objnotification.Entity,
                    IContent = objcontent.Entity,
                    Templates = objtemplate.GetAll().Select(t => t.TemplateId),
                    ListContent = objcontent.GetContentRelation(CurrentLanguage.LanguageId.Value),
                    ListTags = SelectedTags,
                    DeepFollower = Business.Utils.GetDeepFollower(objsection.GetAll(), objcontent.Entity.SectionId.Value),
                    CurrentLanguage = this.CurrentLanguage,
                    Tags = tagFacade.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.TagId.ToString() })
                });
        }

        /// <summary>
        /// inserts or updates a item SystemNotification
        /// </summary>
        /// <param name="model">identifier of module</param>
        /// <param name="contentImage">represents a upload content image</param>
        /// <param name="videoyoutube">represents a list of videos</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SystemNotificationTemplateModel model, HttpPostedFileBase contentImage, List<string> videoyoutube, string existingTags, string newTags)
        {
            SystemNotificationTemplateRepository objnotification = new SystemNotificationTemplateRepository(this.SessionCustom);
            ContentManagement objcontent = new ContentManagement(this.SessionCustom, HttpContext);

            try
            {
                objcontent.ContentImage = contentImage;
                objcontent.CollVideos = videoyoutube;
                this.SessionCustom.Begin();

                model.IContent.LanguageId = CurrentLanguage.LanguageId;
                objcontent.ContentInsert(model.IContent);
                objnotification.Entity = model.SystemNotificationTemplate;
                objnotification.Entity.ExistingTags = !string.Empty.Equals(existingTags) ? existingTags : null;
                objnotification.Entity.NewTags = !string.Empty.Equals(newTags) ? newTags : null;

                if (objnotification.Entity.ContentId != null)
                {
                    objnotification.Entity.ContentId = objcontent.ObjContent.ContentId;
                    objnotification.Update();
                    this.InsertAudit("Update", this.Module.Name + " -> " + model.IContent.Name);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request.Form["TempFiles"]))
                    {
                        string[] files = Request.Form["TempFiles"].Split(',');

                        if (files.Length > 0)
                        {
                            if (!Directory.Exists(Path.Combine(Server.MapPath("~"), @"Files\" + objcontent.ObjContent.ContentId + @"\")))
                            {
                                Directory.CreateDirectory(Path.Combine(Server.MapPath("~"), @"Files\" + objcontent.ObjContent.ContentId + @"\"));
                            }
                        }

                        foreach (var item in files)
                        {
                            string filep = Path.Combine(Server.MapPath("~"), @"Files\Images\" + Path.GetFileName(item));
                            if (System.IO.File.Exists(filep))
                            {
                                string filedestin = Path.Combine(Server.MapPath("~"), @"Files\Images\" + Path.GetFileName(item));
                                System.IO.File.Move(filep, Path.Combine(Server.MapPath("~"), @"Files\" + objcontent.ObjContent.ContentId + @"\" + Path.GetFileName(item)));
                            }
                        }
                    }

                    objnotification.Entity.ContentId = objcontent.ObjContent.ContentId;
                    objnotification.Insert();

                    this.InsertAudit("Insert", this.Module.Name + " -> " + model.IContent.Name);
                }

                this.SessionCustom.Commit();
            }
            catch (Exception ex)
            {
                SessionCustom.RollBack();
                Utils.InsertLog(
                    this.SessionCustom,
                    "Error" + this.Module.Name,
                    ex.Message + " " + ex.StackTrace);
            }

            if (Request.Form["GetOut"] == "0")
            {
                return this.RedirectToAction("Index", "Content", new { mod = Module.ModulId });
            }
            else
            {
                return this.RedirectToAction("Detail", "SystemNotification", new { mod = Module.ModulId, id = objnotification.Entity.ContentId });
            }
        }
    }
}
