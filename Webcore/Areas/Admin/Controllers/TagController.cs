// <copyright file="TagController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;
    using Webcore.Areas.Admin.Models;
    
    /// <summary>
    /// Controller for tag module
    /// </summary>
    [ModulAuthorize]
    public class TagController : AdminController
    {
        /// <summary>
        /// gets the home of tag module
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="name">criteria search</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Index(int? page, string name)
        {
            TagRepository objtag = new TagRepository(SessionCustom);
            PaginInfo paginInfo = new PaginInfo() { PageIndex = page != null ? page.Value : 1 };

            return this.View(new Tags()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                CollTags = objtag.GetAllPaging(name, paginInfo, CurrentLanguage.LanguageId.Value),
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
            TagRepository objtag = new TagRepository(SessionCustom);
            Domain.Entities.Tag tag = null;

            if (id != null)
            {
                objtag.Entity.TagId = id;
                objtag.Load();
                tag = objtag.Entity;
                ViewBag.id = id;
            }

            return this.View(new Tags()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                TagCustom = tag,
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
            TagRepository objtag = new TagRepository(SessionCustom);
            objtag.Entity.TagId = id;
            objtag.Delete();

            this.InsertAudit("Delete", this.Module.Name + " -> " + id);

            return this.RedirectToAction("Index", "Tag");
        }

        /// <summary>
        /// inserts or updates a item tag
        /// </summary>
        /// <param name="id">identifier of tag</param>
        /// <param name="model">information to tag</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Detail(int? id, Tags model)
        {
            TagRepository objtag = new TagRepository(SessionCustom);
            objtag.Entity = model.TagCustom;

            if (id != null)
            {
                objtag.Entity.TagId = id;
                objtag.Entity.LanguageId = this.CurrentLanguage.LanguageId;
                objtag.Update();
                this.InsertAudit("Update", this.Module.Name + " -> " + objtag.Entity.Name);
            }
            else
            {
                objtag.Entity.LanguageId = CurrentLanguage.LanguageId;
                objtag.Insert();
                this.InsertAudit("Insert", this.Module.Name + " -> " + objtag.Entity.Name);
            }

            return this.RedirectToAction("Index", "Tag");
        }

        /// <summary>
        /// obtains a list of tags
        /// </summary>
        /// <returns>returns the result to action</returns>
        public JsonResult GetTags()
        {
            TagRepository objtag = new TagRepository(SessionCustom);
            objtag.Entity.LanguageId = CurrentLanguage.LanguageId;
            return this.Json(objtag.GetTags(), JsonRequestBehavior.AllowGet);
        }
    }
}
