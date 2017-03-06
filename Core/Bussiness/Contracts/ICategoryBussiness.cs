// <copyright file="ICategoryBussiness.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace Core.Bussiness.Contracts
{
    using System.Collections.Generic;
    using Domain.Entities.Generic;

    /// <summary>
    /// Expone las acciones para la lógica de negocio de las categorías.
    /// </summary>
    public interface ICategoryBussiness
    {
        /// <summary>
        /// Consulta las categorías configuradas en el sistema.
        /// </summary>
        /// <param name="languageId">Id del lenguaje en el que se mostraran las categorías.</param>
        /// <returns>Una lista con las categorías configuradas en el sistema.</returns>
        IList<Category> GetAll(int languageId);
    }
}
