// <copyright file="Log.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Log</c> object mapped table <c>Log</c>.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class
        /// </summary>
        public Log()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Log(IDataRecord obj)
        {
            this.LogId = Convert.ToInt32(obj["LogId"]);
            this.Message = Convert.ToString(obj["Message"]);
            this.Joindate = Convert.ToDateTime(obj["Joindate"]);
            this.Description = Convert.ToString(obj["Description"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the Log
        /// </summary>
        [InfoDatabase(DbType.Int32, "@LogId")]
        public int? LogId { get; set; }

        /// <summary>
        /// Gets or sets the name of the field log
        /// </summary>
        [InfoDatabase(DbType.String, "@Message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the creation date
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@Joindate")]
        public DateTime? Joindate { get; set; }

        /// <summary>
        /// Gets or sets the description log
        /// </summary>
        [InfoDatabase(DbType.String, "@Description")]
        public string Description { get; set; }
    }
}