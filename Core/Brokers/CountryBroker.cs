// <copyright file="CountryBroker.cs" company="Intergrupo">
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
    /// Representa la clase <see cref="Core.DataAccess.CountryBroker"/>.
    /// </summary>
    internal class CountryBroker : ICountryRepository
    {
        #region Constantes

        protected const string CONNECTION_NAME = "keyconn";

        #endregion

        #region Procedimientos Almacenados

        private const string GX_COUNTRY = "GXCountry";

        #endregion

        /// <summary>
        /// Inicializa una instancia de la clase <see cref="Core.DataAccess.CountryBroker"/>.
        /// </summary>
        public CountryBroker()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(), false);
        }

        /// <summary>
        /// Consulta los países configurados en el sistema.
        /// </summary>        
        /// <returns>Una lista con los países configurados en el sistema.</returns>
        public IList<Country> GetAll()
        {
            Database dataBase = DatabaseFactory.CreateDatabase(CONNECTION_NAME);

            IList<Country> countryList = new List<Country>();

            DbCommand dbCommand = dataBase.GetStoredProcCommand(GX_COUNTRY);
            dataBase.AddInParameter(dbCommand, "@Action", DbType.Int32, DataBaseActionEnum.Select);

            using (IDataReader dataReader = dataBase.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    countryList.Add(new Country(dataReader));
                }
            }

            return countryList;
        }
    }
}
