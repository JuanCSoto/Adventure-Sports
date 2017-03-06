// <copyright file="DataRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Abstract
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;

    /// <summary>
    /// <para></para>
    /// </summary>
    /// <typeparam name="T">the type of the object</typeparam>
    public class DataRepository<T>
    {
        /// <summary>
        /// name of the stored procedure
        /// </summary>
        private string storedprocedure;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRepository{T}"/> class.
        /// </summary>
        /// <param name="session">Represents the interaction with a repository of information</param>
        /// <param name="sp">The name of the parameter</param>
        public DataRepository(ISession session, string sp)
        {
            this.Session = session;
            this.storedprocedure = sp;
        }

        /// <summary>
        /// Gets or sets the type of the object
        /// </summary>
        public T Entity { get; set; }

        /// <summary>
        /// Gets or sets a framework that establishes communication between the application and the database
        /// </summary>
        protected ISession Session { get; set; }

        /// <summary>
        /// insert a new field in to the table
        /// </summary>
        /// <returns>returns the identifier of the object</returns>
        public object Insert()
        {
            this.BindParameters(2);
            return this.Session.ExecuteScalar();
        }

        /// <summary>
        /// update the field to the table
        /// </summary>
        public void Update()
        {
            this.BindParameters(1);
            this.Session.ExecuteNonQuery();
        }

        /// <summary>
        /// delete the field to the table
        /// </summary>
        public void Delete()
        {
            this.BindParameters(3);
            this.Session.ExecuteNonQuery();
        }

        /// <summary>
        /// load the object according to the object properties
        /// </summary>
        public virtual void Load()
        {
            this.BindParameters(0);
            this.Session.ExecuteReader();
        }

        /// <summary>
        /// load the object according to the identifier
        /// </summary>
        public virtual void LoadByKey()
        {
            this.BindParameters(4);
            this.Session.ExecuteReader();
        }

        /// <summary>
        /// Gets a strongly typed list of objects that can be accessed by index.
        /// </summary>
        /// <returns>return a strongly typed list of objects that can be accessed by index.</returns>
        public virtual List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a collection of objects that can be individually accessed by index.
        /// </summary>
        /// <returns>return a collection of objects that can be individually accessed by index.</returns>
        public IList<T> GetAllReadOnly()
        {
            return this.GetAll().AsReadOnly();
        }

        /// <summary>
        /// complete all of the stored procedure parameters
        /// </summary>
        /// <param name="action">determines which action is to be executed</param>
        protected void BindParameters(int action)
        {
            this.Session.CreateCommand(this.storedprocedure, true);
            this.Session.AddParameter("@Action", action, DbType.Int32);

            PropertyInfo[] col = typeof(T).GetProperties();
            foreach (PropertyInfo item in col)
            {
                object val = item.GetValue(this.Entity, null);
                Attribute att = Attribute.GetCustomAttribute(item, typeof(InfoDatabase));

                if (att != null)
                {
                    InfoDatabase atr = att as InfoDatabase;
                    this.Session.AddParameter(atr.Name, val, atr.Type);
                }
            }
        }
    }
}