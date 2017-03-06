// <copyright file="EmailNotification.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// EmailNotification object mapped table <c>EmailNotification</c>.
    /// </summary>
    public class EmailNotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailNotification"/> class
        /// </summary>
        public EmailNotification()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailNotification"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public EmailNotification(IDataRecord obj)
        {
            this.EmailNotificationId = Convert.ToInt32(obj["EmailNotificationId"]);
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.Value = Convert.ToString(obj["Value"]);
            this.TargetURL = Convert.ToString(obj["TargetURL"]);
            this.Seen = Convert.ToBoolean(obj["Seen"]);
            this.CreationDate = Convert.ToDateTime(obj["CreationDate"]);
        }

        /// <summary>
        /// Gets or sets the email notification id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@EmailNotificationId")]
        public int? EmailNotificationId { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        [InfoDatabase(DbType.String, "@Value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the target url
        /// </summary>
        [InfoDatabase(DbType.String, "@TargetURL")]
        public string TargetURL { get; set; }

        /// <summary>
        /// Gets or sets a value indication whether the notification had been seen by the user
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Seen")]
        public bool? Seen { get; set; }

        /// <summary>
        /// Gets or sets the creation date
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@CreationDate")]
        public DateTime? CreationDate { get; set; }
    }
}