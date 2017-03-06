// <copyright file="Notification.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>
// <modified>Ferney Osorio</modified>
namespace Business.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using Business.Services;
    using Domain.Abstract;
    using Domain.Concrete;

    /// <summary>
    /// Notification system
    /// </summary>
    public static class Notification
    {
        /// <summary>
        /// start a new thread for the related content action
        /// </summary>
        /// <param name="currentUserId">current user id</param>
        /// <param name="ideaId">idea id</param>
        /// <param name="emailType">email notification type</param>
        /// <param name="systemType">system notification type</param>
        /// <param name="actionUserId">action user id</param>
        /// <param name="url">target URL</param>
        /// <param name="processId">process id</param>
        /// <param name="elementId">element id</param>
        /// <param name="reason">text reason</param>
        /// <param name="rank">user rank</param>
        /// <param name="previousRank">user previous rank</param>
        /// <param name="context">HTTP context</param>
        /// <param name="language">current language object</param>
        public static void StartRelatedContentUser(
            int currentUserId,
            int ideaId,
            Domain.Entities.Basic.EmailNotificationType? emailType,
            Domain.Entities.Basic.SystemNotificationType? systemType,
            int? actionUserId,
            string url,
            int? processId,
            int elementId,
            string reason,
            string rank,
            string previousRank,
            HttpContextBase context,
            Domain.Entities.Language language)
        {
            SqlSession session = new SqlSession();
            Thread thread = new Thread(() => Business.Utilities.Notification.RelatedContentUser(
                currentUserId,
                ideaId,
                emailType,
                systemType,
                actionUserId,
                url,
                processId,
                elementId,
                reason,
                rank,
                previousRank,
                session,
                context,
                language));
            thread.Start();
        }

        /// <summary>
        /// start a new thread for the re-activate process action
        /// </summary>
        /// <param name="frienlyname">friendly name of the process</param>
        /// <param name="contentId">content id</param>
        /// <param name="context">HTTP context</param>
        /// <param name="language">Language object</param>
        public static void StartReActivateProcess(string frienlyname, int contentId, HttpContextBase context, Domain.Entities.Language language)
        {
            SqlSession session = new SqlSession();
            Thread thread = new Thread(() => Business.Utilities.Notification.ReActivateProcess(frienlyname, contentId, session, context, language));
            thread.Start();
        }

        /// <summary>
        /// start a new thread for the new process action
        /// </summary>
        /// <param name="frienlyname">friendly name of the process</param>
        /// <param name="contentId">content id</param>
        /// <param name="context">HTTP context</param>
        /// <param name="language">Language object</param>
        public static void StartNewProcess(string frienlyname, int contentId, HttpContextBase context, Domain.Entities.Language language)
        {
            SqlSession session = new SqlSession();
            Thread thread = new Thread(() => Business.Utilities.Notification.NewProcess(frienlyname, contentId, session, context, language));
            thread.Start();
        }

        /// <summary>
        /// start a new thread for the finishing process action
        /// </summary>
        /// <param name="frienlyname">friendly name of the process</param>
        /// <param name="contentId">content id</param>        
        /// <param name="context">HTTP context</param>
        /// <param name="language">Language object</param>
        public static void StartFinishingProcess(string frienlyname, int contentId, HttpContextBase context, Domain.Entities.Language language)
        {
            SqlSession session = new SqlSession();
            Thread thread = new Thread(() => Business.Utilities.Notification.FinishingProcess(frienlyname, contentId, session, context, language));
            thread.Start();
        }

        /// <summary>
        /// start a new thread for the finished process action
        /// </summary>
        /// <param name="frienlyname">friendly name of the process</param>
        /// <param name="contentId">content id</param>        
        /// <param name="context">HTTP context</param>
        /// <param name="language">Language object</param>
        public static void StartFinishedProcess(string frienlyname, int contentId, HttpContextBase context, Domain.Entities.Language language)
        {
            SqlSession session = new SqlSession();
            Thread thread = new Thread(() => Business.Utilities.Notification.FinishingProcess(frienlyname, contentId, session, context, language));
            thread.Start();
        }

        /// <summary>
        /// Creates a new email or system notification
        /// </summary>
        /// <param name="targetUserId">target user id</param>
        /// <param name="emailType">email notification type</param>
        /// <param name="systemType">system notification type</param>
        /// <param name="actionUserId">action user id</param>
        /// <param name="url">target URL</param>
        /// <param name="processId">process id</param>
        /// <param name="elementId">element id</param>
        /// <param name="reason">text reason</param>
        /// <param name="rank">user rank</param>
        /// <param name="previousRank">user previous rank</param>
        /// <param name="session">SQL session</param>
        /// <param name="context">HTTP context</param>
        /// <param name="language">current language object</param>
        /// <returns>true if the notification was created false if not</returns>
        public static bool NewNotification(
            int targetUserId,
            Domain.Entities.Basic.EmailNotificationType? emailType,
            Domain.Entities.Basic.SystemNotificationType? systemType,
            int? actionUserId,
            string url,
            int? processId,
            int elementId,
            string reason,
            string rank,
            string previousRank,
            ISession session,
            HttpContextBase context,
            Domain.Entities.Language language)
        {
            //// i.   [yo/], nombre del usuario que recibe la notificación.
            //// ii.  [usuario/], nombre del usuario que realiza la acción.
            //// iii. [url]html[/url], vincula la selección al contenido de la notificación. 
            //// iv.  [tipo/], muestra el tipo del proceso (reto o pregunta).
            //// v.   [proceso/], muestra el nombre del reto o pregunta.
            //// vi.  [n/], muestra el valor numérico configurado.
            //// vii. [rango/], muestra el rango del usuario.
            //// viii.[rango-anterior/], muestra el rango anterior del usuario

            string targetName = null, targetEmail = null, actionName = null, processType = null, processName = null, subject = null, senderName = null;
            int userlanguage = 0;
            UserRepository userRepository = new UserRepository(session);
            userRepository.Entity.UserId = targetUserId;
            userRepository.LoadByKey();
            userlanguage = userRepository.Entity.LanguageId.Value;
            targetName = userRepository.Entity.Names;
            targetEmail = userRepository.Entity.Email;
            bool targetActive = userRepository.Entity.Active.Value;
            bool result = false;

            if (targetActive)
            {
                if (actionUserId.HasValue)
                {
                    userRepository.Entity.UserId = actionUserId;
                    userRepository.LoadByKey();
                    actionName = userRepository.Entity.Names;
                }
                else
                {
                    actionName = "Ciudadano";
                }

                if (processId.HasValue)
                {
                    FrontEndManagement objman = new FrontEndManagement(session, context, FrontEndManagement.Type.Content, language);
                    objman.BindInfo(processId.Value, targetUserId);

                    processName = objman.Content.Name;
                    if (objman.Detail is Business.FrontEnd.Question)
                    {
                        Business.FrontEnd.Question detail = (Business.FrontEnd.Question)objman.Detail;
                        switch (detail.ObjQuestion.Type)
                        {
                            case Domain.Entities.Question.TypeQuestion.Abierta:
                                processType = "Pregunta Abierta";
                                break;
                            case Domain.Entities.Question.TypeQuestion.Seleccion_Multiple:
                                processType = "Seleccion Multiple";
                                break;
                            case Domain.Entities.Question.TypeQuestion.Ubicacion:
                                processType = "Ubicacion";
                                break;
                        }
                    }
                    else if (objman.Detail is Business.FrontEnd.Challenge)
                    {
                        Business.FrontEnd.Challenge detail = (Business.FrontEnd.Challenge)objman.Detail;
                        switch (detail.ObjChallenge.Type)
                        {
                            case Domain.Entities.Challenge.TypeChallenge.Participacion_Ciudadana:
                                processType = "Participacion ciudadana";
                                break;
                            case Domain.Entities.Challenge.TypeChallenge.Reto_Ciudad:
                                processType = "Reto ciudad";
                                break;
                        }
                    }
                }

                StringBuilder builder = new StringBuilder();
                if (systemType.HasValue)
                {
                    SystemNotificationTemplateRepository notificationRepository = new SystemNotificationTemplateRepository(session);
                    notificationRepository.Entity.ContentId = (int)systemType;
                    notificationRepository.LoadByKey();

                    if (userlanguage == (int)Domain.Entities.Enums.LanguageEnum.Spanish)
                    {
                        builder.Append(notificationRepository.Entity.Description);
                    }
                    else
                    {
                        builder.Append(notificationRepository.Entity.DescriptionIngles);
                    }
                }
                else if (emailType.HasValue)
                {
                    EmailNotificationTemplateRepository notificationRepository = new EmailNotificationTemplateRepository(session);
                    notificationRepository.Entity.ContentId = (int)emailType;
                    notificationRepository.LoadByKey();
                    senderName = notificationRepository.Entity.SenderName;

                    if (userlanguage == (int)Domain.Entities.Enums.LanguageEnum.Spanish)
                    {
                        builder.Append(notificationRepository.Entity.Description);
                    }
                    else
                    {
                        builder.Append(notificationRepository.Entity.DescriptionIngles);
                    }

                    ContentRepository contentRepository = new ContentRepository(session);
                    contentRepository.Entity.ContentId = (int)emailType;
                    contentRepository.LoadByKey();

                    if (userlanguage == (int)Domain.Entities.Enums.LanguageEnum.Spanish)
                    {
                        subject = contentRepository.Entity.Shortdescription;
                    }
                    else
                    {
                        subject = contentRepository.Entity.ShortdescriptionIngles;
                    }
                }

                builder.Replace("[yo/]", targetName)
                    .Replace("[usuario/]", actionName)
                    .Replace("[tipo/]", processType)
                    .Replace("[proceso/]", processName)
                    .Replace("[url]", string.Concat("<a href='", url, "'>"))
                    .Replace("[/url]", string.Concat("</a>"))
                    .Replace("[url/]", string.Concat("<a href='", url, "'>", url, "</a>"))
                    .Replace("[razon/]", reason)
                    .Replace("[rango/]", rank)
                    .Replace("[rango-anterior/]", previousRank);
                ////.Replace("[n/]", number) biene desde base de datos solo para unos pocos de email

                if (systemType.HasValue)
                {
                    result = SaveSystemNotification(actionUserId, targetUserId, builder.ToString(), url, (int)systemType, elementId, session);
                }
                else if (emailType.HasValue)
                {
                    bool send = false;
                    bool token = false;
                    string keyWord = string.Empty;
                    string keyValue = string.Empty;
                    switch (emailType)
                    {
                        case Domain.Entities.Basic.EmailNotificationType.RECEIVE_N_IDEA_LIKE:
                            keyWord = "send-receive-n-idea-like";
                            keyValue = "value-receive-n-idea-like";
                            token = true;
                            break;
                        case Domain.Entities.Basic.EmailNotificationType.NEW_PROCESS:
                            keyWord = "send-new-process";
                            token = true;
                            break;
                        case Domain.Entities.Basic.EmailNotificationType.FINISHING_PROCESS:
                            keyWord = "send-finishing-process";
                            keyValue = "value-finishing-process";
                            token = true;
                            break;
                        case Domain.Entities.Basic.EmailNotificationType.FINISHED_PROCESS:
                            keyWord = "send-finished-process";
                            token = true;
                            break;
                        case Domain.Entities.Basic.EmailNotificationType.IDEA_BLOCKED:
                            keyWord = "send-idea-blocked";
                            token = true;
                            break;
                        case Domain.Entities.Basic.EmailNotificationType.USER_LEAVE_ADMIN:
                        case Domain.Entities.Basic.EmailNotificationType.USER_LEAVE_USER:
                        case Domain.Entities.Basic.EmailNotificationType.ADMIN_KICKOUT_ADMIN:
                        case Domain.Entities.Basic.EmailNotificationType.ADMIN_KICKOUT_USER:
                        case Domain.Entities.Basic.EmailNotificationType.PROMOTION:
                        case Domain.Entities.Basic.EmailNotificationType.POSTULATESTORY:
                        case Domain.Entities.Basic.EmailNotificationType.PUBLICATEDESTORY:
                        case Domain.Entities.Basic.EmailNotificationType.REJECTEDSTORY:
                            send = true;
                            break;
                    }

                    if (!send)
                    {
                        UserSettingRepository setting = new UserSettingRepository(session);
                        setting.Entity.UserId = targetUserId;
                        setting.Entity.KeyWord = keyWord;
                        setting.Load();
                        send = Convert.ToBoolean(setting.Entity.Value);

                        if (send && !string.IsNullOrEmpty(keyValue) && !string.IsNullOrEmpty(targetEmail))
                        {
                            send = false;
                            setting.Entity = new Domain.Entities.UserSetting();
                            setting.Entity.UserId = targetUserId;
                            setting.Entity.KeyWord = keyValue;
                            setting.Load();

                            int value = 0;
                            if (int.TryParse(setting.Entity.Value, out value) && value != 0)
                            {
                                if (emailType == Domain.Entities.Basic.EmailNotificationType.RECEIVE_N_IDEA_LIKE)
                                {
                                    IdeaRepository idea = new IdeaRepository(session);
                                    idea.Entity.IdeaId = elementId;
                                    idea.LoadByKey();
                                    if (value == idea.Entity.Likes)
                                    {
                                        builder.Replace("[n/]", value.ToString());
                                        send = true;
                                    }
                                }
                                else if (emailType == Domain.Entities.Basic.EmailNotificationType.FINISHING_PROCESS || emailType == Domain.Entities.Basic.EmailNotificationType.FINISHED_PROCESS)
                                {
                                    // ya biene validado desde el automatic
                                    builder.Replace("[n/]", value.ToString());
                                    send = true;
                                }
                            }
                        }
                    }

                    if (send && !string.IsNullOrEmpty(targetEmail))
                    {
                        string key = string.Empty;
                        if (token)
                        {
                            key = GetNotificationToken(targetUserId, emailType.Value, session);
                        }

                        result = SendEmailNotification(builder.ToString(), subject, senderName, targetUserId, targetEmail, url, key, session);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Save a new system notification
        /// </summary>
        /// <param name="actionUserId">action user id</param>
        /// <param name="userId">user id</param>
        /// <param name="value">text value</param>
        /// <param name="targetURL">target URL</param>
        /// <param name="templateId">template id</param>
        /// <param name="elementId">element id</param>
        /// <param name="session">SQL session</param>
        /// <returns>true if the notification was created false if not</returns>
        private static bool SaveSystemNotification(int? actionUserId, int userId, string value, string targetURL, int templateId, int elementId, ISession session)
        {
            bool result = false;
            SystemNotificationRepository notificationRepository = new SystemNotificationRepository(session);
            notificationRepository.Entity.UserId = userId;
            notificationRepository.Entity.ActionUserId = actionUserId;
            notificationRepository.Entity.TemplateId = templateId;
            notificationRepository.Entity.ElementId = elementId;
            notificationRepository.Entity.Value = value;
            notificationRepository.Entity.TargetURL = targetURL;
            notificationRepository.Entity.Seen = false;
            notificationRepository.Entity.CreationDate = DateTime.Now;
            object newId = notificationRepository.Insert();
            if (newId != null)
            {
                int id = 0;
                if (int.TryParse(newId.ToString(), out id))
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Save a new email notification and send the email
        /// </summary>
        /// <param name="body">html body</param>
        /// <param name="subject">email subject</param>
        /// <param name="senderName">sender name</param>
        /// <param name="userId">user id</param>
        /// <param name="email">user email</param>
        /// <param name="targetURL">target URL</param>
        /// <param name="token">email token</param>
        /// <param name="session">SQL session</param>
        /// <returns>true if the notification was created false if not</returns>
        private static bool SendEmailNotification(string body, string subject, string senderName, int userId, string email, string targetURL, string token, ISession session)
        {
            List<string> emailDebug = new List<string>();
            emailDebug.Add(email);
            email = email.ToLower();

            bool result = false;
            EmailNotificationRepository notificationRepository = new EmailNotificationRepository(session);
            notificationRepository.Entity.UserId = userId;
            notificationRepository.Entity.Value = body;
            notificationRepository.Entity.TargetURL = targetURL;
            notificationRepository.Entity.Seen = false;
            notificationRepository.Entity.CreationDate = DateTime.Now;
            object newId = notificationRepository.Insert();
            if (newId != null && emailDebug.Exists(ed => ed == email))
            {
                StringBuilder builder = new StringBuilder();
                StringBuilder builderLink = new StringBuilder();
                string linkNotification = "<tr><td style=\"padding: 20px; font-family: helvetica, arial; font-size: 10px; text-align: center\">Si no quieres recibir mas este correo haz clic <a href=\"@URL/perfil/notificacion/@KEY\" target=\"_blank\">aquí</a></td></tr>";
                string template = "<html><head><title>Cities for Life</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head><body bgcolor=\"#FFFFFF\" leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\"><table width=\"548\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td><img src=\"@URL/resources/images/mail/mail-mm_01.jpg\" width=\"548\" height=\"88\" alt=\"\"></td></tr><tr><td><img src=\"@URL/resources/images/mail/mail-mm_02.jpg\" width=\"548\" height=\"44\" alt=\"\"></td></tr><tr><td style=\"padding: 20px; font-family: helvetica, arial; font-size: 13px;\">@BODY</td></tr>@NOTIFICATION<tr><td><img src=\"@URL/resources/images/mail/mail-mm_04.jpg\" width=\"548\" height=\"52\" alt=\"\"></td></tr></table></body></html>";
                string url = System.Configuration.ConfigurationManager.AppSettings["PathHost"];

                builder.Append(template)
                    .Replace("@URL", url)
                    .Replace("@BODY", body);

                if (!string.IsNullOrEmpty(token))
                {
                    builderLink.Append(linkNotification)
                        .Replace("@URL", url)
                        .Replace("@KEY", token);

                    builder.Replace("@NOTIFICATION", builderLink.ToString());
                }
                else
                {
                    builder.Replace("@NOTIFICATION", string.Empty);
                }

                int id = 0;
                if (int.TryParse(newId.ToString(), out id))
                {
                    SendMail mail = new SendMail();
                    mail.From = string.Concat(senderName, " <", System.Configuration.ConfigurationManager.AppSettings["MailContact"], ">");
                    mail.To = email;
                    mail.Subject = subject;
                    mail.Body = builder.ToString();
                    mail.SendMessage();

                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Generates or retrieve a notification token
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="type">email notification type</param>
        /// <param name="session">SQL session</param>
        /// <returns>A email notification token</returns>
        private static string GetNotificationToken(int userId, Domain.Entities.Basic.EmailNotificationType type, ISession session)
        {
            NotificationKeyRepository key = new NotificationKeyRepository(session);
            key.Entity.UserId = userId;
            key.Entity.ContentId = (int)type;
            key.Load();

            if (string.IsNullOrEmpty(key.Entity.Key))
            {
                key.Entity.Key = Business.Utils.EncriptMD5(string.Concat(Business.Utils.SecretWord(), userId, type));
                key.Insert();
            }

            return key.Entity.Key;
        }

        /// <summary>
        /// content relation process
        /// </summary>
        /// <param name="currentUserId">current user id</param>
        /// <param name="ideaId">idea id</param>
        /// <param name="emailType">email notification type</param>
        /// <param name="systemType">system notification type</param>
        /// <param name="actionUserId">action user id</param>
        /// <param name="url">target URL</param>
        /// <param name="processId">process id</param>
        /// <param name="elementId">element id</param>
        /// <param name="reason">text reason</param>
        /// <param name="rank">user rank</param>
        /// <param name="previousRank">user previous rank</param>
        /// <param name="session">SQL session</param>
        /// <param name="context">HTTP context</param>
        /// <param name="language">current language object</param>
        private static void RelatedContentUser(
            int currentUserId,
            int ideaId,
            Domain.Entities.Basic.EmailNotificationType? emailType,
            Domain.Entities.Basic.SystemNotificationType? systemType,
            int? actionUserId,
            string url,
            int? processId,
            int elementId,
            string reason,
            string rank,
            string previousRank,
            ISession session,
            HttpContextBase context,
            Domain.Entities.Language language)
        {
            IdeaRepository idea = new IdeaRepository(session);
            List<int> relatedUsers = idea.IdeaRelatedUsers(ideaId);
            relatedUsers.Remove(currentUserId);
            foreach (int id in relatedUsers)
            {
                Business.Utilities.Notification.NewNotification(id, emailType, systemType, actionUserId, url, processId, elementId, reason, rank, previousRank, session, context, language);
            }
        }

        /// <summary>
        /// finished process notification creation
        /// </summary>
        /// <param name="frienlyname">friendly name of the process</param>
        /// <param name="contentId">content id</param>
        /// <param name="session">SQL session object</param>
        /// <param name="context">HTTP context</param>
        /// <param name="language">Language object</param>
        private static void FinishedProcess(string frienlyname, int contentId, ISession session, HttpContextBase context, Domain.Entities.Language language)
        {
            frienlyname = string.Concat("/", frienlyname);
            UserRepository userRepo = new UserRepository(session);
            userRepo.Entity.Active = true;
            List<Domain.Entities.User> users = userRepo.GetAll();
            foreach (Domain.Entities.User user in users)
            {
                Business.Utilities.Notification.NewNotification(user.UserId.Value, null, Domain.Entities.Basic.SystemNotificationType.FINISHED_PROCESS, null, frienlyname, contentId, contentId, null, null, null, session, context, language);
            }
        }

        /// <summary>
        /// re-activate process notification creation
        /// </summary>
        /// <param name="frienlyname">friendly name of the process</param>
        /// <param name="contentId">content id</param>
        /// <param name="session">SQL session object</param>
        /// <param name="context">HTTP context</param>
        /// <param name="language">Language object</param>
        private static void ReActivateProcess(string frienlyname, int contentId, ISession session, HttpContextBase context, Domain.Entities.Language language)
        {
            frienlyname = string.Concat("/", frienlyname);
            UserRepository userRepo = new UserRepository(session);
            userRepo.Entity.Active = true;
            List<Domain.Entities.User> users = userRepo.GetAll();
            foreach (Domain.Entities.User user in users)
            {
                Business.Utilities.Notification.NewNotification(user.UserId.Value, null, Domain.Entities.Basic.SystemNotificationType.RE_ACTIVATE_PROCESS, null, frienlyname, contentId, contentId, null, null, null, session, context, language);
            }
        }

        /// <summary>
        /// new process notification creation
        /// </summary>
        /// <param name="frienlyname">friendly name of the process</param>
        /// <param name="contentId">content id</param>
        /// <param name="session">SQL session object</param>
        /// <param name="context">HTTP context</param>
        /// <param name="language">Language object</param>
        private static void NewProcess(string frienlyname, int contentId, ISession session, HttpContextBase context, Domain.Entities.Language language)
        {
            UserRepository userRepo = new UserRepository(session);
            userRepo.Entity.Active = true;
            List<Domain.Entities.User> users = userRepo.GetAll();
            foreach (Domain.Entities.User user in users)
            {
                Business.Utilities.Notification.NewNotification(user.UserId.Value, null, Domain.Entities.Basic.SystemNotificationType.NEW_PROCESS, null, string.Concat("/", frienlyname), contentId, contentId, null, null, null, session, context, language);
            }
        }

        /// <summary>
        /// finishing process notification creation
        /// </summary>
        /// <param name="frienlyname">friendly name of the process</param>
        /// <param name="contentId">content id</param>
        /// <param name="session">SQL session object</param>
        /// <param name="context">HTTP context</param>
        /// <param name="language">Language object</param>
        private static void FinishingProcess(string frienlyname, int contentId, ISession session, HttpContextBase context, Domain.Entities.Language language)
        {
            frienlyname = string.Concat("/", frienlyname);
            UserRepository userRepo = new UserRepository(session);
            userRepo.Entity.Active = true;
            List<Domain.Entities.User> users = userRepo.GetAll();
            foreach (Domain.Entities.User user in users)
            {
                Business.Utilities.Notification.NewNotification(user.UserId.Value, null, Domain.Entities.Basic.SystemNotificationType.FINISHING_PROCESS, null, frienlyname, contentId, contentId, null, null, null, session, context, language);
            }
        }
    }
}
