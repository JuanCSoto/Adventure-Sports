// <copyright file="CountryFacade.cs" company="Intergrupo">
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
    /// Clase que representa la fachada del país.
    /// </summary>
    public class CountryFacade
    {
        /// <summary>
        /// Consulta los países configurados en el sistema.
        /// </summary>        
        /// <returns>Una lista con los países configurados en el sistema.</returns>
        public IList<Country> GetAll()
        {
            ICountryBussiness countryBussiness = new CountryBussiness(new CountryBroker());
            return countryBussiness.GetAll();
        }
    }
}
