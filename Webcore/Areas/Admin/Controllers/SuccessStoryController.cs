// <copyright file="SuccessStoryController.cs" company="Dasigno">
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
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities.FrontEnd;
    using Webcore.Areas.Admin.Models;
    using Core.Facades;
    using Domain.Entities;

    /// <summary>
    /// Controller for SuccessStoryEntry module
    /// </summary>
    [ModulAuthorize]
    public class SuccessStoryController : AdminController
    {
        /// <summary>
        /// gets the home of SuccessStoryEntry module
        /// </summary>
        /// <param name="mod">identifier of module</param>
        /// <param name="sectionId">identifier of section</param>
        /// <param name="idpostulate">Id de la postulacion</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Index(int mod, int? sectionId, int? idpostulate)
        {
            ContentManagement objcontentman = new ContentManagement(this.SessionCustom, HttpContext);
            SectionRepository objsection = new SectionRepository(this.SessionCustom);
            TemplateRepository objtemplate = new TemplateRepository(this.SessionCustom);
            TagFacade tagFacade = new TagFacade();
            objtemplate.Entity.Type = 2;

            if (idpostulate != null)
            {
                Core.Facades.SuccessStoryPostulateFacade dbpostulate = new SuccessStoryPostulateFacade();

                Domain.Entities.Generic.SuccessStoryPostulate postulate = dbpostulate.GetById(idpostulate.Value, this.CurrentLanguage.LanguageId.Value);
                Domain.Entities.SuccessStory successtory = new Domain.Entities.SuccessStory();
                Domain.Entities.Content content = new Domain.Entities.Content();

                content.ModulId = mod;
                content.SectionId = sectionId;

                if (postulate.LanguageId == (int)Domain.Entities.Enums.LanguageEnum.English)
                {
                    content.NameIngles = postulate.Name;
                    successtory.DescriptionIngles = postulate.Description;
                    successtory.ProblemsSolvedIngles = postulate.ConcreteProblems;
                    successtory.SocialImpactIngles = postulate.InnovativeUrbanSolution;
                }
                else
                {
                    content.Name = postulate.Name;
                    successtory.Description = postulate.Description;
                    successtory.ProblemsSolved = postulate.ConcreteProblems;
                    successtory.SocialImpact = postulate.InnovativeUrbanSolution;
                }

                content.Category = postulate.CategoryName;
                content.UserId = postulate.UserId;
                content.LanguageId = postulate.LanguageId;
                successtory.InstitutionImplements = postulate.ResponsibleOrganization;
                successtory.InstitutionSource = postulate.ResponsibleNames;
                successtory.Category = postulate.CategoryName;
                successtory.Url = postulate.Documents;
                successtory.CityID = postulate.CityId;

                IList<Tag> listaTags = tagFacade.GetBySuccessStoryPostulate(idpostulate.Value);
                string tagsNews = string.Empty;
                string existingTags = string.Empty;

                foreach (Tag temp in listaTags)
                {
                    if (temp.TagId != null)
                    {
                        existingTags += temp.TagId + "|";
                    }
                    else
                    {
                        tagsNews += temp.Name + "|";
                    }
                }

                if (tagsNews.EndsWith("|"))
                {
                    tagsNews = tagsNews.Substring(0, tagsNews.Length - 1);
                }

                if (existingTags.EndsWith("|"))
                {
                    existingTags = existingTags.Substring(0, existingTags.Length - 1);
                }

                this.ViewBag.SelectedTags = existingTags;
                this.ViewBag.NewsTags = tagsNews;

                return this.View(new SuccessStoryModel()
                {
                    PostulateId = idpostulate,
                    SuccessStory = successtory,
                    IContent = content,
                    UserPrincipal = this.CustomUser,
                    Module = this.Module,
                    ColModul = CustomMemberShipProvider.GetModuls(this.CustomUser.UserId, this.SessionCustom, HttpContext),
                    Templates = objtemplate.GetAll().Select(t => t.TemplateId),
                    DeepFollower = sectionId != null ? Business.Utils.GetDeepFollower(objsection.GetAll(), sectionId.Value) : null,
                    CurrentLanguage = this.CurrentLanguage,
                    Tags = tagFacade.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.TagId.ToString() })
                });
            }

            return this.View(new SuccessStoryModel()
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
        /// obtains the challenge detail
        /// </summary>
        /// <param name="mod">identifier of module</param>
        /// <param name="id">identifier of section</param>
        /// <param name="sectionId">seccion del molulo</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(int mod, int id, int? sectionId)
        {
            ContentManagement objcontentman = new ContentManagement(SessionCustom, HttpContext);
            ContentRepository objcontent = new ContentRepository(SessionCustom);
            SuccessStoryRepository objSuccessStory = new SuccessStoryRepository(SessionCustom);
            FileattachRepository objfiles = new FileattachRepository(SessionCustom);
            TagRepository objtag = new TagRepository(SessionCustom);
            SectionRepository objsection = new SectionRepository(SessionCustom);
            TemplateRepository objtemplate = new TemplateRepository(SessionCustom);
            CommentRepository objcomment = new CommentRepository(SessionCustom);
            TagFacade tagFacade = new TagFacade();

            objtemplate.Entity.Type = 0;

            objSuccessStory.Entity.ContentId =
                objfiles.Entity.ContentId =
                objcomment.Entity.ContentId = 
                objcontent.Entity.ContentId = id;

            objSuccessStory.LoadByKey();
            objcontent.LoadByKey();

            int totalComments = 0;
            List<CommentsPaging> comments = objcomment.CommentsPagingContent(0, 50, out totalComments, id);
            ViewBag.TotalComments = totalComments;

            IEnumerable<Tag> SelectedTags = objtag.GetTagbycontent(id);
            this.ViewBag.SelectedTags = string.Join("|", SelectedTags.Select(t => t.TagId));
            this.ViewBag.NewsTags = string.Empty;

            return this.View(
                "Index",
                new SuccessStoryModel()
                {
                    UserPrincipal = this.CustomUser,
                    ColModul = CustomMemberShipProvider.GetModuls(this.CustomUser.UserId, this.SessionCustom, HttpContext),
                    Module = this.Module,
                    ListFiles = objfiles.GetAllReadOnly(),
                    SuccessStory = objSuccessStory.Entity,
                    IContent = objcontent.Entity,
                    Templates = objtemplate.GetAll().Select(t => t.TemplateId),
                    ListContent = objcontent.GetContentRelation(CurrentLanguage.LanguageId.Value),
                    ListTags = SelectedTags,
                    DeepFollower = Business.Utils.GetDeepFollower(objsection.GetAll(), objcontent.Entity.SectionId.Value),
                    CurrentLanguage = this.CurrentLanguage,
                    ListComments = comments,
                    Tags = tagFacade.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.TagId.ToString() })
                });
        }

        /// <summary>
        /// inserts or updates a item SuccessStory entry
        /// </summary>
        /// <param name="model">identifier of module</param>
        /// <param name="contentImage">represents a upload content image</param>
        /// <param name="contentCoverImage">represents a upload content cover image</param>
        /// <param name="videoyoutube">represents a list of videos</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SuccessStoryModel model, HttpPostedFileBase contentImage, HttpPostedFileBase contentCoverImage, List<string> videoyoutube, string existingTags, string newTags)
        {
            SuccessStoryRepository objSuccessStory = new SuccessStoryRepository(this.SessionCustom);
            ContentManagement objcontent = new ContentManagement(this.SessionCustom, HttpContext);

            try
            {
                objcontent.ContentImage = contentImage;
                objcontent.ContentCoverImage = contentCoverImage;
                objcontent.CollVideos = videoyoutube;
                this.SessionCustom.Begin();

                model.IContent.LanguageId = CurrentLanguage.LanguageId;
                objcontent.ContentInsert(model.IContent, 4);
                objSuccessStory.Entity = model.SuccessStory;
                objSuccessStory.Entity.ExistingTags = !string.Empty.Equals(existingTags) ? existingTags : null;
                objSuccessStory.Entity.NewTags = !string.Empty.Equals(newTags) ? newTags : null;

                if (objSuccessStory.Entity.ContentId != null)
                {
                    objSuccessStory.Update();
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

                    objSuccessStory.Entity.ContentId = objcontent.ObjContent.ContentId;
                    objSuccessStory.Insert();

                    this.InsertAudit("Insert", this.Module.Name + " -> " + model.IContent.Name);
                }

                this.SessionCustom.Commit();

                if (model.PostulateId != null)
                {
                    SuccessStoryPostulateFacade dbpostulate = new SuccessStoryPostulateFacade();
                    Domain.Entities.Generic.SuccessStoryPostulate postulate = dbpostulate.GetById(model.PostulateId.Value, this.CurrentLanguage.LanguageId.Value);
                    postulate.SuccessStoryId = objcontent.ObjContent.ContentId;
                    postulate.State = (int)Domain.Entities.Enums.SuccessStoryPostulateStateEnum.Published;
                    if (dbpostulate.Update(postulate))
                    {
                        string domainUrl = string.Format("/SuccessStory/Story/{0}", objcontent.ObjContent.ContentId.Value);
                        this.SendUserNotification(postulate.UserId, domainUrl, objcontent.ObjContent.ContentId.Value);
                    }

                    this.InsertAudit("Update Postulate", this.Module.Name + " -> " + model.IContent.Name);
                }
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
                return this.RedirectToAction("Detail", "SuccessStory", new { mod = Module.ModulId, id = objSuccessStory.Entity.ContentId });
            }
        }

        /// <summary>
        /// Envia notificación en la plataforma y correo electrónico del usuario.
        /// </summary>
        /// <param name="userId">Id del usuario actual.</param>
        /// <param name="url">Ruta completa de la url.</param>
        /// <param name="successStoryId">Identificador del caso de exito</param>
        private void SendUserNotification(int userId, string url, int successStoryId)
        {
            string citiesUrl = System.Configuration.ConfigurationManager.AppSettings["PathHost"];
            Business.Utilities.Notification.NewNotification(userId, Domain.Entities.Basic.EmailNotificationType.PUBLICATEDESTORY, null, userId, citiesUrl + url, successStoryId, successStoryId, null, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
            Business.Utilities.Notification.NewNotification(userId, null, Domain.Entities.Basic.SystemNotificationType.PUBLICATEDESTORY, userId, url, successStoryId, successStoryId, null, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
        }
    }
}
