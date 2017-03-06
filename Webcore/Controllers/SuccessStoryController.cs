// <copyright file="SuccessStoryController.cs" company="Intergrupo">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
namespace Webcore.Controllers
{
   using System;
   using System.Configuration;
   using System.Web.Mvc;
   using Business.Services;
   using Core.Facades;
   using Domain.Concrete;
   using Domain.Entities;
   using Domain.Entities.FrontEnd;
   using Webcore.Models;

    /// <summary>
    /// Controler from Success Case
    /// </summary>
    public class SuccessStoryController : FrontEndController
    {
        /// <summary>
        /// Presenta la el detalle del casos de exito.
        /// </summary>
        /// <param name="id">Id del caso de exito</param>
        /// <returns>Retorna la vista</returns>
        public ActionResult Story(int id = 0)
        {
            int? currentUserId = null;
            this.UpdateLanguage();
            FrontEndManagement objmanagement = new FrontEndManagement(this.SessionCustom, HttpContext, FrontEndManagement.Type.Content, this.CurrentLanguage);
            UserRepository user = new UserRepository(this.SessionCustom);
            SuccessStoryFacade successCase = new SuccessStoryFacade();
            SuccessStoryList collSuccessCase = successCase.GetById(id, this.CurrentLanguage.LanguageId);
            IdeaRepository idea = new IdeaRepository(this.SessionCustom);
            FileattachRepository file = new FileattachRepository(this.SessionCustom);
            FESuccessStory story = new FESuccessStory();

            if (User.Identity.IsAuthenticated)
            {
                currentUserId = ((CustomPrincipal)User).UserId;
                objmanagement.BindInfo(id, currentUserId);
                ViewBag.CurrentUserId = currentUserId;
            }
            else
            {
                objmanagement.BindInfo(id, null);
            }

            story.Story = collSuccessCase;

            if (story.Story.UserId > 0)
            {
                user.Entity.UserId = story.Story.UserId;
                user.LoadByKey();
            }

            file.Entity.ContentId = id;
            string[] opciones = new string[] { "\r\n" };
            string[] urls = collSuccessCase.Url.Split(opciones, StringSplitOptions.None);

            this.ViewBag.Urls = urls;

            story.CollFiles = file.GetAll();
            story.ObjUser = user.Entity;
            story.MetaTags = objmanagement.Metatags;
            story.UserPrincipal = this.CustomUser;
            story.CurrentLanguage = this.CurrentLanguage;
            story.IdeasCountAll = idea.IdeasCountAll();
            story.Section = objmanagement.Section;
            story.PageTitle = string.Format("{0} | {1}", ConfigurationManager.AppSettings["TitleHome"], story.Story.Name);

            return View(story);
        }

        /// <summary>
        /// Update Language
        /// </summary>
        private void UpdateLanguage()
        {
            if (HttpContext.Session["lang"] != null)
            {
                this.CurrentLanguage = (Language)HttpContext.Session["lang"];
            }
            else
            {
                LanguageRepository languagerepo = new LanguageRepository(this.SessionCustom);
                languagerepo.GetByUser(this.CustomUser.UserId);
                this.CurrentLanguage = languagerepo.Entity;
                if (this.CurrentLanguage.Name != null)
                {
                    HttpContext.Session.Add("lang", this.CurrentLanguage);
                }
                else
                {
                    languagerepo.GetLanguageDefault();
                    HttpContext.Session.Add("lang", this.CurrentLanguage);
                }

            }
        }
    }
}
