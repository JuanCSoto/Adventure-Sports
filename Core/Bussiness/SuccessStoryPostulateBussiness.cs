// <copyright file="ISuccessStoryPostulateRepository.cs" company="Intergrupo">
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
    using Domain.Entities.Generic;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Representa la clase <see cref="Core.Bussiness.SuccessStoryPostulateBussiness"/>.
    /// </summary>    
    internal class SuccessStoryPostulateBussiness : ISuccessStoryPostulateBussiness
    {
        /// <summary>
        /// Interfaz de tipo <see cref="Core.Bussiness.Repositories.successStoryPostulateRepository"/> para acceder al repositorio de postulación de casos de éxito.
        /// </summary>
        private ISuccessStoryPostulateRepository successStoryPostulateRepository;

        /// <summary>
        /// Inicialiaza una instancia de la clase <see cref="Core.Bussiness.SuccessStoryPostulateBussiness"/>.
        /// </summary>
        /// <param name="successStoryPostulateRepository">Interfaz de tipo <see cref="Core.Bussiness.Repositories.ISuccessStoryPostulateRepository"/> para acceder al repositorio de postulación de casos de éxito.</param>
        public SuccessStoryPostulateBussiness(ISuccessStoryPostulateRepository successStoryPostulateRepository)
        {
            this.successStoryPostulateRepository = successStoryPostulateRepository;
        }

        /// <summary>
        /// Consulta todos los casos de éxito postulados.
        /// </summary>        
        /// <returns>Una lista con los casos de éxito postulados.</returns>
        public IList<SuccessStoryPostulate> GetAll()
        {
            return this.successStoryPostulateRepository.GetAll();
        }

        /// <summary>
        /// Consulta todos los casos de éxito postulados.
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="language">Idioma de los textos</param>
        /// <returns>Una lista con los casos de éxito postulados.</returns>
        public IList<SuccessStoryPostulatePaging> GetPaging(int pageIndex, int pageSize, out int totalCount, int language)
        {
            return this.successStoryPostulateRepository.GetPaging(pageIndex, pageSize, out totalCount, language);
        }

        /// <summary>
        /// Consulta la postulación de un caso de éxito.
        /// </summary>
        /// <param name="id">Id de la postulación del caso de éxito a consultar</param>
        /// <param name="languageId">Id del lenguaje</param>
        /// <returns>La postulación del caso de éxito.</returns>
        public SuccessStoryPostulate GetById(int id, int languageId)
        {
            return this.successStoryPostulateRepository.GetById(id, languageId);
        }

        /// <summary>
        /// Actualiza la postulación de un caso de éxito.
        /// </summary>
        /// <param name="successStoryPostulate">Postulación del caso de éxito a guardar.</param>
        /// <returns>true si se realizó la actualizacion, de lo contrario false.</returns>
        public bool Update(SuccessStoryPostulate successStoryPostulate)
        {
            return this.successStoryPostulateRepository.Update(successStoryPostulate);
        }

        /// <summary>
        /// Guarda la postulación de un caso de éxito.
        /// </summary>
        /// <param name="successStoryPostulate">Postulación del caso de éxito a guardar.</param>
        /// <returns>true si se realizó el guardado, de lo contrario false.</returns>
        public bool Save(SuccessStoryPostulate successStoryPostulate)
        {
            return this.successStoryPostulateRepository.Save(successStoryPostulate);
        }
    }
}
