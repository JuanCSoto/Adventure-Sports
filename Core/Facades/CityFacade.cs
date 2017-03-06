// <copyright file="CityFacade.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace Core.Facades
{
    using System.Collections.Generic;
    using Core.Bussiness;
    using Core.Bussiness.Contracts;
    using Core.DataAccess;
    using Domain.Entities;

    /// <summary>
    /// Clase que representa la fachada de ciudades.
    /// </summary>
    public class CityFacade
    {
        /// <summary>
        /// Consulta las ciudades configuradas en el sistema filtradas por país.
        /// </summary>
        /// <param name="countryId">Id del país.</param>
        /// <returns>Una lista con las ciudades configuradas en el sistema filtradas por país.</returns>
        public IList<City> GetByCountry(int countryId)
        {
            ICityBussiness cityBussiness = new CityBussiness(new CityBroker());
            return cityBussiness.GetByCountry(countryId);
        }
    }
}
