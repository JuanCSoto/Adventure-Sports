// <copyright file="TagFacade.cs" company="Intergrupo">
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
    /// Clase que representa la fachada de tags.
    /// </summary>
    public class TagFacade
    {
        /// <summary>
        /// Consulta los tags configurados en el sistema.
        /// </summary>        
        /// <returns>Una lista con los tags configurados en el sistema.</returns>
        public IList<Tag> GetAll()
        {
            ITagBussiness tagBussiness = new TagBussiness(new TagBroker());
            return tagBussiness.GetAll();
        }

        /// <summary>
        /// Consulta los tags asociados a la postulación de un caso de éxito.
        /// </summary>
        /// <param name="idSuccessStoryPostulate">Id de la postulación de un caso de éxito.</param>
        /// <returns>Una lista con los tags asociados a la postulación de un caso de éxito.</returns>
        public IList<Tag> GetBySuccessStoryPostulate(int idSuccessStoryPostulate)
        {
            ITagBussiness tagBussiness = new TagBussiness(new TagBroker());
            return tagBussiness.GetBySuccessStoryPostulate(idSuccessStoryPostulate);
        }
    }
}
