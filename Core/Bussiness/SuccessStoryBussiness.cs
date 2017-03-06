// <copyright file="SuccessStoryBussiness.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("UnitTest")]
namespace Core.Bussiness
{
    using Core.Bussiness.Contracts;
    using Core.Bussiness.Repositories;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Representa la clase <see cref="SuccessStoryBussiness"/>.
    /// </summary>    
    internal class SuccessStoryBussiness : ISuccessStoryBussiness
    {
        /// <summary>
        /// Interfaz de tipo <see cref="ISuccessStoryRepository"/> para acceder al repositorio de casos de exito.
        /// </summary>
        private ISuccessStoryRepository successRepository;

        /// <summary>
        /// Inicialiaza una instancia de la clase <see cref="SuccessStoryBussiness"/>.
        /// </summary>
        /// <param name="successRepository">Interfaz de tipo <see cref="ISuccessStoryRepository"/> para acceder al repositorio de casos de exito.</param>
        public SuccessStoryBussiness(ISuccessStoryRepository successRepository)
        {
            this.successRepository = successRepository;
        }

        /// <summary>
        /// Consulta el caso de exito deseado.
        /// </summary>
        /// <param name="id">Id del caso</param>
        /// <param name="languageId">Id del lenguaje.</param>
        /// <returns>Caso de exito.</returns>
        public SuccessStoryList GetById(int id, int? languageId)
        {
            return this.successRepository.GetById(id, languageId);
        }
    }
}
