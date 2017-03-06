// <copyright file="HomeController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Business.Services;
    using Domain.Concrete;
    using Webcore.Areas.Admin.Models;
    
    /// <summary>
    /// Controller for administrator home
    /// </summary>
    public class HomeController : AdminController
    {
        /// <summary>
        /// gets the home of administrator
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext).Count > 0)
                {
                    return this.RedirectToAction("Inicio", "Home", null);
                }
            }

            return this.View();
        }

        /// <summary>
        /// check user data to grant access
        /// </summary>
        /// <param name="objLogin">login model</param>
        /// <param name="returnUrl"><c>URL</c> to returns</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Index(Login objLogin, string returnUrl)
        {
            CustomMemberShipProvider objmember = new CustomMemberShipProvider(SessionCustom, HttpContext);

            try
            {
                CustomMemberShipProvider.AuthencReturn result = objmember.ValidateUser(HttpContext, objLogin.Email, objLogin.Password, null);
                string message = null;

                switch (result)
                {
                    case CustomMemberShipProvider.AuthencReturn.USER_OK:
                        if (returnUrl == null)
                        {
                            return this.RedirectToAction("Inicio", "Home", null);
                        }
                        else
                        {
                            return this.Redirect(returnUrl);
                        }

                    case CustomMemberShipProvider.AuthencReturn.NOT_FOUND:
                        message = Resources.Extend.Messages.USER_NOT_FOUND;
                        break;
                    case CustomMemberShipProvider.AuthencReturn.BAD_PASSWORD:
                        message = Resources.Extend.Messages.USER_BAD_PASSWORD;
                        break;
                    default:
                        message = Resources.Extend.Messages.SYSTEM_ERROR;
                        break;
                }

                ModelState.AddModelError("UserNotFound", message);
                return this.View(objLogin);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Applicationerror", Resources.Extend.Messages.FAILED_INFORMATION);
                return this.View(objLogin);
            }
        }

        /// <summary>
        /// close the active session
        /// </summary>
        /// <returns>returns the result to action</returns>
        [Authorize]
        public ActionResult Closesession()
        {
            FormsAuthentication.SignOut();
            HttpContext.Session.RemoveAll();
            HttpContext.Session.Abandon();
            return this.RedirectToAction("Index", "Home", null);
        }

        /// <summary>
        /// Administrator gets the start
        /// </summary>
        /// <returns>returns the result to action</returns>
        [Authorize]
        public ActionResult Inicio()
        {
            AuditRepository objaudit = new AuditRepository(SessionCustom);
            objaudit.Entity.Username = CustomUser.UserId;
            Module.Name = "CMS";
           
            return this.View(new Principal()
            {
                Module = this.Module,
                UserPrincipal = CustomUser,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CollRols = CustomMemberShipProvider.GetRols(SessionCustom, HttpContext),
                CollAudit = objaudit.GetAutidtop(),
                CurrentLanguage = CurrentLanguage
            });
        }

    
        /// <summary>
        /// gets the search page
        /// </summary>
        /// <param name="search">criteria search</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        [Authorize]
        public ActionResult Busqueda(string search)
        {
            ContentRepository objcontent = new ContentRepository(SessionCustom);
            Module.Name = "Busqueda";

            return this.View(new Modelsearch()
            {
                Module = this.Module,
                UserPrincipal = CustomUser,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CollResult = objcontent.GetSearchGeneral(search, CurrentLanguage.LanguageId.Value),
                Criteria = search,
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// obtains a list language
        /// </summary>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        [Authorize]
        public JsonResult GetLanguages()
        {
            LanguageRepository languagerepo = new LanguageRepository(SessionCustom);
            return this.Json(languagerepo.GetAll());
        }

        /// <summary>
        /// sets the language default
        /// </summary>
        /// <param name="id">identifier of language</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        [Authorize]
        public JsonResult SetLanguage(int id)
        {
            try
            {
                UserRepository userrepo = new UserRepository(SessionCustom);
                LanguageRepository languagerepo = new LanguageRepository(SessionCustom);

                userrepo.Entity.UserId = CustomUser.UserId;
                userrepo.Entity.LanguageId = id;
                userrepo.Update();

                languagerepo.Entity.LanguageId = id;
                languagerepo.LoadByKey();

                if (HttpContext.Session["lang"] == null)
                {
                    HttpContext.Session.Add("lang", languagerepo.Entity);
                }
                else
                {
                    HttpContext.Session["lang"] = languagerepo.Entity;
                }

                return this.Json(new { result = true });
            }
            catch (Exception)
            {
                return this.Json(new { result = false });
            }
        }
    }
}
