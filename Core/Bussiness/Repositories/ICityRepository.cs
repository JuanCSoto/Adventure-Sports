// <copyright file="ICityRepository.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace Core.Bussiness.Repositories
{
    using System.Collections.Generic;
    using Domain.Entities;

    /// <summary>
    /// Expone las acciones para acceder al repositorio de ciudades.
    /// </summary>
    public interface ICityRepository
    {
        /// <summary>
        /// Consulta las ciudades configuradas en el sistema filtradas por país.
        /// </summary>
        /// <param name="countryId">Id del país.</param>
        /// <returns>Una lista con las ciudades configuradas en el sistema filtradas por país.</returns>
        IList<City> GetByCountry(int countryId);
    }
}
