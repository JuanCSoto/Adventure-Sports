// <copyright file="CityController.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace Webcore.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Core.Facades;
    using Domain.Entities.Enums;

    /// <summary>
    /// Representa la clase <see cref="Webcore.Controllers.CityController"/>
    /// </summary>
    public class CityController : FrontEndController
    {

        /// <summary>
        /// Ejecucata la acción GetByCountry (get).
        /// </summary>
        /// <param name="countryId">Id del país.</param>
        /// <returns>El resultado de acción GetByCountry (get).</returns>        
        [HttpGet]
        public JsonResult GetByCountry(int countryId)
        {
            CityFacade cityFacade = new CityFacade();
            return Json(cityFacade.GetByCountry(countryId).Select(c => new { id = c.CityID, name = (int)CurrentLanguage.LanguageId == (int)LanguageEnum.English ? c.NameEn : c.NameEs }), JsonRequestBehavior.AllowGet);
        }
    }
}
