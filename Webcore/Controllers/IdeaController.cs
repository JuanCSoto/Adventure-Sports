// <copyright file="IdeaController.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Webcore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// controller for the ideas action
    /// </summary>
    public class IdeaController : FrontEndController
    {
        /// <summary>
        /// gets the idea page
        /// </summary>
        /// <param name="id">idea id</param>
        /// <returns>returns the result to action</returns>
        [HttpGet]
        public ActionResult Index(int id)
        {
            FrontEndManagement objman = new FrontEndManagement(SessionCustom, HttpContext, FrontEndManagement.Type.Idea, CurrentLanguage);
            int? currentUserId = null;
            if (User.Identity.IsAuthenticated)
            {
                currentUserId = ((CustomPrincipal)User).UserId;
                objman.BindInfo(id, currentUserId);
                ViewBag.CurrentUserId = currentUserId;
            }
            else
            {
                objman.BindInfo(id, null);
            }

            IdeaRepository idea = new IdeaRepository(SessionCustom);
            idea.Entity.IdeaId = id;
            idea.LoadByKey();

            if (idea.Entity.UserId == currentUserId)
            {
                ViewBag.IdeaOwner = true;
            }
            else
            {
                ViewBag.IdeaOwner = false;
            }

            ContentRepository content = new ContentRepository(SessionCustom);
            content.Entity.ContentId = idea.Entity.ContentId;
            content.LoadByKey();

            UserRepository user = new UserRepository(SessionCustom);
            user.Entity.UserId = idea.Entity.UserId;
            user.LoadByKey();

            int? userid = null;
            if (User.Identity.IsAuthenticated)
            {
                userid = ((CustomPrincipal)User).UserId;
            }

            Domain.Entities.FrontEnd.IdeasPaging ideaPaging = idea.IdeaPagingById(id, userid);
            ViewBag.IdeaPaging = ideaPaging;

            int total = 0;
            CommentRepository comment = new CommentRepository(SessionCustom);
            List<Domain.Entities.FrontEnd.CommentsPaging> comments = comment.CommentsPaging(1, 6, out total, id);

            return this.View(new Models.FEIdea()
            {
                UserPrincipal = CustomUser,
                Entity = idea.Entity,
                CollComments = comments,
                ObjContent = content.Entity,
                ObjUser = user.Entity,
                MetaTags = objman.Metatags,
                CurrentLanguage = CurrentLanguage,
                CommentCount = total,
                IdeasCountAll = idea.IdeasCountAll()
            });
        }

        /// <summary>
        /// gets the idea layer
        /// </summary>
        /// <param name="id">idea id</param>
        /// <param name="layer">indicates if the layer shall be presented or not</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Index(int id, bool layer)
        {
            FrontEndManagement objman = new FrontEndManagement(SessionCustom, HttpContext, FrontEndManagement.Type.Idea, CurrentLanguage);
            int? currentUserId = null;
            if (User.Identity.IsAuthenticated)
            {
                currentUserId = ((CustomPrincipal)User).UserId;
                objman.BindInfo(id, currentUserId);
                ViewBag.CurrentUserId = currentUserId;
            }
            else
            {
                objman.BindInfo(id, null);
            }

            IdeaRepository idea = new IdeaRepository(SessionCustom);
            idea.Entity.IdeaId = id;
            idea.LoadByKey();

            if (idea.Entity.UserId == currentUserId)
            {
                ViewBag.IdeaOwner = true;
            }
            else
            {
                ViewBag.IdeaOwner = false;
            }

            ContentRepository content = new ContentRepository(SessionCustom);
            content.Entity.ContentId = idea.Entity.ContentId;
            content.LoadByKey();

            UserRepository user = new UserRepository(SessionCustom);
            user.Entity.UserId = idea.Entity.UserId;
            user.LoadByKey();

            int? userid = null;
            if (User.Identity.IsAuthenticated)
            {
                userid = ((CustomPrincipal)User).UserId;
            }

            Domain.Entities.FrontEnd.IdeasPaging ideaPaging = idea.IdeaPagingById(id, userid);
            ViewBag.IdeaPaging = ideaPaging;

            int total = 0;
            CommentRepository comment = new CommentRepository(SessionCustom);
            List<Domain.Entities.FrontEnd.CommentsPaging> comments = comment.CommentsPaging(1, 6, out total, id);

            return this.View(
                "LayerIndex",
                new Models.FEIdea()
            {
                Entity = idea.Entity,
                CollComments = comments,
                ObjContent = content.Entity,
                ObjUser = user.Entity,
                CommentCount = total
            });
        }

        /// <summary>
        /// Creates a new idea
        /// </summary>
        /// <param name="contentId">content id</param>
        /// <param name="text">idea text</param>
        /// <param name="image">idea image</param>
        /// <param name="video">idea video</param>
        /// <param name="xCoordinate">idea x coordinate</param>
        /// <param name="yCoordinate">idea y coordinate</param>
        /// <returns>returns a JSON object with the result</returns>
        [Authorize]
        public JsonResult Crear(int contentId, string text, string image, string video, double? xCoordinate, double? yCoordinate)
        {
            bool result = false;

            SessionCustom.Begin();

            IdeaRepository idea = new IdeaRepository(SessionCustom);
            idea.Entity.Text = text;
            idea.Entity.ContentId = contentId;
            idea.Entity.UserId = ((CustomPrincipal)User).UserId;
            idea.Entity.Creationdate = DateTime.Now;
            idea.Entity.Active = true;
            idea.Entity.Views = 0;
            idea.Entity.Distinguished = false;
            idea.Entity.Recommended = false;
            idea.Entity.Views = 0;
            idea.Entity.XCoordinate = xCoordinate;
            idea.Entity.YCoordinate = yCoordinate;
            object id = idea.Insert();
            int ideaId;
            if (int.TryParse(id.ToString(), out ideaId))
            {
                FriendlyurlRepository friendlyurl = new FriendlyurlRepository(SessionCustom);
                friendlyurl.Entity.Friendlyurlid = Utils.GetFindFrienlyName(this.SessionCustom, text, ideaId);
                friendlyurl.Entity.Id = ideaId;
                friendlyurl.Entity.Type = Friendlyurl.FriendlyType.Idea;
                friendlyurl.Entity.LanguageId = CurrentLanguage.LanguageId;
                friendlyurl.Insert();

                if (!string.IsNullOrEmpty(image))
                {
                    image = System.IO.Path.GetFileName(image);
                    string serverMap = Server.MapPath("~");
                    string origin = serverMap + @"\resources\temporal\ideas\" + image;
                    if (System.IO.File.Exists(origin))
                    {
                        if (!System.IO.Directory.Exists(serverMap + @"\files\ideas\"))
                        {
                            System.IO.Directory.CreateDirectory(serverMap + @"\files\ideas\");
                        }

                        System.IO.File.Move(serverMap + @"\resources\temporal\ideas\560x515-" + image, serverMap + @"\files\ideas\560x515-" + image);
                        System.IO.File.Move(serverMap + @"\resources\temporal\ideas\580x445-" + image, serverMap + @"\files\ideas\580x445-" + image);
                        System.IO.File.Move(serverMap + @"\resources\temporal\ideas\100x70-" + image, serverMap + @"\files\ideas\100x70-" + image);
                        System.IO.File.Move(origin, serverMap + @"\files\ideas\" + image);
                    }

                    idea.Entity = new Idea();
                    idea.Entity.IdeaId = ideaId;
                    idea.Entity.Image = image;
                    idea.Update();
                }
                else if (!string.IsNullOrEmpty(video))
                {
                    idea.Entity = new Idea();
                    idea.Entity.IdeaId = ideaId;
                    idea.Entity.Video = video;
                    idea.Update();
                }

                Regex regular = new Regex(@"(#\w+)");
                MatchCollection hashTags = regular.Matches(text);
                if (hashTags.Count > 0)
                {
                    HashTagRepository hashTagRepository = new HashTagRepository(SessionCustom);
                    IdeaHashTagRepository ideaHashTagRepository = new IdeaHashTagRepository(SessionCustom);
                    int hashTagId = 0;
                    foreach (Match match in hashTags)
                    {
                        hashTagRepository.Entity.Value = match.Value;
                        hashTagRepository.Entity.HashTagId = null;
                        hashTagRepository.Load();
                        if (hashTagRepository.Entity.HashTagId.HasValue)
                        {
                            hashTagId = hashTagRepository.Entity.HashTagId.Value;
                        }
                        else
                        {
                            hashTagRepository.Entity.Count = 1;
                            object scalar = hashTagRepository.Insert();
                            if (!int.TryParse(scalar.ToString(), out hashTagId))
                            {
                                hashTagId = 0;
                            }
                        }

                        if (hashTagId > 0)
                        {
                            ideaHashTagRepository.Entity.HashTagId = hashTagId;
                            ideaHashTagRepository.Entity.IdeaId = ideaId;

                            if (!ideaHashTagRepository.Exist())
                            {
                                ideaHashTagRepository.Insert();
                            }
                        }
                    }
                }

                result = true;
                SessionCustom.Commit();

                ContentRepository content = new ContentRepository(SessionCustom);
                content.Entity.ContentId = contentId;
                content.LoadByKey();
                if (content.Entity.Template == "Challenge")
                {
                    Utils.SetUserRewardAction(((CustomPrincipal)User).UserId, RewardAction.UserActionType.IdeaChallenge, 408, 21, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage);
                }
                else if (content.Entity.Template == "Question")
                {
                    Utils.SetUserRewardAction(((CustomPrincipal)User).UserId, RewardAction.UserActionType.IdeaQuestion, 154, 12, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage);
                }

                Business.UserRelation.SaveRelationAction(((CustomPrincipal)User).UserId, null, ideaId, "idea", this.SessionCustom);
            }
            else
            {
                SessionCustom.RollBack();
            }

            return this.Json(new { result = result });
        }

        /// <summary>
        /// Show a form to report an idea
        /// </summary>
        /// <param name="id">idea id</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Reportar(int id)
        {
            return this.View("_ReportIdea", id);
        }

        /// <summary>
        /// receive a form report
        /// </summary>
        /// <param name="motive">report motive</param>
        /// <param name="text">text motive</param>
        /// <param name="ideaId">idea id</param>
        /// <returns>returns a JSON object with the result</returns>
        [Authorize]
        public JsonResult ReportarIdea(string motive, string text, int ideaId)
        {
            bool result = false;

            SessionCustom.Begin();

            IdeaReportRepository ideaReport = new IdeaReportRepository(SessionCustom);
            ideaReport.Entity.IdeaId = ideaId;
            ideaReport.Entity.UserId = ((CustomPrincipal)User).UserId;
            ideaReport.Entity.Text = text;
            ideaReport.Entity.Motive = motive;

            object id = ideaReport.Insert();
            int ideaReportId;
            if (int.TryParse(id.ToString(), out ideaReportId))
            {
                result = true;
                SessionCustom.Commit();
            }
            else
            {
                SessionCustom.RollBack();
            }

            return this.Json(new { result = result });
        }

        /// <summary>
        /// Show a form to upload an image for an idea
        /// </summary>
        /// <param name="edit">a value indicating if editing or creating</param>
        /// <param name="ideaId">idea id when editing</param>
        /// <returns>returns the result to action</returns>
        public ActionResult UploadIdeaImage(bool? edit, int? ideaId)
        {
            ViewBag.Edit = edit;
            ViewBag.IdeaId = ideaId;
            return this.View("_UploadIdeaImage");
        }

        /// <summary>
        /// receive a form image for an idea
        /// </summary>
        /// <param name="ideaFile">posted file</param>
        /// <param name="edit">a value indicating if editing or creating</param>
        /// <param name="ideaId">idea id</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult UploadIdeaImage(HttpPostedFileBase ideaFile, bool? edit, int? ideaId)
        {
            ViewBag.Edit = edit;
            ViewBag.IdeaId = ideaId;
            string serverMap = Server.MapPath("~");
            string fileName = Business.Utils.UploadFile(ideaFile, serverMap, @"resources\temporal\ideas\", string.Empty);

            ImageResize imageResize = new ImageResize(serverMap);
            imageResize.Width = 560;
            imageResize.Height = 515;
            imageResize.Prefix = "560x515-";
            imageResize.Resize(serverMap + @"\resources\temporal\ideas\" + fileName, ImageResize.TypeResize.Proportional);

            imageResize.Width = 580;
            imageResize.Height = 445;
            imageResize.Prefix = "580x445-";
            imageResize.Resize(serverMap + @"\resources\temporal\ideas\" + fileName, ImageResize.TypeResize.Proportional);

            imageResize.Width = 100;
            imageResize.Height = 70;
            imageResize.Prefix = "100x70-";
            imageResize.Resize(serverMap + @"\resources\temporal\ideas\" + fileName, ImageResize.TypeResize.Proportional);

            return this.View("_UploadIdeaImage", (object)fileName);
        }

        /// <summary>
        /// Deletes an image from an idea
        /// </summary>
        /// <param name="image">image name</param>
        /// <returns>returns a JSON object with the result</returns>
        [HttpPost]
        public JsonResult DeleteIdeaImage(string image)
        {
            string pathImage = Server.MapPath("~/resources/temporal/ideas/100x70-" + image);
            if (System.IO.File.Exists(pathImage))
            {
                System.IO.File.Delete(pathImage);
            }

            return this.Json(new { result = true });
        }

        /// <summary>
        /// Obtiene un nuevo versus
        /// </summary>
        /// <param name="id">contenido del versus</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Versus(int id)
        {
            return this.View(id);
        }

        /// <summary>
        /// Obtiene un nuevo versus
        /// </summary>
        /// <param name="contentId">contenido del versus</param>
        /// <returns>returns the result to action</returns>
        public ActionResult GetVersus(int contentId)
        {
            VersusRepository versus = new VersusRepository(SessionCustom);
            ContentRepository content = new ContentRepository(SessionCustom);

            content.Entity.ContentId =
                versus.Entity.ContentId = contentId;
            content.LoadByKey();

            versus.Entity.UserId = ((CustomPrincipal)User).UserId;
            List<Domain.Entities.FrontEnd.IdeasPaging> ideas = versus.GetVersus();
            ViewBag.Content = content.Entity;

            return this.View("_ContentIdeasVersus", ideas);
        }

        /// <summary>
        /// Obtiene el listado del top de ideas para un contenido
        /// </summary>
        /// <param name="contentId">Contenido al cual pertenecen las ideas</param>
        /// <param name="count">number of ideas to show</param>
        /// <returns>View del top</returns>
        public ActionResult TopIdeas(int contentId, int count)
        {
            IdeaRepository idea = new IdeaRepository(SessionCustom);
            idea.Entity.ContentId = contentId;
            List<Domain.Entities.FrontEnd.IdeasPaging> ideas = idea.TopIdeas(count);

            return this.View("_ContentIdeasTop", ideas);
        }

        /// <summary>
        /// Action to vote for an idea in a versus
        /// </summary>
        /// <param name="contentId">content id</param>
        /// <param name="ideaIdA">first idea id</param>
        /// <param name="ideaIdB">second idea id</param>
        /// <param name="voteIdeaId">idea id that received the vote</param>
        /// <returns>returns a JSON object with the result</returns>
        [Authorize]
        public JsonResult VotarVersus(int contentId, int ideaIdA, int ideaIdB, int voteIdeaId)
        {
            bool result = false;

            if ((ideaIdA == voteIdeaId || ideaIdB == voteIdeaId) && ideaIdB != ideaIdA)
            {
                SessionCustom.Begin();

                VersusRepository versus = new VersusRepository(SessionCustom);
                versus.Entity.ContentId = contentId;
                versus.Entity.UserId = ((CustomPrincipal)User).UserId;
                versus.Entity.IdeaIdA = ideaIdA;
                versus.Entity.IdeaIdB = ideaIdB;
                versus.Entity.WinnerId = voteIdeaId;

                if (!versus.VoteExists())
                {
                    object id = versus.Insert();
                    int versusId;
                    if (int.TryParse(id.ToString(), out versusId))
                    {
                        result = true;
                        SessionCustom.Commit();

                        IdeaRepository idea = new IdeaRepository(SessionCustom);
                        idea.Entity.IdeaId = voteIdeaId;
                        idea.LoadByKey();
                        bool update = idea.Entity.UserId.Value == ((CustomPrincipal)User).UserId ? true : false;
                        Utils.SetUserRewardAction(idea.Entity.UserId.Value, RewardAction.UserActionType.ReciveVs, 12, 50000, this.SessionCustom, ControllerContext.HttpContext, update, this.CurrentLanguage);
                        Utils.SetUserRewardAction(((CustomPrincipal)User).UserId, RewardAction.UserActionType.VsIdea, 12, 21, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage);
                        SessionCustom.Begin();

                        IdeaVoteRepository vote = new IdeaVoteRepository(SessionCustom);
                        vote.Entity.Favorable = true;
                        vote.Entity.IdeaId = voteIdeaId;

                        int totalVote = vote.GetAll().Count();
                        int totalVersus = versus.VersusIdeaWon().Count();

                        idea.Entity.Likes = totalVote + totalVersus;
                        idea.Update();
                        SessionCustom.Commit();
                    }
                    else
                    {
                        SessionCustom.RollBack();
                    }
                }
            }

            return this.Json(new { result = result });
        }

        /// <summary>
        /// Action to like or don't like an idea
        /// </summary>
        /// <param name="ideaId">idea id</param>
        /// <param name="favorable">indicates if like or don't like</param>
        /// <returns>returns a JSON object with the result</returns>
        [Authorize]
        public JsonResult Vote(int ideaId, bool favorable)
        {
            bool result = false;

            List<Domain.Entities.Modul> moduls = Business.Services.CustomMemberShipProvider.GetModuls(((CustomPrincipal)User).UserId, SessionCustom, ControllerContext.HttpContext);
            if (moduls.Find(t => t.ModulId == 57) == null)
            {
                int userId = ((CustomPrincipal)User).UserId;
                SessionCustom.Begin();

                IdeaVoteRepository vote = new IdeaVoteRepository(SessionCustom);
                vote.Entity.UserId = userId;
                vote.Entity.Favorable = favorable;
                vote.Entity.IdeaId = ideaId;
                vote.Entity.Date = DateTime.Now;
                vote.Insert();

                vote.Entity = new IdeaVote();
                vote.Entity.Favorable = favorable;
                vote.Entity.IdeaId = ideaId;
                int totalVote = vote.GetAll().Count();

                SessionCustom.Commit();

                IdeaRepository idea = new IdeaRepository(SessionCustom);
                idea.Entity.IdeaId = ideaId;
                idea.LoadByKey();
                bool update = idea.Entity.UserId.Value == ((CustomPrincipal)User).UserId ? true : false;
                if (favorable)
                {
                    VersusRepository versus = new VersusRepository(SessionCustom);
                    versus.Entity.WinnerId = ideaId;
                    int totalVersus = versus.VersusIdeaWon().Count();

                    idea.Entity.Likes = totalVote + totalVersus;
                    Utils.SetUserRewardAction(((CustomPrincipal)User).UserId, RewardAction.UserActionType.LikeIdea, 12, 21, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage);
                    Utils.SetUserRewardAction(idea.Entity.UserId.Value, RewardAction.UserActionType.ReciveLike, 12, 50000, this.SessionCustom, ControllerContext.HttpContext, update, this.CurrentLanguage);

                    if (idea.Entity.UserId.Value != userId)
                    {
                        Business.Utilities.Notification.NewNotification(idea.Entity.UserId.Value, null, Domain.Entities.Basic.SystemNotificationType.RECEIVE_IDEA_LIKE, userId, string.Concat("/", idea.Entity.Friendlyurlid), idea.Entity.ContentId, idea.Entity.IdeaId.Value, null, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
                    }

                    // hilo notificaciones de usuarios relacionados
                    Business.Utilities.Notification.StartRelatedContentUser(userId, idea.Entity.IdeaId.Value, null, Domain.Entities.Basic.SystemNotificationType.RECEIVE_IDEA_RELATED_LIKE, userId, string.Concat("/", idea.Entity.Friendlyurlid), idea.Entity.ContentId, idea.Entity.IdeaId.Value, null, null, null, this.HttpContext, this.CurrentLanguage);
                }
                else
                {
                    idea.Entity.NoLikes = totalVote;
                    Utils.SetUserRewardAction(((CustomPrincipal)User).UserId, RewardAction.UserActionType.HateIdea, 4, 7, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage);
                    Utils.SetUserRewardAction(idea.Entity.UserId.Value, RewardAction.UserActionType.ReciveHate, 0, 50000, this.SessionCustom, ControllerContext.HttpContext, update, this.CurrentLanguage);
                }

                idea.Update();
                Business.Utilities.Notification.NewNotification(idea.Entity.UserId.Value, Domain.Entities.Basic.EmailNotificationType.RECEIVE_N_IDEA_LIKE, null, userId, string.Concat("/", idea.Entity.Friendlyurlid), idea.Entity.ContentId, idea.Entity.IdeaId.Value, null, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);

                if (idea.IsIdeaInTop10(idea.Entity.IdeaId.Value))
                {
                    SystemNotificationRepository notification = new SystemNotificationRepository(SessionCustom);
                    int count = notification.SystemNotificationCount(idea.Entity.UserId.Value, (int)Domain.Entities.Basic.SystemNotificationType.IDEA_TOP_10, idea.Entity.IdeaId.Value);
                    if (count == 0)
                    {
                        Business.Utilities.Notification.NewNotification(idea.Entity.UserId.Value, null, Domain.Entities.Basic.SystemNotificationType.IDEA_TOP_10, null, string.Concat("/", idea.Entity.Friendlyurlid), idea.Entity.ContentId, idea.Entity.IdeaId.Value, null, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
                    }
                }

                if (idea.IsIdeaInTop5Home(idea.Entity.IdeaId.Value))
                {
                    SystemNotificationRepository notification = new SystemNotificationRepository(SessionCustom);
                    int count = notification.SystemNotificationCount(idea.Entity.UserId.Value, (int)Domain.Entities.Basic.SystemNotificationType.POPULAR_IDEA_TOP_5, idea.Entity.IdeaId.Value);
                    if (count == 0)
                    {
                        Business.Utilities.Notification.NewNotification(idea.Entity.UserId.Value, null, Domain.Entities.Basic.SystemNotificationType.POPULAR_IDEA_TOP_5, null, string.Concat("/", idea.Entity.Friendlyurlid), idea.Entity.ContentId, idea.Entity.IdeaId.Value, null, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
                    }
                }

                if (idea.IsIdeaInTopVoted5Home(idea.Entity.IdeaId.Value))
                {
                    SystemNotificationRepository notification = new SystemNotificationRepository(SessionCustom);
                    int count = notification.SystemNotificationCount(idea.Entity.UserId.Value, (int)Domain.Entities.Basic.SystemNotificationType.VOTED_IDEA_TOP_5, idea.Entity.IdeaId.Value);
                    if (count == 0)
                    {
                        Business.Utilities.Notification.NewNotification(idea.Entity.UserId.Value, null, Domain.Entities.Basic.SystemNotificationType.VOTED_IDEA_TOP_5, null, string.Concat("/", idea.Entity.Friendlyurlid), idea.Entity.ContentId, idea.Entity.IdeaId.Value, null, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
                    }
                }

                Business.UserRelation.SaveRelationAction(((CustomPrincipal)User).UserId, idea.Entity.UserId.Value, ideaId, "like", this.SessionCustom);

                result = true;
                return this.Json(new { result = result });
            }
            else
            {
                return this.Json(new { result = result, error = 101 });
            }
        }

        /// <summary>
        /// Obtiene si hay un versus disponible
        /// </summary>
        /// <param name="contentId">contenido del versus</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        public JsonResult CheckVersus(int contentId)
        {
            bool result = true;
            if (User.Identity.IsAuthenticated)
            {
                result = Utils.CheckVersus(((CustomPrincipal)User).UserId, contentId, this.SessionCustom);
            }

            return this.Json(new { result = result });
        }

        /// <summary>
        /// Show a form to edit an idea
        /// </summary>
        /// <param name="id">idea id</param>
        /// <param name="type">type of the idea parent content</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        [HttpGet]
        public ActionResult Editar(int id, string type)
        {
            UserRepository user = new UserRepository(SessionCustom);
            user.Entity.UserId = ((CustomPrincipal)User).UserId;
            user.LoadByKey();

            IdeaRepository idea = new IdeaRepository(SessionCustom);
            idea.Entity.IdeaId = id;
            idea.LoadByKey();

            if (user.Entity.UserId == idea.Entity.UserId || ((CustomPrincipal)User).IsFrontEndAdmin)
            {
                QuestionRepository question = new QuestionRepository(SessionCustom);

                question.Entity.ContentId = idea.Entity.ContentId;
                question.LoadByKey();
                if (question.Entity.EndDate.HasValue)
                {
                    type = "question";
                }
                else
                {
                    type = "challenge";
                }

                ViewBag.CurrentUser = user.Entity;

                if (type == "question")
                {
                    ViewBag.TextLenght = 350;
                }
                else if (type == "challenge")
                {
                    ViewBag.TextLenght = 700;
                }

                return this.View("_EditIdea", idea.Entity);
            }

            return null;
        }

        /// <summary>
        /// Receive a form to edit an idea
        /// </summary>
        /// <param name="ideaId">idea id</param>
        /// <param name="text">idea text</param>
        /// <param name="image">idea image</param>
        /// <param name="video">idea video</param>
        /// <returns>returns a JSON object with the result</returns>
        [Authorize]
        [HttpPost]
        public JsonResult Editar(int ideaId, string text, string image, string video)
        {
            bool result = false;
            int userId = ((CustomPrincipal)User).UserId;

            IdeaRepository idea = new IdeaRepository(SessionCustom);
            idea.Entity.IdeaId = ideaId;
            idea.LoadByKey();

            if (userId == idea.Entity.UserId || ((CustomPrincipal)User).IsFrontEndAdmin)
            {
                SessionCustom.Begin();

                idea.Entity.Text = text;

                if (!string.IsNullOrEmpty(image))
                {
                    image = System.IO.Path.GetFileName(image);
                    string serverMap = Server.MapPath("~");
                    string origin = serverMap + @"\resources\temporal\ideas\" + image;
                    if (System.IO.File.Exists(origin))
                    {
                        if (!System.IO.Directory.Exists(serverMap + @"\files\ideas\"))
                        {
                            System.IO.Directory.CreateDirectory(serverMap + @"\files\ideas\");
                        }

                        System.IO.File.Move(serverMap + @"\resources\temporal\ideas\560x515-" + image, serverMap + @"\files\ideas\560x515-" + image);
                        System.IO.File.Move(serverMap + @"\resources\temporal\ideas\580x445-" + image, serverMap + @"\files\ideas\580x445-" + image);
                        System.IO.File.Move(serverMap + @"\resources\temporal\ideas\100x70-" + image, serverMap + @"\files\ideas\100x70-" + image);
                        System.IO.File.Move(origin, serverMap + @"\files\ideas\" + image);
                    }

                    idea.Entity.Image = image;
                    idea.Entity.Video = string.Empty;
                }
                else if (!string.IsNullOrEmpty(video))
                {
                    idea.Entity.Video = video;
                    idea.Entity.Image = string.Empty;
                }
                else
                {
                    idea.Entity.Image = string.Empty;
                    idea.Entity.Video = string.Empty;
                }

                idea.Update();

                Regex regular = new Regex(@"(#\w+)");
                MatchCollection hashTags = regular.Matches(idea.Entity.Text);
                if (hashTags.Count > 0)
                {
                    HashTagRepository hashTagRepository = new HashTagRepository(SessionCustom);
                    IdeaHashTagRepository ideaHashTagRepository = new IdeaHashTagRepository(SessionCustom);
                    int hashTagId = 0;
                    foreach (Match match in hashTags)
                    {
                        hashTagRepository.Entity.Value = match.Value;
                        hashTagRepository.Entity.HashTagId = null;
                        hashTagRepository.Load();
                        if (hashTagRepository.Entity.HashTagId.HasValue)
                        {
                            hashTagId = hashTagRepository.Entity.HashTagId.Value;
                        }
                        else
                        {
                            hashTagRepository.Entity.Count = 1;
                            object scalar = hashTagRepository.Insert();
                            if (!int.TryParse(scalar.ToString(), out hashTagId))
                            {
                                hashTagId = 0;
                            }
                        }

                        if (hashTagId > 0)
                        {
                            ideaHashTagRepository.Entity.HashTagId = hashTagId;
                            ideaHashTagRepository.Entity.IdeaId = ideaId;

                            if (!ideaHashTagRepository.Exist())
                            {
                                ideaHashTagRepository.Insert();
                            }
                        }
                    }
                }

                result = true;
                SessionCustom.Commit();

                // hilo notificaciones de usuarios relacionados
                Business.Utilities.Notification.StartRelatedContentUser(userId, idea.Entity.IdeaId.Value, null, Domain.Entities.Basic.SystemNotificationType.EDIT_RELATED_IDEA, userId, string.Concat("/", idea.Entity.Friendlyurlid), idea.Entity.ContentId, idea.Entity.IdeaId.Value, null, null, null, this.HttpContext, this.CurrentLanguage);
            }

            return this.Json(new { result = result });
        }

        /// <summary>
        /// "deletes" an idea
        /// </summary>
        /// <param name="ideaId">idea id</param>
        /// <returns>returns a JSON object with the result</returns>
        [Authorize]
        [HttpPost]
        public JsonResult Borrar(int ideaId)
        {
            bool result = false;

            UserRepository user = new UserRepository(SessionCustom);
            user.Entity.UserId = ((CustomPrincipal)User).UserId;
            user.LoadByKey();

            IdeaRepository idea = new IdeaRepository(SessionCustom);
            idea.Entity.IdeaId = ideaId;
            idea.LoadByKey();

            if (user.Entity.UserId == idea.Entity.UserId)
            {
                idea.Entity.Active = false;
                idea.Update();
                result = true;
            }

            return this.Json(new { result = result });
        }

        /// <summary>
        /// Search for hash tags in the site
        /// </summary>
        /// <param name="value">search value</param>
        /// <returns>returns a JSON object with the result</returns>
        public JsonResult GetHashTags(string value)
        {
            bool result = false;
            List<HashTag> col = new List<HashTag>();
            if (value.Length >= 2)
            {
                HashTagRepository hashtag = new HashTagRepository(SessionCustom);
                hashtag.Entity.Value = value.Substring(0, 2);
                col = hashtag.SearchHashTag();
                result = true;
            }

            return this.Json(new { result = result, hashTags = col });
        }

        /// <summary>
        /// Return a view with ideas
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="contentId">content id</param>
        /// <param name="text">search text</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Ideas(int page, int? contentId, string text)
        {
            List<IdeasPaging> ideas = new List<IdeasPaging>();
            IdeaRepository idea = new IdeaRepository(SessionCustom);
            CommentRepository comment = new CommentRepository(SessionCustom);
            int? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = ((CustomPrincipal)User).UserId;
            }

            int total = 0;
            if (string.IsNullOrEmpty(text))
            {
                text = null;
            }
            else if (text.Length > 100)
            {
                text = text.Substring(0, 100);
            }

            ideas = idea.Ideas(page, 6, out total, contentId, userId, text);
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
                QuestionRepository question = new QuestionRepository(SessionCustom);
                question.Entity.ContentId = contentId;
                question.LoadByKey();
                if (question.Entity.EndDate.HasValue)
                {
                    ideas[0].QuestionType = question.Entity.Type;
                }
            }

            ViewBag.CurrentUserId = userId;
            ViewBag.ShowHeader = !contentId.HasValue;

            if (page == 0)
            {
                return this.View("_CardsIdeas", ideas);
            }
            else
            {
                return this.View("_CardIdeasList", ideas);
            }
        }
    }
}
