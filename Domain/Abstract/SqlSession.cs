// <copyright file="SqlSession.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Abstract
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Represents the interaction with the data base
    /// </summary>
    public class SqlSession : ISession
    {
        /// <summary>
        /// Represents a transact instruction in the data base
        /// </summary>
        private SqlCommand command = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlSession"/> class.
        /// </summary>
        public SqlSession()
        {
            this.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["keyconn"].ConnectionString);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the interaction is a transaction.
        /// </summary>
        public bool IsTransaction { get; set; }

        /// <summary>
        /// Gets or sets a connection to data base
        /// </summary>
        public SqlConnection Connection { get; set; }

        /// <summary>
        /// Gets or sets a transaction to data base
        /// </summary>
        public SqlTransaction Transaction { get; set; }

        /// <summary>
        /// Gets or sets a way to read a data stream
        /// </summary>
        public IDataReader Reader { get; set; }

        /// <summary>
        /// Create a new instance of Command
        /// </summary>
        /// <param name="sqlText">The transact statement</param>
        /// <param name="usestoredprocedure">If user Stored procedure or a text command</param>
        public void CreateCommand(string sqlText, bool usestoredprocedure)
        {
            this.command = new SqlCommand()
            {
                CommandText = sqlText,
                CommandType = usestoredprocedure ? CommandType.StoredProcedure : CommandType.Text,
                CommandTimeout = this.Connection.ConnectionTimeout,
                Connection = this.Connection
            };

            if (this.IsTransaction)
            {
                this.Open();
                if (null == this.Transaction)
                {
                    this.Transaction = this.Connection.BeginTransaction(IsolationLevel.ReadCommitted);
                }

                this.command.Transaction = this.Transaction;
            }
        }
        
        /// <summary>
        /// Add parameter to collection
        /// </summary>
        /// <param name="parameter">a new SQL parameter</param>
        public void AddParameter(SqlParameter parameter)
        {
            this.command.Parameters.Add(parameter);
        }

        /// <summary>
        /// Add parameter to collection
        /// </summary>
        /// <param name="name">Name to the parameter</param>
        /// <param name="value">Value of the parameter</param>
        /// <param name="type">The type of the parameter</param>
        public void AddParameter(string name, object value, DbType type)
        {
            this.AddParameter(name, value, ParameterDirection.Input, type);
        }

        /// <summary>
        /// Add parameter to collection
        /// </summary>
        /// <param name="name">Name to the parameter</param>
        /// <param name="value">Value of the parameter</param>
        /// <param name="direction">Indicates whether the parameter is input-only or output-only or bidirectional</param>
        /// <param name="type">The type of the parameter</param>
        public void AddParameter(string name, object value, ParameterDirection direction, DbType type)
        {
            this.command.Parameters.Add(new SqlParameter()
            {
                DbType = type,
                Direction = direction,
                ParameterName = name,
                Value = value
            });
        }

        /// <summary>
        /// Gets value of parameter
        /// </summary>
        /// <param name="nameParameter">The name of the parameter to retrieve</param>
        /// <returns>Get result of parameter</returns>
        public object GetOutParameter(string nameParameter)
        {
            return this.command.Parameters[nameParameter].Value;
        }

        /// <summary>
        /// Sends and executes query or stored procedure
        /// </summary>
        /// <returns>Returns the number of rows affected</returns>
        public int ExecuteNonQuery()
        {
            try
            {
                this.Open();
                return this.command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!this.IsTransaction)
                {
                    this.Close();
                }
            }
        }

        /// <summary>
        /// Sends and executes query or stored procedure
        /// </summary>
        /// <returns>Returns a table with the result</returns>
        public DataTable GetTable()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            try
            {
                adapter = new SqlDataAdapter(this.command);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!this.IsTransaction)
                {
                }
            }

            return table;
        }

        /// <summary>
        /// Sends and executes query or stored procedure
        /// </summary>
        /// <returns>Returns a data set with the result</returns>
        public DataSet GetDataSet()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            try
            {
                adapter = new SqlDataAdapter(this.command);
                adapter.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!this.IsTransaction)
                {
                }
            }

            return ds;
        }

        /// <summary>
        /// Sends and executes query or stored procedure
        /// </summary>
        /// <returns>Returns the first column of the first row</returns>
        public object ExecuteScalar()
        {
            try
            {
                this.Open();
                return this.command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!this.IsTransaction)
                {
                    this.Close();
                }
            }
        }

        /// <summary>
        /// Sends and executes query or stored procedure
        /// </summary>
        public void ExecuteReader()
        {
            try
            {
                this.Open();
                if (this.IsTransaction)
                {
                    this.Reader = this.command.ExecuteReader();
                }
                else
                {
                    this.Reader = this.command.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                this.Close();
                throw ex;
            }
        }

        /// <summary>
        /// Advances to the next record
        /// </summary>
        /// <returns>true if there are more rows; otherwise, false.</returns>
        public bool Read()
        {
            try
            {
                if (this.Reader.Read())
                {
                    return true;
                }
                else
                {
                    this.CloseReader();
                    if (!this.IsTransaction)
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                this.CloseReader();
                throw ex;
            }

            return false;
        }

        /// <summary>
        /// Gets the column located at the specified index
        /// </summary>
        /// <param name="name">The name of column to get</param>
        /// <returns>he column located at the specified index as an System.Object.</returns>
        public object GetValue(string name)
        {
            return this.Reader[name];
        }

        /// <summary>
        /// Gets the column located at the specified index
        /// </summary>
        /// <param name="index">The zero-base index of column to get</param>
        /// <returns>he column located at the specified index as an System.Object.</returns>
        public object GetValue(int index)
        {
            return this.Reader[index];
        }

        /// <summary>
        /// Closes the data reader
        /// </summary>
        public void CloseReader()
        {
            if (this.Reader != null)
            {
                this.Reader.Close();
            }
        }

        /// <summary>
        /// Commits the data base transaction
        /// </summary>
        public void Commit()
        {
            if (this.Transaction != null)
            {
                this.Transaction.Commit();
                this.Close();
            }
        }

        /// <summary>
        /// Rolls back a transaction
        /// </summary>
        public void RollBack()
        {
            if (this.Transaction != null)
            {
                this.Transaction.Rollback();
                this.Close();
            }
        }

        /// <summary>
        /// Opens a data base connection
        /// </summary>
        public void Open()
        {
            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }
        }

        /// <summary>
        /// Closes a data base connection
        /// </summary>
        public void Close()
        {
            if (this.Connection != null)
            {
                this.Connection.Close();
            }
        }

        /// <summary>
        /// Starts a database transaction.
        /// </summary>
        public void Begin()
        {
            this.IsTransaction = true;
            this.Open();
            this.Transaction = this.Connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.Reader != null && this.Reader.IsClosed)
            {
                this.Reader.Dispose();
            }

            if (this.Transaction != null)
            {
                this.Transaction.Dispose();
            }

            if (this.command != null && this.command.Connection.State == ConnectionState.Closed)
            {
                this.command.Dispose();
                this.Connection.Dispose();
            }
        }

        #endregion
    }
}