// <copyright file="ContentManagement.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using Domain.Abstract;
    using Domain.Concrete;
    using Domain.Entities;

    /// <summary>
    /// Represents a management of content
    /// </summary>
    public class ContentManagement
    {
        /// <summary>
        /// framework that establishes communication between the application and the database
        /// </summary>
        private ISession session;

        /// <summary>
        /// HTTP context
        /// </summary>
        private HttpContextBase context;

        /// <summary>
        /// Resize image methods
        /// </summary>
        private ImageResize objimagerez;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManagement"/> class
        /// </summary>
        /// <param name="session">framework that establishes communication between the application and the database</param>
        /// <param name="context">HTTP context</param>
        public ContentManagement(ISession session, HttpContextBase context)
        {
            this.session = session;
            this.context = context;
        }

        /// <summary>
        /// Gets or sets a content object
        /// </summary>
        public Content ObjContent { get; set; }

        /// <summary>
        /// Gets or sets a list of videos
        /// </summary>
        public IEnumerable<string> CollVideos { get; set; }

        /// <summary>
        /// Gets or sets a provide access to individual files that have been uploaded by a client
        /// </summary>
        public HttpPostedFileBase ContentImage { get; set; }

        /// <summary>
        /// Gets or sets a provide access to individual files that have been uploaded by a client
        /// </summary>
        public HttpPostedFileBase ContentCoverImage { get; set; }

        /// <summary>
        /// Inserts a content in data base
        /// </summary>
        /// <param name="content">the object content</param>
        public void ContentInsert(Content content)
        {
            this.ObjContent = content;
            string serverMap = this.context.Server.MapPath("~");
            ContentRepository objcontent = new ContentRepository(this.session);
            this.objimagerez = new ImageResize(serverMap);
            objcontent.Entity = this.ObjContent;
            objcontent.Entity.Updatedate = DateTime.Now;

            if (this.ContentImage != null && this.ContentImage.ContentLength > 0 && Utils.IsImage(this.ContentImage.FileName))
            {
                objcontent.Entity.Image = Utils.UploadFile(
                    this.ContentImage,
                    serverMap,
                    @"resources\temporal\",
                    null);
            }

            if (this.ContentCoverImage != null && this.ContentCoverImage.ContentLength > 0 && Utils.IsImage(this.ContentCoverImage.FileName))
            {
                objcontent.Entity.CoverImage = Utils.UploadFile(
                    this.ContentCoverImage,
                    serverMap,
                    @"resources\temporal\",
                    "cover-",
                    1800);
            }

            if (content.ContentId != null)
            {
                objcontent.Update();

                FriendlyurlRepository friendlyrepo = new FriendlyurlRepository(this.session);
                friendlyrepo.Entity.Id = content.ContentId;
                friendlyrepo.Entity.Friendlyurlid = objcontent.Entity.Frienlyname;
                friendlyrepo.Entity.Type = Friendlyurl.FriendlyType.Content;
                friendlyrepo.Entity.LanguageId = content.LanguageId;
                friendlyrepo.Update();
            }
            else
            {
                objcontent.Entity.Orderliness = objcontent.GetMaxOrder();
                objcontent.Entity.Frienlyname = Utils.GetFindFrienlyName(this.session, objcontent.Entity.Name, objcontent.Entity.Orderliness.Value);
                objcontent.Entity.Joindate = objcontent.Entity.Updatedate;

                if (content.UserId == null)
                {
                    objcontent.Entity.UserId = (this.context.User as CustomPrincipal).UserId;
                }
                else
                {
                    objcontent.Entity.UserId = content.UserId;
                }
                
                this.ObjContent.ContentId = Convert.ToInt32(objcontent.Insert());

                FriendlyurlRepository friendlyrepo = new FriendlyurlRepository(this.session);
                friendlyrepo.Entity.Id = this.ObjContent.ContentId;
                friendlyrepo.Entity.Friendlyurlid = objcontent.Entity.Frienlyname;
                friendlyrepo.Entity.Type = Friendlyurl.FriendlyType.Content;
                friendlyrepo.Entity.LanguageId = content.LanguageId;
                friendlyrepo.Insert();
            }

            if (objcontent.Entity.Image != null)
            {
                string pathtruncate = Path.Combine(serverMap, @"resources\temporal\");
                string pathorigin = pathtruncate + objcontent.Entity.Image;
                string newpath = Path.Combine(serverMap, @"files\" + this.ObjContent.ContentId + @"\");

                if (File.Exists(pathorigin))
                {
                    int newWidth = 0;
                    int newHeight = 0;
                    bool isResized = false;

                    if (int.TryParse(this.context.Request.Form["imgwidth"], out newWidth) &&
                        int.TryParse(this.context.Request.Form["imgheight"], out newHeight))
                    {
                        ImageResize objimage = new ImageResize(serverMap);

                        objimage.Prefix = "_";
                        objimage.Width = newWidth;
                        objimage.Height = newHeight;
                        isResized = objimage.Resize(@"resources\temporal\" + objcontent.Entity.Image, ImageResize.TypeResize.BackgroundProportional);
                    }

                    if (!Directory.Exists(newpath))
                    {
                        Directory.CreateDirectory(newpath);
                    }

                    if (!isResized)
                    {
                        File.Move(pathorigin, newpath + objcontent.Entity.Image);
                        File.Delete(pathorigin);
                    }
                    else
                    {
                        File.Move(pathtruncate + "_" + objcontent.Entity.Image, newpath + objcontent.Entity.Image);
                        File.Delete(pathorigin);
                        File.Delete(pathtruncate + "_" + objcontent.Entity.Image);
                    }
                }

                string fileroute = @"files\" + this.ObjContent.ContentId + @"\" + objcontent.Entity.Image;

                this.objimagerez.Resize(
                    fileroute,
                    ImageResize.TypeResize.PartialProportional);

                this.objimagerez.Width = 170;
                this.objimagerez.Height = 105;
                this.objimagerez.Prefix = "170x105-";
                this.objimagerez.Resize(fileroute, ImageResize.TypeResize.CropProportional);

                this.objimagerez.Width = 340;
                this.objimagerez.Height = 250;
                this.objimagerez.Prefix = "340x250-";
                this.objimagerez.Resize(fileroute, ImageResize.TypeResize.CropProportional);

                this.objimagerez.Width = 340;
                this.objimagerez.Height = 320;
                this.objimagerez.Prefix = "340x320-";
                this.objimagerez.Resize(fileroute, ImageResize.TypeResize.CropProportional);

                this.objimagerez.Width = 511;
                this.objimagerez.Height = 320;
                this.objimagerez.Prefix = "511x320-";
                this.objimagerez.Resize(fileroute, ImageResize.TypeResize.CropProportional);

                this.objimagerez.Width = 683;
                this.objimagerez.Height = 320;
                this.objimagerez.Prefix = "683x320-";
                this.objimagerez.Resize(fileroute, ImageResize.TypeResize.CropProportional);

                this.objimagerez.Width = 191;
                this.objimagerez.Height = 191;
                this.objimagerez.Prefix = "191x191-";
                this.objimagerez.Resize(fileroute, ImageResize.TypeResize.CropProportional);

                this.objimagerez.Width = 511;
                this.objimagerez.Height = 255;
                this.objimagerez.Prefix = "511x255-";
                this.objimagerez.Resize(fileroute, ImageResize.TypeResize.CropProportional);
            }

            if (objcontent.Entity.CoverImage != null)
            {
                string pathtruncate = Path.Combine(serverMap, @"resources\temporal\");
                string pathorigin = pathtruncate + objcontent.Entity.CoverImage;
                string newpath = Path.Combine(serverMap, @"files\" + this.ObjContent.ContentId + @"\");

                if (File.Exists(pathorigin))
                {
                    if (!Directory.Exists(newpath))
                    {
                        Directory.CreateDirectory(newpath);
                    }

                    File.Move(pathtruncate + objcontent.Entity.CoverImage, newpath + objcontent.Entity.CoverImage);
                    File.Delete(pathorigin);
                    File.Delete(pathtruncate + objcontent.Entity.CoverImage);                    
                }

                string fileroute = @"files\" + this.ObjContent.ContentId + @"\" + objcontent.Entity.CoverImage;

                ////this.objimagerez.Resize(fileroute, ImageResize.TypeResize.PartialProportional);

                this.objimagerez.Width = 1800;
                this.objimagerez.Height = 800;
                this.objimagerez.Prefix = "1800x800-";
                this.objimagerez.Resize(fileroute, ImageResize.TypeResize.Proportional);
            }

            this.InsertAttachFiles();
            this.InsertTags();
        }

        /// <summary>
        /// Inserts a content in data base
        /// </summary>
        /// <param name="content">Content entity to insert or update</param>
        /// <param name="type">Content type to link the friendly URL</param>        
        public void ContentInsert(Content content, int type)
        {
            this.ObjContent = content;
            string serverMap = this.context.Server.MapPath("~");
            ContentRepository objcontent = new ContentRepository(this.session);
            this.objimagerez = new ImageResize(serverMap);
            objcontent.Entity = this.ObjContent;
            objcontent.Entity.Updatedate = DateTime.Now;

            if (this.ContentImage != null && this.ContentImage.ContentLength > 0 && Utils.IsImage(this.ContentImage.FileName))
            {
                objcontent.Entity.Image = Utils.UploadFile(
                    this.ContentImage,
                    serverMap,
                    @"resources\temporal\",
                    null);
            }

            if (this.ContentCoverImage != null && this.ContentCoverImage.ContentLength > 0 && Utils.IsImage(this.ContentCoverImage.FileName))
            {
                objcontent.Entity.CoverImage = Utils.UploadFile(
                    this.ContentCoverImage,
                    serverMap,
                    @"resources\temporal\",
                    "cover-",
                    1800);
            }

            if (content.ContentId != null)
            {
                objcontent.Update();

                FriendlyurlRepository friendlyrepo = new FriendlyurlRepository(this.session);
                friendlyrepo.Entity.Id = content.ContentId;
                friendlyrepo.Entity.Friendlyurlid = objcontent.Entity.Frienlyname;
                friendlyrepo.Entity.Type = (Friendlyurl.FriendlyType)type;
                friendlyrepo.Entity.LanguageId = content.LanguageId;
                friendlyrepo.Update();
            }
            else
            {
                objcontent.Entity.Orderliness = objcontent.GetMaxOrder();
                objcontent.Entity.Frienlyname = Utils.GetFindFrienlyName(this.session, objcontent.Entity.Name, objcontent.Entity.Orderliness.Value);
                objcontent.Entity.Joindate = objcontent.Entity.Updatedate;

                if (content.UserId == null)
                {
                    objcontent.Entity.UserId = (this.context.User as CustomPrincipal).UserId;
                }
                else
                {
                    objcontent.Entity.UserId = content.UserId;
                }

                this.ObjContent.ContentId = Convert.ToInt32(objcontent.Insert());

                FriendlyurlRepository friendlyrepo = new FriendlyurlRepository(this.session);
                friendlyrepo.Entity.Id = this.ObjContent.ContentId;
                friendlyrepo.Entity.Friendlyurlid = objcontent.Entity.Frienlyname;
                friendlyrepo.Entity.Type = (Friendlyurl.FriendlyType)type;
                friendlyrepo.Entity.LanguageId = content.LanguageId;
                friendlyrepo.Insert();
            }

            if (objcontent.Entity.Image != null)
            {
                string pathtruncate = Path.Combine(serverMap, @"resources\temporal\");
                string pathorigin = pathtruncate + objcontent.Entity.Image;
                string newpath = Path.Combine(serverMap, @"files\" + this.ObjContent.ContentId + @"\");

                if (File.Exists(pathorigin))
                {
                    int newWidth = 0;
                    int newHeight = 0;
                    bool isResized = false;

                    if (int.TryParse(this.context.Request.Form["imgwidth"], out newWidth) &&
                        int.TryParse(this.context.Request.Form["imgheight"], out newHeight))
                    {
                        ImageResize objimage = new ImageResize(serverMap);

                        objimage.Prefix = "_";
                        objimage.Width = newWidth;
                        objimage.Height = newHeight;
                        isResized = objimage.Resize(@"resources\temporal\" + objcontent.Entity.Image, ImageResize.TypeResize.BackgroundProportional);
                    }

                    if (!Directory.Exists(newpath))
                    {
                        Directory.CreateDirectory(newpath);
                    }

                    if (!isResized)
                    {
                        File.Move(pathorigin, newpath + objcontent.Entity.Image);
                        File.Delete(pathorigin);
                    }
                    else
                    {
                        File.Move(pathtruncate + "_" + objcontent.Entity.Image, newpath + objcontent.Entity.Image);
                        File.Delete(pathorigin);
                        File.Delete(pathtruncate + "_" + objcontent.Entity.Image);
                    }
                }

                string fileroute = @"files\" + this.ObjContent.ContentId + @"\" + objcontent.Entity.Image;

                this.objimagerez.Resize(
                    fileroute,
                    ImageResize.TypeResize.PartialProportional);

                this.objimagerez.Width = 170;
                this.objimagerez.Height = 105;
                this.objimagerez.Prefix = "170x105-";
                this.objimagerez.Resize(fileroute, ImageResize.TypeResize.CropProportional);

                this.objimagerez.Width = 340;
                this.objimagerez.Height = 250;
                this.objimagerez.Prefix = "340x250-";
                this.objimagerez.Resize(fileroute, ImageResize.TypeResize.CropProportional);

                this.objimagerez.Width = 340;
                this.objimagerez.Height = 320;
                this.objimagerez.Prefix = "340x320-";
                this.objimagerez.Resize(fileroute, ImageResize.TypeResize.CropProportional);

                this.objimagerez.Width = 511;
                this.objimagerez.Height = 320;
                this.objimagerez.Prefix = "511x320-";
                this.objimagerez.Resize(fileroute, ImageResize.TypeResize.CropProportional);

                this.objimagerez.Width = 683;
                this.objimagerez.Height = 320;
                this.objimagerez.Prefix = "683x320-";
                this.objimagerez.Resize(fileroute, ImageResize.TypeResize.CropProportional);

                this.objimagerez.Width = 127;
                this.objimagerez.Height = 127;
                this.objimagerez.Prefix = "127x127-";
                this.objimagerez.Resize(fileroute, ImageResize.TypeResize.CropProportional);

                this.objimagerez.Width = 511;
                this.objimagerez.Height = 255;
                this.objimagerez.Prefix = "511x255-";
                this.objimagerez.Resize(fileroute, ImageResize.TypeResize.CropProportional);
            }

            if (objcontent.Entity.CoverImage != null)
            {
                string pathtruncate = Path.Combine(serverMap, @"resources\temporal\");
                string pathorigin = pathtruncate + objcontent.Entity.CoverImage;
                string newpath = Path.Combine(serverMap, @"files\" + this.ObjContent.ContentId + @"\");

                if (File.Exists(pathorigin))
                {
                    if (!Directory.Exists(newpath))
                    {
                        Directory.CreateDirectory(newpath);
                    }

                    File.Move(pathtruncate + objcontent.Entity.CoverImage, newpath + objcontent.Entity.CoverImage);
                    File.Delete(pathorigin);
                    File.Delete(pathtruncate + objcontent.Entity.CoverImage);                    
                }

                string fileroute = @"files\" + this.ObjContent.ContentId + @"\" + objcontent.Entity.CoverImage;

                ////this.objimagerez.Resize(fileroute, ImageResize.TypeResize.PartialProportional);

                this.objimagerez.Width = 2000;
                this.objimagerez.Height = 1000;
                this.objimagerez.Prefix = "1800x800-";
                this.objimagerez.Resize(fileroute, ImageResize.TypeResize.Proportional);
            }

            this.InsertAttachFiles();
            this.InsertTags();
        }

        /// <summary>
        /// Deletes content from data base
        /// </summary>
        /// <param name="listContentId">list of content's identifiers</param>
        public void DeleteContent(string[] listContentId)
        {
            List<int> collIds = new List<int>();

            foreach (string item in listContentId)
            {
                collIds.Add(int.Parse(item));
            }

            ContentRepository objcontent = new ContentRepository(this.session);
            FriendlyurlRepository friendlyrepo = new FriendlyurlRepository(this.session);

            try
            {
                this.session.Begin();

                foreach (int item in collIds)
                {
                    string path = Path.Combine(this.context.Server.MapPath("~"), @"files\" + item);
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }

                    objcontent.Entity.ContentId = item;
                    objcontent.Delete();

                    friendlyrepo.Entity.Id = item;
                    friendlyrepo.Entity.Type = Friendlyurl.FriendlyType.Content;
                    friendlyrepo.Delete();
                }

                Utils.InsertAudit(
                    this.session,
                    new Audit()
                    {
                        Joindate = DateTime.Now,
                        Description = "Delete contents",
                        Username = (this.context.User as CustomPrincipal).UserId,
                        Auditaction = "Delete"
                    });

                this.session.Commit();
            }
            catch (Exception ex)
            {
                this.session.RollBack();
                Utils.InsertLog(this.session, "Error Delete", ex.ToString());
            }
        }

        /// <summary>
        /// Gets the type file
        /// </summary>
        /// <param name="filename">file name</param>
        /// <returns>returns if the file is a image</returns>
        public Fileattach.TypeFile GetTypeFile(string filename)
        {
            string fileExtension = Path.GetExtension(filename).ToLower();
            switch (fileExtension)
            {
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".png":
                case ".bmp":
                    return Fileattach.TypeFile.Image;
                default:
                    return Fileattach.TypeFile.File;
            }
        }

        /// <summary>
        /// List of files
        /// </summary>
        /// <param name="path">the path of the file's origin</param>
        /// <returns>returns a list of <c>cshtml</c> files to templates</returns>
        public IEnumerable<string> CollFiles(string path)
        {
            string directoryPath = Path.Combine(
                this.context.Server.MapPath("~"),
                path);

            string[] filesnames = Directory.GetFiles(directoryPath, "*.cshtml");
            foreach (string item in filesnames)
            {
                yield return Path.GetFileNameWithoutExtension(item);
            }
        }

        /// <summary>
        /// Attached or detached a contents
        /// </summary>
        /// <param name="contentId">identifier of content</param>
        /// <param name="contentIdChild">identifier of content child</param>
        /// <param name="attach">if the content attached</param>
        /// <returns>returns true if the operation is successful</returns>
        public bool AttachDettachContent(int contentId, int contentIdChild, bool attach)
        {
            try
            {
                ContentrelationRepository objrel = new ContentrelationRepository(this.session);
                objrel.Entity.ContentId = contentId;
                objrel.Entity.ContentIdChild = contentIdChild;
                if (attach)
                {
                    objrel.Insert();
                }
                else
                {
                    objrel.Delete();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="id">identifier of file</param>
        /// <returns>returns true if the operation is successful</returns>
        public bool DeleteFileAttach(int id)
        {
            try
            {
                FileattachRepository fileattach = new FileattachRepository(this.session);
                fileattach.Entity.FileattachId = id;
                fileattach.LoadByKey();

                if (fileattach.Entity.Filename != null)
                {
                    string path = Path.Combine(this.context.Server.MapPath("~"), @"files\" + fileattach.Entity.ContentId + @"\");
                    if (File.Exists(path + fileattach.Entity.Filename))
                    {
                        File.Delete(path + fileattach.Entity.Filename);
                    }

                    if (File.Exists(path + "thumb_" + fileattach.Entity.Filename))
                    {
                        File.Delete(path + "thumb_" + fileattach.Entity.Filename);
                    }

                    fileattach.Delete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Utils.InsertLog(this.session, "Error Delete", ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Changes the order of contents
        /// </summary>
        /// <param name="contentId">identifier of content</param>
        /// <param name="prevId">Identifier of previous content</param>
        /// <param name="limit">if the content is limit</param>
        /// <returns>returns true if the operation is successful</returns>
        public bool ChangeOrder(int contentId, int prevId, bool limit)
        {
            try
            {
                ContentRepository objcontent = new ContentRepository(this.session);
                objcontent.ChangeOrder(contentId, prevId, limit);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Changes a order of files
        /// </summary>
        /// <param name="fileattachId">identifier of file</param>
        /// <param name="prevId">identifier of previous file</param>
        /// <param name="limit">if the content is limit</param>
        /// <returns>returns true if the operation is successful</returns>
        public bool ChangeOrderFile(int fileattachId, int prevId, bool limit)
        {
            try
            {
                FileattachRepository objfileattach = new FileattachRepository(this.session);
                objfileattach.ChangeOrder(fileattachId, prevId, limit);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Changes a content´s section
        /// </summary>
        /// <param name="contentId">identifier of content</param>
        /// <param name="sectionId">identifier of section</param>
        /// <returns>returns true if the operation is success</returns>
        public bool ChangeSection(int contentId, int sectionId)
        {
            try
            {
                ContentRepository objcontent = new ContentRepository(this.session);
                objcontent.Entity.ContentId = contentId;
                objcontent.Entity.SectionId = sectionId;
                objcontent.Update();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Updates a file
        /// </summary>
        /// <param name="fileattachId">identifier of file</param>
        /// <param name="name">name of file</param>
        /// <returns>returns true if the operations is success</returns>
        public bool UpdateFile(int fileattachId, string name)
        {
            try
            {
                FileattachRepository fileattachrepo = new FileattachRepository(this.session);
                fileattachrepo.Entity.FileattachId = fileattachId;
                fileattachrepo.Entity.Name = name;
                fileattachrepo.Update();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Copy a content from the other content
        /// </summary>
        /// <param name="contentId">identifier of content</param>
        /// <param name="languageId">identifier of language</param>
        /// <param name="sectionId">identifier of section</param>
        /// <returns>returns true if the operation is successful</returns>
        public bool Clone(int contentId, int languageId, int sectionId)
        {
            try
            {
                this.session.Begin();
                ContentRepository contentrepo = new ContentRepository(this.session);
                FileattachRepository fileattachrepo = new FileattachRepository(this.session);
                FriendlyurlRepository friendlyrepo = new FriendlyurlRepository(this.session);

                fileattachrepo.Entity.ContentId =
                friendlyrepo.Entity.Id =
                contentrepo.Entity.ContentId = contentId;
                contentrepo.LoadByKey();

                friendlyrepo.Entity.Type = Friendlyurl.FriendlyType.Content;
                friendlyrepo.Load();

                List<Fileattach> collfiles = fileattachrepo.GetAll();

                contentrepo.Entity.SectionId = sectionId;
                contentrepo.Entity.Name = "Copy of " + contentrepo.Entity.Name;
                contentrepo.Entity.Orderliness = contentrepo.GetMaxOrder();
                int newid = Convert.ToInt32(contentrepo.Insert());

                friendlyrepo.Entity.Id = newid;
                friendlyrepo.Entity.Friendlyurlid = "copy_of_" + friendlyrepo.Entity.Friendlyurlid;
                friendlyrepo.Entity.LanguageId = languageId;
                friendlyrepo.Insert();

                foreach (Fileattach item in collfiles)
                {
                    fileattachrepo.Entity = item;
                    fileattachrepo.Entity.ContentId = newid;
                    fileattachrepo.Insert();
                }

                this.CompleteClone(contentId, newid, contentrepo.Entity.ModulId.Value);

                string originpath = Path.Combine(this.context.Server.MapPath("~"), @"Files\" + contentId + @"\");
                string newpath = Path.Combine(this.context.Server.MapPath("~"), @"Files\" + newid + @"\");

                if (Directory.Exists(originpath))
                {
                    DirectoryInfo dir = new DirectoryInfo(originpath);
                    FileInfo[] files = dir.GetFiles();

                    if (!Directory.Exists(newpath))
                    {
                        Directory.CreateDirectory(newpath);
                    }

                    foreach (FileInfo item in files)
                    {
                        File.Copy(item.FullName, newpath + item.Name);
                    }
                }

                this.session.Commit();

                return true;
            }
            catch (Exception ex)
            {
                this.session.RollBack();
                Utils.InsertLog(this.session, "Clone Content ", ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Inserts a new tag
        /// </summary>
        private void InsertTags()
        {
            ContenttagRepository objtag = new ContenttagRepository(this.session);
            objtag.Entity.ContentId = this.ObjContent.ContentId;
            objtag.Delete();

            IEnumerable<int> collTags = this.CollTagsId();
            foreach (int item in collTags)
            {
                objtag.Entity.TagId = item;
                objtag.Insert();
            }
        }

        /// <summary>
        /// Gets a list of tags
        /// </summary>
        /// <returns>returns a list of tags</returns>
        private IEnumerable<int> CollTagsId()
        {
            string[] tags = this.context.Request.Form["Tags[]"] != null ? this.context.Request.Form["Tags[]"].Split(',') : new string[0];

            foreach (string item in tags)
            {
                int id = 0;
                if (!int.TryParse(item, out id))
                {
                    TagRepository objtag = new TagRepository(this.session);
                    objtag.Entity.Name = item;
                    objtag.Entity.LanguageId = this.ObjContent.LanguageId;
                    id = Convert.ToInt32(objtag.Insert());
                }

                yield return id;
            }
        }

        /// <summary>
        /// Inserts the files associated with the content
        /// </summary>
        private void InsertAttachFiles()
        {
            FileattachRepository objfiles = new FileattachRepository(this.session);
            objfiles.Entity.ContentId = this.ObjContent.ContentId;

            for (int i = 0; i < this.context.Request.Files.Count; i++)
            {
                if (this.context.Request.Files.GetKey(i).Contains("upload"))
                {
                    HttpPostedFileBase httpfile = this.context.Request.Files[i];
                    string ipnputname = "NP" + this.context.Request.Files.GetKey(i);

                    if (httpfile.ContentLength > 0)
                    {
                        objfiles.Entity.Filename = Utils.UploadFile(
                            httpfile,
                            this.context.Server.MapPath("~"),
                            @"files\" + this.ObjContent.ContentId + @"\",
                            i.ToString());

                        string valueinput = this.context.Request.Form[ipnputname];

                        objfiles.Entity.Type = this.GetTypeFile(objfiles.Entity.Filename);
                        objfiles.Entity.Name = !string.IsNullOrEmpty(valueinput) ? valueinput : null;
                        objfiles.Entity.Joindate = DateTime.Now;
                        objfiles.Insert();

                        if (objfiles.Entity.Type.Value == Fileattach.TypeFile.Image)
                        {
                            string fileroute = @"files\" + this.ObjContent.ContentId + @"\" + objfiles.Entity.Filename;
                            this.objimagerez.Resize(
                                fileroute,
                                ImageResize.TypeResize.PartialProportional);

                            this.objimagerez.Width = 170;
                            this.objimagerez.Height = 105;
                            this.objimagerez.Prefix = "170x105-";
                            this.objimagerez.Resize(fileroute, ImageResize.TypeResize.CropProportional);
                            
                            this.objimagerez.Width = 683;
                            this.objimagerez.Height = 320;
                            this.objimagerez.Prefix = "683x320-";
                            this.objimagerez.Resize(fileroute, ImageResize.TypeResize.CropProportional);
                        }
                    }
                }
            }

            if (this.CollVideos != null)
            {
                foreach (string item in this.CollVideos)
                {
                    string codyoutube = Utils.CodeYoutube(item);
                    if (!string.IsNullOrEmpty(codyoutube))
                    {
                        objfiles.Entity.Filename = codyoutube;
                        objfiles.Entity.Type = Fileattach.TypeFile.Video;
                        objfiles.Entity.Name = item;
                        objfiles.Entity.Joindate = DateTime.Now;
                        objfiles.Insert();
                    }
                }
            }
        }

        /// <summary>
        /// Completes the content's copy
        /// </summary>
        /// <param name="oldId">Old identifier</param>
        /// <param name="newId">new identifier</param>
        /// <param name="modulId">identifier of module</param>
        private void CompleteClone(int oldId, int newId, int modulId)
        {
            ////News
            if (modulId == 13)
            {
                NewsRepository newsrepo = new NewsRepository(this.session);
                newsrepo.Entity.ContentId = oldId;
                newsrepo.LoadByKey();

                string oldpath = "/Files/" + oldId;
                string newpath = "/Files/" + newId;

                if (!string.IsNullOrEmpty(newsrepo.Entity.Xmlcontent))
                {
                    newsrepo.Entity.Xmlcontent = newsrepo.Entity.Xmlcontent.Replace(oldpath, newpath);
                }

                newsrepo.Entity.ContentId = newId;
                newsrepo.Insert();
            }
        }
    }
}