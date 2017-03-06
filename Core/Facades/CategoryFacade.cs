// <copyright file="CategoryFacade.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace Core.Facades
{
    using System.Collections.Generic;
    using Core.Bussiness;
    using Core.Bussiness.Contracts;
    using Core.DataAccess;
    using Domain.Entities.Generic;

    /// <summary>
    /// Clase que representa la fachada de la categoria.
    /// </summary>
    public class CategoryFacade
    {
        /// <summary>
        /// Consulta las categorías configuradas en el sistema.
        /// </summary>
        /// <param name="languageId">Id del lenguaje en el que se mostraran las categorías.</param>
        /// <returns>Una lista con las categorías configuradas en el sistema.</returns>
        public IList<Category> GetAll(int languageId)
        {
            ICategoryBussiness categoryBussiness = new CategoryBussiness(new CategoryBroker());
            return categoryBussiness.GetAll(languageId);
        }
    }
}
