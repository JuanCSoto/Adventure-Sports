// <copyright file="RolesController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Business.Services;
    using Domain.Concrete;
    using Webcore.Areas.Admin.Models;

    /// <summary>
    /// Controller for roles module
    /// </summary>
    [ModulAuthorize]
    public class RolesController : AdminController
    {
        /// <summary>
        /// gets the home of roles module
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            RolRepository objrol = new RolRepository(SessionCustom);

            return this.View(new Roles()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                CollRols = objrol.GetAll(),
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// obtains the role detail
        /// </summary>
        /// <param name="id">identifier of role</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(int? id)
        {
            RolRepository objrol = new RolRepository(SessionCustom);
            ModulRepository objmod = new ModulRepository(SessionCustom);
            RolmodulRepository objrolmodul = new RolmodulRepository(SessionCustom);
            objmod.Entity.LanguageId = CurrentLanguage.LanguageId.Value;
            Domain.Entities.Rol rol = null;

            if (id != null)
            {
                objrolmodul.Entity.RolId = objrol.Entity.RolId = id;
                objrol.Load();
                rol = objrol.Entity;
                ViewBag.id = id;
            }

            return this.View(new Roles()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                RolCustom = rol,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CollModuls = objmod.GetAllReadOnly(),
                CollRolmodul = id != null ? objrolmodul.GetAllReadOnly() : null,
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// delete role
        /// </summary>
        /// <param name="id">identifier of role</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Delete(int id)
        {
            RolRepository objrol = new RolRepository(SessionCustom);
            objrol.Entity.RolId = id;
            objrol.Delete();

            this.InsertAudit("Delete", this.Module.Name + " -> " + id);

            return this.RedirectToAction("Index", "Roles");
        }

        /// <summary>
        /// inserts or updates a item role
        /// </summary>
        /// <param name="id">identifier of role</param>
        /// <param name="model">information to role</param>
        /// <param name="idsModul">list of identifiers modules</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Detail(int? id, Roles model, List<int> idsModul)
        {
            RolRepository objrol = new RolRepository(SessionCustom);
            RolmodulRepository objrolmodul = new RolmodulRepository(SessionCustom);

            objrol.Entity = model.RolCustom;

            if (id != null)
            {
                objrolmodul.Entity.RolId = objrol.Entity.RolId = id;
                objrol.Update();
                objrolmodul.Delete();
                this.InsertAudit("Update", this.Module.Name + " -> " + objrol.Entity.Name);
            }
            else
            {
                objrolmodul.Entity.RolId = Convert.ToInt32(objrol.Insert());
                this.InsertAudit("Insert", this.Module.Name + " -> " + objrol.Entity.Name);
            }

            if (idsModul != null)
            {
                foreach (int item in idsModul)
                {
                    objrolmodul.Entity.ModulId = item;
                    objrolmodul.Insert();
                }
            }

            return this.RedirectToAction("Index", "Roles");
        }
    }
}
