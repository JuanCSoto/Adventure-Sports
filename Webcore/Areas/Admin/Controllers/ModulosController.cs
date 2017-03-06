// <copyright file="ModulosController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System;
    using System.Web.Mvc;
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using Webcore.Areas.Admin.Models;
    
    /// <summary>
    /// Controller for modules module
    /// </summary>
    [ModulAuthorize]
    public class ModulosController : AdminController
    {
        /// <summary>
        /// gets the home of modules module
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            ModulRepository objmod = new ModulRepository(SessionCustom);
            objmod.Entity.LanguageId = CurrentLanguage.LanguageId;
            return this.View(new Modulos()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                CollModuls = objmod.GetAll(),
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// obtains a detail of modules module
        /// </summary>
        /// <param name="id">identifier of module</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(int? id)
        {
            ModulRepository objmod = new ModulRepository(SessionCustom);
            Domain.Entities.Modul objmodul = null;
            objmod.Entity.LanguageId = CurrentLanguage.LanguageId;
            
            if (id != null)
            {
                objmod.Entity.ModulId = id;
                objmod.Load();
                objmodul = objmod.Entity;
                objmod.Entity = new Domain.Entities.Modul() { LanguageId = CurrentLanguage.LanguageId };
                ViewBag.id = id;
            }

            return this.View(new Modulos()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                Modul = objmodul,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CollModuls = objmod.GetAllReadOnly(),
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// deletes a module
        /// </summary>
        /// <param name="id">identifier of module</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Delete(int id)
        {
            ModulRepository objmod = new ModulRepository(SessionCustom);
            objmod.Entity.ModulId = id;
            objmod.Delete();

            this.InsertAudit("Delete", this.Module.Name + " -> " + id);

            return this.RedirectToAction("Index", "Modulos");
        }

        /// <summary>
        /// inserts or updates a item module
        /// </summary>
        /// <param name="id">identifier of module</param>
        /// <param name="model">information of module</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Detail(int? id, Modulos model)
        {
            ModulRepository objmod = new ModulRepository(SessionCustom);
            ModullanguageRepository modullang = new ModullanguageRepository(SessionCustom);
            
            objmod.Entity = model.Modul;

            try
            {
                SessionCustom.Begin();
                if (id != null)
                {
                    objmod.Entity.ModulId = id;
                    objmod.Update();

                    modullang.Entity.ModulId = id;
                    modullang.Entity.LanguageId = CurrentLanguage.LanguageId;
                    modullang.Load();

                    if (modullang.Entity.Name != null)
                    {
                        modullang.Entity.Name = model.Modul.Name;
                        modullang.Update();
                    }
                    else
                    {
                        modullang.Entity.Name = model.Modul.Name;
                        modullang.Insert();
                    }

                    this.InsertAudit("Update", this.Module.Name + " -> " + objmod.Entity.Name);
                }
                else
                {
                    objmod.Entity.IsBasic = false;
                    int modulId = Convert.ToInt32(objmod.Insert());
                    modullang.Entity.ModulId = modulId;
                    modullang.Entity.LanguageId = CurrentLanguage.LanguageId;
                    modullang.Entity.Name = model.Modul.Name;
                    modullang.Insert();
                    this.InsertAudit("Insert", this.Module.Name + " -> " + objmod.Entity.Name);
                }

                this.SessionCustom.Commit();
            }
            catch (Exception ex)
            {
                this.SessionCustom.RollBack();
                Utils.InsertLog(this.SessionCustom, "Error " + this.Module.Name, ex.ToString());
            }

            return this.RedirectToAction("Index", "Modulos");
        }
    }
}
