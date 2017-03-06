// <copyright file="RecoveryToken.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// RecoveryToken object mapped table <c>RecoveryToken</c>.
    /// </summary>
    public class RecoveryToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecoveryToken"/> class
        /// </summary>
        public RecoveryToken()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecoveryToken"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public RecoveryToken(IDataRecord obj)
        {
            this.Token = Convert.ToString(obj["Token"]);
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.Creationdate = Convert.ToDateTime(obj["Creationdate"]);
            this.Used = Convert.ToBoolean(obj["Used"]);
        }

        /// <summary>
        /// Gets or sets the token
        /// </summary>
        [InfoDatabase(DbType.String, "@Token")]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the creation date
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@Creationdate")]
        public DateTime? Creationdate { get; set; }

        /// <summary>
        /// Gets or sets a value indication whether the token had been used or not
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Used")]
        public bool? Used { get; set; }
    }
}