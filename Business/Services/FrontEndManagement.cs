// <copyright file="FrontEndManagement.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Remoting;
    using System.Web;
    using Business.FrontEnd;
    using Domain.Abstract;
    using Domain.Concrete;
    using Domain.Entities;
    
    /// <summary>
    /// Management front end
    /// </summary>
    public class FrontEndManagement
    {
        /// <summary>
        /// framework that establishes communication between the application and the database
        /// </summary>
        private ISession session;

        /// <summary>
        /// HTTP Context
        /// </summary>
        private HttpContextBase context;

        /// <summary>
        /// Type of front end object
        /// </summary>
        private Type type;

        /// <summary>
        /// language of thread
        /// </summary>
        private Language language;

        /// <summary>
        /// list of sections
        /// </summary>
        private List<Section> sections;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrontEndManagement"/> class
        /// </summary>
        /// <param name="session">framework that establishes communication between the application and the database</param>
        /// <param name="context">HTTP Context</param>
        /// <param name="type">Type of front end object</param>
        /// <param name="language">language of thread</param>
        public FrontEndManagement(ISession session, HttpContextBase context, Type type, Language language)
        {
            this.session = session;
            this.context = context;
            this.language = language;
            this.type = type;

            SectionRepository objsec = new SectionRepository(session);
            this.sections = new List<Section>();
            InfoCache<List<Section>> cache = new InfoCache<List<Section>>(context) { TimeOut = 120 };

            this.sections = cache.GetCache("sectionsactive");

            if (this.sections == null)
            {
                objsec.Entity.Active = true;
                this.sections = objsec.GetAll();
                cache.SetCache("sectionsactive", this.sections);
            }
        }

        /// <summary>
        /// Type of front end object
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// object section
            /// </summary>
            Section,

            /// <summary>
            /// object content
            /// </summary>
            Content,

            /// <summary>
            /// object idea
            /// </summary>
            Idea
        }

        /// <summary>
        /// result of operation
        /// </summary>
        public enum Result
        {
            /// <summary>
            /// success operation
            /// </summary>
            Ok,

            /// <summary>
            /// the user not have authorization
            /// </summary>
            NotAuthorize,

            /// <summary>
            /// the object not found
            /// </summary>
            NotFound,

            /// <summary>
            /// system error
            /// </summary>
            Error
        }

        /// <summary>
        /// Gets or sets the operation result
        /// </summary>
        public Result Outcome { get; set; }

        /// <summary>
        /// Gets or sets the object content
        /// </summary>
        public Content Content { get; set; }

        /// <summary>
        /// Gets or sets the object idea
        /// </summary>
        public Idea Idea { get; set; }

        /// <summary>
        /// Gets or sets the object section
        /// </summary>
        public Section Section { get; set; }

        /// <summary>
        /// Gets or sets the output template
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets the output model
        /// </summary>
        public object Model { get; set; }

        /// <summary>
        /// Gets or sets the output layout
        /// </summary>
        public string Layout { get; set; }

        /// <summary>
        /// Gets or sets a list of meta tags
        /// </summary>
        public List<KeyValuePair<KeyValue, KeyValue>> Metatags { get; set; }

        /// <summary>
        /// Gets or sets the result of operation
        /// </summary>
        public object Detail { get; set; }

        /// <summary>
        /// Gets or sets the deep follower
        /// </summary>
        public HtmlString DeepFollower { get; set; }

        /// <summary>
        /// Fill a front end information
        /// </summary>
        /// <param name="key">search criteria</param>
        /// <param name="userId">current user ID</param>
        public void BindInfo(int key, int? userId)
        {
            ContentRepository objcont = new ContentRepository(this.session);

            this.Metatags = new List<KeyValuePair<KeyValue, KeyValue>>();

            if (this.type == Type.Content)
            {
                objcont.Entity.ContentId = key;
                objcont.LoadByKey();

                if (objcont.Entity.ContentId == null)
                {
                    this.Outcome = Result.NotFound;
                }
                else
                {
                    this.Section = this.sections.Find(t => t.SectionId == objcont.Entity.SectionId);

                    if (Section != null)
                    {
                        this.Content = objcont.Entity;
                        this.Layout = this.Section.Layout;
                        this.Template = objcont.Entity.Template;

                        this.Metatags.Add(
                            new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                                new Domain.Entities.KeyValue("name", "title"),
                                new Domain.Entities.KeyValue("content", objcont.Entity.Name)));

                        this.Metatags.Add(
                            new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                                new Domain.Entities.KeyValue("name", "description"),
                                new Domain.Entities.KeyValue("content", objcont.Entity.Shortdescription)));

                        this.Metatags.Add(
                            new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                                new Domain.Entities.KeyValue("property", "og:title"),
                                new Domain.Entities.KeyValue("content", objcont.Entity.Name)));

                        this.Metatags.Add(
                            new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                                new Domain.Entities.KeyValue("property", "og:description"),
                                new Domain.Entities.KeyValue("content",  objcont.Entity.Shortdescription)));

                        if (HttpContext.Current != null)
                        {
                            FileattachRepository fileRepository = new FileattachRepository(new SqlSession());
                            fileRepository.Entity.ContentId = objcont.Entity.ContentId;
                            Fileattach file = fileRepository.GetAll().FirstOrDefault(t => t.Type == Domain.Entities.Fileattach.TypeFile.Video);
                            string fileName = string.Empty;
                            if (file != null)
                            {
                                fileName = string.Concat(file.Name, "v=", file.Filename);
                            }

                            string siteUrlRoot = ("http://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath).TrimEnd('/');
                            Domain.Entities.FrontEnd.Video video = Business.Utils.GetVideoFromUrl(fileName);
                            string picture = siteUrlRoot + "/1024.png";
                            if (!string.IsNullOrEmpty(objcont.Entity.Image))
                            {
                                picture = siteUrlRoot + "/files/" + objcont.Entity.ContentId + "/" + objcont.Entity.Image;
                            }
                            else if (video != null && video.Type == "youtube")
                            {
                                picture = "http://img.youtube.com/vi/" + video.ID + "/default.jpg";
                            }

                            this.Metatags.Add(
                                new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                                    new Domain.Entities.KeyValue("property", "og:image"),
                                    new Domain.Entities.KeyValue("content", picture)));

                            this.Metatags.Add(
                                new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                                    new Domain.Entities.KeyValue("property", "og:url"),
                                    new Domain.Entities.KeyValue("content", ("http://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath).TrimEnd('/') + "/" + objcont.Entity.Frienlyname)));
                        }
                        
                        this.Outcome = Result.Ok;
                        this.Detail = this.GetDetail(this.GetNameType(objcont.Entity.Template), objcont.Entity.ContentId, userId, this.language.LanguageId);
                        this.DeepFollower = Utils.GetDeepFollowerFrontEnd(this.sections, this.Section.SectionId.Value, this.Content, this.language);
                        objcont.Entity.Views++;
                        objcont.Update();
                    }
                    else
                    {
                        this.Outcome = Result.NotFound;
                    }
                }
            }
            else if (this.type == Type.Idea)
            {
                IdeaRepository objidea = new IdeaRepository(this.session);
                objidea.Entity.IdeaId = key;
                objidea.LoadByKey();

                if (objidea.Entity.ContentId == null)
                {
                    this.Outcome = Result.NotFound;
                }
                else
                {
                    objcont.Entity.ContentId = objidea.Entity.ContentId;
                    objcont.LoadByKey();
                    this.Section = this.sections.Find(t => t.SectionId == objcont.Entity.SectionId);

                    if (Section != null)
                    {
                        this.Idea = objidea.Entity;
                        this.Layout = this.Section.Layout;

                        this.Metatags.Add(
                            new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                                new Domain.Entities.KeyValue("name", "title"),
                                new Domain.Entities.KeyValue("content", objcont.Entity.Name)));

                        this.Metatags.Add(
                            new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                                new Domain.Entities.KeyValue("name", "description"),
                                new Domain.Entities.KeyValue("content", objcont.Entity.Shortdescription)));

                        this.Metatags.Add(
                            new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                                new Domain.Entities.KeyValue("property", "og:title"),
                                new Domain.Entities.KeyValue("content", objcont.Entity.Name)));

                        this.Metatags.Add(
                            new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                                new Domain.Entities.KeyValue("property", "og:description"),
                                new Domain.Entities.KeyValue("content", objcont.Entity.Shortdescription)));

                        string siteUrlRoot = ("http://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath).TrimEnd('/');
                        Domain.Entities.FrontEnd.Video video = Business.Utils.GetVideoFromUrl(objidea.Entity.Video);
                        string picture = siteUrlRoot + "/1024.png";
                        if (!string.IsNullOrEmpty(objidea.Entity.Image))
                        {
                            picture = siteUrlRoot + "/files/ideas/" + objidea.Entity.Image;
                        }
                        else if (video != null && video.Type == "youtube")
                        {
                            picture = "http://img.youtube.com/vi/" + video.ID + "/default.jpg";
                        }

                        this.Metatags.Add(
                            new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                                new Domain.Entities.KeyValue("property", "og:image"),
                                new Domain.Entities.KeyValue("content", picture)));

                        FriendlyurlRepository url = new FriendlyurlRepository(this.session);
                        url.Entity.Id = key;
                        url.Entity.Type = Friendlyurl.FriendlyType.Idea;
                        url.Load();

                        this.Metatags.Add(
                            new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                                new Domain.Entities.KeyValue("property", "og:url"),
                                new Domain.Entities.KeyValue("content", ("http://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath).TrimEnd('/') + "/" + url.Entity.Friendlyurlid)));

                        this.Outcome = Result.Ok;
                        this.Detail = this.GetDetail(this.GetNameType(objcont.Entity.Template), objcont.Entity.ContentId, userId,this.language.LanguageId);

                        this.DeepFollower = Utils.GetDeepFollowerFrontEnd(this.sections, this.Section.SectionId.Value, this.Content, this.language);
                        objidea.Entity.Views++;
                        objidea.Update();

                        if (objidea.IsIdeaInTop10(objidea.Entity.IdeaId.Value))
                        {
                            SystemNotificationRepository notification = new SystemNotificationRepository(this.session);
                            int count = notification.SystemNotificationCount(objidea.Entity.UserId.Value, (int)Domain.Entities.Basic.SystemNotificationType.IDEA_TOP_10, objidea.Entity.IdeaId.Value);
                            if (count == 0)
                            {
                                Business.Utilities.Notification.NewNotification(objidea.Entity.UserId.Value, null, Domain.Entities.Basic.SystemNotificationType.IDEA_TOP_10, null, string.Concat("/", objidea.Entity.Friendlyurlid), objidea.Entity.ContentId, objidea.Entity.IdeaId.Value, null, null, null, this.session, this.context, this.language);
                            }
                        }

                        if (objidea.IsIdeaInTop5Home(objidea.Entity.IdeaId.Value))
                        {
                            SystemNotificationRepository notification = new SystemNotificationRepository(this.session);
                            int count = notification.SystemNotificationCount(objidea.Entity.UserId.Value, (int)Domain.Entities.Basic.SystemNotificationType.POPULAR_IDEA_TOP_5, objidea.Entity.IdeaId.Value);
                            if (count == 0)
                            {
                                Business.Utilities.Notification.NewNotification(objidea.Entity.UserId.Value, null, Domain.Entities.Basic.SystemNotificationType.POPULAR_IDEA_TOP_5, null, string.Concat("/", objidea.Entity.Friendlyurlid), objidea.Entity.ContentId, objidea.Entity.IdeaId.Value, null, null, null, this.session, this.context, this.language);
                            }
                        }
                    }
                    else
                    {
                        this.Outcome = Result.NotFound;
                    }
                }
            }
            else
            {
                this.Section = this.sections.Find(t => t.SectionId == key);

                if (this.Section == null)
                {
                    this.Outcome = Result.NotFound;
                }
                else
                {
                    this.Layout = this.Section.Layout;
                    this.Template = this.Section.Template;

                    this.Metatags.Add(
                        new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                            new Domain.Entities.KeyValue("name", "title"),
                            new Domain.Entities.KeyValue("content", this.Section.Name)));

                    this.Metatags.Add(
                        new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                            new Domain.Entities.KeyValue("name", "description"),
                            new Domain.Entities.KeyValue("content", this.Section.Description)));

                    this.Metatags.Add(
                        new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                            new Domain.Entities.KeyValue("property", "og:title"),
                            new Domain.Entities.KeyValue("content", this.Section.Name)));

                    this.Metatags.Add(
                        new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                            new Domain.Entities.KeyValue("property", "og:description"),
                            new Domain.Entities.KeyValue("content", this.Section.Description)));

                    this.Outcome = Result.Ok;

                    this.Detail = this.GetDetail(this.GetNameType(this.Section.Template), this.Section.SectionId, userId, this.language.LanguageId);
                    this.DeepFollower = Utils.GetDeepFollowerFrontEnd(this.sections, this.Section.SectionId.Value, null, this.language);
                }
            }
        }

        /// <summary>
        /// obtains a instance of class represents a front end object
        /// </summary>
        /// <param name="nameObject">name of class</param>
        /// <param name="id">identifier of object</param>
        /// <param name="userId">current user ID</param>
        /// <returns>returns a instance of class</returns>
        private object GetDetail(string nameObject, int? id, int? userId, int? LanguageId)
        {
            ObjectHandle objectmanagement = AppDomain.CurrentDomain.CreateInstance("Business", nameObject);
            object unwrap = objectmanagement.Unwrap();
            (unwrap as IFrontEnd).Bind(this.context, this.session, id, userId,LanguageId);
            return unwrap;
        }

        /// <summary>
        /// obtains the name of class
        /// </summary>
        /// <param name="template">templates name</param>
        /// <returns>returns a name of class</returns>
        private string GetNameType(string template)
        {
            List<Template> templates = new List<Template>();
            InfoCache<List<Template>> cache = new InfoCache<List<Domain.Entities.Template>>(this.context) { TimeOut = 120 };
            Domain.Entities.Template temp;

            

          
                TemplateRepository objtemplate = new TemplateRepository(this.session);
                templates = objtemplate.GetAll();
                cache.SetCache("templates", templates);
           

            temp = templates.Find(t => t.TemplateId == template);

            if (temp != null)
            {
                return temp.Nameclass;
            }
            else
            {
                return null;
            }
        }
    }
}
