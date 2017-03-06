// <copyright file="GeneralFindFacade.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
namespace Core.Facades
{
    using Core.Brokers;
    using Core.Bussiness;
    using Core.Bussiness.Contracts;
    using Domain.Entities.FrontEnd;
    using System.Collections.Generic;

    public class GeneralFindFacade
    {
        /// <summary>
        /// Entrega el listado completo de todas las posibles busquedas.
        /// </summary>
        /// <returns>Lista de la clase GeneralFind.</returns>
        public List<GeneralFind> GetAll()
        {
            IGeneralFindBussiness successBussiness = new GeneralFindBussiness(new GeneralFindBroker());
            return successBussiness.GetAll();
        }

        /// <summary>
        /// Entrega el listado OR de la busqueda por el nombre y/o tipo.
        /// </summary>
        /// <param name="name">Informacion a buscar.</param>
        /// <param name="type">Tipo de busqueda retos, casos de exitos, blogs.</param>
        /// <param name="pageIndex">page index.</param>
        /// <param name="pageSize">page size.</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send.</param>
        /// <returns>Lista de la clase GeneralFind.</returns>
        public List<GeneralFindPaging> GetByFindOr(string name, string type, int pageIndex, int pageSize, out int totalCount)
        {
            IGeneralFindBussiness successBussiness = new GeneralFindBussiness(new GeneralFindBroker());
            return successBussiness.GetByFindOr(name, type, pageIndex, pageSize, out totalCount);
        }

        /// <summary>
        /// Entrega el listado de la busqueda por el nombre y/o tipo.
        /// </summary>
        /// <param name="name">Informacion a buscar.</param>
        /// <param name="type">Tipo de busqueda retos, casos de exitos, blogs.</param>
        /// <param name="pageIndex">page index.</param>
        /// <param name="pageSize">page size.</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send.</param>
        /// <returns>Lista de la clase GeneralFind.</returns>
        public List<GeneralFindPaging> GetByFindAnd(string name, string type, int pageIndex, int pageSize, out int totalCount)
        {
            IGeneralFindBussiness successBussiness = new GeneralFindBussiness(new GeneralFindBroker());
            return successBussiness.GetByFindAnd(name, type, pageIndex, pageSize, out totalCount);
        }
    }
}