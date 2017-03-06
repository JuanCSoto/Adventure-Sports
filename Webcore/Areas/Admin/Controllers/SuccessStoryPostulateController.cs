// <copyright file="SuccessStoryPostulateController.cs" company="Intergrupo">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Business.Services;
    using Core.Facades;
    using Domain.Concrete;
    using Domain.Entities.Enums;
    using Domain.Entities.FrontEnd;
    using Domain.Entities.Generic;
    using Webcore.Areas.Admin.Models;

    /// <summary>
    /// Controller for SuccessStoryPostulateController module
    /// </summary>
    public class SuccessStoryPostulateController : AdminController
    {
        /// <summary>
        /// Obtiene las postulaciones
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="mod">Identificador del modulo</param>
        /// <param name="sectionId">Identificador de la seccion</param>
        /// <returns>Vista de las postulaciones</returns>
        public ActionResult List(int? page, int? mod, int? sectionId)
        {
            int total = 0;
            int pageSize = 10;
            int pageNumber = page ?? 1;
            SuccessStoryPostulateFacade postulates = new SuccessStoryPostulateFacade();
            ModulRepository modul = new ModulRepository(SessionCustom);
            List<SuccessStoryPostulatePaging> paging = postulates.GetPaging(pageNumber, pageSize, out total, this.CurrentLanguage.LanguageId.Value).ToList();
            modul.Entity.ModulId = mod;
            modul.Entity.LanguageId = this.CurrentLanguage.LanguageId;
            modul.Load();
            decimal nropagesdec = decimal.Parse(total.ToString()) / decimal.Parse(pageSize.ToString());
            decimal nropagesint = (int)(total / pageSize);
            this.ViewBag.CurrentPage = pageNumber;
            this.ViewBag.TotalRows = total;
            this.ViewBag.SizePage = pageSize;
            this.ViewBag.NroPages = nropagesdec > nropagesint ? nropagesint + 1 : nropagesint;
            this.ViewBag.SectionId = sectionId;

            return this.View(
                "List",
                new SuccessStoryPostulateModel()
                {
                    UserPrincipal = this.CustomUser,
                    ListaSuccessStoryPostulate = paging,
                    ColModul = CustomMemberShipProvider.GetModuls(this.CustomUser.UserId, this.SessionCustom, HttpContext),

                    Module = modul.Entity,
                    CurrentLanguage = this.CurrentLanguage,
                });
        }

        /// <summary>
        /// Ejecucata la acción detail (get).
        /// </summary>
        /// <param name="id">Id de la postulación del caso de éxito.</param>
        /// <param name="mod">Modulo de la aplicación.</param>
        /// <param name="sectionId">Identificador de la seccion</param>
        /// <returns>El resultado de acción detail (get).</returns>
        [HttpGet]
        public ActionResult Detail(int id, int? mod, int? sectionId)
        {
            SuccessStoryPostulateFacade successStoryPostulateFacade = new SuccessStoryPostulateFacade();
            TagFacade tagFacade = new TagFacade();
            ModulRepository modul = new ModulRepository(SessionCustom);
            modul.Entity.ModulId = mod;
            modul.Entity.LanguageId = this.CurrentLanguage.LanguageId;
            modul.Load();
            this.ViewBag.SectionId = sectionId;

            SuccessStoryPostulate successStoryPostulate = successStoryPostulateFacade.GetById(id, (int)this.CurrentLanguage.LanguageId);
            successStoryPostulate.State = (byte)SuccessStoryPostulateStateEnum.Pending;
            successStoryPostulateFacade.Update(successStoryPostulate);

            return this.View(new DetailModel
            {
                SuccessStoryPostulate = successStoryPostulate,
                TagsText = string.Join(", ", tagFacade.GetBySuccessStoryPostulate(id).Select(t => t.Name)),
                UserPrincipal = this.CustomUser,
                ColModul = CustomMemberShipProvider.GetModuls(this.CustomUser.UserId, this.SessionCustom, HttpContext),
                Module = modul.Entity,
                CurrentLanguage = this.CurrentLanguage,
            });
        }

        /// <summary>
        /// Ejecucata la acción Reject (get).
        /// </summary>
        /// <param name="id">Id de la postulación del caso de éxito.</param>
        /// <param name="mod">Modulo de la aplicación.</param>
        /// <param name="sectionId">Identificador de la seccion</param>
        /// <returns>El resultado de acción detail (get).</returns>
        [HttpGet]
        public ActionResult Reject(int id, int? mod, int? sectionId)
        {
            SuccessStoryPostulateFacade successStoryPostulateFacade = new SuccessStoryPostulateFacade();
            SuccessStoryPostulate successStoryPostulate = successStoryPostulateFacade.GetById(id, (int)this.CurrentLanguage.LanguageId);
            successStoryPostulate.State = (byte)SuccessStoryPostulateStateEnum.Rejected;
            successStoryPostulateFacade.Update(successStoryPostulate);
            this.SendUserNotification(successStoryPostulate.UserId, successStoryPostulate.Name);
            return this.RedirectToAction("List", new { mod = mod, sectionId = sectionId });
        }

        /// <summary>
        /// Envia notificación en la plataforma y correo electrónico del usuario.
        /// </summary>
        /// <param name="userId">Id del usuario actual.</param>
        /// <param name="name">Nombre del proceso.</param>
        private void SendUserNotification(int userId, string name)
        {
            Business.Utilities.Notification.NewNotification(userId, Domain.Entities.Basic.EmailNotificationType.REJECTEDSTORY, null, userId, string.Empty, null, 0, name, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
            Business.Utilities.Notification.NewNotification(userId, null, Domain.Entities.Basic.SystemNotificationType.REJECTEDSTORY, userId, string.Empty, null, 0, name, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
        }
    }
}