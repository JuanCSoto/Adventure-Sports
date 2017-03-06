// <copyright file="ISession.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Abstract
{
    using System;
    using System.Data;
using System.Data.SqlClient;

    /// <summary>
    /// Represents the interaction with a repository of information
    /// its implementation can perform actions on data repository
    /// </summary>
    public interface ISession : IDisposable
    {
        /// <summary>
        /// Gets or sets a means of reading one or more forward-only streams of results sets obtained
        /// </summary>
        IDataReader Reader { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the action to run this on transactional
        /// </summary>
        bool IsTransaction { get; set; }

        /// <summary>
        /// Moves the cursor to the next record
        /// </summary>
        /// <returns> true if there are more rows; otherwise, false.</returns>
        bool Read();

        /// <summary>
        /// Closes IDataReader
        /// </summary>
        void CloseReader();

        /// <summary>
        /// Gets the column located in the specified index
        /// </summary>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>he column located at the specified index as an System.Object.</returns>
        object GetValue(string name);

        /// <summary>
        /// Gets the column with the specified name
        /// </summary>
        /// <param name="index">The zero-based index of the column to get.</param>
        /// <returns>he column located at the specified index as an System.Object.</returns>
        object GetValue(int index);

        /// <summary>
        /// Send the Command property and creates a object Data Reader
        /// </summary>
        void ExecuteReader();

        /// <summary>
        /// Send the Command property and creates a object DataTable
        /// </summary>
        /// <returns>A data table</returns>
        DataTable GetTable();

        /// <summary>
        /// Send the Command property and creates a object DataSet
        /// </summary>
        /// <returns>A data table</returns>
        DataSet GetDataSet();

        /// <summary>
        /// starts the transaction
        /// </summary>
        void Begin();

        /// <summary>
        /// Confirms the transaction in the data base
        /// </summary>
        void Commit();

        /// <summary>
        /// Rolls back the transaction in the data base
        /// </summary>
        void RollBack();

        /// <summary>
        /// Creates de object type command
        /// </summary>
        /// <param name="sqltext">text or stored procedure for the transaction</param>
        /// <param name="usestoredprocedure">indicating how the <c>System.Data.SqlClient.SqlCommand.CommandText</c> property is to be interpreted</param>
        void CreateCommand(string sqltext, bool usestoredprocedure);

        /// <summary>
        /// add to the collection's parameters the new parameter
        /// </summary>
        /// <param name="parameter">a new SQL parameter</param>
        void AddParameter(SqlParameter parameter);

        /// <summary>
        /// add to the collection's parameters the new parameter
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value of the parameter</param>
        /// <param name="type">Data Type of the field</param>
        void AddParameter(string name, object value, DbType type);

        /// <summary>
        /// add to the collection's parameters the new parameter
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value of the parameter</param>
        /// <param name="direction">In put or Out put</param>
        /// <param name="type">Data Type of the field</param>
        void AddParameter(string name, object value, ParameterDirection direction, DbType type);

        /// <summary>
        /// Executes a transact query in the data base
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        int ExecuteNonQuery();

        /// <summary>
        /// Executes a transact query in the data base and returns the first column of the first row in the result returned
        /// </summary>
        /// <returns>The first column of the first row in the result set, or a null reference</returns>
        object ExecuteScalar();

        /// <summary>
        /// Open the connection to the data base
        /// </summary>
        void Open();

        /// <summary>
        /// Close the connection to the data base
        /// </summary>
        void Close();

        /// <summary>
        /// Gets value of parameter
        /// </summary>
        /// <param name="nameParameter">The name of the parameter to retrieve</param>
        /// <returns>Get result of parameter</returns>
        object GetOutParameter(string nameParameter);
    }
}