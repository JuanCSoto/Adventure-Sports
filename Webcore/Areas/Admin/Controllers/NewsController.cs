// <copyright file="NewsController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Xml;
    using Business;
    using Business.Services;
    using Core.Facades;
    using Domain.Concrete;
    using Domain.Entities;
    using Webcore.Areas.Admin.Models;
    
    /// <summary>
    /// Controller for News module
    /// </summary>
    [ModulAuthorize]
    public class NewsController : AdminController
    {
        /// <summary>
        /// gets the home of news module
        /// </summary>
        /// <param name="mod">identifier of module</param>
        /// <param name="sectionId">identifier of section</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Index(int mod, int? sectionId)
        {
            ContentManagement objcontentman = new ContentManagement(this.SessionCustom, HttpContext);
            SectionRepository objsection = new SectionRepository(this.SessionCustom);
            TemplateRepository objtemplate = new TemplateRepository(this.SessionCustom);
            ContentRepository objcontent = new ContentRepository(this.SessionCustom);
            TagFacade tagFacade = new TagFacade();
            objtemplate.Entity.Type = 0;

            MoldRepository objMold = new MoldRepository(this.SessionCustom);

            List<Domain.Entities.Mold> collMold = objMold.GetAll();
            collMold.Insert(
                0, 
                new Domain.Entities.Mold()
            {
                Name = Resources.Global.Messages.SELECT
            });

            return this.View(new NewsModel()
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
                CollMold = collMold,
                Categories = objcontent.Categories(),
                Tags = tagFacade.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.TagId.ToString() })
            });
        }

        /// <summary>
        /// obtains the news detail
        /// </summary>
        /// <param name="mod">identifier of module</param>
        /// <param name="id">identifier of section</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(int mod, int id)
        {
            ContentManagement objcontentman = new ContentManagement(SessionCustom, HttpContext);
            ContentRepository objcontent = new ContentRepository(SessionCustom);
            NewsRepository objnews = new NewsRepository(SessionCustom);
            FileattachRepository objfiles = new FileattachRepository(SessionCustom);
            TagRepository objtag = new TagRepository(SessionCustom);
            SectionRepository objsection = new SectionRepository(SessionCustom);
            TemplateRepository objtemplate = new TemplateRepository(SessionCustom);
            MoldRepository objMold = new MoldRepository(SessionCustom);
            XmlNodeList collXmlNode = null;
            TagFacade tagFacade = new TagFacade();

            List<Domain.Entities.Mold> collMold = objMold.GetAll();
            collMold.Insert(
                0, 
                new Domain.Entities.Mold() 
            {
                Name = Resources.Global.Messages.SELECT
            });

            objtemplate.Entity.Type = 0;

            objnews.Entity.ContentId = 
                objfiles.Entity.ContentId = 
                objcontent.Entity.ContentId = id;

            objnews.LoadByKey();
            objcontent.LoadByKey();

            if (objnews.Entity.MoldId != null)
            {
                XmlDocument objXmlDocument = new XmlDocument();
                objXmlDocument.LoadXml(objnews.Entity.Xmlcontent);
                collXmlNode = objXmlDocument.GetElementsByTagName("node");
            }

            IEnumerable<Tag> SelectedTags = objtag.GetTagbycontent(id);
            this.ViewBag.SelectedTags = string.Join("|", SelectedTags.Select(t => t.TagId));
            this.ViewBag.NewsTags = string.Empty;

            return this.View(
                "Index", 
                new NewsModel()
            {
                UserPrincipal = this.CustomUser,
                ColModul = CustomMemberShipProvider.GetModuls(this.CustomUser.UserId, this.SessionCustom, HttpContext),
                Module = this.Module,
                ListFiles = objfiles.GetAllReadOnly(),
                News = objnews.Entity,
                IContent = objcontent.Entity,
                Templates = objtemplate.GetAll().Select(t => t.TemplateId),
                ListContent = objcontent.GetContentRelation(CurrentLanguage.LanguageId.Value),
                ListTags = SelectedTags,
                DeepFollower = Business.Utils.GetDeepFollower(objsection.GetAll(), objcontent.Entity.SectionId.Value),
                CurrentLanguage = this.CurrentLanguage,
                CollMold = collMold,
                CollXmlNode = collXmlNode,
                Categories = objcontent.Categories(),
                Tags = tagFacade.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.TagId.ToString() })
            });
        }

        /// <summary>
        /// inserts or updates a item news
        /// </summary>
        /// <param name="model">identifier of module</param>
        /// <param name="contentImage">represents a upload content image</param>
        /// <param name="videoyoutube">represents a list of videos</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(NewsModel model, HttpPostedFileBase contentImage, List<string> videoyoutube, string existingTags, string newTags)
        {
            NewsRepository objnews = new NewsRepository(this.SessionCustom);
            ContentManagement objcontent = new ContentManagement(this.SessionCustom, HttpContext);
            
            try
            {
                objcontent.ContentImage = contentImage;
                objcontent.CollVideos = videoyoutube;
                this.SessionCustom.Begin();

                model.IContent.LanguageId = CurrentLanguage.LanguageId;
                objcontent.ContentInsert(model.IContent);
                objnews.Entity = model.News;
                objnews.Entity.ExistingTags = !string.Empty.Equals(existingTags) ? existingTags : null;
                objnews.Entity.NewTags = !string.Empty.Equals(newTags) ? newTags : null;

                if (objnews.Entity.ContentId != null)
                {
                    if (model.News.MoldId != null)
                    {
                        MoldRepository objMoldRepository = new MoldRepository(this.SessionCustom);
                        objMoldRepository.Entity.MoldId = model.News.MoldId;
                        objMoldRepository.LoadByKey();

                        XmlDocument objDoc = this.GetXmlInformation(objMoldRepository.Entity.Xmlcontent, objnews.Entity.ContentId.Value);
                        model.News.Xmlcontent = objDoc.InnerXml;
                    }
                    
                    objnews.Update();
                    this.InsertAudit("Update", this.Module.Name + " -> " + model.IContent.Name);
                }
                else
                {
                    if (model.News.MoldId != null)
                    {
                        MoldRepository objMoldRepository = new MoldRepository(SessionCustom);
                        objMoldRepository.Entity.MoldId = model.News.MoldId;
                        objMoldRepository.LoadByKey();

                        XmlDocument objDoc = this.GetXmlInformation(objMoldRepository.Entity.Xmlcontent, objcontent.ObjContent.ContentId.Value);
                        model.News.Xmlcontent = objDoc.InnerXml;
                    }
                    
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

                            if (!string.IsNullOrEmpty(objnews.Entity.Xmlcontent))
                            {
                                objnews.Entity.Xmlcontent = objnews.Entity.Xmlcontent.Replace(item, "/Files/" + objcontent.ObjContent.ContentId + "/" + Path.GetFileName(item));
                            }
                        }
                    }
                    
                    objnews.Entity.ContentId = objcontent.ObjContent.ContentId;
                    objnews.Insert();
                    
                    this.InsertAudit("Insert", this.Module.Name + " -> " + model.IContent.Name);
                }

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
                return this.RedirectToAction("Index", "Content", new { mod = Module.ModulId });
            }
            else
            {
                return this.RedirectToAction("Detail", "News", new { mod = Module.ModulId, id = objnews.Entity.ContentId });
            }
        }

        /// <summary>
        /// obtains a <c>XML</c> information to specific mold
        /// </summary>
        /// <param name="id">identifier of mold</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ContentResult GetMold(int id)
        {
            MoldRepository objMoldRepository = new MoldRepository(SessionCustom);
            objMoldRepository.Entity.MoldId = id;
            objMoldRepository.LoadByKey();

            XmlDocument objDoc = new XmlDocument();
            objDoc.LoadXml(objMoldRepository.Entity.Xmlcontent);
            string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(objDoc.ChildNodes[1]);

            return new ContentResult()
            {
                Content = json,
                ContentType = "application/json"
            };
        }

        /// <summary>
        /// build a <c>XML</c> structure from a string
        /// </summary>
        /// <param name="xmlString">string information</param>
        /// <param name="contentId">identifier of content</param>
        /// <returns>returns a <c>XmlDocument</c> object</returns>
        private XmlDocument GetXmlInformation(string xmlString, int contentId)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            int count = 0;
            foreach (XmlNode item in xmlDoc.GetElementsByTagName("node"))
            {
                count++;
                switch (item.Attributes["control"].InnerText)
                {
                    case "TextBox":
                        item.InnerText = Request.Form["_hf_" + count];
                        break;
                    case "Wysiwyg":
                        XmlCDataSection datasection = xmlDoc.CreateCDataSection(Request.Unvalidated.Form["_hf_" + count]);
                        item.AppendChild(datasection);
                        break;
                    default:
                        if (Request.Files["_hf_" + count] != null && Request.Files["_hf_" + count].ContentLength > 0)
                        {
                            item.InnerText = Utils.UploadFile(Request.Files["_hf_" + count], Server.MapPath("~"), @"Files\" + contentId + @"\", null); 
                        }
                        else if (Request.Form["hd_hf_" + count] != null)
                        {
                            item.InnerText = Request.Form["hd_hf_" + count];
                        }

                        break;
                }
            }

            return xmlDoc;
        }
    }
}
