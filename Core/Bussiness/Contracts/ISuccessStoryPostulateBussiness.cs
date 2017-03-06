// <copyright file="ISuccessStoryPostulateBussiness.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace Core.Bussiness.Contracts
{
    using Domain.Entities.FrontEnd;
    using Domain.Entities.Generic;
    using System.Collections.Generic;

    /// <summary>
    /// Expone las acciones para el negocio de postulación de casos de éxito.
    /// </summary>
    public interface ISuccessStoryPostulateBussiness
    {
        /// <summary>
        /// Consulta todos los casos de éxito postulados.
        /// </summary>        
        /// <returns>Una lista con los casos de éxito postulados.</returns>
        IList<SuccessStoryPostulate> GetAll();

        /// <summary>
        /// Consulta todos los casos de éxito postulados.
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="language">Idioma de los textos</param>
        /// <returns>Una lista con los casos de éxito postulados.</returns>
        IList<SuccessStoryPostulatePaging> GetPaging(int pageIndex, int pageSize, out int totalCount, int language);

        /// <summary>
        /// Consulta la postulación de un caso de éxito.
        /// </summary>
        /// <param name="id">Id de la postulación del caso de éxito a consultar</param>
        /// <param name="languageId">Id del lenguaje</param>
        /// <returns>La postulación del caso de éxito.</returns>
        SuccessStoryPostulate GetById(int id, int languageId);

        /// <summary>
        /// Actualiza la postulación de un caso de éxito.
        /// </summary>
        /// <param name="successStoryPostulate">Postulación del caso de éxito a guardar.</param>
        /// <returns>true si se realizó la actualizacion, de lo contrario false.</returns>
        bool Update(SuccessStoryPostulate successStoryPostulate);

        /// <summary>
        /// Guarda la postulación de un caso de éxito.
        /// </summary>
        /// <param name="successStoryPostulate">Postulación del caso de éxito a guardar.</param>
        /// <returns>true si se realizó el guardado, de lo contrario false.</returns>
        bool Save(SuccessStoryPostulate successStoryPostulate);
    }
}
