// <copyright file="CommentController.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Webcore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;

    /// <summary>
    /// controller for the comment action
    /// </summary>
    public class CommentController : FrontEndController
    {
        /// <summary>
        /// creates a new comment for an idea
        /// </summary>
        /// <param name="ideaId">idea id</param>
        /// <param name="text">comment text</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        [Authorize]
        public JsonResult Crear(int ideaId, string text)
        {
            bool result = false;
            int currentUserId = ((CustomPrincipal)User).UserId;
            CommentRepository commentRepository = new CommentRepository(SessionCustom);
            commentRepository.Entity.Text = text;
            commentRepository.Entity.IdeaId = ideaId;
            commentRepository.Entity.Active = true;
            commentRepository.Entity.UserId = currentUserId;
            commentRepository.Entity.Creationdate = DateTime.Now;
            int commentId = Convert.ToInt32(commentRepository.Insert());

            result = true;            

            IdeaRepository idea = new IdeaRepository(SessionCustom);
            idea.Entity.IdeaId = ideaId;
            idea.LoadByKey();
            bool update = idea.Entity.UserId.Value == commentRepository.Entity.UserId.Value ? true : false;            

            if (idea.Entity.UserId.Value != commentRepository.Entity.UserId.Value)
            {
                Utils.SetUserRewardAction(commentRepository.Entity.UserId.Value, RewardAction.UserActionType.CommentIdea, 21, 21, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage);
                Utils.SetUserRewardAction(idea.Entity.UserId.Value, RewardAction.UserActionType.ReciveComment, 21, int.MaxValue, this.SessionCustom, ControllerContext.HttpContext, update, this.CurrentLanguage);                
                Business.Utilities.Notification.NewNotification(idea.Entity.UserId.Value, null, Domain.Entities.Basic.SystemNotificationType.RECEIVE_IDEA_COMMENT, currentUserId, string.Concat("/", idea.Entity.Friendlyurlid), idea.Entity.ContentId, commentId, null, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
            }
            else
            {
                Utils.SetUserRewardAction(commentRepository.Entity.UserId.Value, RewardAction.UserActionType.CommentIdea, 21, 0, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage);
                Utils.SetUserRewardAction(idea.Entity.UserId.Value, RewardAction.UserActionType.ReciveComment, 21, 0, this.SessionCustom, ControllerContext.HttpContext, update, this.CurrentLanguage);
            }

            // hilo notificaciones de usuarios relacionados
            Business.Utilities.Notification.StartRelatedContentUser(commentRepository.Entity.UserId.Value, idea.Entity.IdeaId.Value, null, Domain.Entities.Basic.SystemNotificationType.RECEIVE_IDEA_RELATED_COMMENT, currentUserId, string.Concat("/", idea.Entity.Friendlyurlid), idea.Entity.ContentId, commentId, null, null, null, this.HttpContext, this.CurrentLanguage);

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

            Business.UserRelation.SaveRelationAction(((CustomPrincipal)User).UserId, idea.Entity.UserId.Value, commentId, "comment", this.SessionCustom);

            return this.Json(new { result = result });
        }

        /// <summary>
        /// creates a new comment for a content (blog)
        /// </summary>
        /// <param name="contentId">content id</param>
        /// <param name="text">comment text</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        [Authorize]
        public JsonResult CrearContent(int contentId, string text)
        {
            bool result = false;

            CommentRepository commentRepository = new CommentRepository(SessionCustom);
            commentRepository.Entity.Text = text;
            commentRepository.Entity.ContentId = contentId;
            commentRepository.Entity.Active = true;
            commentRepository.Entity.UserId = ((CustomPrincipal)User).UserId;
            commentRepository.Entity.Creationdate = DateTime.Now;
            commentRepository.Insert();
            result = true;

            return this.Json(new { result = result });
        }

        /// <summary>
        /// Show a form to edit a comment
        /// </summary>
        /// <param name="id">comment id</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        [HttpGet]
        public ActionResult Editar(int id)
        {
            UserRepository user = new UserRepository(SessionCustom);
            user.Entity.UserId = ((CustomPrincipal)User).UserId;
            user.LoadByKey();

            CommentRepository comment = new CommentRepository(SessionCustom);
            comment.Entity.CommentId = id;
            comment.LoadByKey();

            if (user.Entity.UserId == comment.Entity.UserId || ((CustomPrincipal)User).IsFrontEndAdmin)
            {
                ViewBag.CurrentUser = user.Entity;
                return this.View("_EditComment", comment.Entity);
            }

            return null;
        }

        /// <summary>
        /// Edits a comment
        /// </summary>
        /// <param name="commentId">comment id</param>
        /// <param name="text">new comment text</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        [Authorize]
        [HttpPost]
        public JsonResult Editar(int commentId, string text)
        {
            bool result = false;

            int userId = ((CustomPrincipal)User).UserId;

            CommentRepository comment = new CommentRepository(SessionCustom);
            comment.Entity.CommentId = commentId;
            comment.LoadByKey();

            if (userId == comment.Entity.UserId || ((CustomPrincipal)User).IsFrontEndAdmin)
            {
                SessionCustom.Begin();

                comment.Entity.Text = text;
                comment.Update();

                result = true;

                SessionCustom.Commit();
            }

            return this.Json(new { result = result });
        }

        /// <summary>
        /// "deletes" a comment
        /// </summary>
        /// <param name="commentId">comment id</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        [Authorize]
        [HttpPost]
        public JsonResult Borrar(int commentId)
        {
            bool result = false;

            int userId = ((CustomPrincipal)User).UserId;

            CommentRepository comment = new CommentRepository(SessionCustom);
            comment.Entity.CommentId = commentId;
            comment.LoadByKey();

            IdeaRepository idea = new IdeaRepository(SessionCustom);
            idea.Entity.IdeaId = comment.Entity.IdeaId;
            idea.LoadByKey();

            if (userId == comment.Entity.UserId /*|| userId == idea.Entity.UserId*/)
            {
                comment.Entity.Active = false;
                comment.Update();
                result = true;
            }

            return this.Json(new { result = result });
        }
    }
}
