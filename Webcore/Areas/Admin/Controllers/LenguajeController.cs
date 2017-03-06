// <copyright file="LenguajeController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using Webcore.Areas.Admin.Models;
    
    /// <summary>
    /// Controller for language module
    /// </summary>
    [ModulAuthorize]
    public class LenguajeController : AdminController
    {
        /// <summary>
        /// gets the home of language module
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            LanguageRepository objlang = new LanguageRepository(SessionCustom);

            return this.View(new Lenguaje()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                CollLanguage = objlang.GetAll(),
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// obtains the language detail
        /// </summary>
        /// <param name="id">identifier of language</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(int? id)
        {
            LanguageRepository objlang = new LanguageRepository(SessionCustom);
            Domain.Entities.Language objlanguage = null;

            if (id != null)
            {
                objlang.Entity.LanguageId = id;
                objlang.Load();
                objlanguage = objlang.Entity;
                ViewBag.id = id;
            }

            return this.View(new Lenguaje()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                EntityLanguage = objlanguage,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CollLanguage = objlang.GetAllReadOnly(),
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// inserts or updates a item language
        /// </summary>
        /// <param name="id">identifier of language</param>
        /// <param name="model">language model</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Detail(int? id, Lenguaje model)
        {
            LanguageRepository objlang = new LanguageRepository(SessionCustom);

            objlang.Entity = model.EntityLanguage;

            try
            {
                SessionCustom.Begin();
                if (objlang.Entity.IsDefault.Value)
                {
                    objlang.UpdateLanguage();
                }
                
                if (id != null)
                {
                    objlang.Entity.LanguageId = id;
                    objlang.Update();

                    this.InsertAudit("Update", this.Module.Name + " -> " + objlang.Entity.Name);
                }
                else
                {
                    objlang.Entity.IsDefault = false;
                    objlang.Insert();
                    this.InsertAudit("Insert", this.Module.Name + " -> " + objlang.Entity.Name);
                }

                this.SessionCustom.Commit();
            }
            catch (Exception ex)
            {
                this.SessionCustom.RollBack();
                Utils.InsertLog(this.SessionCustom, "Error " + this.Module.Name, ex.ToString());
            }

            HttpContext.Cache.Remove("languages");

            return this.RedirectToAction("Index", "Lenguaje");
        }
    }
}
