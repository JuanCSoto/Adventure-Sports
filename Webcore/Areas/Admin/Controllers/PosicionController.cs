// <copyright file="PosicionController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using Business.Services;
    using Domain.Concrete;
    
    /// <summary>
    /// Controller for position module
    /// </summary>
    [ModulAuthorize]
    public class PosicionController : AdminController
    {
        /// <summary>
        /// gets the home of position module
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            PositionRepository objposition = new PositionRepository(SessionCustom);

            return this.View(new Models.Posiciones()
            {
                Module = this.Module,
                UserPrincipal = CustomUser,
                CollPositions = objposition.GetAll(),
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// obtains the position detail
        /// </summary>
        /// <param name="id">identifier of position</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(int? id)
        {
            PositionRepository objposition = new PositionRepository(SessionCustom);
            Domain.Entities.Position position = null;

            if (id != null)
            {
                objposition.Entity.PositionId = id;
                objposition.Load();
                position = objposition.Entity;
                ViewBag.id = id;
            }

            return this.View(new Models.Posiciones()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                Position = position,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// delete position item
        /// </summary>
        /// <param name="id">identifier of position</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Delete(int id)
        {
            PositionRepository objposition = new PositionRepository(SessionCustom);
            objposition.Entity.PositionId = id;
            objposition.Delete();

            this.InsertAudit("Delete", this.Module.Name + " -> " + id);

            return this.RedirectToAction("Index", "Posicion");
        }

        /// <summary>
        /// inserts or updates a item position
        /// </summary>
        /// <param name="id">identifier of position</param>
        /// <param name="model">information of position</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Detail(int? id, Models.Posiciones model)
        {
            PositionRepository objposition = new PositionRepository(SessionCustom);
            objposition.Entity = model.Position;

            if (id != null)
            {
                objposition.Entity.PositionId = id;
                objposition.Update();
                this.InsertAudit("Update", this.Module.Name + " -> " + objposition.Entity.Name);
            }
            else
            {
                objposition.Insert();
                this.InsertAudit("Insert", this.Module.Name + " -> " + objposition.Entity.Name);
            }

            return this.RedirectToAction("Index", "Posicion");
        }
    }
}
