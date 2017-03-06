// <copyright file="QuestionController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
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
    /// Controller for Question module
    /// </summary>
    [ModulAuthorize]
    public class QuestionController : AdminController
    {
        /// <summary>
        /// gets the home of question module
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

            return this.View(new QuestionModel()
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
        /// obtains the question detail
        /// </summary>
        /// <param name="mod">identifier of module</param>
        /// <param name="id">identifier of section</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(int mod, int id)
        {
            ContentManagement objcontentman = new ContentManagement(SessionCustom, HttpContext);
            ContentRepository objcontent = new ContentRepository(SessionCustom);
            QuestionRepository objquestion = new QuestionRepository(SessionCustom);
            FileattachRepository objfiles = new FileattachRepository(SessionCustom);
            TagRepository objtag = new TagRepository(SessionCustom);
            SectionRepository objsection = new SectionRepository(SessionCustom);
            TemplateRepository objtemplate = new TemplateRepository(SessionCustom);
            AnswerRepository objanswer = new AnswerRepository(SessionCustom);
            TagFacade tagFacade = new TagFacade();

            objtemplate.Entity.Type = 0;

            objquestion.Entity.ContentId =
                objfiles.Entity.ContentId =
                objcontent.Entity.ContentId =
                objanswer.Entity.ContentId = id;

            objquestion.LoadByKey();
            objcontent.LoadByKey();

            IEnumerable<Tag> SelectedTags = objtag.GetTagbycontent(id);
            this.ViewBag.SelectedTags = string.Join("|", SelectedTags.Select(t => t.TagId));
            this.ViewBag.NewsTags = string.Empty;

            return this.View(
                "Index",
                new QuestionModel()
                {
                    UserPrincipal = this.CustomUser,
                    ColModul = CustomMemberShipProvider.GetModuls(this.CustomUser.UserId, this.SessionCustom, HttpContext),
                    Module = this.Module,
                    ListFiles = objfiles.GetAllReadOnly(),
                    Question = objquestion.Entity,
                    IContent = objcontent.Entity,
                    Templates = objtemplate.GetAll().Select(t => t.TemplateId),
                    ListContent = objcontent.GetContentRelation(CurrentLanguage.LanguageId.Value),
                    ListTags = SelectedTags,
                    DeepFollower = Business.Utils.GetDeepFollower(objsection.GetAll(), objcontent.Entity.SectionId.Value),
                    CurrentLanguage = this.CurrentLanguage,
                    ListAnswers = objanswer.GetAllReadOnly(),
                    Categories = objcontent.Categories(),
                    Tags = tagFacade.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.TagId.ToString() })
                });
        }

        /// <summary>
        /// inserts or updates a item question
        /// </summary>
        /// <param name="model">identifier of module</param>
        /// <param name="contentImage">represents a upload content image</param>
        /// <param name="contentCoverImage">represents a upload content cover image</param>
        /// <param name="videoyoutube">represents a list of videos</param>
        /// <param name="txtanswer">an answer collection</param>
        /// <param name="fileanswer">a file answer collection (images)</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(QuestionModel model, HttpPostedFileBase contentImage, HttpPostedFileBase contentCoverImage, List<string> videoyoutube, List<string> txtanswer, List<HttpPostedFileBase> fileanswer, string existingTags, string newTags)
        {
            QuestionRepository objquestion = new QuestionRepository(this.SessionCustom);
            ContentManagement objcontent = new ContentManagement(this.SessionCustom, HttpContext);

            try
            {
                DateTime? currentEndDate = null;
                if (model.IContent.ContentId.HasValue)
                {
                    objquestion.Entity.ContentId = model.IContent.ContentId;
                    objquestion.LoadByKey();
                    currentEndDate = objquestion.Entity.EndDate;
                    objquestion.Entity = new Domain.Entities.Question();
                }

                objcontent.ContentImage = contentImage;
                objcontent.ContentCoverImage = contentCoverImage;
                objcontent.CollVideos = videoyoutube;
                this.SessionCustom.Begin();

                model.IContent.LanguageId = CurrentLanguage.LanguageId;
                objcontent.ContentInsert(model.IContent);
                objquestion.Entity = model.Question;
                objquestion.Entity.ExistingTags = !string.Empty.Equals(existingTags) ? existingTags : null;
                objquestion.Entity.NewTags = !string.Empty.Equals(newTags) ? newTags : null;

                if (objquestion.Entity.ContentId != null)
                {
                    objquestion.Update();

                    bool reactivated = false;
                    if (currentEndDate < DateTime.Now.Date && model.Question.EndDate >= DateTime.Now.Date)
                    {
                        reactivated = true;
                    }

                    if (reactivated)
                    {
                        ContentRepository content = new ContentRepository(SessionCustom);
                        content.Entity.ContentId = model.Question.ContentId;
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
                                ////string filedestin = Path.Combine(Server.MapPath("~"), @"Files\Images\" + Path.GetFileName(item));
                                string filedestin = Path.Combine(Server.MapPath("~"), @"Files\" + objcontent.ObjContent.ContentId + @"\" + Path.GetFileName(item));
                                string fileroute = @"Files\" + objcontent.ObjContent.ContentId + @"\" + Path.GetFileName(item);
                                System.IO.File.Move(filep, filedestin);
                            }
                        }
                    }

                    objquestion.Entity.ContentId = objcontent.ObjContent.ContentId;
                    objquestion.Insert();

                    ContentRepository content = new ContentRepository(SessionCustom);
                    content.Entity.ContentId = model.Question.ContentId;
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

                if (objquestion.Entity.Type.Equals(Domain.Entities.Question.TypeQuestion.Seleccion_Multiple) && txtanswer != null)
                {
                    AnswerRepository objanswer = new AnswerRepository(this.SessionCustom);

                    for (int i = 0; i < txtanswer.Count; i++)
                    {
                        string strfile = null;
                        string strvideo = null;
                        if (fileanswer[i] != null)
                        {
                            strfile = DateTime.Now.ToString("ddmmyyyyhhmmssFFF") + Path.GetExtension(fileanswer[i].FileName);
                            string filePath = @"Files/" + objquestion.Entity.ContentId.ToString() + "/" + strfile;
                            string fullPath = Path.Combine(Server.MapPath("~"), filePath);
                            fileanswer[i].SaveAs(fullPath);

                            ImageResize objimage = new ImageResize(Server.MapPath("~"));
                            objimage.Prefix = "_";
                            objimage.Width = 255;
                            objimage.Height = 130;
                            bool resized = objimage.Resize(filePath, ImageResize.TypeResize.CropProportional);
                            if (resized)
                            {
                                System.IO.File.Delete(fullPath);
                                System.IO.File.Move(Path.Combine(Server.MapPath("~"), @"Files/" + objquestion.Entity.ContentId.ToString() + "/_" + strfile), fullPath);
                            }
                        }

                        objanswer.Entity = new Domain.Entities.Answer()
                        {
                            ContentId = objquestion.Entity.ContentId,
                            Count = 0,
                            Text = txtanswer[i],
                            Image = strfile,
                            Video = strvideo
                        };
                        objanswer.Insert();
                    }
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
                return this.RedirectToAction("Detail", "Question", new { mod = Module.ModulId, id = objquestion.Entity.ContentId });
            }
        }

        /// <summary>
        /// Return the detail of an answer
        /// </summary>
        /// <param name="id">answer id</param>
        /// <returns>returns the result to action</returns>
        public ActionResult AnswerDetail(int id)
        {
            AnswerRepository objanswer = new AnswerRepository(SessionCustom);
            objanswer.Entity.AnswerId = id;
            objanswer.LoadByKey();

            return this.View(objanswer.Entity);
        }

        /// <summary>
        /// Update the information of an answer
        /// </summary>
        /// <param name="id">answer id</param>
        /// <param name="txtanswer">answer text</param>
        /// <param name="fileanswer">answer file</param>
        /// <returns>returns the result to action</returns>
        public ActionResult UpdateAnswer(int id, string txtanswer, HttpPostedFileBase fileanswer)
        {
            AnswerRepository objanswer = new AnswerRepository(SessionCustom);
            objanswer.Entity.AnswerId = id;
            objanswer.LoadByKey();
            string strfile = objanswer.Entity.Image;

            if (fileanswer != null)
            {
                strfile = DateTime.Now.ToString("ddmmyyyyhhmmssFFF") + Path.GetExtension(fileanswer.FileName);
                string filePath = @"Files/" + objanswer.Entity.ContentId.ToString() + "/" + strfile;
                string fullPath = Path.Combine(Server.MapPath("~"), filePath);
                fileanswer.SaveAs(fullPath);

                ImageResize objimage = new ImageResize(Server.MapPath("~"));
                objimage.Prefix = "_";
                objimage.Width = 255;
                objimage.Height = 130;
                bool resized = objimage.Resize(filePath, ImageResize.TypeResize.CropProportional);
                if (resized)
                {
                    System.IO.File.Delete(fullPath);
                    System.IO.File.Move(Path.Combine(Server.MapPath("~"), @"Files/" + objanswer.Entity.ContentId.ToString() + "/_" + strfile), fullPath);
                }
            }

            objanswer.Entity.Text = txtanswer;
            objanswer.Entity.Image = strfile;

            objanswer.Update();
            this.InsertAudit("Update", this.Module.Name + " -> Answer" + id);
            return this.View("AnswerDetail", objanswer.Entity);
        }
    }
}
