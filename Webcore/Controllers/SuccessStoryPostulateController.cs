// <copyright file="SuccessStoryPostulateController.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace Webcore.Controllers
{
   using System;
   using System.Configuration;
   using System.Linq;
   using System.Web.Mvc;
   using Business.Services;
   using Core.Facades;
   using Domain.Concrete;
   using Domain.Entities.Enums;
   using Domain.Entities.Generic;
   using Webcore.Models.SuccessStoryPostulate;

    /// <summary>
    /// Representa la clase <see cref="Webcore.Controllers.SuccessStoryPostulateController"/>
    /// </summary>
    public class SuccessStoryPostulateController : FrontEndController
    {
        /// <summary>
        /// Ejecucata la acción create (get).
        /// </summary>
        /// <returns>El resultado de acción Create (get).</returns>
       [HttpGet]
       public ActionResult Create()
       {
          if (User.Identity.IsAuthenticated)
          {
             string titulo = string.Format("{0} | {1}", ConfigurationManager.AppSettings["TitleHome"], Resources.Global.Messages.POSTULATES_SUCCESS_STORY);
             FormModel model = new FormModel();

             CategoryFacade categoryFacade = new CategoryFacade();
             CountryFacade countryFacade = new CountryFacade();
             TagFacade tagFacade = new TagFacade();

             IdeaRepository ideaRepository = new IdeaRepository(this.SessionCustom);

             int languageId = (int)CurrentLanguage.LanguageId;

             model.Categories = categoryFacade.GetAll(languageId).Select(c => new SelectListItem { Text = c.CategoryLanguage.FirstOrDefault().Name, Value = c.CategoryId.ToString() });
             model.Countries = countryFacade.GetAll().Select(c => new SelectListItem { Text = languageId == (int)LanguageEnum.English ? c.NameEn : c.NameEs, Value = c.CountryID.ToString() });
             model.Tags = tagFacade.GetAll().Select(t => new SelectListItem { Text = t.Name, Value = t.TagId.ToString() });
             model.UserPrincipal = CustomUser;
             model.CurrentLanguage = CurrentLanguage;
             model.IdeasCountAll = ideaRepository.IdeasCountAll();
             model.PageTitle = titulo;
             return View(model);
          }
          else
          {
             return RedirectToAction("Index", "Seccion");
          }
       }

        /// <summary>
        /// Ejecucata la acción create (post).
        /// </summary>
        /// <param name="model">Modelo con la información de la postulación del caso de éxito.</param>
        /// <returns>El resultado de acción create (post).</returns>
        [HttpPost]
        public ActionResult Create(FormModel model)
        {
            SuccessStoryPostulateFacade successStoryPostulateFacade = new SuccessStoryPostulateFacade();

            int userId = ((CustomPrincipal)User).UserId;

            successStoryPostulateFacade.Save(new SuccessStoryPostulate
            {
                UserId = userId,
                CategoryId = model.CategoryId,
                ResponsibleNames = model.ResponsibleNames,
                ResponsibleEmail = model.ResponsibleEmail,
                ResponsibleJobTitle = model.ResponsibleJobTitle,
                ResponsibleOrganization = model.ResponsibleOrganization,
                CityId = model.CityId,
                ResponsibleEntities = model.ResponsibleEntities,
                Name = model.Name,
                CreationDate = DateTime.Now,
                Description = model.Description,
                ConcreteProblems = model.ConcreteProblems,
                InnovativeUrbanSolution = model.InnovativeUrbanSolution,
                IdsTag = string.Join("|", model.IdsTag),
                Documents = model.Documents,
                State = (byte)SuccessStoryPostulateStateEnum.New,
                LanguageId = (int)CurrentLanguage.LanguageId
            });

            this.SendUserNotification(userId, model.Name);

            return RedirectToAction("Index", "Seccion");
        }

        /// <summary>
        /// Envia notificación en la plataforma y correo electrónico del usuario.
        /// </summary>
        /// <param name="userId">Id del usuario actual.</param>
        /// <param name="name">Nombre del proceso.</param>
        private void SendUserNotification(int userId, string name)
        {
            Business.Utilities.Notification.NewNotification(userId, Domain.Entities.Basic.EmailNotificationType.POSTULATESTORY, null, this.CustomUser.UserId, string.Empty, null, 0, name, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
            Business.Utilities.Notification.NewNotification(userId, null, Domain.Entities.Basic.SystemNotificationType.POSTULATESTORY, this.CustomUser.UserId, string.Empty, null, 0, name, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
        }
    }
}
