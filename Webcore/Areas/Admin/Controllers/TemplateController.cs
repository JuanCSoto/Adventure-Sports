// <copyright file="TemplateController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using Business.Services;
    using Domain.Concrete;
    using Webcore.Areas.Admin.Models;
    
    /// <summary>
    /// Controller for Template module
    /// </summary>
    [ModulAuthorize]
    public class TemplateController : AdminController
    {
        /// <summary>
        /// gets the home of Template module
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            TemplateRepository objtemplate = new TemplateRepository(SessionCustom);

            return this.View(new Templates()
            {
                UserPrincipal = CustomUser,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CollTemplate = objtemplate.GetAll(),
                Module = this.Module,
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// obtains the template detail
        /// </summary>
        /// <param name="key">criteria search</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(string key)
        {
            TemplateRepository objtemplate = new TemplateRepository(SessionCustom);
            Domain.Entities.Template template = null;

            if (key != null)
            {
                objtemplate.Entity.TemplateId = key;
                objtemplate.Load();
                template = objtemplate.Entity;
                ViewBag.id = key;
            }

            return this.View(new Templates()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                TemplateCustom = template,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// delete a template
        /// </summary>
        /// <param name="id">identifier of template</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Delete(string id)
        {
            TemplateRepository objtemplate = new TemplateRepository(SessionCustom);
            objtemplate.Entity.TemplateId = id;
            objtemplate.Delete();

            this.InsertAudit("Delete", this.Module.Name + " -> " + id);

            return this.RedirectToAction("Index", "Template");
        }

        /// <summary>
        /// inserts or updates a template
        /// </summary>
        /// <param name="id">identifier of template</param>
        /// <param name="model">information of template</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Detail(string id, Templates model)
        {
            TemplateRepository objtemplate = new TemplateRepository(SessionCustom);
            objtemplate.Entity = model.TemplateCustom;

            if (!string.IsNullOrEmpty(id))
            {
                objtemplate.Entity.TemplateId = id;
                objtemplate.Update();
                this.InsertAudit("Update", this.Module.Name + " -> " + objtemplate.Entity.TemplateId);
            }
            else
            {
                objtemplate.Insert();
                this.InsertAudit("Insert", this.Module.Name + " -> " + objtemplate.Entity.TemplateId);
            }

            HttpContext.Cache.Remove("templates");

            return this.RedirectToAction("Index", "Template");
        }
    }
}
