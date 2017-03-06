// <copyright file="Audit.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Audit</c> object mapped table <c>Audit</c>.
    /// </summary>
    public class Audit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Audit"/> class
        /// </summary>
        public Audit()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Audit"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Audit(IDataRecord obj)
        {
            this.AuditId = Convert.ToInt32(obj["AuditId"]);
            this.Username = Convert.ToInt32(obj["Username"]);
            this.Auditaction = Convert.ToString(obj["Auditaction"]);
            this.Description = Convert.ToString(obj["Description"]);
            this.Joindate = Convert.ToDateTime(obj["Joindate"]);
        }

        /// <summary>
        /// Gets or sets the identifier of audit
        /// </summary>
        [InfoDatabase(DbType.Int32, "@AuditId")]
        public int? AuditId { get; set; }

        /// <summary>
        /// Gets or sets the name of user
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Username")]
        public int? Username { get; set; }

        /// <summary>
        /// Gets or sets the action of audit field
        /// </summary>
        [InfoDatabase(DbType.String, "@Auditaction")]
        public string Auditaction { get; set; }

        /// <summary>
        /// Gets or sets the description of the audit field
        /// </summary>
        [InfoDatabase(DbType.String, "@Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the date of the audit field 
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@Joindate")]
        public DateTime? Joindate { get; set; }
    }
}