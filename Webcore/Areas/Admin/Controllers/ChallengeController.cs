// <copyright file="ChallengeController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
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
    /// Controller for Challenge module
    /// </summary>
    [ModulAuthorize]
    public class ChallengeController : AdminController
    {
        /// <summary>
        /// gets the home of challenge module
        /// </summary>
        /// <param name="mod">identifier of module</param>
        /// <param name="sectionId">identifier of section</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Index(int mod, int? sectionId)
        {
            ContentManagement objcontentman = new ContentManagement(this.SessionCustom, HttpContext);
            SectionRepository objsection = new SectionRepository(this.SessionCustom);
            TemplateRepository objtemplate = new TemplateRepository(this.SessionCustom);
            ContentRepository objcontent = new ContentRepository(SessionCustom);
            TagFacade tagFacade = new TagFacade();
            objtemplate.Entity.Type = 0;

            return this.View(new ChallengeModel()
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
                Categories = objcontent.Categories(),
                Tags = tagFacade.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.TagId.ToString() })
            });
        }

        /// <summary>
        /// obtains the challenge detail
        /// </summary>
        /// <param name="mod">identifier of module</param>
        /// <param name="id">identifier of section</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(int mod, int id)
        {
            ContentManagement objcontentman = new ContentManagement(SessionCustom, HttpContext);
            ContentRepository objcontent = new ContentRepository(SessionCustom);
            ChallengeRepository objchallenge = new ChallengeRepository(SessionCustom);
            FileattachRepository objfiles = new FileattachRepository(SessionCustom);
            TagRepository objtag = new TagRepository(SessionCustom);
            SectionRepository objsection = new SectionRepository(SessionCustom);
            TemplateRepository objtemplate = new TemplateRepository(SessionCustom);
            TagFacade tagFacade = new TagFacade();

            objtemplate.Entity.Type = 0;

            objchallenge.Entity.ContentId =
                objfiles.Entity.ContentId =
                objcontent.Entity.ContentId = id;

            objchallenge.LoadByKey();
            objcontent.LoadByKey();

            IEnumerable<Tag> SelectedTags = objtag.GetTagbycontent(id);
            this.ViewBag.SelectedTags = string.Join("|", SelectedTags.Select(t => t.TagId));
            this.ViewBag.NewsTags = string.Empty;

            return this.View(
                "Index",
                new ChallengeModel()
                {
                    UserPrincipal = this.CustomUser,
                    ColModul = CustomMemberShipProvider.GetModuls(this.CustomUser.UserId, this.SessionCustom, HttpContext),
                    Module = this.Module,
                    ListFiles = objfiles.GetAllReadOnly(),
                    Challenge = objchallenge.Entity,
                    IContent = objcontent.Entity,
                    Templates = objtemplate.GetAll().Select(t => t.TemplateId),
                    ListContent = objcontent.GetContentRelation(CurrentLanguage.LanguageId.Value),
                    ListTags = SelectedTags,
                    DeepFollower = Business.Utils.GetDeepFollower(objsection.GetAll(), objcontent.Entity.SectionId.Value),
                    CurrentLanguage = this.CurrentLanguage,
                    Categories = objcontent.Categories(),
                    Tags = tagFacade.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.TagId.ToString() })                  
                });
        }

        /// <summary>
        /// inserts or updates a item challenge
        /// </summary>
        /// <param name="model">identifier of module</param>
        /// <param name="contentImage">represents a upload content image</param>
        /// <param name="contentCoverImage">represents a upload content cover image</param>
        /// <param name="videoyoutube">represents a list of videos</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ChallengeModel model, HttpPostedFileBase contentImage, HttpPostedFileBase contentCoverImage, List<string> videoyoutube, string existingTags, string newTags)
        {
            ChallengeRepository objchallenge = new ChallengeRepository(this.SessionCustom);
            ContentManagement objcontent = new ContentManagement(this.SessionCustom, HttpContext);

            try
            {
                DateTime? currentEndDate = null;
                if (model.IContent.ContentId.HasValue)
                {
                    objchallenge.Entity.ContentId = model.IContent.ContentId;
                    objchallenge.LoadByKey();
                    currentEndDate = objchallenge.Entity.EndDate;
                    objchallenge.Entity = new Domain.Entities.Challenge();
                }

                objcontent.ContentImage = contentImage;
                objcontent.ContentCoverImage = contentCoverImage;
                objcontent.CollVideos = videoyoutube;
                this.SessionCustom.Begin();

                model.IContent.LanguageId = CurrentLanguage.LanguageId;
                objcontent.ContentInsert(model.IContent);
                objchallenge.Entity = model.Challenge;
                objchallenge.Entity.ExistingTags = !string.Empty.Equals(existingTags) ? existingTags : null;
                objchallenge.Entity.NewTags = !string.Empty.Equals(newTags) ? newTags : null;

                if (objchallenge.Entity.ContentId != null)
                {
                    objchallenge.Update();

                    bool reactivated = false;
                    if (currentEndDate < DateTime.Now.Date && model.Challenge.EndDate >= DateTime.Now.Date)
                    {
                        reactivated = true;
                    }

                    if (reactivated)
                    {
                        ContentRepository content = new ContentRepository(SessionCustom);
                        content.Entity.ContentId = model.Challenge.ContentId;
                        content.LoadByKey();
                        Business.Utilities.Notification.StartReActivateProcess(content.Entity.Frienlyname, content.Entity.ContentId.Value, this.HttpContext, this.CurrentLanguage);
                    }

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

                    objchallenge.Entity.ContentId = objcontent.ObjContent.ContentId;
                    objchallenge.Entity.Followers = 0;
                    objchallenge.Insert();

                    ContentRepository content = new ContentRepository(SessionCustom);
                    content.Entity.ContentId = model.Challenge.ContentId;
                    content.LoadByKey();

                    ////EmailNotificationRepository emailNotification = new EmailNotificationRepository(SessionCustom);
                    ////List<int> users = emailNotification.SendNewProcessNotification();
                    ////foreach (int userId in users)
                    ////{
                    ////    Business.Utilities.Notification.NewNotification(userId, Domain.Entities.Basic.EmailNotificationType.NEW_PROCESS, null, null, string.Concat("/", content.Entity.Frienlyname), content.Entity.ContentId, content.Entity.ContentId.Value, null, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
                    ////}

                    Business.Utilities.Notification.StartNewProcess(content.Entity.Frienlyname, content.Entity.ContentId.Value, this.HttpContext, this.CurrentLanguage); 

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
                return this.RedirectToAction("Detail", "Challenge", new { mod = Module.ModulId, id = objchallenge.Entity.ContentId });
            }
        }
    }
}
