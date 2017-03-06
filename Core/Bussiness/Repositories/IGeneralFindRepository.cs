// <copyright file="IFindRepository.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
namespace Core.Bussiness.Repositories
{
    using Domain.Entities.FrontEnd;
    using System.Collections.Generic;

    /// <summary>
    /// Expone las acciones para acceder al repositorio de la busqueda general <see cref="IGeneralFindRepository"/>.
    /// </summary>
    public interface IGeneralFindRepository
    {
        /// <summary>
        /// Entrega el listado completo de todas las posibles busquedas.
        /// </summary>
        /// <returns>Lista de la clase GeneralFind.</returns>
        List<GeneralFind> GetAll();

        /// <summary>
        /// Entrega el listado OR de la busqueda por el nombre y/o tipo.
        /// </summary>
        /// <param name="name">Informacion a buscar.</param>
        /// <param name="type">Tipo de busqueda retos, casos de exitos, blogs.</param>
        /// <param name="pageIndex">page index.</param>
        /// <param name="pageSize">page size.</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send.</param>
        /// <returns>Lista de la clase GeneralFind.</returns>
        List<GeneralFindPaging> GetByFindOr(string name, string type, int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// Entrega el listado AND de la busqueda por el nombre y/o tipo.
        /// </summary>
        /// <param name="name">Informacion a buscar.</param>
        /// <param name="type">Tipo de busqueda retos, casos de exitos, blogs.</param>
        /// <param name="pageIndex">page index.</param>
        /// <param name="pageSize">page size.</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send.</param>
        /// <returns>Lista de la clase GeneralFind.</returns>
        List<GeneralFindPaging> GetByFindAnd(string name, string type, int pageIndex, int pageSize, out int totalCount);
    }
}