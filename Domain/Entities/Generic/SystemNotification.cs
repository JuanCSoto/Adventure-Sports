// <copyright file="SystemNotification.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// SystemNotification object mapped table <c>SystemNotification</c>.
    /// </summary>
    public class SystemNotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemNotification"/> class
        /// </summary>
        public SystemNotification()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemNotification"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public SystemNotification(IDataRecord obj)
        {
            this.SystemNotificationId = Convert.ToInt32(obj["SystemNotificationId"]);
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.ActionUserId = obj["ActionUserId"] != DBNull.Value ? Convert.ToInt32(obj["ActionUserId"]) : this.ActionUserId;
            this.Value = Convert.ToString(obj["Value"]);
            this.TargetURL = Convert.ToString(obj["TargetURL"]);
            this.Seen = Convert.ToBoolean(obj["Seen"]);
            this.CreationDate = Convert.ToDateTime(obj["CreationDate"]);
            this.UserImage = obj["UserImage"] != DBNull.Value ? obj["UserImage"].ToString() : this.UserImage;
        }

        /// <summary>
        /// Gets or sets the system notification id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@SystemNotificationId")]
        public int? SystemNotificationId { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the action user id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ActionUserId")]
        public int? ActionUserId { get; set; }

        /// <summary>
        /// Gets or sets the template id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@TemplateId")]
        public int? TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the element id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ElementId")]
        public int? ElementId { get; set; }

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

        /// <summary>
        /// Gets or sets the user image
        /// </summary>
        public string UserImage { get; set; }
    }
}