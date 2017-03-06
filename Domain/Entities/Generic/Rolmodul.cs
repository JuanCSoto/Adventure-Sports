// <copyright file="Rolmodul.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Rolmodul</c> object mapped table <c>Rolmodul</c>.
    /// </summary>
    public class Rolmodul
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rolmodul"/> class
        /// </summary>
        public Rolmodul()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rolmodul"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Rolmodul(IDataRecord obj)
        {
            this.RolId = Convert.ToInt32(obj["RolId"]);
            this.ModulId = Convert.ToInt32(obj["ModulId"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the role
        /// </summary>
        [InfoDatabase(DbType.Int32, "@RolId")]
        public int? RolId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the module
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ModulId")]
        public int? ModulId { get; set; }
    }
}