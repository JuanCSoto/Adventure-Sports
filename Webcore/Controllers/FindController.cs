// <copyright file="FindController.cs" company="Intergrupo">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
namespace Webcore.Controllers
{
   using System.Collections.Generic;
   using System.Configuration;
   using System.Linq;
   using System.Web.Mvc;
   using Core.Facades;
   using Domain.Concrete;
   using Domain.Entities;
   using Domain.Entities.FrontEnd;
   using Webcore.Models;

    /// <summary>
    /// Controlador para buscar.
    /// </summary>
    public class FindController : FrontEndController
    {
        /// <summary>
        /// Index de para el find general.
        /// </summary>
        /// <param name="buscar">criterio de busqueda name.</param>
        /// <param name="type">criterio de busqueda tipo: reto, blog o caso de exito.</param>
        /// <param name="page">pagina de la paginacion.</param>
        /// <returns>Lista de los criterios buscados.</returns>
       public ActionResult Index(string buscar, string type, int? page)
       {
          string titulo = string.Format("{0} | {1}", ConfigurationManager.AppSettings["TitleHome"], Resources.Global.Messages.FINDREULTS);
          this.UpdateLanguage();
          string criteriosBusqueda = string.Empty;
          int pageSize = 30;
          int pageNumber = page ?? 1;
          int total = 0;
          UserRepository user = new UserRepository(this.SessionCustom);
          List<GeneralFindPaging> listaFind = new List<GeneralFindPaging>();

          if (!string.IsNullOrEmpty(buscar))
          {
             char[] separator = { ' ' };
             string[] lisBuscar = buscar.Split(separator);

             foreach (string tempData in lisBuscar)
             {
                if (!string.IsNullOrEmpty(tempData))
                {
                   criteriosBusqueda += string.Format("{0},,", tempData.Trim());
                }
             }

             if (!string.IsNullOrEmpty(criteriosBusqueda))
             {
                if (criteriosBusqueda.EndsWith(",,"))
                {
                   criteriosBusqueda = criteriosBusqueda.Substring(0, criteriosBusqueda.Length - 2);
                }
             }

             listaFind.AddRange(this.GetByFind(criteriosBusqueda, type, pageNumber, pageSize, out total));
          }

          decimal nropagesdec = decimal.Parse(total.ToString()) / decimal.Parse(pageSize.ToString());
          decimal nropagesint = (int)(total / pageSize);
          this.ViewBag.CurrentPage = pageNumber;
          this.ViewBag.TotalRows = total;
          this.ViewBag.SizePage = pageSize;
          this.ViewBag.NroPages = nropagesdec > nropagesint ? nropagesint + 1 : nropagesint;
          this.ViewBag.TipoBusqueda = type;
          this.ViewBag.CriteriosBusqueda = "buscar=" + criteriosBusqueda.Replace(",,", "+");
          this.ViewBag.DatosBusqueda = buscar;
          FEGeneralFind fegeneralfind = new FEGeneralFind();
          fegeneralfind.ListaGeneralFind = listaFind;
          fegeneralfind.UserPrincipal = this.CustomUser;
          fegeneralfind.CurrentLanguage = this.CurrentLanguage;
          fegeneralfind.PageTitle = titulo;

          return this.View(fegeneralfind);
       }

        /// <summary>
        /// Entrega el listado de la busqueda por el nombre y/o tipo.
        /// </summary>
        /// <param name="search">Informacion a buscar.</param>
        /// <param name="type">Tipo de busqueda retos, casos de exitos, blogs.</param>
        /// <param name="pageIndex">page index.</param>
        /// <param name="pageSize">page size.</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send.</param>
        /// <returns>Lista de la clase GeneralFind.</returns>
        private List<GeneralFindPaging> GetByFind(string search, string type, int pageIndex, int pageSize, out int totalCount)
        {
            int totalOr;
            int totalAnd;

            GeneralFindFacade dbgeneralFacade = new GeneralFindFacade();
            List<GeneralFindPaging> listFindOr = new List<GeneralFindPaging>();
            List<GeneralFindPaging> listFindAnd = new List<GeneralFindPaging>();

            string[] separator = { ",," };
            string[] arrFind = search.Split(separator, System.StringSplitOptions.None);

            listFindOr = dbgeneralFacade.GetByFindOr(search, type, pageIndex, pageSize, out totalOr);
            totalCount = totalOr;

            if (arrFind.Length > 1)
            {
                listFindAnd = dbgeneralFacade.GetByFindAnd(search, type, pageIndex, pageSize, out totalAnd);
                
                foreach (var tempData in listFindAnd.OrderBy(x => x.JoinDate))
                {
                    if (listFindOr.Exists(x => x.Identificador == tempData.Identificador))
                    {
                        listFindOr.RemoveAll(x => x.Identificador == tempData.Identificador);
                        listFindOr.Insert(0, tempData);
                    }
                    else
                    {
                        listFindOr.Insert(0, tempData);
                        totalCount++;
                    }
                }
            }

            return listFindOr;
        }

        /// <summary>
        /// Update Language
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
    }
}
