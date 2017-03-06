// <copyright file="ICountryRepository.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace Core.Bussiness.Repositories
{
    using System.Collections.Generic;
    using Domain.Entities;

    /// <summary>
    /// Expone las acciones para acceder al repositorio de países.
    /// </summary>
    public interface ICountryRepository
    {
        /// <summary>
        /// Consulta los países configurados en el sistema.
        /// </summary>        
        /// <returns>Una lista con los países configurados en el sistema.</returns>
        IList<Country> GetAll();
    }
}
