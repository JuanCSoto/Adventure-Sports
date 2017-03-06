// <copyright file="SeccionController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Configuration;
   using System.Linq;
   using System.Web.Mvc;
   using Business;
   using Business.Services;
   using Core.Facades;
   using Domain.Concrete;
   using Domain.Entities;
   using Domain.Entities.FrontEnd;
   using Webcore.Comun;
   using Webcore.Models;

    /// <summary>
    /// controller section base
    /// </summary>
    public class SeccionController : FrontEndController
    {
        /// <summary>
        /// gets the home of section according to identifier
        /// </summary>
        /// <param name="id">identifier of section</param>
        /// <returns>returns the result to action</returns>
        /// <History>
        /// Modificado por      :   Juan Carlos Soto Cruz (JCS)
        /// Descripción cambio  :   Se adiciona la logica para el control del Token que se encontraba en la controladora HomeController.cs
        /// Fecha               :   2015/11/06
        /// </History>
       public ActionResult Index(int id)
       {
          string token = null;

          this.UpdateLanguage();
          this.SetLabel();
          FrontEndManagement objman = new FrontEndManagement(this.SessionCustom, this.HttpContext, FrontEndManagement.Type.Section, this.CurrentLanguage);
          List<KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>> collmeta = (new UtilitiesWeb()).MetaTagsHome(this.Request);

          BannerRepository banner = new BannerRepository(this.SessionCustom);
          IdeaRepository idea = new IdeaRepository(this.SessionCustom);
          if (User.Identity.IsAuthenticated)
          {
             objman.BindInfo(id, ((CustomPrincipal)User).UserId);
          }
          else
          {
             objman.BindInfo(id, null);
          }

          if (this.Request["token"] != null)
          {
             RecoveryTokenRepository recovery = new RecoveryTokenRepository(this.SessionCustom);
             recovery.Entity.Token = this.Request["token"];
             recovery.Load();

             if (recovery.Entity.Creationdate >= DateTime.Now.AddHours(-1) && !recovery.Entity.Used.Value)
             {
                token = recovery.Entity.Token;
             }
          }

          if (objman.Outcome == FrontEndManagement.Result.Ok)
          {
             return this.View(
                 objman.Template,
                 new FESeccion<object>()
             {
                UserPrincipal = this.CustomUser,
                PageTitle = this.EncuentraTituloMenu(objman.Section.Name),
                Section = objman.Section,
                Layout = objman.Layout + ".cshtml",
                Entity = objman.Detail,
                Banners = banner.GetBannersBySection(objman.Section.SectionId.Value, this.CurrentLanguage.LanguageId.Value),
                MetaTags = collmeta,
                DeepFollower = objman.DeepFollower,
                CurrentLanguage = this.CurrentLanguage,
                Token = token,
                IdeasCountAll = idea.IdeasCountAll()
             });
          }
          else if (objman.Outcome == FrontEndManagement.Result.NotFound)
          {
             return this.View(
                 "Mensaje",
                 new FEMessage()
             {
                PageTitle = "Recurso no encontrado",
                UserPrincipal = this.CustomUser,
                Banners = banner.GetBannersBySection(0, this.CurrentLanguage.LanguageId.Value),
                Title = "Recurso no encontrado",
                Description = "Recurso no encontrado",
                Message = Resources.Extend.Messages.RESOURCE_NOT_FOUND,
                CurrentLanguage = this.CurrentLanguage
             });
          }
          else
          {
             return this.View(
                 "Mensaje",
                 new FEMessage()
             {
                PageTitle = "Sistema no disponible",
                UserPrincipal = this.CustomUser,
                Banners = banner.GetBannersBySection(0, this.CurrentLanguage.LanguageId.Value),
                Title = "Sistema no disponible",
                Description = "Sistema no disponible",
                Message = Resources.Extend.Messages.SYSTEM_ERROR,
                CurrentLanguage = this.CurrentLanguage
             });
          }
       }

        /// <summary>
        /// obtains a primary menu of application.
        /// </summary>
        /// <param name="mod">identifier of module.</param>
        /// <param name="parent">parent identifier.</param>
        /// <param name="languageId">language identifier.</param>
        /// <param name="ideasCountAll">ideas total count.</param>
        /// <param name="buscar">Texto a buscar en el buscador general.</param>
        /// <returns>returns the result to action.</returns>
        [ChildActionOnly]
        public ActionResult PrimaryMenu(int mod, int parent, int languageId, int ideasCountAll, string buscar)
        {
            string busqueda = string.Empty;
            InfoCache<List<Domain.Entities.Section>> cache = new InfoCache<List<Domain.Entities.Section>>(this.HttpContext) { TimeOut = 120 };
            List<Domain.Entities.Section> coll = cache.GetCache("sectionsactive");
            GeneralFindFacade dbgeneralFacade = new GeneralFindFacade();
            List<GeneralFind> listaFind = dbgeneralFacade.GetAll();

            var mylist = listaFind.ConvertAll(x => x.Name.Trim());

            var jsonSerialiser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(mylist);

            if (!string.IsNullOrEmpty(buscar))
            {
                char[] separator = { ' ' };
                string[] lisBuscar = buscar.Split(separator);

                foreach (string temp in lisBuscar)
                {
                    busqueda += string.Format("{0},", temp.Trim());
                }

                if (!string.IsNullOrEmpty(busqueda))
                {
                    if (busqueda.EndsWith(","))
                    {
                        busqueda = busqueda.Substring(0, busqueda.Length - 1);
                    }
                }
            }

            this.ViewBag.Busqueda = buscar;
            this.ViewBag.JsonSearch = json + ";";

            SectionRepository section = new SectionRepository(this.SessionCustom);
            section.Entity.Active = true;
            coll = section.GetAll();
            cache.SetCache("sectionsactive", coll);

            return this.PartialView(
                "_PrimaryMenu",
                new FEMenu()
                {
                    Sections = coll.FindAll(t => t.LanguageId == languageId),
                    SectionId = mod,
                    ParentId = parent,
                    CurrentLanguage = this.CurrentLanguage,
                    IdeasCountAll = ideasCountAll
                });
        }

        /// <summary>
        /// obtains a footer menu of application
        /// </summary>
        /// <param name="languageId">language identifier</param>
        /// <returns>returns the result to action</returns>
        [ChildActionOnly]
        public ActionResult FooterMenu(int languageId)
        {
            SectionRepository objSection = new SectionRepository(this.SessionCustom);
            objSection.Entity.Active = true;
            objSection.Entity.LanguageId = languageId;
            return this.PartialView(
                "_FooterMenu",
                new FEMenu()
                {
                    Sections = objSection.GetAllReadOnly(),
                    CurrentLanguage = this.CurrentLanguage
                });
        }

        /// <summary>
        /// obtain a list of challenges
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="active">status of the challenges</param>
        /// <returns>a list of challenges</returns>
        [HttpPost]
        public ActionResult Retos(int pageIndex, int pageSize, bool? active)
        {
            this.SetLabel();
            int total = 0;
            ChallengeRepository challenge = new ChallengeRepository(this.SessionCustom);
            return this.View("ChallengesList", challenge.ChallengesPaging(pageIndex, pageSize, out total, null, this.CurrentLanguage.LanguageId));
        }

        /// <summary>
        /// obtain a list of questions
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="active">status of the questions</param>
        /// <param name="filter">filter for the question result</param>
        /// <param name="questionsId">a list od ideas ids to be excluded from the random result</param>
        /// <returns>a list of questions</returns>
        [HttpPost]
        public ActionResult Preguntas(int pageIndex, int pageSize, bool? active, int filter, int[] questionsId)
        {
            int total = 0;
            QuestionRepository question = new QuestionRepository(this.SessionCustom);

            List<Domain.Entities.FrontEnd.QuestionsPaging> questions = new List<Domain.Entities.FrontEnd.QuestionsPaging>();
            switch (filter)
            {
                case 1:
                    questions = question.QuestionsPagingRandom(pageSize, null, questionsId);
                    break;

                case 2:
                    questions = question.QuestionsPaging(pageIndex, pageSize, out total, null);
                    break;

                case 3:
                    questions = question.QuestionsPagingTop(pageIndex, pageSize, out total, null);
                    break;

                case 4:
                    questions = question.QuestionsPagingRecommended(pageIndex, pageSize, out total, null);
                    break;
            }

            return this.View("QuestionsList", questions);
        }

        /// <summary>
        /// obtain a list of FAQs
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="active">status of the FAQ</param>
        /// <returns>a list of FAQs</returns>
        [HttpPost]
        public ActionResult Nosotros(int pageIndex, int pageSize, bool active)
        {
            int total = 0;
            BlogEntryRepository blog = new BlogEntryRepository(this.SessionCustom);
            List<BlogEntriesPaging> collBlogEntries = blog.BlogEntriesPaging(pageIndex, pageSize, out total, active, this.CurrentLanguage.LanguageId);
            FileattachRepository fileRepository = new FileattachRepository(this.SessionCustom);
            CommentRepository comment = new CommentRepository(this.SessionCustom);
            int totalCount = 0;
            foreach (BlogEntriesPaging blogEntry in collBlogEntries)
            {
                blogEntry.CollComment = comment.CommentsPagingContent(1, 3, out totalCount, blogEntry.ContentId.Value);
                if (blogEntry.CollComment.Count > 0)
                {
                    blogEntry.CollComment[0].CommentContentOwnerId = blogEntry.UserId.Value;
                }

                fileRepository.Entity.ContentId = blogEntry.ContentId;
                Fileattach file = fileRepository.GetAll().FirstOrDefault(t => t.Type == Domain.Entities.Fileattach.TypeFile.Video);
                if (file != null)
                {
                    blogEntry.Video = file.Filename;
                }
            }

            Models.FEListBlogs histories = new FEListBlogs();
            histories.BlogsList = collBlogEntries;
            histories.CurrentLanguage = this.CurrentLanguage;

            return this.View("BlogEntriesList", histories);
        }

        /// <summary>
        /// obtains a view of a blog
        /// </summary>
        /// <param name="id">blog id</param>
        /// <returns>a view of a blog</returns>
        [HttpPost]
        public ActionResult NosotrosById(int id)
        {
           BlogEntryRepository blog = new BlogEntryRepository(this.SessionCustom);
            List<BlogEntriesPaging> collBlogEntries = blog.BlogEntriesById(id);
            FileattachRepository fileRepository = new FileattachRepository(this.SessionCustom);
            CommentRepository comment = new CommentRepository(this.SessionCustom);
            int totalCount = 0;
            foreach (BlogEntriesPaging blogEntry in collBlogEntries)
            {
                blogEntry.CollComment = comment.CommentsPagingContent(1, 3, out totalCount, blogEntry.ContentId.Value);
                if (blogEntry.CollComment.Count > 0)
                {
                    blogEntry.CollComment[0].CommentContentOwnerId = blogEntry.UserId.Value;
                }

                fileRepository.Entity.ContentId = blogEntry.ContentId;
                Fileattach file = fileRepository.GetAll().FirstOrDefault(t => t.Type == Domain.Entities.Fileattach.TypeFile.Video);
                if (file != null)
                {
                    blogEntry.Video = file.Filename;
                }
            }

            Models.FEListBlogs histories = new FEListBlogs();
            histories.BlogsList = collBlogEntries;
            histories.CurrentLanguage = this.CurrentLanguage;

            return this.View("BlogEntriesList", histories);
        }

        /// <summary>
        /// obtains a list language
        /// </summary>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public JsonResult GetLanguages()
        {
           LanguageRepository languagerepo = new LanguageRepository(this.SessionCustom);
            return this.Json(languagerepo.GetAll());
        }

        /// <summary>
        /// sets the language default
        /// </summary>
        /// <param name="id">identifier of language</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public JsonResult SetLanguage(int id)
        {
            try
            {
               LanguageRepository languagerepo = new LanguageRepository(this.SessionCustom);
                languagerepo.Entity.LanguageId = id;
                languagerepo.LoadByKey();

                if (this.HttpContext.Session["lang"] == null)
                {
                   this.HttpContext.Session.Add("lang", languagerepo.Entity);
                }
                else
                {
                   this.HttpContext.Session["lang"] = languagerepo.Entity;
                }

                return this.Json(new { result = true });
            }
            catch (Exception)
            {
                return this.Json(new { result = false });
            }
        }

        /// <summary>
        /// Filtra el caso de exito.
        /// </summary>
        /// <param name="idcity">identifier of city.</param>
        /// <param name="category">Texto categoría.</param>
        /// <param name="intitucionimplementa">intitucion que lo implementa.</param>
        /// <param name="institucionfuente">Institucion fuente.</param>
        /// <returns>returns the result to action.</returns>
        [HttpPost]
        public JsonResult Setfiltrocasosexito(int idcity, string category, string intitucionimplementa, string institucionfuente)
        {
            try
            {
                return this.Json(new { result = true });
            }
            catch (Exception)
            {
                return this.Json(new { result = false });
            }
        }

        /// <summary>
        /// Actualiza el Idioma.
        /// </summary>
        private void UpdateLanguage()
        {
           if (this.HttpContext.Session["lang"] != null)
            {
               this.CurrentLanguage = (Language)this.HttpContext.Session["lang"];
            }
            else
            {
                LanguageRepository languagerepo = new LanguageRepository(this.SessionCustom);
                languagerepo.GetByUser(this.CustomUser.UserId);
                this.CurrentLanguage = languagerepo.Entity;

                if (this.CurrentLanguage.Name != null)
                {
                    this.HttpContext.Session.Add("lang", this.CurrentLanguage);
                }
                else
                {
                    languagerepo.GetLanguageDefault();
                    this.HttpContext.Session.Add("lang", this.CurrentLanguage);
                }
            }
        }

        /// <summary>
        /// Set lenguage label
        /// </summary>
        private void SetLabel()
        {
            LabelManagement objlabel = new LabelManagement(this.SessionCustom, this.HttpContext);
            this.ViewBag.TXTPARCIU = objlabel.GetLabelByName("TXTPARCIU", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.US = objlabel.GetLabelByName("US", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTUS1 = objlabel.GetLabelByName("TXTUS1", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTUS2 = objlabel.GetLabelByName("TXTUS2", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTUS3 = objlabel.GetLabelByName("TXTUS3", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTNUEENL = objlabel.GetLabelByName("TXTNUEENL", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.FILE = objlabel.GetLabelByName("FILE", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.ALLIES = objlabel.GetLabelByName("ALLIES", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTJUNCIU = objlabel.GetLabelByName("TXTJUNCIU", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTVERMAS = objlabel.GetLabelByName("TXTVERMAS", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.COMMENT = objlabel.GetLabelByName("COMMENT", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.CLOSE = objlabel.GetLabelByName("CLOSE", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.ACTIVE = objlabel.GetLabelByName("ACTIVE", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.LASTEST = objlabel.GetLabelByName("LASTEST", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.POPULAR = objlabel.GetLabelByName("POPULAR", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.RECOMMENDED = objlabel.GetLabelByName("RECOMMENDED", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.RESIDUARY = objlabel.GetLabelByName("RESIDUARY", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.AGES = objlabel.GetLabelByName("AGES", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTNOHAYRESBUS = objlabel.GetLabelByName("TXTNOHAYRESBUS", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.INTERESTS = objlabel.GetLabelByName("INTERESTS", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.PROFESSIONS = objlabel.GetLabelByName("PROFESSIONS", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TRENDS = objlabel.GetLabelByName("TRENDS", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTCATDEPUL = objlabel.GetLabelByName("TXTCATDEPUL", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.PREMIUM = objlabel.GetLabelByName("PREMIUM", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.ACTIVITY = objlabel.GetLabelByName("ACTIVITY", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.GENERAL = objlabel.GetLabelByName("GENERAL", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.RANKING = objlabel.GetLabelByName("RANKING", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.CATEGORIES = objlabel.GetLabelByName("CATEGORIES", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTULTSEM = objlabel.GetLabelByName("TXTULTSEM", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTULTMES = objlabel.GetLabelByName("TXTULTMES", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTALLYEAR = objlabel.GetLabelByName("TXTALLYEAR", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTBEGOFTIME = objlabel.GetLabelByName("TXTBEGOFTIME", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTDATRANG = objlabel.GetLabelByName("TXTDATRANG", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.VIEW = objlabel.GetLabelByName("VIEW", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTPREFRE = objlabel.GetLabelByName("TXTPREFRE", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.ACCEPT = objlabel.GetLabelByName("ACCEPT", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.CLOSESSION = objlabel.GetLabelByName("CLOSESSION", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.SUCCESSSTORY = objlabel.GetLabelByName("SUCCESSSTORY", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTUNTTRA = objlabel.GetLabelByName("TXTUNTTRA", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTINSTIMPLE = objlabel.GetLabelByName("TXTINSTIMPLE", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.CITY = objlabel.GetLabelByName("CITY", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTINSTSOURCE = objlabel.GetLabelByName("TXTINSTSOURCE", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.TXTLOOKING = objlabel.GetLabelByName("TXTLOOKING", this.CurrentLanguage.LanguageId.Value);
            this.ViewBag.PULSES = objlabel.GetLabelByName("PULSES", this.CurrentLanguage.LanguageId.Value);
        }

        /// <summary>
        /// Encuentras the titulo menu.
        /// </summary>
        /// <param name="seccion">The seccion.</param>
        /// <returns>Entrega el titulo.</returns>
        private string EncuentraTituloMenu(string seccion)
        {
           string titulo = ConfigurationManager.AppSettings["TitleHome"] + " | {0}";

           switch (seccion)
           {
              case "Home": titulo = string.Format(titulo, Resources.Global.Messages.HOME); break;
              case "Noticias": titulo = string.Format(titulo, Resources.Global.Messages.BLOG); break;
              case "FAQ": titulo = string.Format(titulo, "FAQ"); break;
              case "Ciudadanos": titulo = string.Format(titulo, Resources.Global.Messages.CITIZENS); break;
              case "SuccessStory": titulo = string.Format(titulo, Resources.Global.Messages.SUCCESSSTORY); break;
              case "Postula": titulo = string.Format(titulo, Resources.Global.Messages.POSTULATES_SUCCESS_STORY); break;
              default: titulo = string.Format(titulo, seccion); break;
           }

           return titulo;
        }
    }
}