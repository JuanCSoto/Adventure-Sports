// <copyright file="CountryBussiness.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTest")]
namespace Core.Bussiness
{
    using System.Collections.Generic;
    using Core.Bussiness.Contracts;
    using Core.Bussiness.Repositories;
    using Domain.Entities;

    /// <summary>
    /// Representa la clase <see cref="Core.Bussiness.CountryBussiness"/>.
    /// </summary>    
    internal class CountryBussiness : ICountryBussiness
    {
        /// <summary>
        /// Interfaz de tipo <see cref="Core.Bussiness.Repositories.ICountryRepository"/> para acceder al repositorio de países.
        /// </summary>
        private ICountryRepository countryRepository;

        /// <summary>
        /// Inicialiaza una instancia de la clase <see cref="Core.Bussiness.CountryBussiness"/>.
        /// </summary>
        /// <param name="categoryRepository">Interfaz de tipo <see cref="Core.Bussiness.Repositories.ICountryRepository"/> para acceder al repositorio de países.</param>
        public CountryBussiness(ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        /// <summary>
        /// Consulta los países configurados en el sistema.
        /// </summary>        
        /// <returns>Una lista con los países configurados en el sistema.</returns>
        public IList<Country> GetAll()
        {
            return this.countryRepository.GetAll();
        }
    }
}
