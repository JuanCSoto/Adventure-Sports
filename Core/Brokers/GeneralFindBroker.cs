// <copyright file="GeneralFindBroker.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
namespace Core.Brokers
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using Core.Bussiness.Repositories;
    using Domain.Entities.FrontEnd;
    using Microsoft.Practices.EnterpriseLibrary.Data;

    /// <summary>
    /// Representa la clase <see cref="GeneralFindBroker"/>.
    /// </summary>
    internal class GeneralFindBroker : IGeneralFindRepository
    {
        #region Constantes

        /// <summary>
        /// Llave de conexion.
        /// </summary>
        protected const string CONNECTIONNAME = "keyconn";

        #endregion Constantes

        #region Procedimientos Almacenados

        /// <summary>
        /// Procedimiento CTFindContentAndTags.
        /// </summary>
        private const string CTFINDCONTENTANDTAGS = "CTFindContentAndTags";

        /// <summary>
        /// Procedimiento CTFindContentPagingOr.
        /// </summary>
        private const string CTFINDCONTENTPAGINGOR = "CTFindContentPagingOr";

        /// <summary>
        /// Procedimiento CTFindContentPagingAnd.
        /// </summary>
        private const string CTFINDCONTENTPAGINGAND = "CTFindContentPagingAnd";

        #endregion Procedimientos Almacenados

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralFindBroker" /> class.
        /// </summary>
        public GeneralFindBroker()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(), false);
        }

        #region Implentacion de IGeneralFindRepository

        /// <summary>
        /// Entrega el listado completo de todas las posibles busquedas.
        /// </summary>
        /// <returns>Lista de la clase GeneralFind.</returns>
        public List<GeneralFind> GetAll()
        {
            List<GeneralFind> listFind = new List<GeneralFind>();
            GeneralFind find = new GeneralFind();

            Database dataBase = DatabaseFactory.CreateDatabase(CONNECTIONNAME);
            DbCommand dbcommand = dataBase.GetStoredProcCommand(CTFINDCONTENTANDTAGS);

            using (IDataReader dataReader = dataBase.ExecuteReader(dbcommand))
            {
                while (dataReader.Read())
                {
                    find = new GeneralFind(dataReader);
                    listFind.Add(find);
                }
            }

            return listFind;
        }

        /// <summary>
        /// Entrega el listado OR de la busqueda por el nombre y/o tipo.
        /// </summary>
        /// <param name="search">Informacion a buscar.</param>
        /// <param name="type">Tipo de busqueda retos, casos de exitos, blogs.</param>
        /// <param name="pageIndex">page index.</param>
        /// <param name="pageSize">page size.</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send.</param>
        /// <returns>Lista de la clase GetByFindOr.</returns>
        public List<GeneralFindPaging> GetByFindOr(string search, string type, int pageIndex, int pageSize, out int totalCount)
        {
            List<GeneralFindPaging> listFind = new List<GeneralFindPaging>();
            GeneralFindPaging find = new GeneralFindPaging();

            Database dataBase = DatabaseFactory.CreateDatabase(CONNECTIONNAME);
            DbCommand dbcommand = dataBase.GetStoredProcCommand(CTFINDCONTENTPAGINGOR);
            dataBase.AddOutParameter(dbcommand, "@TotalCount", DbType.Int32, -1);
            dataBase.AddInParameter(dbcommand, "@PageIndex", DbType.Int32, pageIndex);
            dataBase.AddInParameter(dbcommand, "@PageSize", DbType.Int32, pageSize);
            dataBase.AddInParameter(dbcommand, "@Search", System.Data.DbType.String, search);
            dataBase.AddInParameter(dbcommand, "@Type", System.Data.DbType.String, type);

            using (IDataReader dataReader = dataBase.ExecuteReader(dbcommand))
            {
                while (dataReader.Read())
                {
                    find = new GeneralFindPaging(dataReader);
                    listFind.Add(find);
                }
            }

            totalCount = System.Convert.ToInt32(dataBase.GetParameterValue(dbcommand, "@TotalCount"));

            return listFind;
        }

        /// <summary>
        /// Entrega el listado AND de la busqueda por el nombre y/o tipo.
        /// </summary>
        /// <param name="search">Informacion a buscar.</param>
        /// <param name="type">Tipo de busqueda retos, casos de exitos, blogs.</param>
        /// <param name="pageIndex">page index.</param>
        /// <param name="pageSize">page size.</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send.</param>
        /// <returns>Lista de la clase GetByFindAnd.</returns>
        public List<GeneralFindPaging> GetByFindAnd(string search, string type, int pageIndex, int pageSize, out int totalCount)
        {
            List<GeneralFindPaging> listFind = new List<GeneralFindPaging>();
            GeneralFindPaging find = new GeneralFindPaging();

            Database dataBase = DatabaseFactory.CreateDatabase(CONNECTIONNAME);
            DbCommand dbcommand = dataBase.GetStoredProcCommand(CTFINDCONTENTPAGINGAND);
            dataBase.AddOutParameter(dbcommand, "@TotalCount", DbType.Int32, -1);
            dataBase.AddInParameter(dbcommand, "@PageIndex", DbType.Int32, pageIndex);
            dataBase.AddInParameter(dbcommand, "@PageSize", DbType.Int32, pageSize);
            dataBase.AddInParameter(dbcommand, "@Search", System.Data.DbType.String, search);
            dataBase.AddInParameter(dbcommand, "@Type", System.Data.DbType.String, type);

            using (IDataReader dataReader = dataBase.ExecuteReader(dbcommand))
            {
                while (dataReader.Read())
                {
                    find = new GeneralFindPaging(dataReader);
                    listFind.Add(find);
                }
            }

            totalCount = System.Convert.ToInt32(dataBase.GetParameterValue(dbcommand, "@TotalCount"));

            return listFind;
        }

        #endregion Implentacion de IGeneralFindRepository
    }
}