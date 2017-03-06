// <copyright file="BannerController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;
    using Webcore.Areas.Admin.Models;

    /// <summary>
    /// Controller for banner module
    /// </summary>
    [ModulAuthorize]
    public class BannerController : AdminController
    {
        /// <summary>
        /// gets the home of banner module
        /// </summary>
        /// <param name="page">index page</param>
        /// <param name="name">criteria search</param>
        /// <param name="lang">culture name</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Index(int? page, string name, string lang)
        {
            BannerRepository objbanner = new BannerRepository(SessionCustom);
            PaginInfo paginInfo = new PaginInfo() { PageIndex = page != null ? page.Value : 1 };

            return this.View(new Banners()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CollBanners = objbanner.GetAllPaging(name, paginInfo, CurrentLanguage.LanguageId.Value),
                Pagininfo = paginInfo,
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// gets the detail of a banner
        /// </summary>
        /// <param name="id">identifier of banner</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(int? id)
        {
            BannerRepository objbanner = new BannerRepository(SessionCustom);
            SectionManagement objsection = new SectionManagement(SessionCustom, HttpContext);
            SectionRepository sectionrepository = new SectionRepository(SessionCustom);
            PositionRepository objposition = new PositionRepository(SessionCustom);
                        
            Domain.Entities.Banner banner = null;
            bool? isHome = null;
            if (id != null)
            {
                objbanner.Entity.BannerId = id;
                objbanner.Load();
                banner = objbanner.Entity;
                ViewBag.id = id;

                BannersectionRepository objbannersection = new BannersectionRepository(SessionCustom);
                objbannersection.Entity.BannerId = id;
               
                List<Bannersection> collsections = objbannersection.GetAll();
                objsection.CreateTreeViewCheck(sectionrepository.GetAll().FindAll(t => t.LanguageId == CurrentLanguage.LanguageId), collsections);
                isHome = collsections.Exists(t => t.SectionId == 0);
            }
            else
            {
                objsection.CreateTreeViewCheck(sectionrepository.GetAll().FindAll(t => t.LanguageId == CurrentLanguage.LanguageId), null);
            }

            return this.View(new Banners()
            {
                UserPrincipal = CustomUser,
                Banner = banner,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                Module = this.Module,
                TreeView = objsection.Tree,
                Collposition = objposition.GetAll(),
                IsHome = isHome,
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// delete a banner
        /// </summary>
        /// <param name="id">identifier of banner</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Delete(int id)
        {
            BannerRepository objbanner = new BannerRepository(SessionCustom);
            objbanner.Entity.BannerId = id;
            objbanner.LoadByKey();

            if (objbanner.Entity.Bannertype != 2)
            {
                string path = Path.Combine(Server.MapPath("~"), @"resources\banners\" + objbanner.Entity.Bannerfile);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            objbanner.Delete();

            this.InsertAudit("Delete", this.Module.Name + " -> " + id);

            return this.RedirectToAction("Index", "Banner");
        }

        /// <summary>
        /// inserts or updates a banner
        /// </summary>
        /// <param name="id">identifier of banner</param>
        /// <param name="model">model to upload</param>
        /// <param name="bannerFile">upload user file</param>
        /// <param name="sectionsid">sections id</param>
        /// <param name="bannerhtml">html string to banner</param>
        /// <param name="type">type of banner</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Detail(int? id, Banners model, HttpPostedFileBase bannerFile, string sectionsid, string bannerhtml, short type)
        {
            BannerRepository objbanner = new BannerRepository(SessionCustom);
            BannersectionRepository objsections = new BannersectionRepository(SessionCustom);
            objbanner.Entity = model.Banner;
            objbanner.Entity.LanguageId = CurrentLanguage.LanguageId;
            int bannerId = 0;

            if (type != 2)
            {
                if (bannerFile != null && bannerFile.ContentLength > 0)
                {
                    string extension = Path.GetExtension(bannerFile.FileName).ToLower();
                    objbanner.Entity.Bannertype = extension == ".swf" ? short.Parse("0") : short.Parse("1");
                    objbanner.Entity.Bannerfile = Utils.UploadFile(bannerFile, Server.MapPath("~"), @"Resources\Banners\", null);
                }
            }
            else
            {
                objbanner.Entity.Bannertype = short.Parse("2");
                objbanner.Entity.Bannerfile = bannerhtml;
            }

            if (id != null)
            {
                objbanner.Entity.BannerId = id;
                bannerId = id.Value;
                objbanner.Update();
                this.InsertAudit("Update", this.Module.Name + " -> " + objbanner.Entity.Name);
            }
            else
            {
                objbanner.Entity.Bannerdate = DateTime.Now;
                bannerId = Convert.ToInt32(objbanner.Insert());
                this.InsertAudit("Insert", this.Module.Name + " -> " + objbanner.Entity.Name);
            }

            objsections.Entity.BannerId = bannerId;
            string[] arrid = sectionsid.Split(',');
            List<Bannersection> collbann = objsections.GetAll();
                        
            foreach (string item in arrid)
            {
                int sectionId = int.Parse(item);
                if (!collbann.Exists(t => t.SectionId == sectionId))
                {
                    objsections.Entity.SectionId = sectionId;
                    objsections.Insert();
                }
                else
                {
                    collbann.RemoveAll(t => t.SectionId == sectionId);
                }
            }

            foreach (Bannersection item in collbann)
            {
                objsections.Entity = item;
                objsections.Delete();
            }

            return this.RedirectToAction("Index", "Banner");
        }

        /// <summary>
        /// upload file from html editor
        /// </summary>
        /// <param name="imageName">upload user file</param>
        [HttpPost]
        public void UploadImage(HttpPostedFileBase imageName)
        {
            if (imageName.ContentLength > 0)
            {
                string strfile = DateTime.Now.ToString("ddmmyyyyhhmmss") + Path.GetExtension(imageName.FileName);
                imageName.SaveAs(Path.Combine(Server.MapPath("~"), @"Resources/Banners/" + strfile));
                Response.Write("<div id='image'>" + VirtualPathUtility.ToAbsolute("~/Resources/Banners/" + strfile) + "</div>");
            }
        }
    }
}
