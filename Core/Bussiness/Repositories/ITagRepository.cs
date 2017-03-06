// <copyright file="ITagRepository.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace Core.Bussiness.Repositories
{
    using System.Collections.Generic;
    using Domain.Entities;

    /// <summary>
    /// Expone las acciones para acceder al repositorio de tags.
    /// </summary>
    public interface ITagRepository
    {
        /// <summary>
        /// Consulta los tags configurados en el sistema.
        /// </summary>        
        /// <returns>Una lista con los tags configurados en el sistema.</returns>
        IList<Tag> GetAll();        

        /// <summary>
        /// Consulta los tags asociados a la postulación de un caso de éxito.
        /// </summary>
        /// <param name="idSuccessStoryPostulate">Id de la postulación de un caso de éxito.</param>
        /// <returns>Una lista con los tags asociados a la postulación de un caso de éxito.</returns>
        IList<Tag> GetBySuccessStoryPostulate(int idSuccessStoryPostulate);
    }
}
