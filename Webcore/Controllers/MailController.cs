// <copyright file="MailController.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Webcore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;

    /// <summary>
    /// controller for the mail action
    /// </summary>
    public class MailController : FrontEndController
    {
        /// <summary>
        /// send the weekly email (disabled) see line 74
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult UserMail()
        {
            UserRepository userRepocitory = new UserRepository(SessionCustom);
            IdeaRepository ideaRepocitory = new IdeaRepository(SessionCustom);
            ChallengeRepository challengeRepocitory = new ChallengeRepository(SessionCustom);
            QuestionRepository questionRepocitory = new QuestionRepository(SessionCustom);
            ContentRepository contentRepository = new ContentRepository(SessionCustom);

            userRepocitory.Entity.Active = true;

            List<User> users = userRepocitory.GetAll().Where(u => u.News == true).ToList();
            int total = 0;

            List<Domain.Entities.FrontEnd.ChallengesPaging> challenges = challengeRepocitory.ChallengesPaging(1, 3, out total, true,CurrentLanguage.LanguageId);
            List<Domain.Entities.FrontEnd.QuestionsPaging> questions = questionRepocitory.QuestionsPaging(1, 3, out total, true);

            foreach (User user in users)
            {
                List<Domain.Entities.FrontEnd.MailIdeasPaging> ideas = ideaRepocitory.IdeasUserMail(user.UserId.Value, null);
                foreach (Domain.Entities.FrontEnd.MailIdeasPaging idea in ideas)
                {
                    contentRepository.Entity = new Domain.Entities.Content();
                    contentRepository.Entity.ContentId = idea.ContentId;
                    contentRepository.LoadByKey();
                    idea.objContent = contentRepository.Entity;
                }

                string folderhtml = string.Empty;
                ViewData.Model = new Domain.Entities.FrontEnd.UserMail()
                {
                    objUser = user,
                    CollIdeas = ideas,
                    CollChallenges = challenges,
                    CollQuestions = questions
                };
                using (StringWriter sw = new StringWriter())
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "UserMail");
                    ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    folderhtml = sw.GetStringBuilder().ToString();
                }

                /*
                SendMail mail = new SendMail();
                mail.From = ConfigurationManager.AppSettings["MailContact"];
                mail.To = user.Email;
                mail.Body = folderhtml;
                mail.Subject = "Mi medellín - semanal";
                mail.SendMessage();
                */
            }

            return this.View();
        }

        /// <summary>
        /// show a preview of the welcome email to the user
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Welcome()
        {
            IdeaRepository idea = new IdeaRepository(SessionCustom);
            UserRepository user = new UserRepository(SessionCustom);
            int totalParticipants;
            user.Participants(0, out totalParticipants);
            ViewBag.CountIdeas = idea.IdeasCountAll();
            ViewBag.CountUsers = totalParticipants;
            ViewBag.UserName = User.Identity.Name;

            return this.View();
        }

        /// <summary>
        /// show a form to send an email to the users of the site
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="contentId">content id</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        [HttpGet]
        public ActionResult ManualEmail(int? userId, int? contentId)
        {
            if (((CustomPrincipal)User).IsFrontEndAdmin)
            {
                if (userId.HasValue)
                {
                    UserRepository user = new UserRepository(SessionCustom);
                    user.Entity.UserId = userId;
                    user.LoadByKey();
                    ViewBag.User = user.Entity;
                }
                else if (contentId.HasValue)
                {
                    int total = 0;
                    UserRepository user = new UserRepository(SessionCustom);
                    List<User> users = user.Users(0, 20000, out total, contentId, null, null, null).Where(u => u.Active.Value == true && u.News.Value == true && u.Email != null).ToList();
                    ViewBag.Users = users;
                    ViewBag.ContentId = contentId.Value;
                }
                else
                {
                    int total = 0;
                    UserRepository user = new UserRepository(SessionCustom);
                    List<User> users = user.Users(0, 40000, out total, null, null, null, null).Where(u => u.Active.Value == true && u.News.Value == true && u.Email != null).ToList();
                    ViewBag.Users = users;
                }

                return this.View();
            }

            return null;
        }

        /// <summary>
        /// receive a form to send an email to the users of the site
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="contentId">content id</param>
        /// <param name="name">sender name</param>
        /// <param name="subject">email subject</param>
        /// <param name="body">email body</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ManualEmail(int? userId, int? contentId, string name, string subject, string body)
        {
            bool result = false;
            if (((CustomPrincipal)User).IsFrontEndAdmin)
            {
                List<string> emailDebug = new List<string>();
                emailDebug.Add("andres.paris@dasigno.com");

                emailDebug.Add("adrianaoviedo_82@yahoo.com");
                emailDebug.Add("pruebatbg@yahoo.com");
                emailDebug.Add("morenojuan300@yahoo.com");
                emailDebug.Add("juanbello12@outlook.com");
                emailDebug.Add("jennyjim28@hotmail.com");

                body = Utils.FormatEmail(body, string.Empty);

                if (userId.HasValue)
                {
                    UserRepository user = new UserRepository(SessionCustom);
                    user.Entity.UserId = userId;
                    user.LoadByKey();

                    if (user.Entity.Active.Value && emailDebug.Exists(ed => ed == user.Entity.Email))
                    {
                        SendMail mail = new SendMail();
                        mail.To = user.Entity.Email;
                        mail.From = string.Concat(name, "<", ConfigurationManager.AppSettings["MailContact"], ">");
                        mail.Subject = subject;
                        mail.Body = body;
                        mail.SendMessage();

                        result = true;
                    }
                }
                else if (contentId.HasValue)
                {
                    int total = 0;
                    UserRepository userRepo = new UserRepository(SessionCustom);
                    List<User> users = userRepo.Users(0, 20000, out total, contentId, null, null, null).Where(u => u.Active.Value == true && u.News.Value == true).ToList();
                    name = string.Concat(name, "<", ConfigurationManager.AppSettings["MailContact"], ">");

                    SendMail mail;
                    foreach (User user in users)
                    {
                        if (user.Active.Value && emailDebug.Exists(ed => ed == user.Email))
                        {
                            mail = new SendMail();
                            mail.To = user.Email;
                            mail.From = name;
                            mail.Subject = subject;
                            mail.Body = body;
                            mail.SendMessage();
                        }
                    }

                    result = true;
                }
                else
                {
                    int total = 0;
                    UserRepository userRepo = new UserRepository(SessionCustom);
                    List<User> users = userRepo.Users(0, 40000, out total, null, null, null, null).Where(u => u.Active.Value == true && u.News.Value == true).ToList();
                    name = string.Concat(name, "<", ConfigurationManager.AppSettings["MailContact"], ">");

                    SendMail mail;
                    foreach (User user in users)
                    {
                        if (user.Active.Value && emailDebug.Exists(ed => ed == user.Email))
                        {
                            mail = new SendMail();
                            mail.To = user.Email;
                            mail.From = name;
                            mail.Subject = subject;
                            mail.Body = body;
                            mail.SendMessage();
                        }
                    }

                    result = true;
                }
            }

            return this.Json(new { result = result });
        }
    }
}
