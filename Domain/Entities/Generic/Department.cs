// <copyright file="Department.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Department</c> object mapped table <c>Department</c>.
    /// </summary>
    public class Department
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Department"/> class
        /// </summary>
        public Department()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Department"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Department(IDataRecord obj)
        {
            this.DepartmentId = Convert.ToInt32(obj["DepartmentId"]);
            this.CountryId = Convert.ToInt32(obj["CountryId"]);
            this.Name = Convert.ToString(obj["Name"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the department
        /// </summary>
        [InfoDatabase(DbType.Int32, "@DepartmentId")]
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the country
        /// </summary>
        [InfoDatabase(DbType.Int32, "@CountryId")]
        public int? CountryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the country
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        public string Name { get; set; }
    }
}