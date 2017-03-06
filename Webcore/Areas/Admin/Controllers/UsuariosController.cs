// <copyright file="UsuariosController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;
    using Webcore.Areas.Admin.Models;
    
    /// <summary>
    /// Controller for user module
    /// </summary>
    [ModulAuthorize]
    public class UsuariosController : AdminController
    {
        /// <summary>
        /// gets the home of user module
        /// </summary>
        /// <param name="page">index page</param>
        /// <param name="name">criteria search</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Index(int? page, string name)
        {
            UserRepository objUser = new UserRepository(SessionCustom);
            PaginInfo paginInfo = new PaginInfo() { PageIndex = page != null ? page.Value : 1 };

            return this.View(new Usuarios()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                CollUsers = objUser.GetAllPaging(name, paginInfo),
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                Pagininfo = paginInfo,
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// obtains the user detail
        /// </summary>
        /// <param name="id">identifier of user</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(int? id)
        {
            UserRepository objUser = new UserRepository(SessionCustom);
            RolRepository objrol = new RolRepository(SessionCustom);
            RolUserRepository objuserrol = new RolUserRepository(SessionCustom);

            if (id != null)
            {
                objuserrol.Entity.UserId = objUser.Entity.UserId = id;
                objUser.Load();
                ViewBag.id = id;
            }

            return this.View(new Usuarios()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CollRols = objrol.GetAll(),
                UserCustom = objUser.Entity,
                CollUserrol = id != null ? objuserrol.GetAllReadOnly() : null,
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// delete a user
        /// </summary>
        /// <param name="id">identifier of user</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Delete(int id)
        {
            CustomMemberShipProvider objCustom = new CustomMemberShipProvider(SessionCustom, HttpContext);
            objCustom.DeleteUser(id);

            return this.RedirectToAction("Index", "Usuarios");
        }

        /// <summary>
        /// inserts or updates a item user
        /// </summary>
        /// <param name="id">identifier of user</param>
        /// <param name="model">information user</param>
        /// <param name="userimage">represents the user image</param>
        /// <param name="colrols">represents the identifiers roles</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Detail(int? id, Usuarios model, HttpPostedFileBase userimage, List<int> colrols)
        {
            CustomMemberShipProvider objCustom = new CustomMemberShipProvider(SessionCustom, HttpContext);
            RolUserRepository objuserrol = new RolUserRepository(SessionCustom);
            model.UserCustom.LanguageId = 2;

            if (model.UserCustom.Password != null)
            {
                model.UserCustom.Password = Utils.EncriptSHA1(model.UserCustom.Password);
            }

            if (userimage != null && userimage.ContentLength > 0)
            {
                string userImage = Utils.UploadFile(
                    userimage,
                    HttpContext.Server.MapPath("~"), 
                    @"resources\imagesuser\", 
                    null);

                ImageResize objresize = new ImageResize(HttpContext.Server.MapPath("~"));
                objresize.Width = 100;
                objresize.Height = 95;
                objresize.Prefix = "_";
                objresize.Resize(@"resources\imagesuser\" + userImage, ImageResize.TypeResize.BackgroundProportional);

                System.IO.File.Delete(System.IO.Path.Combine(HttpContext.Server.MapPath("~"), @"resources\imagesuser\" + userImage));
                model.UserCustom.Image = "_" + userImage;
            }

            if (id != null)
            {
                objuserrol.Entity.UserId = model.UserCustom.UserId = id;
                objCustom.ChangeData(model.UserCustom);
                objuserrol.Delete();
            }
            else
            {
                model.UserCustom.Joindate = DateTime.Now;
                objuserrol.Entity.UserId = objCustom.CreateUser(model.UserCustom);
            }

            if (colrols != null)
            {
                foreach (int item in colrols)
                {
                    objuserrol.Entity.RolId = item;
                    objuserrol.Insert();
                }
            }

            return this.RedirectToAction("Index", "Usuarios");
        }
    }
}
