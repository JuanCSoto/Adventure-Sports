// <copyright file="Utils.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Script.Serialization;
    using Business.Services;
    using Domain.Abstract;
    using Domain.Concrete;
    using Domain.Entities;
    using Twitterizer;

    /// <summary>
    /// provides utilities to implement
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Utils"/> class
        /// </summary>
        public Utils()
        {
        }

        /// <summary>
        /// reduces the size of an image
        /// </summary>
        /// <param name="originalFile">original file path</param>
        /// <param name="newFile">new file path</param>
        /// <param name="newWidth">new with</param>
        /// <param name="onlyResizeIfWider">indicates whether the images is only resize if is wider that the new with</param>
        /// <returns>true if the action was completed false if not</returns>
        public static bool ReduceImage(string originalFile, string newFile, int newWidth, bool onlyResizeIfWider)
        {
            try
            {
                System.Drawing.Image fullsizeImage = System.Drawing.Image.FromFile(originalFile);

                fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

                if (onlyResizeIfWider)
                {
                    if (fullsizeImage.Width <= newWidth)
                    {
                        newWidth = fullsizeImage.Width;
                    }
                }

                int newHeight = fullsizeImage.Height * newWidth / fullsizeImage.Width;
                System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);
                fullsizeImage.Dispose();

                ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                newImage.Save(newFile, jgpEncoder, myEncoderParameters);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// check if the user has any available versus
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="contentId">content id</param>
        /// <param name="session">SQL session</param>
        /// <returns>true if the user was available versus false if not</returns>
        public static bool CheckVersus(int userId, int contentId, ISession session)
        {
            VersusRepository versus = new VersusRepository(session);
            ContentRepository content = new ContentRepository(session);

            content.Entity.ContentId = versus.Entity.ContentId = contentId;
            content.LoadByKey();

            versus.Entity.UserId = userId;
            List<Domain.Entities.FrontEnd.IdeasPaging> ideas = versus.GetVersus();

            bool result = ideas.Count == 2;
            return result;
        }

        /// <summary>
        /// Set a new reward for the user action
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="userAction">user action type</param>
        /// <param name="medallos">point to reward</param>
        /// <param name="maxPerDay">max actions per day</param>
        /// <param name="session">SQL session</param>
        /// <param name="context">HTTP context</param>
        /// <param name="updateTicket">indicates whether to refresh the security ticket information or not</param>
        /// <param name="language">language object</param>
        public static void SetUserRewardAction(int userId, Domain.Entities.RewardAction.UserActionType userAction, int medallos, int maxPerDay, ISession session, HttpContextBase context, bool updateTicket, Domain.Entities.Language language, bool onlyOne = false)
        {
            Domain.Concrete.RewardActionRepository reward = new RewardActionRepository(session);
            UserRepository user = new UserRepository(session);
            user.Entity.UserId = userId;
            user.LoadByKey();
            User beforeUser = user.Entity;
            if (reward.SetUserRewardAction(userId, userAction, ref medallos, maxPerDay, onlyOne))
            {
                user.LoadByKey();
                User afterUser = user.Entity;

                if (!beforeUser.UserRank.Equals(afterUser.UserRank))
                {
                    Business.Utilities.Notification.NewNotification(userId, null, Domain.Entities.Basic.SystemNotificationType.NEW_RANK, null, "/", null, userId, null, beforeUser.UserRank, afterUser.UserRank, session, context, language);
                }

                List<User> activeUsers = user.CountActiveUserByDate(null, null, 0, 6);
                if (activeUsers.Exists(au => au.UserId == userId))
                {
                    SystemNotificationRepository notification = new SystemNotificationRepository(session);
                    int count = notification.SystemNotificationCount(userId, (int)Domain.Entities.Basic.SystemNotificationType.ACTIVE_USER_TOP_5, userId);
                    if (count == 0)
                    {
                        Business.Utilities.Notification.NewNotification(userId, null, Domain.Entities.Basic.SystemNotificationType.ACTIVE_USER_TOP_5, null, "/estadisticas", null, userId, null, null, null, session, context, language);
                    }
                }

                if (/*Business.Utils.IsBlogAdmin(userId)*/Business.Utils.IsMaxLevel(userId) && !beforeUser.UserRank.Equals(afterUser.UserRank))
                {
                    Business.Utilities.Notification.NewNotification(userId, Domain.Entities.Basic.EmailNotificationType.PROMOTION, null, null, "/", null, userId, null, beforeUser.UserRank, afterUser.UserRank, session, context, language);
                }

                if (updateTicket)
                {
                    Business.Services.CustomMemberShipProvider membership = new Services.CustomMemberShipProvider(session, context);
                    membership.Updateticket(medallos, null);
                }
            }
        }

        /// <summary>
        /// Have the "secret" word of the site
        /// </summary>
        /// <returns>the "secret" word of the site</returns>
        public static string SecretWord()
        {
            return "este es el.m3jor secreto para gen3rar.lLaves";
        }

        /// <summary>
        /// Return an editable value of the site
        /// </summary>
        /// <param name="type">editable front end type</param>
        /// <returns>a string with the value of the editable</returns>
        public static string GetFronEndValue(Domain.Entities.Basic.ForntEndEditableType type)
        {
            string result = string.Empty;
            Domain.Abstract.SqlSession session = new Domain.Abstract.SqlSession();
            FrontEndEditableRepository repository = new FrontEndEditableRepository(session);
            repository.Entity.EditableId = (int)type;
            repository.LoadByKey();

            if (string.IsNullOrEmpty(repository.Entity.CurrentValue))
            {
                result = repository.Entity.OriginalValue;
            }
            else
            {
                result = repository.Entity.CurrentValue;
            }

            return result;
        }

        /// <summary>
        /// Return an editable value of the site
        /// </summary>
        /// <param name="type">editable front end type</param>
        /// <returns>a string with the value of the editable</returns>
        public static string GetFronEndValue(Domain.Entities.Basic.ForntEndEditableType type,int? LanguageId)
        {
            string result = string.Empty;
            Domain.Abstract.SqlSession session = new Domain.Abstract.SqlSession();
            FrontEndEditableRepository repository = new FrontEndEditableRepository(session);
            repository.Entity.EditableId = (int)type;
            repository.Entity.LanguageId = LanguageId;
            repository.Load();

            if (string.IsNullOrEmpty(repository.Entity.CurrentValue))
            {
                result = repository.Entity.OriginalValue;
            }
            else
            {
                result = repository.Entity.CurrentValue;
            }

            return result;
        }

        /// <summary>
        /// encrypts a string to <c>sha1</c>
        /// </summary>
        /// <param name="value">value to encrypt</param>
        /// <returns>returns the string encrypt</returns>
        public static string EncriptSHA1(string value)
        {
            SHA1 sha1 = SHA1Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha1.ComputeHash(encoding.GetBytes(value));
            for (int i = 0; i < stream.Length; i++)
            {
                sb.AppendFormat("{0:x2}", stream[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// encrypts a string to <c>md5</c>
        /// </summary>
        /// <param name="value">value to encrypt</param>
        /// <returns>returns the string encrypt</returns>
        public static string EncriptMD5(string value)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(value));

            for (int i = 0; i < stream.Length; i++)
            {
                sb.AppendFormat("{0:x2}", stream[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Upload a user file to server application
        /// </summary>
        /// <param name="file">file uploaded to a client</param>
        /// <param name="serverPath">path of application</param>
        /// <param name="path">path where the file will be saved</param>
        /// <param name="pref">prefix to file</param>
        /// <param name="width">maximum width for the site uploaded images</param>
        /// <returns>returns the path file</returns>
        public static string UploadFile(HttpPostedFileBase file, string serverPath, string path, string pref, int width = 700)
        {
            try
            {
                string pathfile = Path.Combine(serverPath, path);

                if (!Directory.Exists(pathfile))
                {
                    Directory.CreateDirectory(pathfile);
                }

                string newfile = (pref ?? string.Empty) + DateTime.Now.ToString("ddmmyyyyhhmmss") + Path.GetExtension(file.FileName);
                file.SaveAs(Path.Combine(pathfile, newfile));
                CorrectImageOrientationEXIF(Path.Combine(pathfile, newfile));
                int filesize = file.ContentLength / 1024;
                if (filesize > 250)
                {
                    string filepath = Path.Combine(pathfile, newfile);
                    string filepathreduced = filepath.Replace(System.IO.Path.GetExtension(filepath), ".jpg");
                    ReduceImage(filepath, filepathreduced, width, true);
                }

                return newfile;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// obtains the content from the text file
        /// </summary>
        /// <param name="path">path of the file</param>
        /// <returns>returns the string content from the file</returns>
        public static string GetContentFile(string path)
        {
            StreamReader sr = null;
            try
            {
                sr = System.IO.File.OpenText(path);
                string fileContent = sr.ReadToEnd();
                return fileContent;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
        }

        /// <summary>
        /// generates a random string
        /// </summary>
        /// <param name="length">length to the string</param>
        /// <returns>returns the random string</returns>
        public static string GenerateRandomString(int length)
        {
            string allowedChars = "abcdefghijkmnpqrstuvwxyz123456789ABCDEFGHIJKLMNPQRSTUVXYZ";
            Random randomNum = new Random();
            StringBuilder newPassWord = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                newPassWord.Append(allowedChars[randomNum.Next(0, allowedChars.Length)]);
            }

            return newPassWord.ToString();
        }

        /// <summary>
        /// obtains the friendly name to the string
        /// </summary>
        /// <param name="session">framework that establishes communication between the application and the database</param>
        /// <param name="name">string to convert</param>
        /// <param name="order">order parameter</param>
        /// <returns>returns the friendly name string </returns>
        public static string GetFindFrienlyName(ISession session, string name, int order)
        {
            string textnorm = name.Normalize(NormalizationForm.FormD);

            Regex reg = new Regex("[^a-zA-Z0-9 ]");
            string friendlyname = reg.Replace(textnorm, string.Empty)
                .Trim()
                .Replace(" ", "-").ToLower();

            if (friendlyname.Length > 200)
            {
                friendlyname.Substring(0, 200);
            }

            FriendlyurlRepository frindly = new FriendlyurlRepository(session);
            frindly.Entity.Friendlyurlid = friendlyname;
            frindly.LoadByKey();

            if (frindly.Entity.Id != null)
            {
                return order + "-" + friendlyname;
            }
            else
            {
                return friendlyname;
            }
        }

        /// <summary>
        /// obtains the content image
        /// </summary>
        /// <param name="image">name of the image</param>
        /// <param name="contentId">identifier of image</param>
        /// <param name="width">width of the image</param>
        /// <param name="height">height of the image</param>
        /// <returns>returns the URL image</returns>
        public static string GetImageContent(string image, int contentId, int? width, int? height)
        {
            if (!string.IsNullOrEmpty(image))
            {
                if (width != null && height != null)
                {
                    string serverpath = HttpContext.Current.Server.MapPath("~");
                    string prefix = height + "X" + width;

                    if (!File.Exists(Path.Combine(serverpath, @"files\" + contentId + @"\" + prefix + image)))
                    {
                        ImageResize objimg = new ImageResize(serverpath)
                        {
                            Height = height.Value,
                            Width = width.Value,
                            Prefix = prefix
                        };

                        objimg.Resize(@"files\" + contentId + @"\" + image, ImageResize.TypeResize.PartialProportional);
                    }

                    return VirtualPathUtility.ToAbsolute("~/files/" + contentId + "/" + prefix + image);
                }
                else
                {
                    return VirtualPathUtility.ToAbsolute("~/files/" + contentId + "/" + image);
                }
            }
            else
            {
                return VirtualPathUtility.ToAbsolute("~/resources/images/Default.jpg");
            }
        }

        /// <summary>
        /// Render a banner
        /// </summary>
        /// <param name="banner">object banner</param>
        /// <param name="width">width of banner</param>
        /// <param name="height">height of banner</param>
        /// <returns>returns the string to render</returns>
        public static string GetBannerPreview(Banner banner, int? width, int? height)
        {
            if (banner.Bannertype == 0)
            {
                int heigthO = banner.Height.Value;
                int widthO = banner.Width.Value;

                if (width != null && height != null)
                {
                    if (widthO > width.Value)
                    {
                        widthO = width.Value;
                        heigthO = Convert.ToInt32((Convert.ToDouble(heigthO) * Convert.ToDouble(width.Value)) / Convert.ToDouble(banner.Width.Value));
                    }

                    if (heigthO > height)
                    {
                        int alturao = height.Value;
                        heigthO = height.Value;
                        widthO = Convert.ToInt32((Convert.ToDouble(widthO) * Convert.ToDouble(height.Value)) / Convert.ToDouble(alturao));
                    }
                }

                return "<script>writeswf('" + VirtualPathUtility.ToAbsolute("~/resources/banners/" + banner.Bannerfile) + "', " + widthO + ", " + heigthO + ", '" + banner.BannerId + "')</script>";
            }
            else
            {
                if (width != null && height != null)
                {
                    string serverpath = HttpContext.Current.Server.MapPath("~");
                    string prefix = height + "X" + width;

                    if (!File.Exists(Path.Combine(serverpath, @"resources\banners\" + prefix + banner.Bannerfile)))
                    {
                        ImageResize objimg = new ImageResize(serverpath)
                        {
                            Height = height.Value,
                            Width = width.Value,
                            Prefix = prefix
                        };

                        objimg.Resize(@"resources\banners\" + banner.Bannerfile, ImageResize.TypeResize.PartialProportional);
                    }

                    return "<img src='" + VirtualPathUtility.ToAbsolute("~/resources/banners/" + prefix + banner.Bannerfile) + "' alt=\"banner\" />";
                }
                else
                {
                    return "<img src='" + VirtualPathUtility.ToAbsolute("~/resources/banners/" + banner.Bannerfile) + "' alt=\"banner\" />";
                }
            }
        }

        /// <summary>
        /// obtains a thumbnail image from content
        /// </summary>
        /// <param name="image">name of image</param>
        /// <param name="contentId">identifier of content</param>
        /// <returns>returns the URL image</returns>
        public static string GetThumbContent(string image, int contentId)
        {
            if (image != null)
            {
                return VirtualPathUtility.ToAbsolute("~/files/" + contentId + "/thumb" + image);
            }
            else
            {
                return VirtualPathUtility.ToAbsolute("~/resources/images/thumbDefault.jpg");
            }
        }

        /// <summary>
        /// obtains the deep follower to administrator
        /// </summary>
        /// <param name="collSection">list of object sections</param>
        /// <param name="sectionId">identifier of section</param>
        /// <returns>returns the string to render</returns>
        public static string GetDeepFollower(List<Domain.Entities.Section> collSection, int sectionId)
        {
            Stack<string> stackNameSection = new Stack<string>();
            bool isOver = true;
            Domain.Entities.Section objsec = collSection.Find(t => t.SectionId == sectionId);
            stackNameSection.Push(objsec.Name);
            while (isOver)
            {
                if (objsec.ParentId != null)
                {
                    Domain.Entities.Section nodeInfo = collSection.Find(t => t.SectionId == objsec.ParentId);
                    stackNameSection.Push(nodeInfo.Name);
                    objsec = nodeInfo;
                }
                else
                {
                    isOver = false;
                }
            }

            StringBuilder name = new StringBuilder();
            name.Append("Raiz");

            while (stackNameSection.Count > 0)
            {
                name.Append(" / " + stackNameSection.Pop());
            }

            return name.ToString();
        }

        /// <summary>
        /// obtains the deep follower to front end
        /// </summary>
        /// <param name="collSection">list of sections</param>
        /// <param name="sectionId">identifier of section</param>
        /// <param name="content">object content</param>
        /// <param name="language">object language</param>
        /// <returns>returns string to render</returns>
        public static HtmlString GetDeepFollowerFrontEnd(List<Domain.Entities.Section> collSection, int sectionId, Content content, Language language)
        {
            Stack<Section> stackNameSection = new Stack<Section>();
            bool isOver = true;
            Domain.Entities.Section objsec = collSection.Find(t => t.SectionId == sectionId);
            stackNameSection.Push(objsec);
            while (isOver)
            {
                if (objsec.ParentId != null)
                {
                    Domain.Entities.Section nodeInfo = collSection.Find(t => t.SectionId == objsec.ParentId);
                    stackNameSection.Push(nodeInfo);
                    objsec = nodeInfo;
                }
                else
                {
                    isOver = false;
                }
            }

            StringBuilder name = new StringBuilder();
            name.Append("<nav class='breadcrumb'><a href='" + VirtualPathUtility.ToAbsolute("~/") + (!language.IsDefault.Value ? language.Culturename + "/" : string.Empty) + "'>Inicio</a>");
            while (stackNameSection.Count > 0)
            {
                Section sec = stackNameSection.Pop();
                name.AppendFormat(" &gt; <a href='{0}'>{1}</a>", !sec.Url.Value ? (!string.IsNullOrEmpty(sec.Template) ? VirtualPathUtility.ToAbsolute("~/" + (!language.IsDefault.Value ? language.Culturename + "/" : string.Empty) + sec.Friendlyname) : "javascript:void(0);") : sec.Navigateurl, sec.Name);
            }

            if (content != null)
            {
                name.AppendFormat(" &gt; <a href='{0}'>{1}</a>", VirtualPathUtility.ToAbsolute("~/" + (!language.IsDefault.Value ? language.Culturename + "/" : string.Empty) + content.Frienlyname), content.Name);
            }

            name.Append("</nav>");
            return new HtmlString(name.ToString());
        }

        /// <summary>
        /// obtains code from the <c>URl</c> <c>youtube</c>
        /// </summary>
        /// <param name="urlYoutube"><c>URl</c> <c>youtube</c></param>
        /// <returns>returns string code</returns>
        public static string CodeYoutube(string urlYoutube)
        {
            /*if (urlYoutube.IndexOf("http") != 0)
            {
                urlYoutube = "http://" + urlYoutube;
            }
            Uri url = new Uri(urlYoutube);

            string[] query = url.Query.Split(new string[] { "v=" }, StringSplitOptions.RemoveEmptyEntries);
            string idVideo = null;
            query = query[1].Split(new char[] { '&' });
            if (query.Length > 0)
            {
                idVideo = query[0];
            }

            return idVideo;*/

            string regexYoutube = @"(^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|&v=)([^#\&\?]*).*)";
            Match matchYoutube = Regex.Match(urlYoutube, regexYoutube);
            string idVideo = string.Empty;
            if (matchYoutube.Success)
            {
                idVideo = matchYoutube.Groups[3].Value;
            }
            else
            {
                string regexVimeo = @"(^.*(vimeo.com\/)([^#\&\?]*).*)";
                Match matchVimeo = Regex.Match(urlYoutube, regexVimeo);
                if (matchVimeo.Success)
                {
                    idVideo = matchVimeo.Groups[3].Value;
                }
            }

            return idVideo;
        }

        /// <summary>
        /// insert a new item to Log table
        /// </summary>
        /// <param name="session">framework that establishes communication between the application and the database</param>
        /// <param name="message">message log</param>
        /// <param name="description">description log</param>
        public static void InsertLog(ISession session, string message, string description)
        {
            try
            {
                LogRepository log = new LogRepository(session);
                log.Entity.Joindate = DateTime.Now;
                log.Entity.Message = message;
                log.Entity.Description = description;
                log.Insert();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// insert a new item to Audit table
        /// </summary>
        /// <param name="session">framework that establishes communication between the application and the database</param>
        /// <param name="audit">object audit</param>
        public static void InsertAudit(ISession session, Audit audit)
        {
            if (bool.Parse(ConfigurationManager.AppSettings["EnabledAudit"]))
            {
                AuditRepository objaudit = new AuditRepository(session);
                objaudit.Entity = audit;
                objaudit.Insert();
            }
        }

        /// <summary>
        /// obtains if the file name is a image
        /// </summary>
        /// <param name="path">file name</param>
        /// <returns>returns true if the file is image</returns>
        public static bool IsImage(string path)
        {
            string extension = Path.GetExtension(path.ToLower());
            Regex objreg = new Regex("(.jpg|.gif|.png|.jpeg|.bmp)");
            return objreg.IsMatch(extension);
        }

        /// <summary>
        /// Check if a user can administrate de blog
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>true if the user can administrate de content false if not</returns>
        public static bool IsBlogAdmin(int userId)
        {
            //UserRepository user = new UserRepository(new SqlSession());
            //user.Entity.UserId = userId;
            //user.LoadByKey();

            //if (user.Entity.Medallos >= 501)
            //{
            //    return true;
            //}

            return false;
        }

        /// <summary>
        /// Check if a user have the max level
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>true if the user have the max level false if not</returns>
        public static bool IsMaxLevel(int userId)
        {
            UserRepository user = new UserRepository(new SqlSession());
            user.Entity.UserId = userId;
            user.LoadByKey();

            if (user.Entity.Medallos >= 233000)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// obtains the icon file
        /// </summary>
        /// <param name="file">type of file</param>
        /// <returns>returns the URL icon</returns>
        public static string GetIconFile(Fileattach file)
        {
            if (file.Type == Fileattach.TypeFile.File)
            {
                return VirtualPathUtility.ToAbsolute("~/resources/images/" + Utils.GetIconExtension(file.Filename));
            }
            else if (file.Type == Fileattach.TypeFile.Image)
            {
                return VirtualPathUtility.ToAbsolute("~/files/" + file.ContentId + "/thumb_" + file.Filename);
            }
            else
            {
                return "http://img.youtube.com/vi/" + file.Filename + "/1.jpg";
            }
        }

        /// <summary>
        /// obtains the image name to the extension associated
        /// </summary>
        /// <param name="extension">extension of image</param>
        /// <returns>returns string image</returns>
        public static string GetIconExtension(string extension)
        {
            string ext = Path.GetExtension(extension.ToLower());

            switch (ext)
            {
                case ".xls":
                case ".xlsx":
                    return "excel.gif";
                case ".doc":
                case ".docx":
                    return "word.gif";
                case ".ppt":
                case ".pptx":
                case ".pps":
                    return "powerpoint.gif";
                case ".vst":
                    return "visio.gif";
                case ".pdf":
                    return "pdf.gif";
                case ".cshtml":
                    return "cshtml.jpg";
                case ".dll":
                    return "dll.jpg";
                case ".txt":
                    return "txt.jpg";
                case ".css":
                    return "css.jpg";
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".bmp":
                case ".png":
                    return "image.gif";
                default:
                    return "default.gif";
            }
        }

        /// <summary>
        /// truncate a string according to the limit
        /// </summary>
        /// <param name="value">string to truncate</param>
        /// <param name="limit">limit of new string</param>
        /// <returns>returns a truncate string</returns>
        public static string TruncateWord(string value, int limit)
        {
            string originalWord = value.ToString().Trim();
            string sourceWord = originalWord;

            if (originalWord.Length > limit)
            {
                string[] firstword = originalWord.Split(' ');
                if (firstword.Length > 1)
                {
                    if (firstword[0].Length > limit)
                    {
                        originalWord = firstword[0];
                    }
                }

                if (originalWord.IndexOf(" ") != -1)
                {
                    for (int i = limit - 1; i > 0; i--)
                    {
                        if (originalWord.Substring(i, 1) == " ")
                        {
                            sourceWord = originalWord.Substring(0, i) + " ...";
                            break;
                        }
                    }
                }
                else
                {
                    sourceWord = originalWord.Substring(0, limit - 1) + " ...";
                }
            }

            return sourceWord;
        }

        /// <summary>
        /// replaces search criteria in string 
        /// </summary>
        /// <param name="text">string where the change is made</param>
        /// <param name="patter">search criteria</param>
        /// <param name="newText">new string to replace</param>
        /// <returns>returns a new string</returns>
        public static string Replace(string text, string patter, string newText)
        {
            Regex reg = new Regex(@"\b" + patter + @"\b", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            string newstring = reg.Replace(text, newText);
            reg = null;
            return newstring;
        }

        /// <summary>
        /// Converts the current time to an unix time stamp
        /// </summary>
        /// <returns>Current time stamp</returns>
        public static long GetUnixTimeStamp()
        {
            DateTime unixStart = new DateTime(1970, 1, 1);
            TimeSpan span = DateTime.Now - unixStart;
            return Convert.ToInt64(span.TotalSeconds);
        }

        /// <summary>
        /// Transform a date to a string indicating how much time have passed
        /// </summary>
        /// <param name="time">Date time to check</param>
        /// <returns>A string with the difference in time from the date supplied to now</returns>
        public static string ElapsedTime(DateTime time,string cultura)
        {
            string result = string.Empty;
            DateTime now = DateTime.Now;
            TimeSpan span = now - time;
            if (span.Days > 1)
            {
                if (cultura == "es")
                {
                    result = string.Concat("Hace " ,span.Days, " días");
                }
                else
                {
                    result = string.Concat(span.Days, " Days ago");
                }
            }
            else if (span.Days == 1)
            {
                if (cultura == "es")
                {

                    result = string.Concat("Hace " ,span.Days, " día");
                }
                else
                {
                    result = string.Concat(span.Days, " day ago");
                }
                
            }
            else if (span.Hours > 1)
            {
                if (cultura == "es")
                {
                    result = string.Concat("Hace " , span.Hours, " horas");
                }
                else
                {
                    result = string.Concat(span.Hours, " hours ago");
                }
            }
            else if (span.Hours == 1)
            {
                if (cultura == "es")
                {
                    result = string.Concat("Hace " ,span.Hours, " hora");
                }
                else
                {
                    result = string.Concat(span.Hours, " hour");
                }
            }
            else if (span.Minutes > 1)
            {
                if (cultura == "es")
                {
                    result = string.Concat("Hace ", span.Minutes, " minutos");
                }
                else
                {
                    result = string.Concat(span.Minutes, " minutes ago");
                }
            }
            else if (span.Minutes == 1)
            {
                if (cultura == "es")
                {
                    result = string.Concat("Hace ", span.Minutes, " minuto");
                }
                else
                {
                    result = string.Concat(span.Minutes, " minute ago");
                }
            }            
            else
            {
                if (cultura == "es")
                {
                    result = "Hace un momento";
                }
                else{

                    result = "Just a moment ago";
                }
            }

            return result;
        }

        /// <summary>
        /// Rotates the image to the intended position in which it was created and corrects its orientation EXIF
        /// </summary>
        /// <param name="originalFile">Location of the file to correct</param>
        /// <returns>True if the image was corrected, false if the image wasn't corrected</returns>
        public static bool CorrectImageOrientationEXIF(string originalFile)
        {
            bool result = false;
            System.Drawing.Image image = System.Drawing.Image.FromFile(originalFile);
            try
            {
                PropertyItem orientationProperty = image.GetPropertyItem(Entities.EXIFOrientations.OrientationID);
                Entities.EXIFOrientations.Orientations orientation = (Entities.EXIFOrientations.Orientations)orientationProperty.Value[0];

                System.Drawing.RotateFlipType rotationToApply = System.Drawing.RotateFlipType.RotateNoneFlipNone;

                switch (orientation)
                {
                    case Entities.EXIFOrientations.Orientations.TopLeft:
                        rotationToApply = System.Drawing.RotateFlipType.RotateNoneFlipNone;
                        break;
                    case Entities.EXIFOrientations.Orientations.TopRight:
                        rotationToApply = System.Drawing.RotateFlipType.RotateNoneFlipX;
                        break;
                    case Entities.EXIFOrientations.Orientations.BottomRight:
                        rotationToApply = System.Drawing.RotateFlipType.Rotate180FlipNone;
                        break;
                    case Entities.EXIFOrientations.Orientations.BottomLeft:
                        rotationToApply = System.Drawing.RotateFlipType.Rotate180FlipX;
                        break;
                    case Entities.EXIFOrientations.Orientations.LeftTop:
                        rotationToApply = System.Drawing.RotateFlipType.Rotate90FlipX;
                        break;
                    case Entities.EXIFOrientations.Orientations.RightTop:
                        rotationToApply = System.Drawing.RotateFlipType.Rotate90FlipNone;
                        break;
                    case Entities.EXIFOrientations.Orientations.RightBottom:
                        rotationToApply = System.Drawing.RotateFlipType.Rotate270FlipX;
                        break;
                    case Entities.EXIFOrientations.Orientations.LeftBottom:
                        rotationToApply = System.Drawing.RotateFlipType.Rotate270FlipNone;
                        break;
                    default:
                        rotationToApply = System.Drawing.RotateFlipType.RotateNoneFlipNone;
                        break;
                }

                image.RotateFlip(rotationToApply);
                image.GetPropertyItem(Entities.EXIFOrientations.OrientationID).Value[0] = (byte)Entities.EXIFOrientations.Orientations.TopLeft;

                System.Drawing.Image newImage = image.GetThumbnailImage(image.Width, image.Height, null, IntPtr.Zero);
                image.Dispose();

                ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 80L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                newImage.Save(originalFile, jgpEncoder, myEncoderParameters);
                newImage.Dispose();

                result = true;
            }
            catch
            {
                image.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Transform a "YOUTUBE" or "VIMEO" url into an object video
        /// </summary>
        /// <param name="url">video URL</param>
        /// <returns>a video object</returns>
        public static Domain.Entities.FrontEnd.Video GetVideoFromUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Regex youtubeReg = new Regex(@"^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|&v=)([^#\&\?]*).*");
                string[] youtubeResult = youtubeReg.Split(url);
                if (youtubeResult.Length >= 3 && youtubeResult[2].Length == 11)
                {
                    return new Domain.Entities.FrontEnd.Video() { Type = "youtube", ID = youtubeResult[2] };
                }

                Regex vimeoReg = new Regex(@"^.*(vimeo.com\/)([^#\&\?]*).*");
                string[] vimeoResult = vimeoReg.Split(url);
                if (vimeoResult.Length >= 3 && vimeoResult[2].Length > 0)
                {
                    return new Domain.Entities.FrontEnd.Video() { Type = "vimeo", ID = vimeoResult[2] };
                }
            }

            return null;
        }

        /// <summary>
        /// calculates the user profile image path
        /// </summary>
        /// <param name="path">image path</param>
        /// <returns>the user profile image path</returns>
        public static string fixLocalUserImagePath(string path)
        {
            if (!string.IsNullOrEmpty(path) && path[0] == '~' && path.IndexOf("twimg.com") == -1)
            {
                return path.Replace("~", HttpContext.Current.Request.ApplicationPath.ToLower().TrimEnd('/'));
            }
            else if (string.IsNullOrEmpty(path) || path.IndexOf("twimg.com") >= 0)
            {
                return string.Concat(HttpContext.Current.Request.ApplicationPath.ToLower().TrimEnd('/'), "/files/imagesuser/default.png");
            }

            return path;
        }

        /// <summary>
        /// convert a string to bytes
        /// </summary>
        /// <param name="str">the string to convert</param>
        /// <returns>an array of bytes</returns>
        public static byte[] StringToBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// convert an array of bytes to string
        /// </summary>
        /// <param name="bytes">the array to convert</param>
        /// <returns>a string</returns>
        public static string BytesToString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        /// <summary>
        /// Get the information of a google user that had grated permissions to the application
        /// </summary>
        /// <param name="token">google security token</param>
        /// <returns>a google user object</returns>
        public static Domain.Entities.FrontEnd.GoogleUser GetGoogleUser(string token)
        {
            Domain.Entities.FrontEnd.GoogleUser googleUser = null;
            WebClient client = new WebClient();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = client.DownloadString("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token=" + token);
            Domain.Entities.FrontEnd.Google google = serializer.Deserialize<Domain.Entities.FrontEnd.Google>(json);

            if (string.IsNullOrEmpty(google.error))
            {
                json = client.DownloadString("https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=" + token);
                googleUser = serializer.Deserialize<Domain.Entities.FrontEnd.GoogleUser>(json);

                if (string.IsNullOrEmpty(googleUser.picture))
                {
                    googleUser.picture = "//ssl.gstatic.com/accounts/ui/avatar_2x.png";
                }
            }

            return googleUser;
        }

        /// <summary>
        /// Get the information of a face book user that had grated permissions to the application
        /// </summary>
        /// <param name="token">facebook security token</param>
        /// <returns>a face book user object</returns>
        public static Domain.Entities.FrontEnd.FacebookUser GetFacebookUser(string token)
        {
            WebClient client = new WebClient();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = client.DownloadString("https://graph.facebook.com/me?access_token=" + token);
            Domain.Entities.FrontEnd.FacebookUser facebookUser = serializer.Deserialize<Domain.Entities.FrontEnd.FacebookUser>(json);

            return facebookUser;
        }

        /// <summary>
        /// Get the information of a twitter user that had grated permissions to the application
        /// </summary>
        /// <param name="screenname">the screen name of the twitter user</param>
        /// <returns>A dictionary with the user information</returns>
        public static System.Collections.Generic.Dictionary<string, object> GetTwitterUser(string screenname)
        {
            var oauthConsumerKey = ConfigurationManager.AppSettings["TwitterApiKey"];
            var oauthConsumerSecret = ConfigurationManager.AppSettings["TwitterConsumerSecret"];
            var oauthUrl = "https://api.twitter.com/oauth2/token";

            // Do the Authenticate
            var authHeaderFormat = "Basic {0}";

            var authHeader = string.Format(
                authHeaderFormat,
                Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(oauthConsumerKey) + ":" + Uri.EscapeDataString(oauthConsumerSecret))));

            var postBody = "grant_type=client_credentials";

            HttpWebRequest authRequest = (HttpWebRequest)WebRequest.Create(oauthUrl);
            authRequest.Headers.Add("Authorization", authHeader);
            authRequest.Method = "POST";
            authRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            authRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (Stream stream = authRequest.GetRequestStream())
            {
                byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
                stream.Write(content, 0, content.Length);
            }

            authRequest.Headers.Add("Accept-Encoding", "gzip");

            WebResponse authResponse = authRequest.GetResponse();
            System.Collections.Generic.Dictionary<string, object> objdim;
            System.Collections.Generic.Dictionary<string, object> objinformation;

            using (authResponse)
            {
                using (var reader = new StreamReader(authResponse.GetResponseStream()))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var objectText = reader.ReadToEnd();
                    var dim = js.Deserialize(objectText, typeof(object));
                    objdim = (System.Collections.Generic.Dictionary<string, object>)dim;
                }
            }

            var avatarFormat =
                "https://api.twitter.com/1.1/users/show.json?screen_name={0}";
            var avatarUrl = string.Format(avatarFormat, screenname);
            HttpWebRequest avatarRequest = (HttpWebRequest)WebRequest.Create(avatarUrl);
            var timelineHeaderFormat = "{0} {1}";
            avatarRequest.Headers.Add("Authorization", string.Format(timelineHeaderFormat, objdim["token_type"], objdim["access_token"]));
            avatarRequest.Method = "Get";
            WebResponse timeLineResponse = avatarRequest.GetResponse();

            var avatarJson = string.Empty;
            using (authResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    avatarJson = reader.ReadToEnd();
                    var dim = js.Deserialize(avatarJson, typeof(object));
                    objinformation = (System.Collections.Generic.Dictionary<string, object>)dim;
                }
            }

            return objinformation;
        }

        /// <summary>
        /// Get the information of a linked in user that had grated permissions to the application
        /// </summary>
        /// <param name="token">twitter security token</param>
        /// <returns>a twitter user object</returns>
        public static Domain.Entities.FrontEnd.LinkedInUser GetLinkedInUser(string token)
        {
            WebClient client = new WebClient();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = client.DownloadString("http://api.linkedin.com/v1/people/~?oauth2_access_token=" + token);
            Domain.Entities.FrontEnd.LinkedInUser linkedInUser = serializer.Deserialize<Domain.Entities.FrontEnd.LinkedInUser>(json);

            return linkedInUser;
        }

        /// <summary>
        /// Email template for the notification of the site
        /// </summary>
        /// <param name="body">body to merge in the template</param>
        /// <param name="notification">cancel notification link with token</param>
        /// <returns>HTML template</returns>
        public static string FormatEmail(string body, string notification)
        {
            string url = System.Configuration.ConfigurationManager.AppSettings["PathHost"];
            string template = "<html><head><title>Cities for Life</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head><body bgcolor=\"#FFFFFF\" leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\"><table width=\"548\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td><img src=\"@URL/resources/images/mail/mail-mm_01.jpg\" width=\"548\" height=\"88\" alt=\"\"></td></tr><tr><td><img src=\"@URL/resources/images/mail/mail-mm_02.jpg\" width=\"548\" height=\"44\" alt=\"\"></td></tr><tr><td style=\"padding: 20px; font-family: helvetica, arial; font-size: 13px;\">@BODY</td></tr>@NOTIFICATION<tr><td><img src=\"@URL/resources/images/mail/mail-mm_04.jpg\" width=\"548\" height=\"52\" alt=\"\"></td></tr></table></body></html>";            
            StringBuilder builder = new StringBuilder();
            builder.Append(template)
                .Replace("@URL", url)
                .Replace("@BODY", body)
                .Replace("@NOTIFICATION", notification);

            return builder.ToString();
        }

        /// <summary>
        /// Get an image encoder
        /// </summary>
        /// <param name="format">the format of the encoder</param>
        /// <returns>An image encoder</returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

       

    }
}
