// <copyright file="CustomMemberShipProvider.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Domain.Abstract;
    using Domain.Concrete;
    using Domain.Entities;

    /// <summary>
    /// user management
    /// </summary>
    public class CustomMemberShipProvider
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
        /// Initializes a new instance of the <see cref="CustomMemberShipProvider"/> class
        /// </summary>
        /// <param name="session">framework that establishes communication between the application and the database</param>
        /// <param name="context">HTTP context</param>
        public CustomMemberShipProvider(ISession session, HttpContextBase context)
        {
            this.session = session;
            this.context = context;
            this.Userrepository = new UserRepository(this.session);
        }

        /// <summary>
        /// response to a user authentication
        /// </summary>
        public enum AuthencReturn
        {
            /// <summary>
            /// the user is found
            /// </summary>
            USER_OK,

            /// <summary>
            /// the user is not found
            /// </summary>
            NOT_FOUND,

            /// <summary>
            /// the password is not correct
            /// </summary>
            BAD_PASSWORD,

            /// <summary>
            /// System error
            /// </summary>
            ERROR,

            /// <summary>
            /// User is blocked
            /// </summary>
            USER_BLOCKED,

            /// <summary>
            /// User have a password and phone
            /// </summary>
            PHONE_PASSWORD,

            /// <summary>
            /// User phone is available
            /// </summary>
            PHONE_AVAILABLE
        }

        /// <summary>
        /// Gets or sets a user repository
        /// </summary>
        public UserRepository Userrepository { get; set; }

        /// <summary>
        /// Gets or sets a class for provide access to individual files
        /// that have been uploaded by a client.
        /// </summary>
        public HttpPostedFileBase Userimage { get; set; }

        /// <summary>
        /// obtains user modules
        /// </summary>
        /// <param name="userId">identifier of user</param>
        /// <param name="session">framework that establishes communication between the application and the database</param>
        /// <param name="context">HTTP context</param>
        /// <returns>returns a list of modules</returns>
        public static List<Modul> GetModuls(int userId, ISession session, HttpContextBase context)
        {
            Language language = null;

            if (context.Session["lang"] != null)
            {
                language = (Language)context.Session["lang"];
            }

            if (language == null || !language.LanguageId.HasValue)
            {
                LanguageRepository languagerepo = new LanguageRepository(session);
                languagerepo.GetByUser(userId);
                language = languagerepo.Entity;
                context.Session.Add("lang", language);

                if (!language.LanguageId.HasValue)
                {
                    language.LanguageId = 2;
                }
            }

            ModulRepository objmodul = new ModulRepository(session);
            return objmodul.GetModulsbyuser(userId, language.LanguageId.Value);
        }

        /// <summary>
        /// obtains children modules
        /// </summary>
        /// <param name="modulId">identifier of module</param>
        /// <param name="collMod">list of modules</param>
        /// <returns>returns a list of children modules</returns>
        public static IEnumerable<Modul> GetModulChidren(int modulId, List<Modul> collMod)
        {
            Modul mod = collMod.Find(t => t.ModulId == modulId);
            return collMod.FindAll(i => i.ParentId == mod.ParentId);
        }

        /// <summary>
        /// obtains a user roles
        /// </summary>
        /// <param name="session">framework that establishes communication between the application and the database</param>
        /// <param name="context">HTTP context</param>
        /// <returns>returns a list of roles</returns>
        public static IEnumerable<Rol> GetRols(ISession session, HttpContextBase context)
        {
            RolRepository objrol = new RolRepository(session);
            int userId = (context.User as CustomPrincipal).UserId;
            return objrol.GetRols(userId);
        }

        /// <summary>
        /// create a new user
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns>returns the identifier of user</returns>
        public int CreateUser(User user)
        {
            try
            {
                this.Userrepository.Entity = user;
                return Convert.ToInt32(this.Userrepository.Insert());
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    return -1;
                }
                else
                {
                    Utils.InsertLog(
                        this.session,
                        "Error User",
                        ex.Message + " " + ex.StackTrace);
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Utils.InsertLog(
                    this.session,
                    "Error User",
                    ex.Message + " " + ex.StackTrace);
                return 0;
            }
        }

        /// <summary>
        /// delete a user
        /// </summary>
        /// <param name="userId">identifier of user</param>
        /// <returns>returns true if the user is removed</returns>
        public bool DeleteUser(int userId)
        {
            try
            {
                this.Userrepository.Entity.UserId = userId;
                this.Userrepository.Delete();
                return true;
            }
            catch (Exception ex)
            {
                Utils.InsertLog(
                    this.session,
                    "Error User",
                    ex.Message + " " + ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// reestablishes the password user
        /// </summary>
        /// <param name="email">email of user</param>
        /// <returns>returns true if sends email</returns>
        public bool RecoveryPassword(string email)
        {
            this.Userrepository.Entity = new User();
            this.Userrepository.Entity.Email = email;
            this.Userrepository.Load();

            if (this.Userrepository.Entity.UserId != null)
            {
                try
                {
                    string password = Utils.GenerateRandomString(7);
                    string body = Utils.GetContentFile(Path.Combine(this.context.Server.MapPath("~"), @"Resources\Templates\PasswordRecovery.txt"));

                    this.Userrepository.Entity.Password = Utils.EncriptSHA1(password);
                    this.Userrepository.Update();

                    SendMail objsend = new SendMail()
                    {
                        Body = body.Replace("@USER", this.Userrepository.Entity.Names)
                        .Replace("@PASSWORD", password)
                        .Replace("@HOST", ConfigurationManager.AppSettings["PathHost"]),
                        To = this.Userrepository.Entity.Email,
                        From = ConfigurationManager.AppSettings["MailFrom"],
                        Subject = "Hemos reestablecido tu contraseña de " + ConfigurationManager.AppSettings["ApplicationName"]
                    };

                    objsend.SendMessage();

                    return true;
                }
                catch (Exception ex)
                {
                    Utils.InsertLog(
                        this.session,
                        "Error Recovery Password",
                        ex.Message + " " + ex.StackTrace);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// validates whether a user exists
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="email">email user</param>
        /// <param name="password">password user</param>
        /// <param name="phone">phone user</param>
        /// <returns>returns a <c>AuthencReturn</c></returns>
        public AuthencReturn ValidateUser(HttpContextBase context, string email, string password, string phone)
        {
            try
            {
                if (string.IsNullOrEmpty(phone))
                {
                    this.Userrepository.Entity.Email = email;
                }
                else
                {
                    this.Userrepository.Entity.Phone = phone;
                }

                this.Userrepository.Load();

                if (this.Userrepository.Entity.UserId != null)
                {
                    if ((string.IsNullOrEmpty(this.Userrepository.Entity.Password) || this.Userrepository.Entity.Password == Utils.EncriptSHA1(password)) && this.Userrepository.Entity.Active == true)
                    {
                        LanguageRepository languagerepo = new LanguageRepository(this.session);
                        languagerepo.Entity.LanguageId = this.Userrepository.Entity.LanguageId;
                        languagerepo.LoadByKey();

                        if (context.Session["lang"] == null)
                        {
                            context.Session.Add("lang", languagerepo.Entity);
                        }
                        else
                        {
                            context.Session["lang"] = languagerepo.Entity;
                        }

                        RolUserRepository objus = new RolUserRepository(this.session);
                        objus.Entity.UserId = this.Userrepository.Entity.UserId;
                        IList<RolUser> coll = objus.GetAllReadOnly();
                        List<Domain.Entities.Modul> moduls = CustomMemberShipProvider.GetModuls(this.Userrepository.Entity.UserId.Value, this.session, context);
                        string isFrontEndAdmin = "false";
                        if (moduls.Exists(t => t.ModulId == 57))
                        {
                            isFrontEndAdmin = "true";
                        }

                        StringBuilder strbRoles = new StringBuilder();

                        foreach (RolUser item in coll)
                        {
                            strbRoles.Append(item.RolId.ToString() + "-");
                        }

                        Utils.SetUserRewardAction(this.Userrepository.Entity.UserId.Value, RewardAction.UserActionType.Login, 2, 1, this.session, context, true, languagerepo.Entity);
                        this.Userrepository.Load();

                        this.Createticket(
                            strbRoles,
                            this.Userrepository.Entity.Names,
                            this.Userrepository.Entity.UserId.Value,
                            this.Userrepository.Entity.Email,
                            this.Userrepository.Entity.Image,
                            this.Userrepository.Entity.Medallos,
                            isFrontEndAdmin);

                        return AuthencReturn.USER_OK;
                    }
                    else if (!string.IsNullOrEmpty(this.Userrepository.Entity.Password) && !string.IsNullOrEmpty(phone))
                    {
                        return AuthencReturn.PHONE_PASSWORD;
                    }
                    else if (this.Userrepository.Entity.Password == Utils.EncriptSHA1(password) && this.Userrepository.Entity.Active == false)
                    {
                        return AuthencReturn.USER_BLOCKED;
                    }
                    else
                    {
                        return AuthencReturn.BAD_PASSWORD;
                    }
                }
                else if (string.IsNullOrEmpty(phone))
                {
                    return AuthencReturn.NOT_FOUND;
                }
                else
                {
                    return AuthencReturn.PHONE_AVAILABLE;
                }
            }
            catch (Exception ex)
            {
                Utils.InsertLog(
                    this.session,
                    "Error Validando Usuario",
                    ex.Message + " " + ex.StackTrace);
                return AuthencReturn.ERROR;
            }
        }
        
        /// <summary>
        /// validates whether a user exists
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="googleId">google id</param>
        /// <returns>returns a <c>AuthencReturn</c></returns>
        public AuthencReturn ValidateUserGoogle(HttpContextBase context, string googleId)
        {
            try
            {
                this.Userrepository.Entity.GoogleId = googleId;
                this.Userrepository.Load();

                if (this.Userrepository.Entity.UserId != null && this.Userrepository.Entity.Active == true)
                {
                    LanguageRepository languagerepo = new LanguageRepository(this.session);
                    languagerepo.Entity.LanguageId = this.Userrepository.Entity.LanguageId;
                    languagerepo.LoadByKey();

                    if (context.Session["lang"] == null)
                    {
                        context.Session.Add("lang", languagerepo.Entity);
                    }
                    else
                    {
                        context.Session["lang"] = languagerepo.Entity;
                    }

                    RolUserRepository objus = new RolUserRepository(this.session);
                    objus.Entity.UserId = this.Userrepository.Entity.UserId;
                    IList<RolUser> coll = objus.GetAllReadOnly();
                    List<Domain.Entities.Modul> moduls = CustomMemberShipProvider.GetModuls(this.Userrepository.Entity.UserId.Value, this.session, context);
                    string isFrontEndAdmin = "false";
                    if (moduls.Exists(t => t.ModulId == 57))
                    {
                        isFrontEndAdmin = "true";
                    }

                    StringBuilder strbRoles = new StringBuilder();

                    foreach (RolUser item in coll)
                    {
                        strbRoles.Append(item.RolId.ToString() + "-");
                    }

                    Utils.SetUserRewardAction(this.Userrepository.Entity.UserId.Value, RewardAction.UserActionType.Login, 2, 1, this.session, context, true, languagerepo.Entity);
                    this.Userrepository.Load();

                    this.Createticket(
                        strbRoles,
                        this.Userrepository.Entity.Names,
                        this.Userrepository.Entity.UserId.Value,
                        this.Userrepository.Entity.Email,
                        this.Userrepository.Entity.Image,
                        this.Userrepository.Entity.Medallos,
                        isFrontEndAdmin);

                    return AuthencReturn.USER_OK;
                }
                else if (this.Userrepository.Entity.UserId != null && this.Userrepository.Entity.Active == false)
                {
                    return AuthencReturn.USER_BLOCKED;
                }
                else
                {
                    return AuthencReturn.NOT_FOUND;
                }
            }
            catch (Exception ex)
            {
                Utils.InsertLog(
                    this.session,
                    "Error Validando Usuario",
                    ex.Message + " " + ex.StackTrace);
                return AuthencReturn.ERROR;
            }
        }

        /// <summary>
        /// validates whether a user exists
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="facebookId">face book id</param>
        /// <returns>returns a <c>AuthencReturn</c></returns>
        public AuthencReturn ValidateUserFacebook(HttpContextBase context, string facebookId)
        {
            try
            {
                this.Userrepository.Entity.FacebookId = facebookId;
                this.Userrepository.Load();

                if (this.Userrepository.Entity.UserId != null && this.Userrepository.Entity.Active == true)
                {
                    LanguageRepository languagerepo = new LanguageRepository(this.session);
                    languagerepo.Entity.LanguageId = this.Userrepository.Entity.LanguageId;
                    languagerepo.LoadByKey();

                    if (context.Session["lang"] == null)
                    {
                        context.Session.Add("lang", languagerepo.Entity);
                    }
                    else
                    {
                        context.Session["lang"] = languagerepo.Entity;
                    }

                    RolUserRepository objus = new RolUserRepository(this.session);
                    objus.Entity.UserId = this.Userrepository.Entity.UserId;
                    IList<RolUser> coll = objus.GetAllReadOnly();
                    List<Domain.Entities.Modul> moduls = CustomMemberShipProvider.GetModuls(this.Userrepository.Entity.UserId.Value, this.session, context);
                    string isFrontEndAdmin = "false";
                    if (moduls.Exists(t => t.ModulId == 57))
                    {
                        isFrontEndAdmin = "true";
                    }

                    StringBuilder strbRoles = new StringBuilder();

                    foreach (RolUser item in coll)
                    {
                        strbRoles.Append(item.RolId.ToString() + "-");
                    }

                    Utils.SetUserRewardAction(this.Userrepository.Entity.UserId.Value, RewardAction.UserActionType.Login, 2, 1, this.session, context, true, languagerepo.Entity);
                    this.Userrepository.Load();

                    this.Createticket(
                        strbRoles,
                        this.Userrepository.Entity.Names,
                        this.Userrepository.Entity.UserId.Value,
                        this.Userrepository.Entity.Email,
                        this.Userrepository.Entity.Image,
                        this.Userrepository.Entity.Medallos,
                        isFrontEndAdmin);

                    return AuthencReturn.USER_OK;
                }
                else if (this.Userrepository.Entity.UserId != null && this.Userrepository.Entity.Active == false)
                {
                    return AuthencReturn.USER_BLOCKED;
                }
                else
                {
                    return AuthencReturn.NOT_FOUND;
                }
            }
            catch (Exception ex)
            {
                Utils.InsertLog(
                    this.session,
                    "Error Validando Usuario",
                    ex.Message + " " + ex.StackTrace);
                return AuthencReturn.ERROR;
            }
        }

        /// <summary>
        /// validates whether a user exists
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="linkedinId">linked in id</param>
        /// <returns>returns a <c>AuthencReturn</c></returns>
        public AuthencReturn ValidateUserLinkedIn(HttpContextBase context, string linkedinId)
        {
            try
            {
                this.Userrepository.Entity.LinkedinId = linkedinId;
                this.Userrepository.Load();

                if (this.Userrepository.Entity.UserId != null && this.Userrepository.Entity.Active == true)
                {
                    LanguageRepository languagerepo = new LanguageRepository(this.session);
                    languagerepo.Entity.LanguageId = this.Userrepository.Entity.LanguageId;
                    languagerepo.LoadByKey();

                    if (context.Session["lang"] == null)
                    {
                        context.Session.Add("lang", languagerepo.Entity);
                    }
                    else
                    {
                        context.Session["lang"] = languagerepo.Entity;
                    }

                    RolUserRepository objus = new RolUserRepository(this.session);
                    objus.Entity.UserId = this.Userrepository.Entity.UserId;
                    IList<RolUser> coll = objus.GetAllReadOnly();
                    List<Domain.Entities.Modul> moduls = CustomMemberShipProvider.GetModuls(this.Userrepository.Entity.UserId.Value, this.session, context);
                    string isFrontEndAdmin = "false";
                    if (moduls.Exists(t => t.ModulId == 57))
                    {
                        isFrontEndAdmin = "true";
                    }

                    StringBuilder strbRoles = new StringBuilder();

                    foreach (RolUser item in coll)
                    {
                        strbRoles.Append(item.RolId.ToString() + "-");
                    }

                    Utils.SetUserRewardAction(this.Userrepository.Entity.UserId.Value, RewardAction.UserActionType.Login, 2, 1, this.session, context, true, languagerepo.Entity);
                    this.Userrepository.Load();

                    this.Createticket(
                        strbRoles,
                        this.Userrepository.Entity.Names,
                        this.Userrepository.Entity.UserId.Value,
                        this.Userrepository.Entity.Email,
                        this.Userrepository.Entity.Image,
                        this.Userrepository.Entity.Medallos,
                        isFrontEndAdmin);

                    return AuthencReturn.USER_OK;
                }
                else if (this.Userrepository.Entity.UserId != null && this.Userrepository.Entity.Active == false)
                {
                    return AuthencReturn.USER_BLOCKED;
                }
                else
                {
                    return AuthencReturn.NOT_FOUND;
                }
            }
            catch (Exception ex)
            {
                Utils.InsertLog(
                    this.session,
                    "Error Validando Usuario",
                    ex.Message + " " + ex.StackTrace);
                return AuthencReturn.ERROR;
            }
        }

        /// <summary>
        /// validates whether a user exists
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="twitterId">twitter id</param>
        /// <returns>returns a <c>AuthencReturn</c></returns>
        public AuthencReturn ValidateUserTwitter(HttpContextBase context, string twitterId)
        {
            try
            {
                this.Userrepository.Entity.TwitterId = twitterId;
                this.Userrepository.Load();

                if (this.Userrepository.Entity.UserId != null && this.Userrepository.Entity.Active == true)
                {
                    LanguageRepository languagerepo = new LanguageRepository(this.session);
                    languagerepo.Entity.LanguageId = this.Userrepository.Entity.LanguageId;
                    languagerepo.LoadByKey();

                    if (context.Session["lang"] == null)
                    {
                        context.Session.Add("lang", languagerepo.Entity);
                    }
                    else
                    {
                        context.Session["lang"] = languagerepo.Entity;
                    }

                    RolUserRepository objus = new RolUserRepository(this.session);
                    objus.Entity.UserId = this.Userrepository.Entity.UserId;
                    IList<RolUser> coll = objus.GetAllReadOnly();
                    List<Domain.Entities.Modul> moduls = CustomMemberShipProvider.GetModuls(this.Userrepository.Entity.UserId.Value, this.session, context);
                    string isFrontEndAdmin = "false";
                    if (moduls.Exists(t => t.ModulId == 57))
                    {
                        isFrontEndAdmin = "true";
                    }

                    StringBuilder strbRoles = new StringBuilder();

                    foreach (RolUser item in coll)
                    {
                        strbRoles.Append(item.RolId.ToString() + "-");
                    }

                    Utils.SetUserRewardAction(this.Userrepository.Entity.UserId.Value, RewardAction.UserActionType.Login, 2, 1, this.session, context, true, languagerepo.Entity);
                    this.Userrepository.Load();

                    this.Createticket(
                        strbRoles,
                        this.Userrepository.Entity.Names,
                        this.Userrepository.Entity.UserId.Value,
                        this.Userrepository.Entity.Email,
                        this.Userrepository.Entity.Image,
                        this.Userrepository.Entity.Medallos,
                        isFrontEndAdmin);

                    return AuthencReturn.USER_OK;
                }
                else if (this.Userrepository.Entity.UserId != null && this.Userrepository.Entity.Active == false)
                {
                    return AuthencReturn.USER_BLOCKED;
                }
                else
                {
                    return AuthencReturn.NOT_FOUND;
                }
            }
            catch (Exception ex)
            {
                Utils.InsertLog(
                    this.session,
                    "Error Validando Usuario",
                    ex.Message + " " + ex.StackTrace);
                return AuthencReturn.ERROR;
            }
        }

        /// <summary>
        /// create a cookie authentication
        /// </summary>
        /// <param name="strbRoles">user roles</param>
        /// <param name="names">name of user</param>
        /// <param name="userId">identifier user</param>
        /// <param name="mail">mail user</param>
        /// <param name="image">image user</param>
        /// <param name="medallos">points user</param>
        /// <param name="isFrontEndAdmin">is front end admin string boolean ( true || false )</param>
        public void Createticket(StringBuilder strbRoles, string names, int userId, string mail, string image, int medallos, string isFrontEndAdmin)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                names,
                DateTime.Now,
                DateTime.Now.AddMinutes(300),
                true,
                string.Concat(mail, "|", userId, "|", strbRoles.ToString().TrimEnd('-'), "|", image, "|", medallos.ToString(), "|", isFrontEndAdmin));

            string hashCookies = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
            this.context.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Updates the security ticket information
        /// </summary>
        /// <param name="medallos">user points</param>
        /// <param name="image">user image</param>
        public void Updateticket(int? medallos, string image)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string cookieName = FormsAuthentication.FormsCookieName;
                HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                string[] data = authTicket.UserData.Split('|');
                if (!string.IsNullOrEmpty(image))
                {
                    data[3] = image;
                }

                if (medallos.HasValue)
                {
                    data[4] = medallos.ToString();
                }

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    authTicket.Name,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(300),
                    true,
                    string.Join("|", data));

                string hashCookies = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
                this.context.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="user">object user</param>
        /// <returns>returns true if success</returns>
        public bool ChangeData(User user)
        {
            try
            {
                this.Userrepository.Entity = user;
                this.Userrepository.Update();
                return true;
            }
            catch (Exception ex)
            {
                Utils.InsertLog(
                    this.session,
                    "Error Update User",
                    ex.Message + " " + ex.StackTrace);
                return false;
            }
        }
    }
}
