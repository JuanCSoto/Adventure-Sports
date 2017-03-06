// <copyright file="TagBussiness.cs" company="Intergrupo">
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
    /// Representa la clase <see cref="Core.Bussiness.TagBussiness"/>.
    /// </summary>    
    internal class TagBussiness : ITagBussiness
    {
        /// <summary>
        /// Interfaz de tipo <see cref="Core.Bussiness.Repositories.ITagRepository"/> para acceder al repositorio de tags.
        /// </summary>
        private ITagRepository tagRepository;

        /// <summary>
        /// Inicialiaza una instancia de la clase <see cref="Core.Bussiness.CityBussiness"/>.
        /// </summary>
        /// <param name="categoryRepository">Interfaz de tipo <see cref="Core.Bussiness.Repositories.ICategoryRepository"/> para acceder al repositorio de ciudades.</param>
        public TagBussiness(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        /// <summary>
        /// Consulta los tags configurados en el sistema.
        /// </summary>        
        /// <returns>Una lista con los tags configurados en el sistema.</returns>
        public IList<Tag> GetAll()
        {
            return this.tagRepository.GetAll();
        }

        /// <summary>
        /// Consulta los tags asociados a la postulación de un caso de éxito.
        /// </summary>
        /// <param name="idSuccessStoryPostulate">Id de la postulación de un caso de éxito.</param>
        /// <returns>Una lista con los tags asociados a la postulación de un caso de éxito.</returns>
        public IList<Tag> GetBySuccessStoryPostulate(int idSuccessStoryPostulate)
        {
            return this.tagRepository.GetBySuccessStoryPostulate(idSuccessStoryPostulate);
        }
    }
}
