// <copyright file="CityBroker.cs" company="Intergrupo">
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
    /// Representa la clase <see cref="Core.DataAccess.CityBroker"/>.
    /// </summary>
    internal class CityBroker : ICityRepository
    {
        #region Constantes

        protected const string CONNECTION_NAME = "keyconn";

        #endregion

        #region Procedimientos Almacenados

        private const string GX_CITY = "GXCity";

        #endregion

        /// <summary>
        /// Inicializa una instancia de la clase <see cref="Core.DataAccess.CityBroker"/>.
        /// </summary>
        public CityBroker()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(), false);
        }

        /// <summary>
        /// Consulta las ciudades configuradas en el sistema filtradas por país.
        /// </summary>
        /// <param name="countryId">Id del país.</param>
        /// <returns>Una lista con las ciudades configuradas en el sistema filtradas por país.</returns>
        public IList<City> GetByCountry(int countryId)
        {
            Database dataBase = DatabaseFactory.CreateDatabase(CONNECTION_NAME);

            IList<City> cityList = new List<City>();

            DbCommand dbCommand = dataBase.GetStoredProcCommand(GX_CITY);
            dataBase.AddInParameter(dbCommand, "@Action", DbType.Int32, DataBaseActionEnum.Select);
            dataBase.AddInParameter(dbCommand, "@CountryID", DbType.Int32, countryId);

            using (IDataReader dataReader = dataBase.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    cityList.Add(new City(dataReader));
                }
            }

            return cityList;
        }
    }
}
