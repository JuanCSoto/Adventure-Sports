// <copyright file="RolUser.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>RolUser</c> object mapped table <c>RolUser</c>.
    /// </summary>
    public class RolUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RolUser"/> class
        /// </summary>
        public RolUser()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RolUser"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public RolUser(IDataRecord obj)
        {
            this.RolId = Convert.ToInt32(obj["RolId"]);
            this.UserId = Convert.ToInt32(obj["UserId"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the role
        /// </summary>
        [InfoDatabase(DbType.Int32, "@RolId")]
        public int? RolId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }
    }
}