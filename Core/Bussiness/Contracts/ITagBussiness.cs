// <copyright file="ITagBussiness.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace Core.Bussiness.Contracts
{
    using System.Collections.Generic;
    using Domain.Entities;

    /// <summary>
    /// Expone las acciones para la lógica de negocio de los tags.
    /// </summary>
    public interface ITagBussiness
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
