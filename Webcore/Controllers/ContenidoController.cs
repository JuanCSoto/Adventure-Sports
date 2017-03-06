// <copyright file="ContenidoController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Configuration;
   using System.Web;
   using System.Web.Mvc;
   using Business;
   using Business.Services;
   using Domain.Concrete;
   using Domain.Entities.FrontEnd;
   using Webcore.Models;

    /// <summary>
    /// controller content base
    /// </summary>
    public class ContenidoController : FrontEndController
    {
        /// <summary>
        /// gets the home of content according to identifier
        /// </summary>
        /// <param name="id">identifier of content</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Index(int id)
        {
            SetLabel();
            bool versus = false;
            FrontEndManagement objman = new FrontEndManagement(SessionCustom, HttpContext, FrontEndManagement.Type.Content, CurrentLanguage);
            BannerRepository banner = new BannerRepository(SessionCustom);
            IdeaRepository idea = new IdeaRepository(SessionCustom);
            UserAnswerRepository userAnswer = new UserAnswerRepository(SessionCustom);
            bool voted = false;

            idea.Entity.ContentId = id;

            int? currentUserId = null;
            if (User.Identity.IsAuthenticated)
            {
                currentUserId = ((CustomPrincipal)User).UserId;
                objman.BindInfo(id, currentUserId.Value);
                versus = Business.Utils.CheckVersus(((CustomPrincipal)User).UserId, id, this.SessionCustom);
                voted = userAnswer.CheckUserVoted(((CustomPrincipal)User).UserId, id, null);
            }
            else
            {
                objman.BindInfo(id, null);
                versus = true;
            }

            ViewBag.CurrentUserId = currentUserId;

            if (!objman.Content.Active.Value)
            {
                return this.Redirect("~/" + objman.Section.Friendlyname);
            }

            if (objman.Outcome == FrontEndManagement.Result.Ok)
            {
                ViewBag.Ideas = idea.GetAll();
                return this.View(
                    objman.Template,
                    new Models.FEContenido()
                {
                    UserPrincipal = CustomUser,
                    PageTitle = string.Format("{0} | {1}", ConfigurationManager.AppSettings["TitleHome"], objman.Content.Name),
                    Content = objman.Content,
                    Section = objman.Section,
                    Layout = objman.Layout + ".cshtml",
                    Entity = objman.Detail,
                    MetaTags = objman.Metatags,
                    Banners = banner.GetBannersBySection(objman.Section.SectionId.Value, CurrentLanguage.LanguageId.Value),
                    DeepFollower = objman.DeepFollower,
                    CurrentLanguage = CurrentLanguage,
                    IdeasCountAll = idea.IdeasCountAll(),
                    Versus = versus,
                    Voted = voted
                });
            }
            else if (objman.Outcome == FrontEndManagement.Result.NotFound)
            {
                return this.View(
                    "Mensaje",
                    new FEMessage()
                {
                    PageTitle = "Recurso no encontrado",
                    UserPrincipal = CustomUser,
                    Banners = banner.GetBannersBySection(0, CurrentLanguage.LanguageId.Value),
                    Title = "Recurso no encontrado",
                    Description = "Recurso no encontrado",
                    Message = Resources.Extend.Messages.RESOURCE_NOT_FOUND,
                    CurrentLanguage = CurrentLanguage
                });
            }
            else
            {
                return this.View(
                    "Mensaje",
                    new FEMessage()
                {
                    PageTitle = "Sistema no disponible",
                    UserPrincipal = CustomUser,
                    Banners = banner.GetBannersBySection(0, CurrentLanguage.LanguageId.Value),
                    Title = "Sistema no disponible",
                    Description = "Sistema no disponible",
                    Message = Resources.Extend.Messages.SYSTEM_ERROR,
                    CurrentLanguage = CurrentLanguage
                });
            }
        }

        /// <summary>
        /// gets the view of ideas according to the parameters
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="contentId">content id</param>
        /// <param name="filter">filter for the result</param>
        /// <param name="view">view to use in the response</param>
        /// <param name="ideasId">idea ids list to be excluded in the random result</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Ideas(int pageIndex, int pageSize, int contentId, int filter, string view, int[] ideasId)
        {
            int total = 0;
            int? currentUserId = null;
            if (User.Identity.IsAuthenticated)
            {
                currentUserId = ((CustomPrincipal)User).UserId;
            }

            IdeaRepository idea = new IdeaRepository(SessionCustom);
            CommentRepository comment = new CommentRepository(SessionCustom);
            QuestionRepository question = new QuestionRepository(SessionCustom);
            List<IdeasPaging> ideas = new List<IdeasPaging>();
            switch (filter)
            {
                case 1:
                    ideas = idea.IdeasPagingRandom(pageSize, out total, contentId, currentUserId, ideasId);
                    break;
                case 2:
                    ideas = idea.IdeasPaging(pageIndex, pageSize, out total, contentId, currentUserId);
                    break;
                case 3:
                    ideas = idea.IdeasPagingTop(pageIndex, pageSize, out total, contentId, currentUserId);
                    break;
                case 4:
                    ideas = idea.IdeasPagingRecommended(pageIndex, pageSize, out total, contentId, currentUserId);
                    break;
            }

            foreach (IdeasPaging item in ideas)
            {
                item.CollComment = comment.CommentsPaging(1, 1, out total, item.IdeaId.Value);
                if (item.CollComment.Count > 0)
                {
                    item.CollComment[0].CommentCount = total;
                }
            }

            if (string.IsNullOrEmpty(view))
            {
                view = "_CardIdeasList";
            }

            if (ideas.Count > 0)
            {
                question.Entity.ContentId = contentId;
                question.LoadByKey();
                if (question.Entity.EndDate.HasValue)
                {
                    ideas[0].QuestionType = question.Entity.Type;
                }
            }

            ViewBag.CurrentUserId = currentUserId;

            return this.View(view, ideas);
        }

        /// <summary>
        /// gets a single idea
        /// </summary>
        /// <param name="ideaId">idea id</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Idea(int ideaId)
        {
            int total = 0;
            int? currentUserId = null;
            if (User.Identity.IsAuthenticated)
            {
                currentUserId = ((CustomPrincipal)User).UserId;
            }

            IdeaRepository idea = new IdeaRepository(SessionCustom);
            CommentRepository comment = new CommentRepository(SessionCustom);
            QuestionRepository question = new QuestionRepository(SessionCustom);
            List<IdeasPaging> ideas = new List<IdeasPaging>();

            IdeasPaging singleIdea = idea.IdeaPagingById(ideaId, currentUserId);
            ideas.Add(singleIdea);
            foreach (IdeasPaging item in ideas)
            {
                item.CollComment = comment.CommentsPaging(1, 1, out total, item.IdeaId.Value);
                if (item.CollComment.Count > 0)
                {
                    item.CollComment[0].CommentCount = total;
                }
            }

            if (ideas.Count > 0)
            {
                question.Entity.ContentId = singleIdea.ContentId;
                question.LoadByKey();
                if (question.Entity.EndDate.HasValue)
                {
                    ideas[0].QuestionType = question.Entity.Type;
                }
            }

            ViewBag.CurrentUserId = currentUserId;

            return this.View("_CardIdeasList", ideas);
        }

        /// <summary>
        /// gets the view of comments according to the parameters
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="ideaId">idea id</param>
        /// <param name="view">view to use in the response</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Comentarios(int pageIndex, int pageSize, int ideaId, string view)
        {
            int? currentUserId = null;
            if (User.Identity.IsAuthenticated)
            {
                currentUserId = ((CustomPrincipal)User).UserId;
            }

            ViewBag.CurrentUserId = currentUserId;

            IdeaRepository idea = new IdeaRepository(SessionCustom);
            idea.Entity.IdeaId = ideaId;
            idea.LoadByKey();
            ViewBag.Idea = idea.Entity;
            if (idea.Entity.UserId == currentUserId)
            {
                ViewBag.IdeaOwner = true;
            }
            else
            {
                ViewBag.IdeaOwner = false;
            }

            if (string.IsNullOrEmpty(view))
            {
                view = "_ContentIdeaCommentsList";
            }

            int total = 0;
            CommentRepository comment = new CommentRepository(SessionCustom);
            List<CommentsPaging> comments = comment.CommentsPaging(pageIndex, pageSize, out total, ideaId);
            if (comments.Count > 0)
            {
                comments[0].CommentCount = total;
            }

            return this.View(view, comments);
        }

        /// <summary>
        /// gets a single comment
        /// </summary>
        /// <param name="commentId">comment id</param>
        /// <param name="view">view to use in the response</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Comentario(int commentId, string view)
        {
            int? currentUserId = null;
            if (User.Identity.IsAuthenticated)
            {
                currentUserId = ((CustomPrincipal)User).UserId;
            }

            ViewBag.CurrentUserId = currentUserId;

            CommentRepository comment = new CommentRepository(SessionCustom);
            comment.Entity.CommentId = commentId;
            comment.LoadByKey();
            int total = 0;
            List<CommentsPaging> result = comment.CommentsPagingById(commentId, comment.Entity.IdeaId, comment.Entity.ContentId, out total);
            if (result.Count > 0)
            {
                result[0].CommentCount = total;
            }

            if (string.IsNullOrEmpty(view))
            {
                if (comment.Entity.IdeaId.HasValue)
                {
                    IdeaRepository idea = new IdeaRepository(SessionCustom);
                    idea.Entity.IdeaId = comment.Entity.IdeaId;
                    idea.LoadByKey();
                    ViewBag.Idea = idea.Entity;

                    view = "_ContentIdeaCommentsList";
                }
                else if (comment.Entity.CommentId.HasValue)
                {
                    view = "_BlogEntryCommentsList";
                }
            }

            if (string.IsNullOrEmpty(view))
            {
                return null;
            }
            else
            {
                return this.View(view, result);
            }
        }

        /// <summary>
        /// gets the view of comments according to the parameters
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="contentId">content id</param>
        /// <param name="view">view to use in the response</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult ComentariosContenido(int pageIndex, int pageSize, int contentId, string view)
        {
            if (string.IsNullOrEmpty(view))
            {
                view = "_BlogEntryCommentsList";
            }

            int total = 0;
            CommentRepository comment = new CommentRepository(SessionCustom);
            List<CommentsPaging> comments = comment.CommentsPagingContent(pageIndex, pageSize, out total, contentId);

            ContentRepository content = new ContentRepository(SessionCustom);
            content.Entity.ContentId = contentId;
            content.LoadByKey();

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserId = ((CustomPrincipal)User).UserId;
            }

            if (content.Entity.UserId.HasValue)
            {
                // blog owner
                ViewBag.IdeaOwner = content.Entity.UserId.Value == ViewBag.CurrentUserId;
            }

            if (content.Entity.UserId.HasValue)
            {
                if (comments.Count > 0)
                {
                    comments[0].CommentContentOwnerId = content.Entity.UserId.Value;
                }
            }

            return this.View(view, comments);
        }

        /// <summary>
        /// gets the view of the blog entries according to the parameters
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="contentId">content id</param>
        /// <param name="view">view to use in the response</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult EntradasBlog(int pageIndex, int pageSize, int contentId, string view)
        {
            int total = 0;
            int? currentUserId = null;
            if (User.Identity.IsAuthenticated)
            {
                currentUserId = ((CustomPrincipal)User).UserId;
            }

            BlogEntryRepository blog = new BlogEntryRepository(SessionCustom);
            CommentRepository comment = new CommentRepository(SessionCustom);
            blog.Entity.ContentId = contentId;

            List<BlogEntriesPaging> blogContentEntries = blog.BlogContentEntriesPaging(pageIndex, pageSize, out total);
            foreach (BlogEntriesPaging content in blogContentEntries)
            {
                content.CollComment = comment.CommentsPagingContent(1, 3, out total, contentId);
            }

            if (string.IsNullOrEmpty(view))
            {
                view = "_ContentBlogEntriesList";
            }

            return this.View(view, blogContentEntries);
        }

        /// <summary>
        /// the current user joins a challenge
        /// </summary>
        /// <param name="contentId">content id</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        [Authorize]
        public JsonResult JoinChallenge(int contentId)
        {
            bool result = true;
            SessionCustom.Begin();

            ChaellengeFollowerRepository follower = new ChaellengeFollowerRepository(SessionCustom);
            follower.Entity.ChallengeId = contentId;
            follower.Entity.UserId = ((CustomPrincipal)User).UserId;
            follower.Entity.Date = DateTime.Now;
            follower.Insert();
            follower.Entity = new Domain.Entities.ChaellengeFollower();
            follower.Entity.ChallengeId = contentId;
            int total = follower.GetAll().Count;

            ChallengeRepository challenge = new ChallengeRepository(SessionCustom);
            challenge.Entity.ContentId = contentId;
            challenge.Entity.Followers = total;
            challenge.Update();

            SessionCustom.Commit();

            Business.UserRelation.SaveRelationAction(((CustomPrincipal)User).UserId, null, contentId, "follow", this.SessionCustom);

            return this.Json(new { result = result });
        }

        /// <summary>
        /// a set of administrator actions can be executed according to the parameters
        /// </summary>
        /// <param name="id">element id</param>
        /// <param name="type">element type</param>
        /// <param name="action">action type</param>
        /// <param name="reason">reason text</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        public JsonResult AdminAction(int id, string type, string action, string reason)
        {
            bool result = false;
            string view = string.Empty;

            if (((CustomPrincipal)User).IsFrontEndAdmin)
            {
                if (type == "idea")
                {
                    IdeaRepository idea = new IdeaRepository(SessionCustom);
                    idea.Entity.IdeaId = id;
                    idea.LoadByKey();
                    switch (action)
                    {
                        case "recommend":
                            idea.Entity.Recommended = true;
                            break;
                        case "disable":
                            idea.Entity.Active = false;
                            string url = string.Concat("http://", Request.Url.Host + Request.ApplicationPath).TrimEnd('/');
                            Business.Utilities.Notification.NewNotification(idea.Entity.UserId.Value, Domain.Entities.Basic.EmailNotificationType.IDEA_BLOCKED, null, null, url, idea.Entity.ContentId, idea.Entity.IdeaId.Value, reason, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
                            break;
                    }

                    idea.Update();
                    result = true;
                }

                if (type == "comment")
                {
                    CommentRepository comment = new CommentRepository(SessionCustom);
                    comment.Entity.CommentId = id;
                    switch (action)
                    {
                        case "disable":
                            comment.Entity.Active = false;
                            break;
                    }

                    comment.Update();
                    result = true;
                }

                if (type == "frontend")
                {
                    switch (id)
                    {
                        case (int)Domain.Entities.Basic.ForntEndEditableType.LOGO:
                            view = "logo";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.MENU_HOME:
                            view = "menuhome";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.MENU_ARTICLES:
                            view = "menuarticles";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.MENU_FAQ:
                            view = "menufaq";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.HEADER_IMAGE:
                        case (int)Domain.Entities.Basic.ForntEndEditableType.HEADER_SMALL:
                        case (int)Domain.Entities.Basic.ForntEndEditableType.HEADER_BIG:
                            view = "header";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.BACKGROUND_COLOR:
                            view = "general";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.PULSES_DESCRIPTION:
                        case (int)Domain.Entities.Basic.ForntEndEditableType.PULSES_TOOLTIP:
                            view = "pulses";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.USERS_DESCRIPTION:
                            view = "users";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.IDEAS_DESCRIPTION:
                            view = "ideas";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.STATISTICS_USER_TOP:
                            view = "Statistics";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.COMMUNITY_TEXT_1:
                            view = "Community";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.CHALLENGES_DESCRIPTION:
                            view = "challenge";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.QUESTIONS_DESCRIPTION:
                            view = "question";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.ARTICLES_SINGULAR:
                        case (int)Domain.Entities.Basic.ForntEndEditableType.ARTICLES_PLURAL:
                            view = "article";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.FOOTER_OWNER:
                            view = "footerowner";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.FOOTER_SPONSOR:
                            view = "footersponsor";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.FOOTER_CONTACT_US:
                            view = "footer";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.PULSE_VS:
                            view = "pulsevs";
                            break;
                        case (int)Domain.Entities.Basic.ForntEndEditableType.PULSE_TOP:
                            view = "pulsetop";
                            break;
                    }

                    result = true;
                }
            }

            return this.Json(new { result = result, view = view });
        }

        /// <summary>
        /// mark an answer for the current user
        /// </summary>
        /// <param name="answerId">answer id</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        [Authorize]
        public JsonResult Respuesta(int answerId)
        {
            bool result = false;
            int userId = ((CustomPrincipal)User).UserId;
            int error = 0;
            SessionCustom.Begin();

            UserAnswerRepository userAnswer = new UserAnswerRepository(SessionCustom);
            if (userAnswer.CheckUserVoted(userId, null, answerId))
            {
                SessionCustom.RollBack();
                error = 102;
            }
            else
            {
                userAnswer.Entity.UserId = userId;
                userAnswer.Entity.AnswerId = answerId;
                userAnswer.Entity.IP = Request.UserHostAddress;
                userAnswer.Entity.Date = DateTime.Now;
                userAnswer.Insert();

                userAnswer.Entity = new Domain.Entities.UserAnswer();
                userAnswer.Entity.AnswerId = answerId;
                int total = userAnswer.GetAll().Count;

                AnswerRepository answer = new AnswerRepository(SessionCustom);
                answer.Entity.AnswerId = answerId;
                answer.Load();
                answer.Entity.Count = total;
                answer.Update();

                result = true;
                SessionCustom.Commit();

                Business.UserRelation.SaveRelationAction(((CustomPrincipal)User).UserId, null, answer.Entity.AnswerId.Value, "vote", this.SessionCustom);
            }

            return this.Json(new { result = result, error = error });
        }

        /// <summary>
        /// set a reward action for sharing the site content
        /// </summary>
        /// <param name="ideaId">idea id</param>
        /// <param name="network">social network name</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        public JsonResult Compartir(int ideaId, string network)
        {
            bool result = true;

            IdeaRepository idea = new IdeaRepository(SessionCustom);
            idea.Entity.IdeaId = ideaId;
            idea.LoadByKey();

            int? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = ((CustomPrincipal)User).UserId;
            }

            Business.Utilities.Notification.NewNotification(idea.Entity.UserId.Value, null, Domain.Entities.Basic.SystemNotificationType.RECEIVE_IDEA_SHARE, userId, string.Concat("/", idea.Entity.Friendlyurlid), idea.Entity.ContentId, idea.Entity.IdeaId.Value, null, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);

            return this.Json(new { result = result });
        }

        /// <summary>
        /// gets the view of the pulses according to the parameters
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="text">search text</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Pulses(int page, string text)
        {
            SetLabel();
            List<Pulse> pulses = new List<Pulse>();
            ContentRepository content = new ContentRepository(SessionCustom);
            int total = 0;
            if (string.IsNullOrEmpty(text))
            {
                text = null;
            }
            else if (text.Length > 100)
            {
                text = text.Substring(0, 100);
            }

            pulses = content.Pulses(page, 6, out total, text, CurrentLanguage.LanguageId);

            if (page == 0)
            {
                return this.View("_Pulses", pulses);
            }
            else
            {
                return this.View("_PulseList", pulses);
            }
        }

          /// <summary>
        /// Set lenguage label
        /// </summary>
        private void SetLabel()
        {
            LabelManagement objlabel = new LabelManagement(SessionCustom, HttpContext);
            ViewBag.TXTFIRSFIELD = objlabel.GetLabelByName("TXTFIRSFIELD", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTSECOFIRS = objlabel.GetLabelByName("TXTSECOFIRS", CurrentLanguage.LanguageId.Value);
            ViewBag.DESCRIPTION = objlabel.GetLabelByName("DESCRIPTION", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTANOFIELD = objlabel.GetLabelByName("TXTANOFIELD", CurrentLanguage.LanguageId.Value);
            ViewBag.PARTICIPATION = objlabel.GetLabelByName("PARTICIPATION", CurrentLanguage.LanguageId.Value);
              ViewBag.TXTPARCIUINI = objlabel.GetLabelByName("TXTPARCIUINI", CurrentLanguage.LanguageId.Value);
             ViewBag.TXTTUIDEA = objlabel.GetLabelByName("TXTTUIDEA", CurrentLanguage.LanguageId.Value);
              ViewBag.CITIZEN = objlabel.GetLabelByName("CITIZEN", CurrentLanguage.LanguageId.Value);
               ViewBag.RETO = objlabel.GetLabelByName("RETO", CurrentLanguage.LanguageId.Value);
               ViewBag.CITY = objlabel.GetLabelByName("CITY", CurrentLanguage.LanguageId.Value);
               ViewBag.DAYS = objlabel.GetLabelByName("DAYS", CurrentLanguage.LanguageId.Value);
               ViewBag.RESIDUARY = objlabel.GetLabelByName("RESIDUARY", CurrentLanguage.LanguageId.Value);
               ViewBag.TXTOPSSORRY = objlabel.GetLabelByName("TXTOPSSORRY", CurrentLanguage.LanguageId.Value);
               ViewBag.VOTE = objlabel.GetLabelByName("VOTE", CurrentLanguage.LanguageId.Value);
               ViewBag.SELECCION = objlabel.GetLabelByName("SELECCION", CurrentLanguage.LanguageId.Value);
               ViewBag.MULTIPLE = objlabel.GetLabelByName("MULTIPLE", CurrentLanguage.LanguageId.Value);
               ViewBag.TXTESCREP = objlabel.GetLabelByName("TXTESCREP", CurrentLanguage.LanguageId.Value);
               ViewBag.TXTOPEQUE = objlabel.GetLabelByName("TXTOPEQUE", CurrentLanguage.LanguageId.Value);
               ViewBag.EDIT = objlabel.GetLabelByName("EDIT", CurrentLanguage.LanguageId.Value);
               ViewBag.DELETE = objlabel.GetLabelByName("DELETE", CurrentLanguage.LanguageId.Value);
               ViewBag.NEARESTCITIZENS = objlabel.GetLabelByName("NEARESTCITIZENS", CurrentLanguage.LanguageId.Value);
               ViewBag.PULSES = objlabel.GetLabelByName("PULSES", CurrentLanguage.LanguageId.Value);
               ViewBag.RETOS = objlabel.GetLabelByName("RETOS", CurrentLanguage.LanguageId.Value);
               ViewBag.TXTUNTTRA = objlabel.GetLabelByName("TXTUNTTRA", CurrentLanguage.LanguageId.Value);
               ViewBag.TXTRESUL = objlabel.GetLabelByName("TXTRESUL", CurrentLanguage.LanguageId.Value);
               ViewBag.TXTPARCIU = objlabel.GetLabelByName("TXTPARCIU", CurrentLanguage.LanguageId.Value);
               ViewBag.PREMIUM = objlabel.GetLabelByName("PREMIUM", CurrentLanguage.LanguageId.Value);
               ViewBag.THEYJOINED = objlabel.GetLabelByName("THEYJOINED", CurrentLanguage.LanguageId.Value);
               ViewBag.TXTDIARES = objlabel.GetLabelByName("TXTDIARES", CurrentLanguage.LanguageId.Value);
               ViewBag.OPEN = objlabel.GetLabelByName("OPEN", CurrentLanguage.LanguageId.Value);
               ViewBag.WISH = objlabel.GetLabelByName("WISH", CurrentLanguage.LanguageId.Value);
               ViewBag.LOCATION = objlabel.GetLabelByName("LOCATION", CurrentLanguage.LanguageId.Value);
               ViewBag.PARTICIPATES = objlabel.GetLabelByName("PARTICIPATES", CurrentLanguage.LanguageId.Value); 
        }
    }
}