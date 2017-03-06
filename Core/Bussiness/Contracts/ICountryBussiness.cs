// <copyright file="ICountryBussiness.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace Core.Bussiness.Contracts
{
    using System.Collections.Generic;
    using Domain.Entities;

    /// <summary>
    /// Expone las acciones para la lógica de negocio de los países.
    /// </summary>
    public interface ICountryBussiness
    {
        /// <summary>
        /// Consulta los países configurados en el sistema.
        /// </summary>        
        /// <returns>Una lista con los países configurados en el sistema.</returns>
        IList<Country> GetAll();
    }
}
