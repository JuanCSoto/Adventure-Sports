// <copyright file="PerfilController.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Webcore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// controller for the profile action
    /// </summary>
    public class PerfilController : FrontEndController
    {
        /// <summary>
        /// gets the user profile page according to the parameters
        /// </summary>
        /// <param name="id">user id (optional)</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Index(int? id)
        {
            SetLabel();
            BannerRepository banner = new BannerRepository(SessionCustom);
            QuestionRepository question = new QuestionRepository(SessionCustom);
            IdeaRepository idea = new IdeaRepository(SessionCustom);
            ChallengeRepository challenge = new ChallengeRepository(SessionCustom);
            ContentRepository content = new ContentRepository(SessionCustom);
            UserRepository user = new UserRepository(SessionCustom);

            List<KeyValuePair<KeyValue, KeyValue>> collmeta = new List<KeyValuePair<KeyValue, KeyValue>>();
            SetLabel();
            collmeta.Add(
                new KeyValuePair<KeyValue, KeyValue>(
                    new KeyValue("name", "title"),
                    new KeyValue("content", ConfigurationManager.AppSettings["TitleHome"])));

            collmeta.Add(new KeyValuePair<KeyValue, KeyValue>(
                new KeyValue("name", "description"),
                new KeyValue("content", ConfigurationManager.AppSettings["DescriptionHome"])));

            collmeta.Add(new KeyValuePair<KeyValue, KeyValue>(
                new KeyValue("property", "og:title"),
                new KeyValue("content", ConfigurationManager.AppSettings["TitleHome"])));

            collmeta.Add(new KeyValuePair<KeyValue, KeyValue>(
                new KeyValue("property", "og:description"),
                new KeyValue("content", ConfigurationManager.AppSettings["DescriptionHome"])));

            int total = 0;
            int userId = 0;
            int? currentUserId = null;
            if (User.Identity.IsAuthenticated)
            {
                currentUserId = ((CustomPrincipal)User).UserId;
                ViewBag.CurrentUserId = currentUserId;
            }

            if (id == null && User.Identity.IsAuthenticated)
            {
                userId = ((CustomPrincipal)User).UserId;
            }
            else if (id.HasValue)
            {
                // toca contra el FriendlyNames, tambien toca actualizar el sistema de url amigable y el objeto por ahora solo el del autenticado.
                userId = id.Value;
            }
            else
            {
                Response.Redirect("~/");
            }

            if (userId > 0)
            {
                user.Entity.UserId = userId;
                user.LoadByKey();

                CommentRepository comment = new CommentRepository(SessionCustom);
                List<IdeasPaging> myIdeas = idea.MyIdeasPaging(1, 6, out total, userId, currentUserId);
                foreach (IdeasPaging myIdea in myIdeas)
                {
                    myIdea.CollComment = comment.CommentsPaging(1, 1, out total, myIdea.IdeaId.Value);
                    if (myIdea.CollComment.Count > 0)
                    {
                        myIdea.CollComment[0].CommentCount = total;
                    }
                }

                string country = string.Empty;
                string city = string.Empty;
                if (user.Entity.CountryId.HasValue && user.Entity.CityId.HasValue)
                {
                    CityRepository cityRepo = new CityRepository(SessionCustom);
                    cityRepo.Entity.CityID = user.Entity.CityId;
                    cityRepo.LoadByKey();
                    city = cityRepo.Entity.NameEs;

                    CountryRepository countryRepo = new CountryRepository(SessionCustom);
                    countryRepo.Entity.CountryID = user.Entity.CountryId;
                    countryRepo.LoadByKey();
                    country = countryRepo.Entity.NameEs;
                }

                UserInterestRepository userInterest = new UserInterestRepository(SessionCustom);
                userInterest.Entity.UserId = userId;
                DocumentTypeRepository documentType = new DocumentTypeRepository(SessionCustom);
                InterestRepository interest = new InterestRepository(SessionCustom);
                UserSettingRepository setting = new UserSettingRepository(SessionCustom);
                setting.Entity.UserId = userId;

                return this.View(new Models.FEPerfil()
                {
                    PageTitle = string.Format("{0} | {1}", ConfigurationManager.AppSettings["TitleHome"], user.Entity.Names),
                    UserPrincipal = CustomUser,
                    MetaTags = collmeta,
                    Banners = banner.GetBannersBySection(0, CurrentLanguage.LanguageId.Value),
                    CurrentLanguage = CurrentLanguage,
                    IdeasCountAll = idea.IdeasCountAll(),
                    CollIdeas = myIdeas,
                    ObjUser = user.Entity,
                    CollRelatedUsers = user.RelatedUsers(0, 12, out total, null, userId, null),
                    Country = country,
                    City = city,
                    CollUserInterest = userInterest.GetAll(),
                    CollInterest = interest.GetAll(),
                    CollSetting = setting.GetAll(),
                    Medallos = user.Entity.Medallos
                });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// return a view with ideas
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="userId">user id</param>
        /// <param name="filter">filter for the result</param>
        /// <returns>returns the result to action</returns>
        public ActionResult MisIdeas(int pageIndex, int pageSize, int userId, int filter)
        {
            int total = 0;
            int? currentUserId = null;
            if (User.Identity.IsAuthenticated)
            {
                currentUserId = ((Business.Services.CustomPrincipal)User).UserId;
                ViewBag.CurrentUserId = currentUserId;
            }

            IdeaRepository idea = new IdeaRepository(SessionCustom);
            CommentRepository comment = new CommentRepository(SessionCustom);
            List<IdeasPaging> ideas = new List<IdeasPaging>();

            switch (filter)
            {
                case 1:
                    ideas = idea.MyIdeasPaging(pageIndex, pageSize, out total, userId, currentUserId);
                    break;
                case 2:
                    ideas = idea.MyIdeasPagingCommented(pageIndex, pageSize, out total, userId, currentUserId);
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

            return this.View("_CardIdeasList", ideas);
        }

        /// <summary>
        /// return a view with ideas
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="userId">user id</param>        
        /// <returns>returns the result to action</returns>
        public ActionResult MisConversaciones(int pageIndex, int pageSize, int userId)
        {
            int total = 0;
            int? currentUserId = null;
            if (User.Identity.IsAuthenticated)
            {
                currentUserId = ((Business.Services.CustomPrincipal)User).UserId;
                ViewBag.CurrentUserId = currentUserId;
            }

            IdeaRepository idea = new IdeaRepository(SessionCustom);
            CommentRepository comment = new CommentRepository(SessionCustom);
            List<IdeasPaging> ideas = idea.MyConversationsPaging(pageIndex, pageSize, out total, userId, currentUserId);
            foreach (IdeasPaging item in ideas)
            {
                item.CollComment = comment.CommentsPaging(1, 1, out total, item.IdeaId.Value);
                if (item.CollComment.Count > 0)
                {
                    item.CollComment[0].CommentCount = total;
                }
            }

            return this.View("_CardIdeasList", ideas);
        }

        /// <summary>
        /// Deletes the information of a user permanently
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="reason">reason to be deleted</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        [Authorize]
        public JsonResult CleanUser(int userId, string reason)
        {
            bool result = false;
            bool close = false;

            int currentUserId = ((CustomPrincipal)User).UserId;

            if (userId == currentUserId || ((CustomPrincipal)User).IsFrontEndAdmin)
            {
                UserRepository user = new UserRepository(SessionCustom);
                result = user.CleanUser(userId);

                string url = string.Concat("http://", Request.Url.Host + Request.ApplicationPath).TrimEnd('/');
                if (userId == currentUserId)
                {
                    List<int> adminUsers = user.AdminUsers();
                    foreach (int adminId in adminUsers)
                    {
                        Business.Utilities.Notification.NewNotification(adminId, Domain.Entities.Basic.EmailNotificationType.USER_LEAVE_ADMIN, null, userId, url, null, userId, reason, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
                    }

                    Business.Utilities.Notification.NewNotification(currentUserId, Domain.Entities.Basic.EmailNotificationType.USER_LEAVE_USER, null, userId, url, null, userId, reason, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
                    close = true;
                }
                else if (((CustomPrincipal)User).IsFrontEndAdmin)
                {
                    Business.Utilities.Notification.NewNotification(currentUserId, Domain.Entities.Basic.EmailNotificationType.ADMIN_KICKOUT_ADMIN, null, currentUserId, url, null, userId, reason, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
                    Business.Utilities.Notification.NewNotification(userId, Domain.Entities.Basic.EmailNotificationType.ADMIN_KICKOUT_USER, null, currentUserId, url, null, userId, reason, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
                }
            }

            return this.Json(new { result = result, close = close });
        }

        /// <summary>
        /// return a view with users
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="contentId">content id</param>
        /// <param name="text">search text</param>
        /// <param name="filter">filter for the result</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Users(int page, int? contentId, string text, int? filter)
        {
            SetLabel();
            List<User> users = new List<User>();
            UserRepository user = new UserRepository(SessionCustom);

            int? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = ((CustomPrincipal)User).UserId;
            }

            int totalUsers = 0;
            int total = 0;
            if (string.IsNullOrEmpty(text))
            {
                text = null;
            }
            else if (text.Length > 100)
            {
                text = text.Substring(0, 100);
            }

            if (filter.HasValue && filter == 3 && User.Identity.IsAuthenticated)
            {
                users = user.RelatedUsers(page, 12, out totalUsers, null, userId, null);
            }
            else
            {
                users = user.Users(page, 12, out totalUsers, contentId, userId, text, filter);
            }

            foreach (User item in users)
            {
                item.CollRelatedUser = user.RelatedUsers(0, 5, out total, null, item.UserId.Value, null);
                if (item.CollRelatedUser.Count < 5)
                {
                    item.CollRelatedUser.AddRange(user.RelatedUsersZero(0, 5 - item.CollRelatedUser.Count, out total, null, item.UserId.Value, null));
                }
            }

            ViewBag.CurrentUserId = userId;
            ViewBag.TotalUsers = totalUsers;
            if (filter.HasValue)
            {
                ViewBag.HideHeader = true;
            }
            else
            {
                ViewBag.HideHeader = false;
            }

            if (page == 0)
            {
                return this.View("_Users", users);
            }
            else
            {
                return this.View("_UserList", users);
            }
        }

        /// <summary>
        /// returns a single user small profile
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>returns the result to action</returns>
        public ActionResult UserSmallProfile(int userId)
        {
            UserRepository user = new UserRepository(SessionCustom);

            int totalUsers = 0;
            int total = 0;

            user.Entity.UserId = userId;
            user.LoadByKey();

            user.Entity.CollRelatedUser = user.RelatedUsers(0, 5, out total, null, userId, null);

            ViewBag.CurrentUserId = userId;
            ViewBag.TotalUsers = totalUsers;

            List<User> result = new List<User>();
            result.Add(user.Entity);
            ////List<User> related = new List<User>();
            ////user.Entity.CollRelatedUser.ForEach(r => related.Add(new User() { Names = r.Names, Image = Business.Utils.fixLocalUserImagePath(r.Image) }));

            ViewBag.SmallProfile = true;
            return this.View("_UserList", result);

            ////return this.Json(new { result = true, names = user.Entity.Names, image = Business.Utils.fixLocalUserImagePath(user.Entity.Image), medallos = user.Entity.Medallos, related = related });
        }

        /// <summary>
        /// return a view with the user system notifications
        /// </summary>
        /// <param name="latest">value indicating whether to show only the last 3 or all notifications</param>
        /// <param name="page">page index</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        public ActionResult Notifications(bool latest, int page)
        {
            int? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = ((CustomPrincipal)User).UserId;
                int total = 0;

                SystemNotificationRepository notifications = new SystemNotificationRepository(SessionCustom);
                notifications.Entity.UserId = userId;
                List<SystemNotification> result;

                if (latest)
                {
                    result = notifications.Notifications(0, 3, userId.Value, out total);
                    notifications.MarkNotifications(userId.Value, result.Select(s => s.SystemNotificationId.Value).ToArray());
                    return this.View("LatestNotificationList", result);
                }
                else
                {
                    result = notifications.Notifications(page, 6, userId.Value, out total);
                    notifications.MarkNotifications(userId.Value, result.Select(s => s.SystemNotificationId.Value).ToArray());
                    return this.View("NotificationList", result);
                }
            }

            return null;
        }

        /// <summary>
        /// receive the form of the user preference in sending email notifications
        /// </summary>
        /// <param name="sendReceiveIdeaLike">like count notification, true or false</param>
        /// <param name="valueReceiveIdeaLike">like count value (number of like to send the notification)</param>
        /// <param name="sendNewProcess">new process notification, true or false</param>
        /// <param name="sendFinishingProcess">finishing process notification, true or false</param>
        /// <param name="valueFinishingProcess">pending days for the finishing notification (number in days)</param>
        /// <param name="sendFinishedProcess">finished process notification, true or false</param>
        /// <param name="sendIdeaBlocked">idea is blocked notification, true or false</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        [Authorize]
        public JsonResult SaveEmailSetting(string sendReceiveIdeaLike, string valueReceiveIdeaLike, string sendNewProcess, string sendFinishingProcess, string valueFinishingProcess, string sendFinishedProcess, string sendIdeaBlocked)
        {
            bool result = false;
            int? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                string trueString = "true";
                string falseString = "false";
                userId = ((CustomPrincipal)User).UserId;

                this.SaveEmailSetting(userId.Value, "send-receive-n-idea-like", string.IsNullOrEmpty(sendReceiveIdeaLike) ? falseString : trueString);
                this.SaveEmailSetting(userId.Value, "value-receive-n-idea-like", string.IsNullOrEmpty(valueReceiveIdeaLike) ? "0" : valueReceiveIdeaLike);
                this.SaveEmailSetting(userId.Value, "send-new-process", string.IsNullOrEmpty(sendNewProcess) ? falseString : trueString);
                this.SaveEmailSetting(userId.Value, "send-finishing-process", string.IsNullOrEmpty(sendFinishingProcess) ? falseString : trueString);
                this.SaveEmailSetting(userId.Value, "value-finishing-process", string.IsNullOrEmpty(valueFinishingProcess) ? "0" : valueFinishingProcess);
                this.SaveEmailSetting(userId.Value, "send-finished-process", string.IsNullOrEmpty(sendFinishedProcess) ? falseString : trueString);
                this.SaveEmailSetting(userId.Value, "send-idea-blocked", string.IsNullOrEmpty(sendIdeaBlocked) ? falseString : trueString);

                result = true;
            }

            return this.Json(new { result = result });
        }

        /// <summary>
        /// take the security token for email cancelation and redirect to the home site
        /// </summary>
        /// <param name="id">email cancelation security token</param>
        /// <returns>redirect to the home site</returns>
        public ActionResult Notificacion(string id)
        {
            this.Session["notification-token"] = id;
            return this.RedirectToAction("index", "home");
        }

        /// <summary>
        /// validate the email cancelation security token and cancel the email notification
        /// </summary>
        /// <param name="id">email cancelation security token</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        public JsonResult CancelNotificacion(string id)
        {
            bool result = false;

            NotificationKeyRepository key = new NotificationKeyRepository(SessionCustom);
            key.Entity.Key = id;
            key.Load();

            if (key.Entity.ContentId.HasValue)
            {
                string keyWord = string.Empty;
                switch (key.Entity.ContentId)
                {
                    case (int)Domain.Entities.Basic.EmailNotificationType.RECEIVE_N_IDEA_LIKE:
                        keyWord = "send-receive-n-idea-like";
                        break;
                    case (int)Domain.Entities.Basic.EmailNotificationType.NEW_PROCESS:
                        keyWord = "send-new-process";
                        break;
                    case (int)Domain.Entities.Basic.EmailNotificationType.FINISHING_PROCESS:
                        keyWord = "send-finishing-process";
                        break;
                    case (int)Domain.Entities.Basic.EmailNotificationType.FINISHED_PROCESS:
                        keyWord = "send-finished-process";
                        break;
                    case (int)Domain.Entities.Basic.EmailNotificationType.IDEA_BLOCKED:
                        keyWord = "send-idea-blocked";
                        break;
                }

                UserSettingRepository setting = new UserSettingRepository(SessionCustom);
                setting.Entity.UserId = key.Entity.UserId;
                setting.Entity.KeyWord = keyWord;
                setting.Load();
                setting.Entity.Value = "false";
                setting.Update();

                result = true;
            }

            return this.Json(new { result = result });
        }

        /// <summary>
        /// Save the user preference in sending email notifications
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="keyword">key word</param>
        /// <param name="value">setting value</param>
        private void SaveEmailSetting(int userId, string keyword, string value)
        {
            UserSettingRepository setting = new UserSettingRepository(SessionCustom);
            setting.Entity.UserId = userId;

            setting.Entity.KeyWord = keyword;
            setting.Entity.Value = null;
            setting.Load();
            if (string.IsNullOrEmpty(setting.Entity.Value))
            {
                setting.Entity.Value = value;
                setting.Insert();
            }
            else
            {
                setting.Entity.Value = value;
                setting.Update();
            }
        }

        /// <summary>
        /// Set label vistas
        /// </summary>
        private void SetLabel()
        {
            LabelManagement objlabel = new LabelManagement(SessionCustom, HttpContext);
            ViewBag.TXTINFORPRIVA = objlabel.GetLabelByName("TXTINFORPRIVA", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTPERSINFOR = objlabel.GetLabelByName("TXTPERSINFOR", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTCOMALL = objlabel.GetLabelByName("TXTCOMALL", CurrentLanguage.LanguageId.Value);
            ViewBag.PROFILEEN = objlabel.GetLabelByName("PROFILEEN", CurrentLanguage.LanguageId.Value);
            ViewBag.VOTES = objlabel.GetLabelByName("VOTES", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTNOTSETTING = objlabel.GetLabelByName("TXTNOTSETTING", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTMAILNOTIFI = objlabel.GetLabelByName("TXTMAILNOTIFI", CurrentLanguage.LanguageId.Value);
            ViewBag.NEARESTCITIZENS = objlabel.GetLabelByName("NEARESTCITIZENS", CurrentLanguage.LanguageId.Value);

        }
    }
}
