// <copyright file="SuccessStoryBroker.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
namespace Core.Brokers
{
    using Core.Bussiness.Repositories;
    using Domain.Entities.FrontEnd;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System.Data;
    using System.Data.Common;

    /// <summary>
    /// Representa la clase <see cref="SuccessStoryBroker"/>.
    /// </summary>
    internal class SuccessStoryBroker : ISuccessStoryRepository
    {
        #region Constantes

        protected const string CONNECTION_NAME = "keyconn";

        #endregion

        #region Procedimientos Almacenados

        private const string CTSUCCESSSTORYFRONTENDBYID = "CTSuccessStoryFrontEndById";

        #endregion

        /// <summary>
        /// Inicializa una instancia de la clase <see cref="CategoryBroker"/>.
        /// </summary>
        public SuccessStoryBroker()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(), false);
        }

        /// <summary>
        /// Consulta el caso de exito deseado.
        /// </summary>
        /// <param name="id">Id del caso</param>
        /// <param name="languageId">Id del lenguaje.</param>
        /// <returns>Caso de exito.</returns>
        public SuccessStoryList GetById(int id, int? languageId)
        {
            Database dataBase = DatabaseFactory.CreateDatabase(CONNECTION_NAME);

            SuccessStoryList story = new SuccessStoryList();

            DbCommand dbCommand = dataBase.GetStoredProcCommand(CTSUCCESSSTORYFRONTENDBYID);
            dataBase.AddInParameter(dbCommand, "@ContentId", System.Data.DbType.Int32, id);
            dataBase.AddInParameter(dbCommand, "@LanguageId", System.Data.DbType.Int32, languageId);

            using (IDataReader dataReader = dataBase.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    story = new SuccessStoryList(dataReader);
                }
            }

            return story;
        }
    }
}
