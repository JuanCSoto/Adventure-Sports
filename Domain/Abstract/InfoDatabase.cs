// <copyright file="InfoDatabase.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Abstract
{
    using System;
    using System.Data;

    /// <summary>
    /// Sets the attributes of the object's fields  
    /// </summary>
    public class InfoDatabase : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InfoDatabase"/> class
        /// </summary>
        /// <param name="type">Specifies the data type of the field</param>
        /// <param name="name">Specifies the name of the parameter in stored procedure</param>
        public InfoDatabase(DbType type, string name)
        {
            this.Type = type;
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the data type of field
        /// </summary>
        public DbType Type { get; set; }

        /// <summary>
        /// Gets or sets the name of stored procedure's parameter
        /// </summary>
        public string Name { get; set; }
    }
}