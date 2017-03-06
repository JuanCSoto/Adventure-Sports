// <copyright file="AuditComp.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    
    /// <summary>
    /// represents a audit information
    /// </summary>
    public class AuditComp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditComp"/> class
        /// </summary>
        public AuditComp()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditComp"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public AuditComp(IDataRecord obj)
        {
            this.Username = Convert.ToString(obj["Names"]);
            this.Auditaction = Convert.ToString(obj["Auditaction"]);
            this.Description = Convert.ToString(obj["Description"]);
            this.Joindate = Convert.ToDateTime(obj["Joindate"]);
        }

        /// <summary>
        /// Gets or sets the user name
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the action audit
        /// </summary>
        public string Auditaction { get; set; }

        /// <summary>
        /// Gets or sets the description audit
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the join date audit
        /// </summary>
        public DateTime? Joindate { get; set; }
    }
}
