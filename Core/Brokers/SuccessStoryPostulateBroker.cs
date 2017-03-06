// <copyright file="SuccessStoryPostulateBroker.cs" company="Intergrupo">
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
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Representa la clase <see cref="Core.DataAccess.SuccessStoryPostulateBroker"/>.
    /// </summary>
    internal class SuccessStoryPostulateBroker : ISuccessStoryPostulateRepository
    {
        #region Constantes

        protected const string CONNECTION_NAME = "keyconn";

        #endregion

        #region Procedimientos Almacenados

        private const string GX_SUCCESS_STORY_POSTULATE = "GXSuccessStoryPostulate";
        private const string CT_SUCCESS_STORY_POSTULATE_PAGING = "CTSuccessStoryPostulatePaging";

        #endregion

        /// <summary>
        /// Inicializa una instancia de la clase <see cref="Core.DataAccess.SuccessStoryPostulateBroker"/>.
        /// </summary>
        public SuccessStoryPostulateBroker()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(), false);
        }

        /// <summary>
        /// Consulta todos los casos de éxito postulados.
        /// </summary>        
        /// <returns>Una lista con los casos de éxito postulados.</returns>
        public IList<SuccessStoryPostulate> GetAll()
        {
            Database dataBase = DatabaseFactory.CreateDatabase(CONNECTION_NAME);

            IList<SuccessStoryPostulate> successStoryPostulateList = new List<SuccessStoryPostulate>();

            DbCommand dbCommand = dataBase.GetStoredProcCommand(GX_SUCCESS_STORY_POSTULATE);
            dataBase.AddInParameter(dbCommand, "@Action", DbType.Int32, DataBaseActionEnum.Select);

            using (IDataReader dataReader = dataBase.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    successStoryPostulateList.Add(new SuccessStoryPostulate(dataReader));
                }
            }

            return successStoryPostulateList;
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
            Database dataBase = DatabaseFactory.CreateDatabase(CONNECTION_NAME);

            IList<SuccessStoryPostulatePaging> successStoryPostulateList = new List<SuccessStoryPostulatePaging>();

            DbCommand dbCommand = dataBase.GetStoredProcCommand(CT_SUCCESS_STORY_POSTULATE_PAGING);
            dataBase.AddOutParameter(dbCommand, "@TotalCount", DbType.Int32, -1);
            dataBase.AddInParameter(dbCommand, "@PageIndex", DbType.Int32, pageIndex);
            dataBase.AddInParameter(dbCommand, "@PageSize", DbType.Int32, pageSize);
            dataBase.AddInParameter(dbCommand, "@LanguageID", DbType.Int32, language);

            using (IDataReader dataReader = dataBase.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    successStoryPostulateList.Add(new SuccessStoryPostulatePaging(dataReader));
                }
            }

            totalCount = System.Convert.ToInt32(dataBase.GetParameterValue(dbCommand, "@TotalCount"));

            return successStoryPostulateList;
        }

        /// <summary>
        /// Consulta la postulación de un caso de éxito.
        /// </summary>
        /// <param name="id">Id de la postulación del caso de éxito a consultar</param>
        /// <param name="languageId">Id del lenguaje</param>
        /// <returns>La postulación del caso de éxito.</returns>
        public SuccessStoryPostulate GetById(int id, int languageId)
        {
            Database dataBase = DatabaseFactory.CreateDatabase(CONNECTION_NAME);

            SuccessStoryPostulate successStoryPostulate = new SuccessStoryPostulate();

            DbCommand dbCommand = dataBase.GetStoredProcCommand(GX_SUCCESS_STORY_POSTULATE);
            dataBase.AddInParameter(dbCommand, "@Action", DbType.Int32, DataBaseActionEnum.SelectByPrimaryKey);
            dataBase.AddInParameter(dbCommand, "@Id", System.Data.DbType.Int32, id);
            dataBase.AddInParameter(dbCommand, "@LanguageId", System.Data.DbType.Int32, languageId);

            using (IDataReader dataReader = dataBase.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    successStoryPostulate = new SuccessStoryPostulate(dataReader);
                }
            }

            return successStoryPostulate;
        }

        /// <summary>
        /// Actualiza la postulación de un caso de éxito.
        /// </summary>
        /// <param name="successStoryPostulate">Postulación del caso de éxito a guardar.</param>
        /// <returns>true si se realizó la actualizacion, de lo contrario false.</returns>
        public bool Update(SuccessStoryPostulate successStoryPostulate)
        {
            Database dataBase = DatabaseFactory.CreateDatabase(CONNECTION_NAME);
            DbCommand dbCommand = dataBase.GetStoredProcCommand(GX_SUCCESS_STORY_POSTULATE);
            dataBase.AddInParameter(dbCommand, "@Id", DbType.Int32, successStoryPostulate.Id);
            dataBase.AddInParameter(dbCommand, "@Action", DbType.Int32, DataBaseActionEnum.Update);
            dataBase.AddInParameter(dbCommand, "@UserId", DbType.Int32, successStoryPostulate.UserId);
            dataBase.AddInParameter(dbCommand, "@CategoryId", DbType.Int32, successStoryPostulate.CategoryId);
            dataBase.AddInParameter(dbCommand, "@ResponsibleNames", DbType.String, successStoryPostulate.ResponsibleNames);
            dataBase.AddInParameter(dbCommand, "@ResponsibleEmail", DbType.String, successStoryPostulate.ResponsibleEmail);
            dataBase.AddInParameter(dbCommand, "@ResponsibleJobTitle", DbType.String, successStoryPostulate.ResponsibleJobTitle);
            dataBase.AddInParameter(dbCommand, "@ResponsibleOrganization", DbType.String, successStoryPostulate.ResponsibleOrganization);
            dataBase.AddInParameter(dbCommand, "@CityId", DbType.Int32, successStoryPostulate.CityId);
            dataBase.AddInParameter(dbCommand, "@ResponsibleEntities", DbType.String, successStoryPostulate.ResponsibleEntities);
            dataBase.AddInParameter(dbCommand, "@Name", DbType.String, successStoryPostulate.Name);
            dataBase.AddInParameter(dbCommand, "@CreationDate", DbType.DateTime, successStoryPostulate.CreationDate);
            dataBase.AddInParameter(dbCommand, "@Description", DbType.String, successStoryPostulate.Description);
            dataBase.AddInParameter(dbCommand, "@ConcreteProblems", DbType.String, successStoryPostulate.ConcreteProblems);
            dataBase.AddInParameter(dbCommand, "@InnovativeUrbanSolution", DbType.String, successStoryPostulate.InnovativeUrbanSolution);
            dataBase.AddInParameter(dbCommand, "@IdsTag", DbType.String, successStoryPostulate.IdsTag);
            dataBase.AddInParameter(dbCommand, "@Documents", DbType.String, successStoryPostulate.Documents);
            dataBase.AddInParameter(dbCommand, "@State", DbType.Byte, successStoryPostulate.State);
            dataBase.AddInParameter(dbCommand, "@SuccessStoryId", DbType.Int32, successStoryPostulate.SuccessStoryId);
            dataBase.AddInParameter(dbCommand, "@LanguageId", DbType.Int32, successStoryPostulate.LanguageId);

            return (int)dataBase.ExecuteNonQuery(dbCommand) == 0 ? false : true;
        }

        /// <summary>
        /// Guarda la postulación de un caso de éxito.
        /// </summary>
        /// <param name="successStoryPostulate">Postulación del caso de éxito a guardar.</param>
        /// <returns>true si se realizó el guardado, de lo contrario false.</returns>
        public bool Save(SuccessStoryPostulate successStoryPostulate)
        {
            Database dataBase = DatabaseFactory.CreateDatabase(CONNECTION_NAME);

            DbCommand dbCommand = dataBase.GetStoredProcCommand(GX_SUCCESS_STORY_POSTULATE);
            dataBase.AddInParameter(dbCommand, "@Action", DbType.Int32, DataBaseActionEnum.Insert);
            dataBase.AddInParameter(dbCommand, "@UserId", DbType.Int32, successStoryPostulate.UserId);
            dataBase.AddInParameter(dbCommand, "@CategoryId", DbType.Int32, successStoryPostulate.CategoryId);
            dataBase.AddInParameter(dbCommand, "@ResponsibleNames", DbType.String, successStoryPostulate.ResponsibleNames);
            dataBase.AddInParameter(dbCommand, "@ResponsibleEmail", DbType.String, successStoryPostulate.ResponsibleEmail);
            dataBase.AddInParameter(dbCommand, "@ResponsibleJobTitle", DbType.String, successStoryPostulate.ResponsibleJobTitle);
            dataBase.AddInParameter(dbCommand, "@ResponsibleOrganization", DbType.String, successStoryPostulate.ResponsibleOrganization);
            dataBase.AddInParameter(dbCommand, "@CityId", DbType.Int32, successStoryPostulate.CityId);
            dataBase.AddInParameter(dbCommand, "@ResponsibleEntities", DbType.String, successStoryPostulate.ResponsibleEntities);
            dataBase.AddInParameter(dbCommand, "@Name", DbType.String, successStoryPostulate.Name);
            dataBase.AddInParameter(dbCommand, "@CreationDate", DbType.DateTime, successStoryPostulate.CreationDate);
            dataBase.AddInParameter(dbCommand, "@Description", DbType.String, successStoryPostulate.Description);
            dataBase.AddInParameter(dbCommand, "@ConcreteProblems", DbType.String, successStoryPostulate.ConcreteProblems);
            dataBase.AddInParameter(dbCommand, "@InnovativeUrbanSolution", DbType.String, successStoryPostulate.InnovativeUrbanSolution);
            dataBase.AddInParameter(dbCommand, "@IdsTag", DbType.String, successStoryPostulate.IdsTag);
            dataBase.AddInParameter(dbCommand, "@Documents", DbType.String, successStoryPostulate.Documents);
            dataBase.AddInParameter(dbCommand, "@State", DbType.Byte, successStoryPostulate.State);
            dataBase.AddInParameter(dbCommand, "@LanguageId", DbType.Int32, successStoryPostulate.LanguageId);

            return (int)dataBase.ExecuteScalar(dbCommand) == 0 ? false : true;
        }
    }
}
