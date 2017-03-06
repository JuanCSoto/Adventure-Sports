// <copyright file="RegistroController.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Webcore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;
    using Twitterizer;

    /// <summary>
    /// controller for the registry action
    /// </summary>
    public class RegistroController : FrontEndController
    {
        /// <summary>
        /// transform especial characters to upper case
        /// </summary>
        /// <param name="s">string value</param>
        /// <returns>encoded string value</returns>
        public static string UpperCaseUrlEncode(string s)
        {
            char[] temp = s.ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = char.ToUpper(temp[i + 1]);
                    temp[i + 2] = char.ToUpper(temp[i + 2]);
                }
            }

            return new string(temp);
        }

        /// <summary>
        /// show a form to the user to update the profile
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>returns the result to action</returns>
        /// <History>
        /// Modificado por      :   Juan Carlos Soto Cruz (JCS)
        /// Descripción cambio  :   Se implementa el manejo de multiples idiomas y se crea el pais vacio por defecto.
        /// Fecha               :   2015/11/13
        /// </History>
        public ActionResult Actualizar(string id)
        {
            this.SetLabel();
            UserInterestRepository userInterest = new UserInterestRepository(SessionCustom);
            userInterest.Entity.UserId = ((Business.Services.CustomPrincipal)User).UserId;

            DocumentTypeRepository documentType = new DocumentTypeRepository(SessionCustom);
            InterestRepository interest = new InterestRepository(SessionCustom);

            UserRepository userRepository = new UserRepository(SessionCustom);
            userRepository.Entity.UserId = ((Business.Services.CustomPrincipal)User).UserId;
            userRepository.LoadByKey();

            int selected = 0;
            if (userRepository.Entity.CountryId.HasValue)
            {
                selected = userRepository.Entity.CountryId.Value;
            }

            CityRepository city = new CityRepository(SessionCustom);
            List<Domain.Entities.City> cities = city.GetAll().Where(c => c.CountryID == selected).OrderBy(c => CurrentLanguage.LanguageId == 1 ? c.NameEn : c.NameEs).ToList();

            Country paisDefecto = new Country();
            paisDefecto.CountryID = 0;

            List<Country> paises = new List<Country>();
            paises.Add(paisDefecto);

            CountryRepository country = new CountryRepository(SessionCustom);

            foreach (Country pais in country.GetAll().OrderBy(c => CurrentLanguage.LanguageId == 1 ? c.NameEn : c.NameEs))
            {
                paises.Add(pais);
            }

            userRepository.Entity.CollCity = cities;
            userRepository.Entity.CollCountry = paises;
            userRepository.Entity.CollDocumentType = documentType.GetAll();
            userRepository.Entity.CollInterest = interest.GetAll();
            userRepository.Entity.CollUserInterest = userInterest.GetAll();

            ViewBag.Option = id;

            return this.View(userRepository.Entity);
        }

        /// <summary>
        /// receive a form to update the user profile
        /// </summary>
        /// <param name="user">user object</param>
        /// <param name="imageName">image name</param>
        /// <param name="interest">list of interests</param>
        /// <param name="cityUpdate">city update</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Actualizar(User user, string imageName, int?[] interest, bool cityUpdate)
        {
            this.SetLabel();
            if (!string.IsNullOrEmpty(imageName))
            {
                UserRepository userRepository = new UserRepository(SessionCustom);
                userRepository.Entity.UserId = ((Business.Services.CustomPrincipal)User).UserId;
                userRepository.LoadByKey();

                user.Medallos = userRepository.Entity.Medallos;
                user.UserId = userRepository.Entity.UserId;

                if (!cityUpdate)
                {
                    user.CityId = userRepository.Entity.CityId;
                    user.CountryId = userRepository.Entity.CountryId;
                }

                userRepository.Entity = user;
                userRepository.Entity.Image = imageName;
                ((Business.Services.CustomPrincipal)User).Image = imageName;
                if (user.Password != null)
                {
                    userRepository.Entity.Password = Business.Utils.EncriptSHA1(user.Password);
                }

                if (imageName[0] == '~')
                {
                    imageName = System.IO.Path.GetFileName(imageName);
                    string serverMap = Server.MapPath("~");
                    string origin = serverMap + @"\resources\temporal\" + imageName;
                    if (System.IO.File.Exists(origin))
                    {
                        System.IO.File.Move(origin, serverMap + @"\files\imagesuser\" + imageName);
                    }
                }

                Business.Services.CustomMemberShipProvider membership = new Business.Services.CustomMemberShipProvider(this.SessionCustom, ControllerContext.HttpContext);
                membership.Updateticket(null, userRepository.Entity.Image);

                userRepository.Update();

                UserInterestRepository userInterest = new UserInterestRepository(this.SessionCustom);
                userInterest.Entity.UserId = ((Business.Services.CustomPrincipal)User).UserId;
                userInterest.Delete();
                if (interest != null && interest.Length > 0)
                {
                    foreach (int? id in interest)
                    {
                        userInterest.Entity.InterestId = id;
                        userInterest.Insert();
                    }
                }

                this.UpdateReward(user, cityUpdate, interest);
            }

            return this.View();
        }

        /// <summary>
        /// closes the user session on the site
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            HttpContext.Session.RemoveAll();
            HttpContext.Session.Abandon();

            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// facebook authentication
        /// </summary>
        /// <param name="token">facebook security token</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        [HttpPost]
        public JsonResult Facebook(string token)
        {
            Domain.Entities.FrontEnd.FacebookUser facebookUser = new Domain.Entities.FrontEnd.FacebookUser();
            bool result = false;
            int authenticated = 0;
            facebookUser = Utils.GetFacebookUser(token);
            if (string.IsNullOrEmpty(facebookUser.error))
            {
                result = true;

                Business.Services.CustomMemberShipProvider authenticate = new Business.Services.CustomMemberShipProvider(SessionCustom, HttpContext);
                CustomMemberShipProvider.AuthencReturn resultAuthenticate = authenticate.ValidateUserFacebook(HttpContext, facebookUser.id.ToString());

                if (resultAuthenticate == CustomMemberShipProvider.AuthencReturn.USER_OK)
                {
                    authenticated = 1;
                }

                if (resultAuthenticate == CustomMemberShipProvider.AuthencReturn.USER_BLOCKED)
                {
                    authenticated = 2;
                }
            }

            return this.Json(new { result = result, authenticated = authenticated });
        }

        /// <summary>
        /// Return a list of cities by country id
        /// </summary>
        /// <param name="countryId">country id</param>
        /// <returns>A JSON object with the cities list</returns>
        /// <History>
        /// Modificado por      :   Juan Carlos Soto Cruz (JCS)
        /// Descripción cambio  :   Se implementa el manejo de multiples idiomas.
        /// Fecha               :   2015/11/13
        /// </History>
        public JsonResult GetCities(int countryId)
        {
            CityRepository city = new CityRepository(SessionCustom);
            List<Domain.Entities.City> cities = city.GetAll().Where(c => c.CountryID == countryId).OrderBy(c => CurrentLanguage.LanguageId == 1 ? c.NameEn : c.NameEs).ToList();

            return this.Json(new { result = true, cities = cities, lenguaje = CurrentLanguage.LanguageId });
        }

        /// <summary>
        /// twitter callback point
        /// </summary>
        /// <returns>A JSON object with the security token</returns>
        [HttpPost]
        public JsonResult GetLinkTwitter()
        {
            Uri url = Request.Url;

            string callbackUrl = ("http://" + Request.Url.Host + Request.ApplicationPath + "/registro/twittergettoken").TrimEnd('/');
            string requestToken = OAuthUtility.GetRequestToken(
                ConfigurationManager.AppSettings["TwitterApiKey"],
                ConfigurationManager.AppSettings["TwitterConsumerSecret"],
                callbackUrl).Token;

            Uri authenticationUri = OAuthUtility.BuildAuthorizationUri(requestToken);
            return this.Json(new { url = authenticationUri.AbsoluteUri });
        }

        /// <summary>
        /// google callback point
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Google()
        {
            return this.View();
        }

        /// <summary>
        /// google callback point
        /// </summary>
        /// <param name="query">google query</param>
        /// <returns>A JSON with the google information</returns>
        [HttpPost]
        public JsonResult Google(string query)
        {
            Domain.Entities.FrontEnd.GoogleUser googleUser = new Domain.Entities.FrontEnd.GoogleUser();
            bool result = false;
            string token = string.Empty;
            string picture = string.Empty;
            int authenticated = 0;
            try
            {
                int start = query.IndexOf("access_token=");
                int end = query.IndexOf('&');
                if (start >= 0 && end >= 13)
                {
                    token = query.Substring(start + 13, end - 13);
                    googleUser = Utils.GetGoogleUser(token);
                    if (string.IsNullOrEmpty(googleUser.error))
                    {
                        result = true;

                        Business.Services.CustomMemberShipProvider authenticate = new Business.Services.CustomMemberShipProvider(SessionCustom, HttpContext);
                        CustomMemberShipProvider.AuthencReturn resultAuthenticate = authenticate.ValidateUserGoogle(HttpContext, googleUser.id);

                        if (resultAuthenticate == CustomMemberShipProvider.AuthencReturn.USER_OK)
                        {
                            authenticated = 1;
                        }

                        if (resultAuthenticate == CustomMemberShipProvider.AuthencReturn.USER_BLOCKED)
                        {
                            authenticated = 2;
                        }
                    }
                }
            }
            catch
            {
            }

            return this.Json(new { result = result, token = token, picture = googleUser.picture, authenticated = authenticated, name = googleUser.name, mail = googleUser.email });
        }

        /// <summary>
        /// Show the login view form
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Ingreso()
        {
            this.SetLabel();
            return this.View(true);
        }

        /// <summary>
        /// receive the login view form
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="password">user password</param>
        /// <param name="phone">user phone</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        [HttpPost]
        public JsonResult Ingreso(string email, string password, string phone)
        {
            bool result = false;
            int authenticated = 0;
            Business.Services.CustomMemberShipProvider authenticate = new Business.Services.CustomMemberShipProvider(SessionCustom, HttpContext);
            CustomMemberShipProvider.AuthencReturn resultAuthenticate = authenticate.ValidateUser(HttpContext, email, password, phone);

            switch (resultAuthenticate)
            {
                case CustomMemberShipProvider.AuthencReturn.USER_OK:
                    result = true;
                    authenticated = 1;
                    this.UpdateLanguage();
                    break;

                case CustomMemberShipProvider.AuthencReturn.USER_BLOCKED:
                    result = true;
                    authenticated = 2;
                    break;

                case CustomMemberShipProvider.AuthencReturn.PHONE_PASSWORD:
                    result = true;
                    authenticated = 3;
                    break;

                case CustomMemberShipProvider.AuthencReturn.PHONE_AVAILABLE:
                    result = true;
                    authenticated = 4;
                    break;
            }

            return this.Json(new { result = result, authenticated = authenticated });
        }

        /// <summary>
        /// linked in authentication
        /// </summary>
        /// <param name="token">linked in security token</param>
        /// <param name="linkedInUserId">linked in user id</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        [HttpPost]
        public JsonResult LinkedIn(string token, string linkedInUserId)
        {
            Domain.Entities.FrontEnd.LinkedInUser linkedInUser = new Domain.Entities.FrontEnd.LinkedInUser();
            bool result = false;
            int authenticated = 0;
            ////TODO Descomentar
            ////linkedInUser = Utils.GetLinkedInUser(token);
            ////TODO Eliminar
            if (linkedInUserId != null)
            {
                linkedInUser.id = linkedInUserId;
                linkedInUser.error = string.Empty;
            }

            ////TODO Fin Eliminar
            if (string.IsNullOrEmpty(linkedInUser.error))
            {
                result = true;

                Business.Services.CustomMemberShipProvider authenticate = new Business.Services.CustomMemberShipProvider(SessionCustom, HttpContext);
                CustomMemberShipProvider.AuthencReturn resultAuthenticate = authenticate.ValidateUserLinkedIn(HttpContext, linkedInUser.id.ToString());

                if (resultAuthenticate == CustomMemberShipProvider.AuthencReturn.USER_OK)
                {
                    authenticated = 1;
                }

                if (resultAuthenticate == CustomMemberShipProvider.AuthencReturn.USER_BLOCKED)
                {
                    authenticated = 2;
                }
            }

            return this.Json(new { result = result, authenticated = authenticated });
        }

        /// <summary>
        /// if user is not authenticated return an error
        /// </summary>
        /// <returns>an error JSON object page</returns>
        [HttpGet]
        public ActionResult NoAutenticado()
        {
            this.SetLabel();
            return this.View();
        }

        /// <summary>
        /// show a form for the user to recover the password
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Recuperar()
        {
            return this.View();
        }

        /// <summary>
        /// receive a password recovery form
        /// </summary>
        /// <param name="email">user email</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        [HttpPost]
        public JsonResult Recuperar(string email)
        {
            bool result = false;
            UserRepository user = new UserRepository(SessionCustom);
            user.Entity.Email = email;
            user.Load();

            if (user.Entity.UserId.HasValue)
            {
                DateTime now = DateTime.Now;

                string token = Utils.EncriptSHA1(email + now.ToString("yyyyMMddHHmmss") + Utils.SecretWord());
                string link = ("http://" + Request.Url.Authority + Request.ApplicationPath).TrimEnd('/') + "/?token=" + token;
                this.SessionCustom.Begin();
                RecoveryTokenRepository recovery = new RecoveryTokenRepository(SessionCustom);
                recovery.Entity.Token = token;
                recovery.Entity.Creationdate = now;
                recovery.Entity.Used = false;
                recovery.Entity.UserId = user.Entity.UserId;
                recovery.Insert();

                string body = string.Concat("Hola ", user.Entity.Names, ", <br/>Para cambiar tu contraseña utiliza el enlace a continuación<br/><a href='", link, "'>Has clic aquí para recuperar tu contraseña</a>.<br/> Gracias, Cities for Life");
                body = Utils.FormatEmail(body, string.Empty);

                SendMail mail = new SendMail();
                mail.From = ConfigurationManager.AppSettings["RecoveryMailFrom"];
                mail.To = email;
                mail.Subject = "Cities for Life - Recordar Contraseña";
                mail.Body = body;
                mail.SendMessage();

                SessionCustom.Commit();

                result = true;
            }

            return this.Json(new { result = result });
        }

        /// <summary>
        /// show a reset password form
        /// </summary>
        /// <param name="token">security token for the password reset</param>
        /// <returns>returns the result to action</returns>
        /// <History>
        /// Modificado por      :   Juan Carlos Soto Cruz (JCS)
        /// Descripción cambio  :   Se adiciona el llamado al metodo SetLabel() para que sean llenados los label de la vista de reseteo de contraseña.
        /// Fecha               :   2015/11/06
        /// </History>
        [HttpGet]
        public ActionResult Reset(string token)
        {
            RecoveryTokenRepository recovery = new RecoveryTokenRepository(SessionCustom);
            recovery.Entity.Token = token;
            recovery.LoadByKey();

            if (recovery.Entity.Creationdate >= DateTime.Now.AddHours(-1) && !recovery.Entity.Used.Value)
            {
                this.SetLabel();
                return this.View();
            }

            return null;
        }

        /// <summary>
        /// receive a reset password form
        /// </summary>
        /// <param name="token">security token for the password reset</param>
        /// <param name="password">new password</param>
        /// <param name="repassword">confirm new password</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        [HttpPost]
        public JsonResult Reset(string token, string password, string repassword)
        {
            bool result = false;
            SessionCustom.Begin();
            RecoveryTokenRepository recovery = new RecoveryTokenRepository(SessionCustom);
            recovery.Entity.Token = token;
            recovery.Load();

            if (recovery.Entity.Creationdate >= DateTime.Now.AddHours(-1) && password == repassword)
            {
                UserRepository userRepository = new UserRepository(SessionCustom);
                userRepository.Entity.UserId = recovery.Entity.UserId;
                userRepository.Entity.Password = Utils.EncriptSHA1(password);
                userRepository.Update();

                recovery.Entity.Used = true;
                recovery.Update();

                result = true;
            }

            SessionCustom.Commit();

            return this.Json(new { result = result });
        }

        /// <summary>
        /// Send a welcome email to the new user
        /// </summary>
        /// <param name="name">user name</param>
        /// <param name="email">user email</param>
        /// <param name="language">user language</param>
        public void SendWelcomeMail(string name, string email, int? language)
        {
            string folderhtml = string.Empty;
            IdeaRepository idea = new IdeaRepository(SessionCustom);
            UserRepository user = new UserRepository(SessionCustom);
            int totalParticipants;
            user.Participants(0, out totalParticipants);
            this.ViewBag.CountIdeas = idea.IdeasCountAll();
            this.ViewBag.CountUsers = totalParticipants;
            this.ViewBag.UserName = name;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "~/Views/Mail/Welcome.cshtml");
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                folderhtml = sw.GetStringBuilder().ToString();
            }

            SendMail mail = new SendMail();
            mail.From = ConfigurationManager.AppSettings["MailContact"];
            mail.To = email;
            mail.Body = folderhtml;
            mail.Subject = Resources.Extend.Messages.SUBJECTWELCOME;
            mail.SendMessage();
        }

        /// <summary>
        /// twitter callback point
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult TwitterGetToken()
        {
            return this.View();
        }

        /// <summary>
        /// twitter in authentication
        /// </summary>
        /// <param name="oauthtoken">twitter security token</param>
        /// <param name="oauthverifier">twitter verifier</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        [HttpPost]
        public JsonResult TwitterInfo(string oauthtoken, string oauthverifier)
        {
            OAuthTokenResponse accessTokenResponse = OAuthUtility.GetAccessToken(
                ConfigurationManager.AppSettings["TwitterApiKey"],
                ConfigurationManager.AppSettings["TwitterConsumerSecret"],
                oauthtoken,
                oauthverifier);

            System.Collections.Generic.Dictionary<string, object> objinformation = Utils.GetTwitterUser(accessTokenResponse.ScreenName);

            Business.Services.CustomMemberShipProvider authenticate = new Business.Services.CustomMemberShipProvider(SessionCustom, HttpContext);
            CustomMemberShipProvider.AuthencReturn resultAuthenticate = authenticate.ValidateUserTwitter(HttpContext, accessTokenResponse.UserId.ToString());

            Response.Cookies.Add(new HttpCookie("user-name", objinformation["name"].ToString()));

            int authenticated = 0;

            if (resultAuthenticate == CustomMemberShipProvider.AuthencReturn.USER_OK)
            {
                authenticated = 1;
            }

            if (resultAuthenticate == CustomMemberShipProvider.AuthencReturn.USER_BLOCKED)
            {
                authenticated = 2;
            }

            return this.Json(new { token = accessTokenResponse.ScreenName, image = objinformation["profile_image_url"], authenticated = authenticated });
        }

        /// <summary>
        /// update a social link for the current user
        /// </summary>
        /// <param name="google">google link</param>
        /// <param name="twitter">twitter link</param>
        /// <param name="linkedin">linked in link</param>
        /// <param name="facebook">facebook link</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        public JsonResult UpdateSocialLink(string google, string twitter, string linkedin, string facebook)
        {
            bool result = false;
            bool empty = true;
            UserRepository user = new UserRepository(SessionCustom);
            user.Entity.UserId = ((Business.Services.CustomPrincipal)User).UserId;
            user.LoadByKey();
            if (!string.IsNullOrEmpty(google))
            {
                empty = false;
                string regular = @"^(https?)://(plus.)?(google).com+([/?].*)?$";
                Regex rgx = new Regex(regular, RegexOptions.IgnoreCase);
                MatchCollection matches = rgx.Matches(google);
                if (matches.Count > 0)
                {
                    user.Entity.GoogleLink = google;
                    result = true;
                }
            }

            if (!string.IsNullOrEmpty(twitter))
            {
                empty = false;
                string regular = @"^(https?)://(www.)?(twitter).com+([/?].*)?$";
                Regex rgx = new Regex(regular, RegexOptions.IgnoreCase);
                MatchCollection matches = rgx.Matches(twitter);
                if (matches.Count > 0)
                {
                    user.Entity.TwitterLink = twitter;
                    result = true;
                }
            }

            if (!string.IsNullOrEmpty(linkedin))
            {
                empty = false;
                string regular = @"^(https?)://(www.)?(linkedin).com+([/?].*)?$";
                Regex rgx = new Regex(regular, RegexOptions.IgnoreCase);
                MatchCollection matches = rgx.Matches(linkedin);
                if (matches.Count > 0)
                {
                    user.Entity.LinkedinLink = linkedin;
                    result = true;
                }
            }

            if (!string.IsNullOrEmpty(facebook))
            {
                empty = false;
                string regular = @"^(https?)://(www.)?(facebook).com+([/?].*)?$";
                Regex rgx = new Regex(regular, RegexOptions.IgnoreCase);
                MatchCollection matches = rgx.Matches(facebook);
                if (matches.Count > 0)
                {
                    user.Entity.FacebookLink = facebook;
                    result = true;
                }
            }

            if (empty)
            {
                if (google != null)
                {
                    user.Entity.GoogleLink = string.Empty;
                    result = true;
                }

                if (twitter != null)
                {
                    user.Entity.TwitterLink = string.Empty;
                    result = true;
                }

                if (linkedin != null)
                {
                    user.Entity.LinkedinLink = string.Empty;
                    result = true;
                }

                if (facebook != null)
                {
                    user.Entity.FacebookLink = string.Empty;
                    result = true;
                }
            }

            user.Update();

            return this.Json(new { result = result });
        }

        /// <summary>
        /// show a form to upload a user image profile
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult UploadUserImage()
        {
            return this.View();
        }

        /// <summary>
        /// receive a form to upload a user image profile
        /// </summary>
        /// <param name="userFile">image file</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult UploadUserImage(HttpPostedFileBase userFile)
        {
            string serverMap = Server.MapPath("~");
            string fileName = Business.Utils.UploadFile(userFile, serverMap, @"resources\temporal\", string.Empty);

            ImageResize imageResize = new ImageResize(serverMap);
            imageResize.Width = 100;
            imageResize.Height = 100;
            imageResize.Prefix = "58x58-";
            imageResize.Resize(serverMap + @"\resources\temporal\" + fileName, ImageResize.TypeResize.Proportional);

            fileName = "58x58-" + fileName;

            return this.View((object)fileName);
        }

        /// <summary>
        /// Show a form to registry the user information (new user)
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Usuario()
        {
            this.SetLabel();
            Domain.Entities.User user = new Domain.Entities.User();
            this.BindInfo(user);

            return this.View(user);
        }

        /// <summary>
        /// Receive the new user information form
        /// </summary>
        /// <param name="user">user object</param>
        /// <param name="imageName">image name</param>
        /// <param name="googleToken">google token</param>
        /// <param name="facebookToken">facebook token</param>
        /// <param name="linkedinToken">linked in token</param>
        /// <param name="twitterToken">twitter token</param>
        /// <param name="captchaValue">captcha value</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Usuario(Domain.Entities.User user, string imageName, string googleToken, string facebookToken, string linkedinToken, string twitterToken, string captchaValue)
        {
            if (user.LanguageId != null)
            {
                this.SetLanguage(user.LanguageId.Value);
            }

            if (!string.IsNullOrEmpty(imageName))
            {
                this.SetLabel();
                Business.Services.CustomMemberShipProvider authenticate = new Business.Services.CustomMemberShipProvider(SessionCustom, HttpContext);
                string rawPassword = user.Password;
                bool social = false;
                user.Password = string.IsNullOrEmpty(user.Password) ? null : Business.Utils.EncriptSHA1(rawPassword);
                user.Email = string.IsNullOrEmpty(user.Email) ? null : user.Email;
                user.Joindate = DateTime.Now;
                user.LanguageId = user.LanguageId;
                user.Image = imageName;
                user.Active = true;
                user.News = true;
                user.Medallos = 0;
                user.Policy = user.Terms;
                int userId = 0; //// authenticate.CreateUser(user);

                if (string.IsNullOrEmpty(user.Password) && string.IsNullOrEmpty(user.Email))
                {
                    if (!CaptchaController.IsValidCaptchaValue(captchaValue))
                    {
                        userId = -2;
                    }
                    else
                    {
                        userId = authenticate.CreateUser(user);
                    }
                }
                else
                {
                    userId = authenticate.CreateUser(user);
                }

                if (userId <= -1)
                {
                    if (userId == -1)
                    {
                        ModelState.AddModelError("Email", "El correo electrónico ya existe en el sistema.");
                    }

                    if (userId == -2)
                    {
                        ModelState.AddModelError("captchaValue", "El valor del captcha no es correcto.");
                    }

                    if (!string.IsNullOrEmpty(googleToken))
                    {
                        Response.Cookies.Add(new HttpCookie("google-token", googleToken));
                    }

                    if (!string.IsNullOrEmpty(facebookToken))
                    {
                        Response.Cookies.Add(new HttpCookie("facebook-token", facebookToken));
                    }

                    if (!string.IsNullOrEmpty(linkedinToken))
                    {
                        Response.Cookies.Add(new HttpCookie("linkedin-token", linkedinToken));
                    }

                    if (!string.IsNullOrEmpty(twitterToken))
                    {
                        Response.Cookies.Add(new HttpCookie("twitter-token", twitterToken));
                    }

                    if (!string.IsNullOrEmpty(user.Phone))
                    {
                        Response.Cookies.Add(new HttpCookie("phone-registry", user.Phone));
                    }

                    this.BindInfo(user);

                    if (!string.IsNullOrEmpty(googleToken))
                    {
                        Domain.Entities.FrontEnd.GoogleUser googleUser = Utils.GetGoogleUser(googleToken);
                        if (string.IsNullOrEmpty(googleUser.error))
                        {
                            user.GoogleId = googleUser.id;
                        }
                    }

                    if (!string.IsNullOrEmpty(facebookToken))
                    {
                        Domain.Entities.FrontEnd.FacebookUser facebookUser = Utils.GetFacebookUser(facebookToken);
                        if (string.IsNullOrEmpty(facebookUser.error))
                        {
                            user.FacebookId = facebookUser.id.ToString();
                        }
                    }

                    if (!string.IsNullOrEmpty(linkedinToken))
                    {
                        Domain.Entities.FrontEnd.LinkedInUser linkedInUser = new Domain.Entities.FrontEnd.LinkedInUser();
                        if (string.IsNullOrEmpty(linkedInUser.error))
                        {
                            user.LinkedinId = linkedInUser.id.ToString();
                        }
                    }

                    if (!string.IsNullOrEmpty(twitterToken))
                    {
                        var accessTokenResponse = Utils.GetTwitterUser(twitterToken);
                        user.TwitterId = accessTokenResponse["id"].ToString();
                    }

                    return this.View(user);
                }

                Utils.SetUserRewardAction(userId, RewardAction.UserActionType.Registry, 21, 1, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage);

                if (imageName[0] == '~')
                {
                    imageName = System.IO.Path.GetFileName(imageName);
                    string serverMap = Server.MapPath("~");
                    string origin = serverMap + @"\resources\temporal\" + imageName;
                    if (System.IO.File.Exists(origin))
                    {
                        System.IO.File.Move(origin, serverMap + @"\files\imagesuser\" + imageName);
                    }
                }

                UserRepository userRepository = new UserRepository(SessionCustom);
                userRepository.Entity.UserId = userId;
                userRepository.Entity.Image = user.Image;
                userRepository.Entity.LocationId = user.LocationId;
                userRepository.Entity.LocationType = user.LocationType;
                userRepository.Entity.Medallos = 21;

                if (!string.IsNullOrEmpty(googleToken))
                {
                    Domain.Entities.FrontEnd.GoogleUser googleUser = Utils.GetGoogleUser(googleToken);
                    if (string.IsNullOrEmpty(googleUser.error))
                    {
                        userRepository.Entity.GoogleId = googleUser.id;
                        userRepository.Update();
                        authenticate.ValidateUserGoogle(this.HttpContext, googleUser.id);
                        social = true;
                        user.GoogleId = googleUser.id;
                    }
                }

                if (!string.IsNullOrEmpty(facebookToken))
                {
                    Domain.Entities.FrontEnd.FacebookUser facebookUser = Utils.GetFacebookUser(facebookToken);
                    if (string.IsNullOrEmpty(facebookUser.error))
                    {
                        userRepository.Entity.FacebookId = facebookUser.id.ToString();
                        userRepository.Update();
                        authenticate.ValidateUserFacebook(this.HttpContext, facebookUser.id.ToString());
                        social = true;
                        user.FacebookId = facebookUser.id.ToString();
                    }
                }

                if (!string.IsNullOrEmpty(linkedinToken))
                {
                    ////Domain.Entities.FrontEnd.LinkedInUser linkedInUser = Utils.GetLinkedInUser(linkedinToken);
                    ////TODO Eliminar
                    Domain.Entities.FrontEnd.LinkedInUser linkedInUser = new Domain.Entities.FrontEnd.LinkedInUser();
                    linkedInUser.id = linkedinToken;
                    linkedInUser.error = string.Empty;
                    ////TODO Fin Eliminar
                    if (string.IsNullOrEmpty(linkedInUser.error))
                    {
                        userRepository.Entity.LinkedinId = linkedInUser.id.ToString();
                        userRepository.Update();
                        authenticate.ValidateUserLinkedIn(this.HttpContext, linkedInUser.id.ToString());
                        social = true;
                        user.LinkedinId = linkedInUser.id.ToString();
                    }
                }

                if (!string.IsNullOrEmpty(twitterToken))
                {
                    if (imageName[0] != '~')
                    {
                        WebClient client = new WebClient();
                        string newfile = "58x58-" + DateTime.Now.ToString("ddmmyyyyhhmmss") + Path.GetExtension(imageName);
                        string serverMap = Server.MapPath("~");
                        string savePath = serverMap + @"\files\imagesuser\" + newfile;
                        userRepository.Entity.Image = "~/files/imagesuser/" + newfile;
                        client.DownloadFile(imageName, savePath);
                    }

                    var accessTokenResponse = Utils.GetTwitterUser(twitterToken);
                    userRepository.Entity.TwitterId = accessTokenResponse["id"].ToString();
                    userRepository.Update();
                    authenticate.ValidateUserTwitter(this.HttpContext, userRepository.Entity.TwitterId);
                    social = true;
                    user.TwitterId = userRepository.Entity.TwitterId;
                }

                if (!social)
                {
                    userRepository.Update();
                    authenticate.ValidateUser(this.HttpContext, user.Email, rawPassword, null);
                }

                user.UserId = userId;
                userRepository.UserSettingInit(userId);

                this.SendWelcomeMail(user.Names, user.Email, user.LanguageId);
            }
            else
            {
                user = null;
            }

            return this.View(user);
        }

        /// <summary>
        /// show and send a demo of the welcome email to the user
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult WelcomeMail()
        {
            string folderhtml = string.Empty;

            IdeaRepository idea = new IdeaRepository(SessionCustom);
            UserRepository user = new UserRepository(SessionCustom);
            int totalParticipants;
            user.Participants(0, out totalParticipants);
            ViewBag.CountIdeas = idea.IdeasCountAll();
            ViewBag.CountUsers = totalParticipants;
            ViewBag.UserName = User.Identity.Name;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "~/Views/Mail/Welcome.cshtml");
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                folderhtml = sw.GetStringBuilder().ToString();
            }

            SendMail mail = new SendMail();
            mail.From = ConfigurationManager.AppSettings["MailContact"];
            mail.To = ((Business.Services.CustomPrincipal)User).Email;
            mail.Body = folderhtml;
            mail.Subject = "Mi medellín - semanal";
            mail.SendMessage();

            return this.RedirectToAction("index", "home");
        }

        /// <summary>
        /// Bind the user information
        /// </summary>
        /// <param name="user">user object</param>
        private void BindInfo(Domain.Entities.User user)
        {
            NeighborhoodRepository neighborhood = new NeighborhoodRepository(SessionCustom);
            neighborhood.Entity.CityId = 1107; //// Medellin

            DepartmentRepository department = new DepartmentRepository(SessionCustom);
            department.Entity.CountryId = 1; ////Colombia
            var departments = department.GetAll();

            CityRepository city = new CityRepository(SessionCustom);
            List<Domain.Entities.City> cities = new List<City>(); // city.GetAll().Where(c => departments.FindIndex(x => x.DepartmentId == c.DepartmentId) >= 0).OrderBy(c => c.Name).ToList();
            CountryRepository country = new CountryRepository(SessionCustom);

            user.CollNeighborhood = neighborhood.GetAll().OrderBy(n => n.Name).ToList();
            user.CollCity = cities;
            user.CollCountry = country.GetAll().OrderBy(c => c.NameEs).ToList();
        }

        /// <summary>
        /// Set label vistas
        /// </summary>
        /// <History>
        /// Modificado por      :   Juan Carlos Soto Cruz (JCS)
        /// Descripción cambio  :   Se inicializan los ViewBag (TXTTERMCOND, TXTPRIVPOLI, TXTCOCREATION) para los Terminos y Condiciones y la Politica de Privacidad.
        ///                         Adicionalmente se centralizan en una sola region para mejor legibilidad del codigo.
        /// Fecha               :   2015/11/04
        /// Modificado por      :   Juan Carlos Soto Cruz (JCS)
        /// Descripción cambio  :   Se centralizan los ViewBag para cambio de contraseña en una sola region.
        /// Fecha               :   2015/11/06
        /// </History>
        private void SetLabel()
        {
            LabelManagement objlabel = new LabelManagement(SessionCustom, HttpContext);

            ViewBag.ACCEPT = objlabel.GetLabelByName("ACCEPT", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTCONFPASS = objlabel.GetLabelByName("TXTCONFPASS", CurrentLanguage.LanguageId.Value);

            ViewBag.TXTCHAPASS = objlabel.GetLabelByName("TXTCHAPASS", CurrentLanguage.LanguageId.Value);
            ViewBag.COUNTRY = objlabel.GetLabelByName("COUNTRY", CurrentLanguage.LanguageId.Value);
            ViewBag.EMAIL = objlabel.GetLabelByName("EMAIL", CurrentLanguage.LanguageId.Value);
            ViewBag.INTERESTS = objlabel.GetLabelByName("INTERESTS", CurrentLanguage.LanguageId.Value);
            ViewBag.AGES = objlabel.GetLabelByName("AGES", CurrentLanguage.LanguageId.Value);
            ViewBag.PROFESSION = objlabel.GetLabelByName("PROFESSION", CurrentLanguage.LanguageId.Value);
            ViewBag.PHONE = objlabel.GetLabelByName("PHONE", CurrentLanguage.LanguageId.Value);
            ViewBag.COUNTRY = objlabel.GetLabelByName("COUNTRY", CurrentLanguage.LanguageId.Value);
            ViewBag.CITY = objlabel.GetLabelByName("CITY", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTIFYOUHAVE = objlabel.GetLabelByName("TXTIFYOUHAVE", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTREMPASS = objlabel.GetLabelByName("TXTREMPASS", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTENTEREMAIL = objlabel.GetLabelByName("TXTENTEREMAIL", CurrentLanguage.LanguageId.Value);

            ////#region ViewBags para Cambio de contraseña

            ViewBag.TXTNEWPASS = objlabel.GetLabelByName("TXTNEWPASS", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTENTPASS = objlabel.GetLabelByName("TXTENTPASS", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTWRITNEW = objlabel.GetLabelByName("TXTWRITNEW", CurrentLanguage.LanguageId.Value);
            ViewBag.CHANGE = objlabel.GetLabelByName("CHANGE", CurrentLanguage.LanguageId.Value);

            ////#endregion ViewBags para Cambio de contraseña

            ViewBag.CREATEACCOUNT = objlabel.GetLabelByName("CREATEACCOUNT", CurrentLanguage.LanguageId.Value);
            ViewBag.PROFILEPICTURE = objlabel.GetLabelByName("PROFILEPICTURE", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTSELIMG = objlabel.GetLabelByName("TXTSELIMG", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTFISRTLAST = objlabel.GetLabelByName("TXTFISRTLAST", CurrentLanguage.LanguageId.Value);
            ViewBag.PASSWORD = objlabel.GetLabelByName("PASSWORD", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTCONFPASS = objlabel.GetLabelByName("TXTCONFPASS", CurrentLanguage.LanguageId.Value);
            ViewBag.MOVIL = objlabel.GetLabelByName("MOVIL", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTLOGWITH = objlabel.GetLabelByName("TXTLOGWITH", CurrentLanguage.LanguageId.Value);
            ViewBag.CLOSESSION = objlabel.GetLabelByName("CLOSESSION", CurrentLanguage.LanguageId.Value);
            ViewBag.LOGIN = objlabel.GetLabelByName("LOGIN", CurrentLanguage.LanguageId.Value);
            ViewBag.REGISTER = objlabel.GetLabelByName("REGISTER", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTMAXIMAGE = objlabel.GetLabelByName("TXTMAXIMAGE", CurrentLanguage.LanguageId.Value);
            ViewBag.PROFILEPI = objlabel.GetLabelByName("PROFILEPI", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTEDITPROF = objlabel.GetLabelByName("TXTEDITPROF", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTSELIMG = objlabel.GetLabelByName("TXTSELIMG", CurrentLanguage.LanguageId.Value);
            ViewBag.IMAGEMX = objlabel.GetLabelByName("IMAGEMX", CurrentLanguage.LanguageId.Value);
            ViewBag.NAMEANDLAST = objlabel.GetLabelByName("NAMEANDLAST", CurrentLanguage.LanguageId.Value);

            ////#region ViewBags para Politica de privacidad y Terminos y Condiciones

            ViewBag.TXTHAVEACCEPT = objlabel.GetLabelByName("TXTHAVEACCEPT", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTTERMCOND = objlabel.GetLabelByName("TXTTERMCOND", CurrentLanguage.LanguageId.Value);
            ViewBag.ANDTHE = objlabel.GetLabelByName("ANDTHE", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTPRIVPOLI = objlabel.GetLabelByName("TXTPRIVPOLI", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTCOCREATION = objlabel.GetLabelByName("TXTCOCREATION", CurrentLanguage.LanguageId.Value);

            ////#endregion ViewBags para Politica de privacidad y Terminos y Condiciones
        }

        /// <summary>
        /// Cambia el Lenguaje del Sistema
        /// </summary>
        /// <param name="id">User Language</param>
        private void SetLanguage(int id)
        {
            try
            {
                LanguageRepository languagerepo = new LanguageRepository(SessionCustom);
                languagerepo.Entity.LanguageId = id;
                languagerepo.LoadByKey();

                if (HttpContext.Session["lang"] == null)
                {
                    HttpContext.Session.Add("lang", languagerepo.Entity);
                    this.CurrentLanguage = (Language)HttpContext.Session["lang"];
                }
                else
                {
                    HttpContext.Session["lang"] = languagerepo.Entity;
                    this.CurrentLanguage = (Language)HttpContext.Session["lang"];
                }

                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(this.CurrentLanguage.Culturename);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(this.CurrentLanguage.Culturename);
            }
            catch
            {
                //// Vacio intencional.
            }
        }

        /// <summary>
        /// Change language get by User
        /// </summary>
        private void UpdateLanguage()
        {
            if (HttpContext.Session["lang"] != null)
            {
                this.CurrentLanguage = (Language)HttpContext.Session["lang"];
            }
            else
            {
                LanguageRepository languagerepo = new LanguageRepository(this.SessionCustom);
                languagerepo.GetByUser(this.CustomUser.UserId);
                this.CurrentLanguage = languagerepo.Entity;
                if (this.CurrentLanguage.Name != null)
                {
                    HttpContext.Session.Add("lang", this.CurrentLanguage);
                }
                else
                {
                    languagerepo.GetLanguageDefault();
                    HttpContext.Session.Add("lang", this.CurrentLanguage);
                }
            }
        }

        /// <summary>
        /// Update reward.
        /// </summary>
        /// <param name="user">Current user.</param>
        /// <param name="cityUpdate">City Update</param>
        /// <param name="interest">User interest.</param>
        private void UpdateReward(User user, bool cityUpdate, int?[] interest)
        {
            if (!string.IsNullOrEmpty(user.Image) && !user.Image.Equals("~/files/imagesuser/default.png"))
            {
                Utils.SetUserRewardAction(user.UserId.Value, RewardAction.UserActionType.FillProfilePhoto, 38, 1, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage, true);
            }

            if (cityUpdate)
            {
                if (!string.IsNullOrEmpty(user.Genre))
                {
                    Utils.SetUserRewardAction(user.UserId.Value, RewardAction.UserActionType.FillProfileGender, 38, 1, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage, true);
                }

                if (user.Age.HasValue)
                {
                    Utils.SetUserRewardAction(user.UserId.Value, RewardAction.UserActionType.FillProfileAge, 38, 1, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage, true);
                }

                if (!string.IsNullOrEmpty(user.Profession))
                {
                    Utils.SetUserRewardAction(user.UserId.Value, RewardAction.UserActionType.FillProfileProfession, 38, 1, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage, true);
                }

                if (!string.IsNullOrEmpty(user.Phone))
                {
                    Utils.SetUserRewardAction(user.UserId.Value, RewardAction.UserActionType.FillProfilePhone, 38, 1, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage, true);
                }

                if (user.CountryId.HasValue)
                {
                    Utils.SetUserRewardAction(user.UserId.Value, RewardAction.UserActionType.FillProfileCountry, 38, 1, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage, true);
                }

                if (user.CityId.HasValue)
                {
                    Utils.SetUserRewardAction(user.UserId.Value, RewardAction.UserActionType.FillProfileCity, 38, 1, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage, true);
                }
            }

            if (interest != null && interest.Length > 0)
            {
                Utils.SetUserRewardAction(user.UserId.Value, RewardAction.UserActionType.FillProfileInterest, 38, 1, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage, true);
            }

            if (!string.IsNullOrEmpty(user.Image) && !user.Image.Equals("~/files/imagesuser/default.png") &&
                !string.IsNullOrEmpty(user.Genre) && user.Age.HasValue && !string.IsNullOrEmpty(user.Profession) &&
                !string.IsNullOrEmpty(user.Phone) && user.CountryId.HasValue && user.CityId.HasValue &&
                interest != null && interest.Length > 0)
            {
                Utils.SetUserRewardAction(user.UserId.Value, RewardAction.UserActionType.CompleteProfile, 120, 1, this.SessionCustom, ControllerContext.HttpContext, true, this.CurrentLanguage, true);
            }
        }
    }
}