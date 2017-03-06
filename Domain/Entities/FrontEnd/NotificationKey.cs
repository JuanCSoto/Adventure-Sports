// <copyright file="NotificationKey.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// NotificationKey object mapped table <c>NotificationKey</c>.
    /// </summary>
    public class NotificationKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationKey"/> class
        /// </summary>
        public NotificationKey()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationKey"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public NotificationKey(IDataRecord obj)
        {
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.Key = Convert.ToString(obj["Key"]);
        }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the content id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentId")]
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the key
        /// </summary>
        [InfoDatabase(DbType.String, "@Key")]
        public string Key { get; set; }
    }
}