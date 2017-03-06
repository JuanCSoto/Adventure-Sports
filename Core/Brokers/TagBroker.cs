// <copyright file="TagBroker.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace Core.DataAccess
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using Core.Bussiness.Repositories;
    using Domain.Entities;
    using Domain.Entities.Enums;
    using Microsoft.Practices.EnterpriseLibrary.Data;

    /// <summary>
    /// Representa la clase <see cref="Core.DataAccess.TagBroker"/>.
    /// </summary>
    internal class TagBroker : ITagRepository
    {
        #region Constantes

        protected const string CONNECTION_NAME = "keyconn";

        #endregion

        #region Procedimientos Almacenados

        private const string GX_TAG = "GXTag";

        private const string CT_TAGS_BY_SUCCESSSTORY_POSTULATE = "CTTagsBySuccessStoryPostulate";

        #endregion

        /// <summary>
        /// Inicializa una instancia de la clase <see cref="Core.DataAccess.TagBroker"/>.
        /// </summary>
        public TagBroker()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(), false);
        }

        /// <summary>
        /// Consulta los tags configurados en el sistema.
        /// </summary>        
        /// <returns>Una lista con los tags configurados en el sistema.</returns>
        public IList<Tag> GetAll()
        {
            Database dataBase = DatabaseFactory.CreateDatabase(CONNECTION_NAME);

            IList<Tag> tagList = new List<Tag>();

            DbCommand dbCommand = dataBase.GetStoredProcCommand(GX_TAG);
            dataBase.AddInParameter(dbCommand, "@Action", DbType.Int32, DataBaseActionEnum.Select);

            using (IDataReader dataReader = dataBase.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    tagList.Add(new Tag(dataReader));
                }
            }

            return tagList;
        }

        /// <summary>
        /// Consulta los tags asociados a la postulación de un caso de éxito.
        /// </summary>
        /// <param name="idSuccessStoryPostulate">Id de la postulación de un caso de éxito.</param>
        /// <returns>Una lista con los tags asociados a la postulación de un caso de éxito.</returns>
        public IList<Tag> GetBySuccessStoryPostulate(int idSuccessStoryPostulate)
        {
            Database dataBase = DatabaseFactory.CreateDatabase(CONNECTION_NAME);

            IList<Tag> tagList = new List<Tag>();

            DbCommand dbCommand = dataBase.GetStoredProcCommand(CT_TAGS_BY_SUCCESSSTORY_POSTULATE);
            dataBase.AddInParameter(dbCommand, "@IdSuccessStoryPostulate", DbType.Int32, idSuccessStoryPostulate);

            using (IDataReader dataReader = dataBase.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    tagList.Add(new Tag(dataReader));
                }
            }

            return tagList;
        }
    }
}
