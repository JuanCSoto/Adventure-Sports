// <copyright file="GestorftpController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Business;
    using Business.Services;
    using Webcore.Areas.Admin.Models;
    
    /// <summary>
    /// Controller for <c>FTP</c> module
    /// </summary>
    public class GestorftpController : AdminController
    {
        /// <summary>
        /// gets the home of <c>FTP</c> module
        /// </summary>
        /// <returns>returns the result to action</returns>
        [ModulAuthorize]
        public ActionResult Index()
        {
            DirectoryInfo objdir = new DirectoryInfo(Server.MapPath("~"));
            Dictionary<string, string> coldic = new Dictionary<string, string>();
            DirectoryInfo[] arrdir = objdir.GetDirectories();
            
            foreach (DirectoryInfo item in arrdir)
            {
                coldic.Add(item.Name, item.FullName);
            }

            return this.View(new Gestorftp()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CollDirectory = coldic,
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// obtains a list of files according to directory path
        /// </summary>
        /// <param name="path">directory path</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        [Authorize]
        public ActionResult GetFiles(string path)
        {
            DirectoryInfo objdir = new DirectoryInfo(path);
            return this.View("_ListFiles", objdir.GetFiles());
        }

        /// <summary>
        /// obtains a list of directories according to directory path
        /// </summary>
        /// <param name="path">directory path</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        [Authorize]
        public JsonResult GetDirectory(string path)
        {
            DirectoryInfo objdir = new DirectoryInfo(path);
            DirectoryInfo[] colldir = objdir.GetDirectories();
            UrlHelper url = new UrlHelper(Request.RequestContext);

            if (colldir.Length > 0)
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("<ul style=\"display:none;\">");

                foreach (DirectoryInfo item in colldir)
                {
                    strb.Append("<li><div onclick=\"bindfiles(this, '" + item.FullName.Replace(@"\", "\\\\") + "')\">");
                    strb.Append("<img onclick=\"expandftp(this, '" + item.FullName.Replace(@"\", "\\\\") + "')\" height=\"15px\" width=\"15px\" src=\"" + url.Content("~/resources/images/15add.gif") + "\" />");
                    strb.Append("<img src=\"" + url.Content("~/resources/images/icondir.gif") + "\" /><nobr>" + item.Name + "</nobr></div></li>");
                }

                strb.Append("</ul>");

                return this.Json(new
                { 
                    html = strb.ToString(), Iscontain = true 
                });
            }
            else
            {
                return this.Json(new
                { 
                    Iscontain = false 
                });
            }
        }

        /// <summary>
        /// upload a file to a server
        /// </summary>
        /// <param name="filePath">represents a upload file</param>
        /// <param name="pathroot">save path</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Uploadfile(HttpPostedFileBase filePath, string pathroot)
        {
            try
            {
                if (filePath != null && filePath.ContentLength > 0 && pathroot != null)
                {
                    string path = Path.Combine(Server.MapPath("~"), pathroot);
                    string newpath = Path.Combine(path, Path.GetFileName(filePath.FileName));
                    if (Directory.Exists(path))
                    {
                        filePath.SaveAs(newpath);
                    }

                    this.InsertAudit("Load File", newpath.Replace(Server.MapPath("~"), string.Empty));
                }
            }
            catch (Exception ex)
            {
                Utils.InsertLog(
                    this.SessionCustom, 
                    "Load File", 
                    ex.Message + " - " + ex.StackTrace);
            }

            return this.RedirectToAction("Index", "Gestorftp");
        }

        /// <summary>
        /// obtains a information file
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public JsonResult Getinfofile(string path)
        {
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                return this.Json(new
                {
                    name = file.Name,
                    datejoin = file.CreationTime.ToShortDateString() + " " + file.CreationTime.ToShortTimeString(),
                    datelast = file.LastWriteTime.ToShortDateString() + " " + file.LastWriteTime.ToShortTimeString(),
                    size = (file.Length / 1024).ToString(),
                    result = true
                });
            }
            else
            {
                return this.Json(new
                {
                    result = false
                });                   
            }
        }

        /// <summary>
        /// delete file from server
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public JsonResult Deletefile(string path)
        {
            try
            {
                FileInfo file = new FileInfo(path);
                string full = file.FullName;
                file.Delete();

                this.InsertAudit("Delete", this.Module.Name + " -> " + full.Replace(Server.MapPath("~"), string.Empty));

                return this.Json(new
                {
                    result = true
                });
            }
            catch (Exception ex)
            {
                Utils.InsertLog(this.SessionCustom, "Delete " + this.Module.Name, ex.ToString());
                return this.Json(new
                {
                    result = false
                });
            }
        }
    }
}
