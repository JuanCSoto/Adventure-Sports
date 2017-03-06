// <copyright file="BlogController.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>
// <author>Modificacion de Intergrupo</author>
namespace Webcore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// controller for the blog action
    /// </summary>
    public class BlogController : FrontEndController
    {
        /// <summary>
        /// gets the blog page according to the parameter
        /// </summary>
        /// <param name="id">blog id</param>
        /// <returns>returns the result to action</returns>
        [HttpGet]
        public ActionResult Index(int id)
        {
            SetLabel();
            FrontEndManagement objman = new FrontEndManagement(SessionCustom, HttpContext, FrontEndManagement.Type.Content, CurrentLanguage);
            int? currentUserId = null;
            if (User.Identity.IsAuthenticated)
            {
                currentUserId = ((CustomPrincipal)User).UserId;
                objman.BindInfo(id, currentUserId);
                ViewBag.CurrentUserId = currentUserId;
            }
            else
            {
                objman.BindInfo(id, null);
            }

            IdeaRepository idea = new IdeaRepository(SessionCustom);

            BlogEntryRepository blog = new BlogEntryRepository(SessionCustom);
            blog.Entity.ContentId = id;
            blog.LoadByKey();

            ContentRepository content = new ContentRepository(SessionCustom);
            content.Entity.ContentId = blog.Entity.ContentId;
            content.LoadByKey();

            if (content.Entity.UserId.HasValue)
            {
                // blog owner
                ViewBag.IdeaOwner = content.Entity.UserId.Value == currentUserId;
            }

            int total = 0;
            CommentRepository comment = new CommentRepository(SessionCustom);
            List<Domain.Entities.FrontEnd.CommentsPaging> comments = comment.CommentsPagingContent(1, 6, out total, id);

            FileattachRepository fileRepository = new FileattachRepository(SessionCustom);
            fileRepository.Entity.ContentId = content.Entity.ContentId;
            Fileattach file = fileRepository.GetAll().FirstOrDefault(t => t.Type == Domain.Entities.Fileattach.TypeFile.Video);
            if (file != null)
            {
                content.Entity.Video = file.Filename;
            }

            return this.View(
                "IndexNivel3",
                new Models.FEBlogEntry()
            {
                UserPrincipal = CustomUser,
                Entity = blog.Entity,
                CollComments = comments,
                ObjContent = content.Entity,
                MetaTags = objman.Metatags,
                CurrentLanguage = CurrentLanguage,
                CommentCount = total,
                IdeasCountAll = idea.IdeasCountAll()
            });
        }

        /// <summary>
        /// gets the blog page according to the parameter
        /// </summary>
        /// <param name="id">blog id</param>
        /// <param name="layer">indicates whether the content will be open in a layer or not</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Index(int id, bool layer)
        {
            BlogEntryRepository blog = new BlogEntryRepository(SessionCustom);
            blog.Entity.ContentId = id;
            blog.LoadByKey();

            ContentRepository content = new ContentRepository(SessionCustom);
            content.Entity.ContentId = blog.Entity.ContentId;
            content.LoadByKey();

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserId = ((CustomPrincipal)User).UserId;
            }

            if (content.Entity.UserId.HasValue)
            {
                // blog owner
                ViewBag.IdeaOwner = content.Entity.UserId.Value == ViewBag.CurrentUserId;
            }

            int total = 0;
            CommentRepository comment = new CommentRepository(SessionCustom);
            List<Domain.Entities.FrontEnd.CommentsPaging> comments = comment.CommentsPagingContent(1, 6, out total, id);

            FileattachRepository fileRepository = new FileattachRepository(SessionCustom);
            fileRepository.Entity.ContentId = content.Entity.ContentId;
            Fileattach file = fileRepository.GetAll().FirstOrDefault(t => t.Type == Domain.Entities.Fileattach.TypeFile.Video);
            if (file != null)
            {
                content.Entity.Video = file.Filename;
            }

            return this.View(new Models.FEBlogEntry()
            {
                Entity = blog.Entity,
                CollComments = comments,
                ObjContent = content.Entity,
                CurrentLanguage = CurrentLanguage,
                CommentCount = total
            });
        }

        /// <summary>
        /// gets the blog archive
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult ArchiveEntries()
        {
            BlogEntryRepository blogEntries = new BlogEntryRepository(SessionCustom);
            List<ArchiveEntry> archiveEntries = blogEntries.ArchiveEntries(CurrentLanguage.LanguageId);
            return this.View(archiveEntries);
        }

        /// <summary>
        /// Set label vistas
        /// </summary>
        private void SetLabel()
        {
            LabelManagement objlabel = new LabelManagement(SessionCustom, HttpContext);
            ViewBag.TXTASITRAIDE = objlabel.GetLabelByName("TXTASITRAIDE", CurrentLanguage.LanguageId.Value);
        }
    }
}
