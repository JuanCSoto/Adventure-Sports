// <copyright file="CategoryBroker.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace Core.DataAccess
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using Core.Bussiness.Repositories;
    using Domain.Entities.Enums;
    using Domain.Entities.Generic;
    using Microsoft.Practices.EnterpriseLibrary.Data;

    /// <summary>
    /// Representa la clase <see cref="Core.DataAccess.CategoryBroker"/>.
    /// </summary>
    internal class CategoryBroker : ICategoryRepository
    {
        #region Constantes

        protected const string CONNECTION_NAME = "keyconn";

        #endregion

        #region Procedimientos Almacenados

        private const string GX_CATEGORY = "GXCategory";

        #endregion

        /// <summary>
        /// Inicializa una instancia de la clase <see cref="Core.DataAccess.CategoryBroker"/>.
        /// </summary>
        public CategoryBroker()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(), false);
        }

        /// <summary>
        /// Consulta las categorías configuradas en el sistema.
        /// </summary>
        /// <param name="languageId">Id del lenguaje en el que se mostraran las categorías.</param>
        /// <returns>Una lista con las categorías configuradas en el sistema.</returns>
        public IList<Category> GetAll(int languageId)
        {
            Database dataBase = DatabaseFactory.CreateDatabase(CONNECTION_NAME);

            IList<Category> categoryList = new List<Category>();

            DbCommand dbCommand = dataBase.GetStoredProcCommand(GX_CATEGORY);
            dataBase.AddInParameter(dbCommand, "@Action", DbType.Int32, DataBaseActionEnum.Select);
            dataBase.AddInParameter(dbCommand, "@LanguageId", DbType.Int32, languageId);

            using (IDataReader dataReader = dataBase.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    categoryList.Add(new Category(dataReader));
                }
            }

            return categoryList;
        }
    }
}
