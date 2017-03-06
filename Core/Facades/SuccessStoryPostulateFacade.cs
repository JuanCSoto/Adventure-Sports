// <copyright file="SuccessStoryPostulateFacade.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace Core.Facades
{
    using System.Collections.Generic;
    using Core.Bussiness;
    using Core.Bussiness.Contracts;
    using Core.DataAccess;
    using Domain.Entities.Generic;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Clase que representa la fachada de la postulación de casos de éxito.
    /// </summary>
    public class SuccessStoryPostulateFacade
    {
        /// <summary>
        /// Consulta todos los casos de éxito postulados.
        /// </summary>        
        /// <returns>Una lista con los casos de éxito postulados.</returns>
        public IList<SuccessStoryPostulate> GetAll()
        {
            ISuccessStoryPostulateBussiness successStoryPostulateBussiness = new SuccessStoryPostulateBussiness(new SuccessStoryPostulateBroker());
            return successStoryPostulateBussiness.GetAll();
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
            ISuccessStoryPostulateBussiness successStoryPostulateBussiness = new SuccessStoryPostulateBussiness(new SuccessStoryPostulateBroker());
            return successStoryPostulateBussiness.GetPaging(pageIndex, pageSize, out totalCount, language);
        }

        /// <summary>
        /// Consulta la postulación de un caso de éxito.
        /// </summary>
        /// <param name="id">Id de la postulación del caso de éxito a consultar</param>
        /// <param name="languageId">Id del lenguaje</param>
        /// <returns>La postulación del caso de éxito.</returns>
        public SuccessStoryPostulate GetById(int id, int languageId)
        {
            ISuccessStoryPostulateBussiness successStoryPostulateBussiness = new SuccessStoryPostulateBussiness(new SuccessStoryPostulateBroker());
            return successStoryPostulateBussiness.GetById(id, languageId);
        }

                /// <summary>
        /// Actualiza la postulación de un caso de éxito.
        /// </summary>
        /// <param name="successStoryPostulate">Postulación del caso de éxito a guardar.</param>
        /// <returns>true si se realizó la actualizacion, de lo contrario false.</returns>
        public bool Update(SuccessStoryPostulate successStoryPostulate)
        {
            ISuccessStoryPostulateBussiness successStoryPostulateBussiness = new SuccessStoryPostulateBussiness(new SuccessStoryPostulateBroker());
            return successStoryPostulateBussiness.Update(successStoryPostulate);
        }

        /// <summary>
        /// Guarda la postulación de un caso de éxito.
        /// </summary>
        /// <param name="successStoryPostulate">Postulación del caso de éxito a guardar.</param>
        /// <returns>true si se realizó el guardado, de lo contrario false.</returns>
        public bool Save(SuccessStoryPostulate successStoryPostulate)
        {
            ISuccessStoryPostulateBussiness successStoryPostulateBussiness = new SuccessStoryPostulateBussiness(new SuccessStoryPostulateBroker());
            return successStoryPostulateBussiness.Save(successStoryPostulate);
        }
    }
}