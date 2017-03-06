// <copyright file="GeneralFindBussiness.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTest")]

namespace Core.Bussiness
{
    using System.Collections.Generic;
    using Core.Bussiness.Contracts;
    using Core.Bussiness.Repositories;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Interfaz de tipo <see cref="IGeneralFindRepository"/> para acceder al repositorio de la busqueda general.
    /// </summary>
    internal class GeneralFindBussiness : IGeneralFindBussiness
    {
        /// <summary>
        /// Interfaz de tipo <see cref="IGeneralFindRepository"/> para acceder al repositorio de la busqueda general.
        /// </summary>
        private IGeneralFindRepository findRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralFindBussiness" /> class.
        /// </summary>
        /// <param name="findRepository">Interfaz de tipo <see cref="IGeneralFindRepository"/> para acceder al repositorio de la busqueda general.</param>
        public GeneralFindBussiness(IGeneralFindRepository findRepository)
        {
            this.findRepository = findRepository;
        }

        #region Implenetacion de la Interface

        /// <summary>
        /// Entrega el listado completo de todas las posibles busquedas.
        /// </summary>
        /// <returns>Lista de la clase GeneralFind.</returns>
        public List<GeneralFind> GetAll()
        {
            return this.findRepository.GetAll();
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
            return this.findRepository.GetByFindOr(name, type, pageIndex, pageSize, out totalCount);
        }

        /// <summary>
        /// Entrega el listado AND de la busqueda por el nombre y/o tipo.
        /// </summary>
        /// <param name="name">Informacion a buscar.</param>
        /// <param name="type">Tipo de busqueda retos, casos de exitos, blogs.</param>
        /// <param name="pageIndex">page index.</param>
        /// <param name="pageSize">page size.</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send.</param>
        /// <returns>Lista de la clase GeneralFind.</returns>
        public List<GeneralFindPaging> GetByFindAnd(string name, string type, int pageIndex, int pageSize, out int totalCount)
        {
            return this.findRepository.GetByFindAnd(name, type, pageIndex, pageSize, out totalCount);
        }

        #endregion Implenetacion de la Interface
    }
}