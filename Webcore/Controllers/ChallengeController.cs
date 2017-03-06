// <copyright file="ChallengeController.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace Webcore.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Core.Facades;
    using Domain.Entities.Generic;
    using Webcore.Models.Challenges;

    /// <summary>
    /// Representa la clase <see cref="Webcore.Controllers.ChallengeController"/>
    /// </summary>
    public class ChallengeController : FrontEndController
    {
        /// <summary>
        /// Ejecucata la acción index (get).
        /// </summary>
        /// <returns>El resultado de acción index (get).</returns>
        [HttpGet]
        public ActionResult Index()
        {
            CategoryFacade categoryFacade = new CategoryFacade();
            IndexModel model = new IndexModel();
            CategoryLanguage categoryLanguage;

            foreach (var category in categoryFacade.GetAll((int)CurrentLanguage.LanguageId))
            {
                categoryLanguage = category.CategoryLanguage.FirstOrDefault();
                model.Categories.Add(new CategoryModel
                {
                    CategoryId = category.CategoryId,
                    LanguageId = categoryLanguage.LanguageId,
                    Image = category.Image,
                    Name = categoryLanguage.Name,
                    Description = categoryLanguage.Description
                });
            }

            model.UserPrincipal = CustomUser;
            model.CurrentLanguage = CurrentLanguage;

            return View(model);
        }

    }
}
