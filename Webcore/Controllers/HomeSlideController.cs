// <copyright file="HomeSlideController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// controller home base
    /// </summary>
    public class HomeSlideController : FrontEndController
    {
        /// <summary>
        /// gets the home of application
        /// </summary>
        /// <returns>returns the result to action</returns>
        /// <History>
        /// Modificado por      :   Juan Carlos Soto Cruz (JCS)
        /// Descripción cambio  :   Se retira la logica para el control del Token y se traslada a la controladora SeccionController.cs
        /// Fecha               :   2015/11/06 
        /// </History> 
        public ActionResult Index()
        {
            //UpdateLanguage();
            SetLabel();
            BannerRepository banner = new BannerRepository(SessionCustom);
            QuestionRepository question = new QuestionRepository(SessionCustom);
            IdeaRepository idea = new IdeaRepository(SessionCustom);
            ChallengeRepository challenge = new ChallengeRepository(SessionCustom);
            ContentRepository content = new ContentRepository(SessionCustom);
            UserRepository user = new UserRepository(SessionCustom);

            List<KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>> collmeta = new List<KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>>();

            collmeta.Add(
                new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                    new Domain.Entities.KeyValue("name", "title"),
                    new Domain.Entities.KeyValue("content", ConfigurationManager.AppSettings["TitleHome"])));

            collmeta.Add(new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                new Domain.Entities.KeyValue("name", "description"),
                new Domain.Entities.KeyValue("content", ConfigurationManager.AppSettings["DescriptionHome"])));

            collmeta.Add(new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                new Domain.Entities.KeyValue("property", "og:title"),
                new Domain.Entities.KeyValue("content", ConfigurationManager.AppSettings["TitleHome"])));

            collmeta.Add(new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                new Domain.Entities.KeyValue("property", "og:description"),
                new Domain.Entities.KeyValue("content", ConfigurationManager.AppSettings["DescriptionHome"])));

            collmeta.Add(new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                new Domain.Entities.KeyValue("property", "og:url"),
                new Domain.Entities.KeyValue("content", ("http://" + Request.Url.Host + Request.ApplicationPath).TrimEnd('/') + "/")));

            collmeta.Add(new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                new Domain.Entities.KeyValue("property", "og:image"),
                new Domain.Entities.KeyValue("content", ("http://" + Request.Url.Host + Request.ApplicationPath).TrimEnd('/') + "/1024.png")));

            int totalParticipants;
            List<Domain.Entities.FrontEnd.UserProfilePaging> partipants = user.Participants(Convert.ToInt32(ConfigurationManager.AppSettings["EstamosParticipando"]), out totalParticipants);

            ViewBag.TotalParticipants = totalParticipants;

            List<FeaturedChallengesQuestions> featuredList = content.FeaturedChallengesQuestions(CurrentLanguage.LanguageId);
            List<FeaturedChallengesQuestions> finishedList = content.FinishedChallengesQuestions(CurrentLanguage.LanguageId);
            FileattachRepository fileRepository = new FileattachRepository(SessionCustom);
            foreach (FeaturedChallengesQuestions itemContent in featuredList)
            {
                fileRepository.Entity.ContentId = itemContent.ContentId;
                Fileattach file = fileRepository.GetAll().FirstOrDefault(t => t.Type == Domain.Entities.Fileattach.TypeFile.Video);
                if (file != null)
                {
                    itemContent.Video = file.Filename;
                }
            }

            string key = (string)Session["notification-token"];
            if (!string.IsNullOrEmpty(key))
            {
                this.Session["notification-token"] = null;
                ViewBag.Key = key;
            }

            return this.View(new Models.FEHome()
            {
                PageTitle = ConfigurationManager.AppSettings["TitleHome"],
                UserPrincipal = CustomUser,
                MetaTags = collmeta,
                Banners = banner.GetBannersBySection(0, CurrentLanguage.LanguageId.Value),
                CurrentLanguage = CurrentLanguage,
                ExpiringQuestions = question.ExpiringQuestions(),
                ExpiringChallenges = challenge.ExpiringChallenges(),
                IdeasCountAll = idea.IdeasCountAll(),
                FeaturedChallengesQuestions = featuredList,
                Participants = partipants,
                BestOfAll = new Domain.Entities.FrontEnd.ContentBestOfAll()
                {
                    InnovativeUsers = user.InnovativeUserListHome(5),
                    TopIdeas = idea.TopIdeasHome(5),
                    TopVotedIdeas = idea.TopVotedIdeasHome(5)
                },
                FinishedChallengesQuestions = finishedList
            });
        }

        /// <summary>
        /// gets a view with the expiring challenges and questions
        /// </summary>
        /// <param name="challenges">challenge collection</param>
        /// <param name="questions">question collection</param>
        /// <param name="challengesQuestions">challenge and question collection</param>
        /// <returns>returns the result to action</returns>
        public ActionResult ExpiringChallengesQuestions(List<Domain.Entities.FrontEnd.ExpiringChallenges> challenges, List<Domain.Entities.FrontEnd.ExpiringQuestions> questions, List<Domain.Entities.FrontEnd.FeaturedChallengesQuestions> challengesQuestions)
        {
            SetLabel();
            ViewBag.Challenges = challenges;
            ViewBag.Questions = questions;
            ViewBag.ChallengesQuestions = challengesQuestions;
            return this.View();
        }

        /// <summary>
        /// gets a view with the finished challenges and questions
        /// </summary>
        /// <param name="challengesQuestions">challenge and question collection</param>
        /// <param name="challenges">challenge collection</param>
        /// <param name="questions">question collection</param>
        /// <returns>returns the result to action</returns>
        public ActionResult FinishedChallengesQuestions(List<Domain.Entities.FrontEnd.FeaturedChallengesQuestions> challengesQuestions, List<Domain.Entities.FrontEnd.ExpiringChallenges> challenges, List<Domain.Entities.FrontEnd.ExpiringQuestions> questions)
        {
            SetLabel();
            ViewBag.ChallengesQuestions = challengesQuestions;
            ViewBag.Challenges = challenges;
            ViewBag.Questions = questions;
            return this.View();
        }

        /// <summary>
        /// obtains a error page
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Error()
        {
            return this.View("Error");
        }

        /// <summary>
        /// gets the user profile small block for the menu bar
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult UserBlock()
        {
            SetLabel();
            if (User.Identity.IsAuthenticated)
            {
                int total = 0;
                new SystemNotificationRepository(SessionCustom).Notifications(0, 0, ((CustomPrincipal)User).UserId, out total);
                ViewBag.Notifications = total;
                ViewBag.UserId = ((CustomPrincipal)User).UserId;

            }
            else
            {
                ViewBag.Notifications = 0;
            }

            return this.View("_UserBlock" );
        }

        /// <summary>
        /// show a form for the user to "contact us"
        /// </summary>
        /// <returns>returns the result to action</returns>
        [HttpGet]
        public ActionResult Contacto()
        {
            return this.View("_ContactUs", new ContactUs());
        }

        /// <summary>
        /// receive a form for the user to "contact us"
        /// </summary>
        /// <param name="model">contact us model</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Contacto(ContactUs model)
        {
            SendMail mail = new SendMail();
            mail.From = ConfigurationManager.AppSettings["MailContact"];
            mail.To = ConfigurationManager.AppSettings["MailContact"];
            mail.Body = "Nombre y Apellidos: <br /> " + model.Names + "<br /><br /><br />Correo electrónico: <br />" + model.Email + "<br /><br /><br /> Mensaje: <br />" + model.Message;
            mail.Subject = model.Subject;
            mail.SendMessage();

            model.Sent = true;

            return this.View("_ContactUs", model);
        }

        /// <summary>
        /// Generates a site map of the sections and content of the site
        /// </summary>
        /// <returns>A file result with the XML site map</returns>
        [HttpGet]
        public FileResult SiteMap()
        {
            SetLabel();
            List<string> urls = new List<string>();

            InfoCache<List<Domain.Entities.Section>> cache = new InfoCache<List<Domain.Entities.Section>>(HttpContext) { TimeOut = 120 };
            List<Domain.Entities.Section> coll = cache.GetCache("sectionsactive");

            if (coll == null)
            {
                SectionRepository section = new SectionRepository(SessionCustom);
                section.Entity.Active = true;
                coll = section.GetAll();
                cache.SetCache("sectionsactive", coll);
            }

            coll.FindAll(t => t.LanguageId == 2);

            string host = string.Concat("http://", Request.Url.Host, Request.ApplicationPath).TrimEnd('/');

            urls.Add(host);
            foreach (Domain.Entities.Section item in coll.Where(t => t.ParentId == null && t.SectionId != 31 && t.SectionId != 32 && t.SectionId != 41 && t.SectionId != 42))
            {
                urls.Add(string.Concat(host, "/", item.Url.Value ? item.Navigateurl : item.Friendlyname));
            }

            ContentRepository content = new ContentRepository(SessionCustom);
            int total = 0;
            List<Pulse> pulses = content.Pulses(0, 1000, out total, null,CurrentLanguage.LanguageId);

            foreach (Pulse pulse in pulses)
            {
                urls.Add(string.Concat(host, "/", pulse.Friendlyurlid));
            }

            var ms = new MemoryStream(Encoding.ASCII.GetBytes(this.GenerateSiteMapXML(urls.ToArray())));

            Response.AppendHeader("Content-Disposition", "inline;filename=sitemapindex.xml");
            return new FileStreamResult(ms, "text/xml");
        }

        /// <summary>
        /// Construct a XML with the list of url
        /// </summary>
        /// <param name="urls">a list of url for the XML file</param>
        /// <returns>a string with the XML file</returns>
        private string GenerateSiteMapXML(string[] urls)
        {
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();

            System.Xml.XmlDeclaration declaration;
            declaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);

            System.Xml.XmlElement element = document.CreateElement("urlset");
            System.Xml.XmlAttribute attribute = document.CreateAttribute("xmlns");
            attribute.Value = "http://www.google.com/schemas/sitemap/0.90";
            element.Attributes.Append(attribute);
            document.AppendChild(element);
            document.InsertBefore(declaration, element);

            System.Xml.XmlElement elementURL;
            System.Xml.XmlElement elementLocation;
            foreach (string url in urls)
            {
                elementURL = document.CreateElement("url");
                elementLocation = document.CreateElement("loc");
                elementLocation.InnerText = url;

                elementURL.AppendChild(elementLocation);
                element.AppendChild(elementURL);
            }

            return document.OuterXml;
        }

        /// <summary>
        /// Actualizacion del Lenguaje
        /// </summary>
        private void UpdateLanguage()
        {
            if (HttpContext.Session["lang"] != null && (Language)HttpContext.Session["lang"] != null)
            {
                this.CurrentLanguage = (Language)HttpContext.Session["lang"];
            }
        }

        /// <summary>
        /// Set label vistas
        /// </summary>
        private void SetLabel()
        {
            LabelManagement objlabel = new LabelManagement(SessionCustom, HttpContext);
            ViewBag.TXTPARCIU = objlabel.GetLabelByName("TXTPARCIU", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTTUIDEA = objlabel.GetLabelByName("TXTTUIDEA", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTPARCIUINI = objlabel.GetLabelByName("TXTPARCIUINI", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTESCREP = objlabel.GetLabelByName("TXTESCREP", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTDIARES = objlabel.GetLabelByName("TXTDIARES", CurrentLanguage.LanguageId.Value);
            ViewBag.RETO = objlabel.GetLabelByName("RETO", CurrentLanguage.LanguageId.Value);
            ViewBag.DAYS = objlabel.GetLabelByName("DAYS", CurrentLanguage.LanguageId.Value);
            ViewBag.PREMIUM = objlabel.GetLabelByName("PREMIUM", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTYASOMO = objlabel.GetLabelByName("TXTYASOMO", CurrentLanguage.LanguageId.Value);
            ViewBag.QUESTIONS = objlabel.GetLabelByName("QUESTIONS", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTTUSIDEA = objlabel.GetLabelByName("TXTTUSIDEA", CurrentLanguage.LanguageId.Value);
            ViewBag.LOCATION = objlabel.GetLabelByName("TXTTUSIDEA", CurrentLanguage.LanguageId.Value);
            ViewBag.MULTIPLE = objlabel.GetLabelByName("MULTIPLE", CurrentLanguage.LanguageId.Value);
            ViewBag.WISH = objlabel.GetLabelByName("MULTIPLE", CurrentLanguage.LanguageId.Value);
            ViewBag.CITY = objlabel.GetLabelByName("CITY", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTUBIRESMAP = objlabel.GetLabelByName("TXTUBIRESMAP", CurrentLanguage.LanguageId.Value);
            ViewBag.RETOS = objlabel.GetLabelByName("RETOS", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTGENSOL = objlabel.GetLabelByName("TXTGENSOL", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTESTPART = objlabel.GetLabelByName("TXTESTPART", CurrentLanguage.LanguageId.Value);
            ViewBag.PEOPLE = objlabel.GetLabelByName("PEOPLE", CurrentLanguage.LanguageId.Value);
            ViewBag.ACCEPT = objlabel.GetLabelByName("ACCEPT", CurrentLanguage.LanguageId.Value);
            ViewBag.CLOSESSION = objlabel.GetLabelByName("CLOSESSION", CurrentLanguage.LanguageId.Value);
             ViewBag.NOTIFICATIONS  = objlabel.GetLabelByName("NOTIFICATIONS", CurrentLanguage.LanguageId.Value);
             ViewBag.VIEWPROFILE = objlabel.GetLabelByName("VIEWPROFILE", CurrentLanguage.LanguageId.Value);
             ViewBag.TXTVERMAS = objlabel.GetLabelByName("TXTVERMAS", CurrentLanguage.LanguageId.Value);
             ViewBag.TXTEARNPOIN = objlabel.GetLabelByName("TXTEARNPOIN", CurrentLanguage.LanguageId.Value);
             ViewBag.SIGNIN = objlabel.GetLabelByName("SIGNIN", CurrentLanguage.LanguageId.Value);
             ViewBag.OPEN = objlabel.GetLabelByName("OPEN", CurrentLanguage.LanguageId.Value);
             ViewBag.TXTCUEOPI = objlabel.GetLabelByName("TXTCUEOPI", CurrentLanguage.LanguageId.Value);
             ViewBag.NEARESTCITIZENS = objlabel.GetLabelByName("NEARESTCITIZENS", CurrentLanguage.LanguageId.Value);
             ViewBag.TXTLOOKING = objlabel.GetLabelByName("TXTLOOKING", CurrentLanguage.LanguageId.Value);
             ViewBag.IDEASPLURAL = objlabel.GetLabelByName("IDEASPLURAL", CurrentLanguage.LanguageId.Value);
             ViewBag.SEARCH = objlabel.GetLabelByName("SEARCH", CurrentLanguage.LanguageId.Value);
          
            
        }

    
    }
}

