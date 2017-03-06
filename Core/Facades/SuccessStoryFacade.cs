// <copyright file="SuccessStoryFacade.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
namespace Core.Facades
{
    using Core.Brokers;
    using Core.Bussiness;
    using Core.Bussiness.Contracts;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Clase que representa la fachada de la casos de exito.
    /// </summary>
    public class SuccessStoryFacade
    {
        /// <summary>
        /// Consulta el caso de exito deseado.
        /// </summary>
        /// <param name="id">Id del caso</param>
        /// <param name="languageId">Id del lenguaje.</param>
        /// <returns>Caso de exito.</returns>
        public SuccessStoryList GetById(int id, int? languageId)
        {
            ISuccessStoryBussiness successBussiness = new SuccessStoryBussiness(new SuccessStoryBroker());
            return successBussiness.GetById(id, languageId);
        }
    }
}
