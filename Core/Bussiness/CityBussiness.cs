// <copyright file="CityBussiness.cs" company="Intergrupo">
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
    /// Representa la clase <see cref="Core.Bussiness.CityBussiness"/>.
    /// </summary>    
    internal class CityBussiness : ICityBussiness
    {
        /// <summary>
        /// Interfaz de tipo <see cref="Core.Bussiness.Repositories.ICityRepository"/> para acceder al repositorio de ciudades.
        /// </summary>
        private ICityRepository cityRepository;

        /// <summary>
        /// Inicialiaza una instancia de la clase <see cref="Core.Bussiness.CityBussiness"/>.
        /// </summary>
        /// <param name="categoryRepository">Interfaz de tipo <see cref="Core.Bussiness.Repositories.ICategoryRepository"/> para acceder al repositorio de ciudades.</param>
        public CityBussiness(ICityRepository cityRepository)
        {
            this.cityRepository = cityRepository;
        }

        /// <summary>
        /// Consulta las ciudades configuradas en el sistema filtradas por país.
        /// </summary>
        /// <param name="countryId">Id del país.</param>
        /// <returns>Una lista con las ciudades configuradas en el sistema filtradas por país.</returns>
        public IList<City> GetByCountry(int countryId)
        {
            return this.cityRepository.GetByCountry(countryId);
        }
    }
}
