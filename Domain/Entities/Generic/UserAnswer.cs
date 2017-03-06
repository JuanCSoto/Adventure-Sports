// <copyright file="UserAnswer.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// UserAnswer object mapped table <c>UserAnswer</c>.
    /// </summary>
    public class UserAnswer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAnswer"/> class
        /// </summary>
        public UserAnswer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAnswer"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public UserAnswer(IDataRecord obj)
        {
            this.AnswerId = Convert.ToInt32(obj["AnswerId"]);
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.Date = Convert.ToDateTime(obj["Date"]);
            this.IP = Convert.ToString(obj["IP"]);
        }

        /// <summary>
        /// Gets or sets the answer id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@AnswerId")]
        public int? AnswerId { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the date
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@Date")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the IP
        /// </summary>
        [InfoDatabase(DbType.String, "@IP")]
        public string IP { get; set; }
    }
}