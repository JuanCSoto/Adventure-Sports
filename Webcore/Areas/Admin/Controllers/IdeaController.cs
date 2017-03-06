// <copyright file="IdeaController.cs" company="Dasigno">
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
    /// Controller for Idea module
    /// </summary>
    [ModulAuthorize]
    public class IdeaController : AdminController
    {
        /// <summary>
        /// gets the index of the idea module
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            IdeaRepository idea = new IdeaRepository(SessionCustom);
            ModulRepository modul = new ModulRepository(SessionCustom);

            modul.Entity.ModulId = 55;
            modul.Entity.LanguageId = CurrentLanguage.LanguageId;
            modul.Load();

            PaginInfo paginInfo = new PaginInfo()
            {
                PageIndex = 1
            };

            return this.View(new Models.Idea()
            {
                UserPrincipal = CustomUser,
                Module = modul.Entity,
                CollIdea = idea.GetAllPaging(null, paginInfo),
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                Total = paginInfo.TotalCount,
                Controller = modul.Entity.Controller,
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// Obtains a string information to render idea list
        /// </summary>
        /// <param name="mod">identifier module</param>
        /// <param name="page">page index</param>
        /// <param name="text">criteria search</param>
        /// <param name="filter">order filter</param>
        /// <param name="active">content active</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public JsonResult GetIdeas(int mod, int page, string text, short? filter, bool? active)
        {
            StringBuilder strbl = new StringBuilder();

            IdeaRepository objidea = new IdeaRepository(SessionCustom);
            PaginInfo paginInfo = new PaginInfo()
            {
                PageIndex = page
            };

            objidea.Entity.Text = text;
            objidea.Entity.Active = active;

            IEnumerable<Domain.Entities.Idea> ideas = objidea.GetAllPaging(filter, paginInfo);
            foreach (Domain.Entities.Idea idea in ideas)
            {
                strbl.AppendLine("<li id=\"li" + idea.IdeaId + "\" ondblclick=\"ctnback.editcontent(" + idea.IdeaId + ")\" onclick=\"if(ctnback.clicOk) { ctnback.contentselect(this, " + idea.IdeaId + "); } else { ctnback.clicOk = true; }\">");
                if (string.IsNullOrEmpty(idea.Image))
                {
                    strbl.AppendLine("<img id=\"" + idea.IdeaId + "\" class=\"handle\" src=\"" + Business.Utils.GetImageContent(idea.Image, idea.IdeaId.Value, 44, 44) + "\" width=\"44\" height=\"44\" />");
                }
                else
                {
                    strbl.AppendLine("<img id=\"" + idea.IdeaId + "\" class=\"handle\" src=\"/files/ideas/560x515-" + idea.Image + "\" width=\"44\" height=\"44\" />");
                }

                strbl.AppendLine("<div class=\"info-content\"><span title=\"Arrastre hacia una sección para cambiarla.\" class=\"sptitle cursor\">" + Business.Utils.TruncateWord(idea.Text, 85) + "</span><br />");
                strbl.AppendLine("<span class=\"spdate\">" + idea.Creationdate.Value.ToString("F") + "</span></div><div class=\"clear\"></div>");
                strbl.AppendLine("</li>");
            }

            return this.Json(new { html = strbl.ToString(), count = ideas.Count(), total = paginInfo.TotalCount }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// obtains the idea detail
        /// </summary>
        /// <param name="mod">identifier of module</param>
        /// <param name="id">identifier of section</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(int mod, int id)
        {
            QuestionRepository objquestion = new QuestionRepository(SessionCustom);
            ContentManagement objcontentman = new ContentManagement(SessionCustom, HttpContext);
            ContentRepository objcontent = new ContentRepository(SessionCustom);
            IdeaRepository objidea = new IdeaRepository(SessionCustom);
            FileattachRepository objfiles = new FileattachRepository(SessionCustom);
            TagRepository objtag = new TagRepository(SessionCustom);
            SectionRepository objsection = new SectionRepository(SessionCustom);
            TemplateRepository objtemplate = new TemplateRepository(SessionCustom);
            CommentRepository objcomment = new CommentRepository(SessionCustom);

            objtemplate.Entity.Type = 0;

            objidea.Entity.IdeaId =
                objcomment.Entity.IdeaId = id;
            objidea.LoadByKey();

            objquestion.Entity.ContentId = 
                objcontent.Entity.ContentId = objidea.Entity.ContentId;
            objcontent.LoadByKey();
            objquestion.LoadByKey();

            if (objquestion.Entity != null && objquestion.Entity.Type.Equals(Domain.Entities.Question.TypeQuestion.Ubicacion))
            {
                ViewBag.Location = true;
            }
            else
            {
                ViewBag.Location = false;
            }

            int totalComments = 0;
            List<CommentsPaging> comments = objcomment.CommentsPaging(0, 50, out totalComments, id);
            ViewBag.TotalComments = totalComments;

            IEnumerable<Tag> SelectedTags = objtag.GetTagbycontent(id);
            this.ViewBag.SelectedTags = string.Join("|", SelectedTags.Select(t => t.TagId));
            this.ViewBag.NewsTags = string.Empty;

            return this.View(
                "Detail",
                new IdeaModel()
                {
                    UserPrincipal = this.CustomUser,
                    ColModul = CustomMemberShipProvider.GetModuls(this.CustomUser.UserId, this.SessionCustom, HttpContext),
                    Module = this.Module,
                    ListFiles = objfiles.GetAllReadOnly(),
                    Idea = objidea.Entity,
                    IContent = objcontent.Entity,
                    Templates = objtemplate.GetAll().Select(t => t.TemplateId),
                    ListContent = objcontent.GetContentRelation(CurrentLanguage.LanguageId.Value),
                    ListTags = SelectedTags,
                    DeepFollower = Business.Utils.GetDeepFollower(objsection.GetAll(), objcontent.Entity.SectionId.Value),
                    CurrentLanguage = this.CurrentLanguage,
                    ListComments = comments
                });
        }

        /// <summary>
        /// obtains the content detail
        /// </summary>
        /// <param name="id">identifier content</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        public ActionResult ViewDetail(int id)
        {
            IdeaRepository objidea = new IdeaRepository(SessionCustom);
            UserRepository objuser = new UserRepository(SessionCustom);
            SectionRepository objSection = new SectionRepository(SessionCustom);

            objidea.Entity.IdeaId = id;
            objidea.Load();

            objuser.Entity.UserId = objidea.Entity.UserId;
            objuser.Load();

            return this.View(new InfoIdea()
            {
                Idea = objidea.Entity,
                Autor = objuser.Entity.Names
            });
        }

        /// <summary>
        /// inserts or updates a item idea
        /// </summary>
        /// <param name="model">identifier of module</param>
        /// <param name="deleteIdeaImage">image name to be deleted</param>        
        /// <returns>returns the result to action</returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(IdeaModel model, string deleteIdeaImage)
        {
            IdeaRepository objidea = new IdeaRepository(this.SessionCustom);

            try
            {
                this.SessionCustom.Begin();

                objidea.Entity = model.Idea;
                if (!string.IsNullOrEmpty(deleteIdeaImage))
                {
                    objidea.Entity.Image = null;
                }

                objidea.Update();
                this.InsertAudit("Update", this.Module.Name + " -> " + model.Idea.IdeaId);

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
                return this.RedirectToAction("Index", "Idea");
            }
            else
            {
                return this.RedirectToAction("Detail", "Idea", new { mod = Module.ModulId, id = objidea.Entity.IdeaId });
            }
        }

        /// <summary>
        /// get the comments of an idea
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="ideaId">idea id</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Comments(int pageIndex, int pageSize, int ideaId)
        {
            int total = 0;
            CommentRepository comment = new CommentRepository(SessionCustom);
            return this.View("CommentsList", comment.CommentsPaging(pageIndex, pageSize, out total, ideaId));
        }

        /// <summary>
        /// Updates a comment text
        /// </summary>
        /// <param name="id">Id of the comment</param>
        /// <param name="text">New text of the comment</param>
        /// <returns>result of the operation</returns>
        [HttpPost]
        public JsonResult UpdateComment(int id, string text)
        {
            CommentRepository objcomment = new CommentRepository(this.SessionCustom);
            objcomment.Entity.CommentId = id;
            objcomment.Entity.Text = text;

            objcomment.Update();
            this.InsertAudit("Update", this.Module.Name + " -> Comment" + id);
            return this.Json(new { result = true });
        }

        /// <summary>
        /// Blocks a comment
        /// </summary>
        /// <param name="id">Id of the comment</param>
        /// <returns>result of the operation</returns>
        [HttpPost]
        public JsonResult BlockComment(int id)
        {
            CommentRepository objcomment = new CommentRepository(this.SessionCustom);
            objcomment.Entity.CommentId = id;
            objcomment.Entity.Active = false;

            objcomment.Update();
            this.InsertAudit("Blocked", this.Module.Name + " -> Comment" + id);
            return this.Json(new { result = true });
        }

        /// <summary>
        /// UnBlocks a comment
        /// </summary>
        /// <param name="id">Id of the comment</param>
        /// <returns>result of the operation</returns>
        [HttpPost]
        public JsonResult UnBlockComment(int id)
        {
            CommentRepository objcomment = new CommentRepository(this.SessionCustom);
            objcomment.Entity.CommentId = id;
            objcomment.Entity.Active = true;

            objcomment.Update();
            this.InsertAudit("Unblocked", this.Module.Name + " -> Comment" + id);
            return this.Json(new { result = true });
        }
    }
}
