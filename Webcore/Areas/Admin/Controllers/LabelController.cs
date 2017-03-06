// <copyright file="LabelController.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Juan Carlos Montoya</author>

namespace Webcore.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;
    using Webcore.Areas.Admin.Models;

    /// <summary>
    /// Controller for Label module
    /// </summary>
    ///[ModulAuthorize]
    public class LabelController : AdminController
    {
        //
        // GET: /Admin/Label/

        public ActionResult Index(int? page, string name, string translation)
        {
            
           LabelRepository objlab = new LabelRepository(SessionCustom);
            PaginInfo paginInfo = new PaginInfo() { PageIndex = page != null ? page.Value : 1 };
           
            return this.View(new Labels()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                CollLabels = objlab.GetAllPaging(name,translation, paginInfo, CurrentLanguage.LanguageId.Value),
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                Pagininfo = paginInfo,
                CurrentLanguage = CurrentLanguage
                
            });
        }

      

        /// <summary>
        /// obtains the tag detail
        /// </summary>
        /// <param name="id">identifier of tag</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(int? id)
        {
            LabelRepository objlab = new LabelRepository(SessionCustom);
            Domain.Entities.Label lab = null;

            if (id != null)
            {
                objlab.Entity.LabelId = id;
                objlab.Load();
                lab = objlab.Entity;
                ViewBag.id = id;
            }

            return this.View(new Labels()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                LabelCustom = lab,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// delete tag item
        /// </summary>
        /// <param name="id">identifier of tag</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Delete(int id)
        {
            LabelRepository objtag = new LabelRepository(SessionCustom);
            objtag.Entity.LabelId = id;
            objtag.Delete();

            this.InsertAudit("Delete", this.Module.Name + " -> " + id);

            return this.RedirectToAction("Index", "Label");
        }

        /// <summary>
        /// inserts or updates a item tag
        /// </summary>
        /// <param name="id">identifier of tag</param>
        /// <param name="model">information to tag</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Detail(int? id, Labels model)
        {
            LabelRepository objlab = new LabelRepository(SessionCustom);
            objlab.Entity = model.LabelCustom;
            SetLabel();
            if (id != null)
            {
                objlab.Entity.LabelId = id;
                objlab.Update();
                this.InsertAudit("Update", this.Module.Name + " -> " + objlab.Entity.Name);
            }
            else
            {
                objlab.Entity.LanguageId = CurrentLanguage.LanguageId;
                objlab.Insert();
                this.InsertAudit("Insert", this.Module.Name + " -> " + objlab.Entity.Name);
            }

            return this.RedirectToAction("Index", "Label");
        }

        private void SetLabel()
        {
            LabelManagement objlabel = new LabelManagement(SessionCustom, HttpContext);
            ViewBag.DELETE_ITEM = objlabel.GetLabelByName("DELETE_ITEM", CurrentLanguage.LanguageId.Value);
            ViewBag.DETAIL = objlabel.GetLabelByName("DETAIL", CurrentLanguage.LanguageId.Value);

        }


        /// <summary>
        /// obtains a list of tags
        /// </summary>
        /// <returns>returns the result to action</returns>
        public JsonResult GetTags()
        {
            LabelRepository objtag = new LabelRepository(SessionCustom);
            objtag.Entity.LanguageId = CurrentLanguage.LanguageId;
            return this.Json(objtag.GetLabels(), JsonRequestBehavior.AllowGet);
        }

    }
}
