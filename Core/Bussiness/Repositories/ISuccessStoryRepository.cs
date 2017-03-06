// <copyright file="ISuccessStoryRepository.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
namespace Core.Bussiness.Repositories
{
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Expone las acciones para acceder al repositorio de Casos de exito.
    /// </summary>
    public interface ISuccessStoryRepository
    {
        /// <summary>
        /// Consulta el caso de exito deseado.
        /// </summary>
        /// <param name="id">Id del caso</param>
        /// <param name="languageId">Id del lenguaje.</param>
        /// <returns>Caso de exito.</returns>
        SuccessStoryList GetById(int id, int? languageId);
    }
}
