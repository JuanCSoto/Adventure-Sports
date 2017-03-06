// <copyright file="Versus.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Versus</c> object mapped table <c>Versus</c>.
    /// </summary>
    public class Versus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Versus"/> class
        /// </summary>
        public Versus()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Versus"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Versus(IDataRecord obj)
        {
            this.VersusId = Convert.ToInt32(obj["VersusId"]);
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.IdeaIdA = Convert.ToInt32(obj["IdeaIdA"]);
            this.IdeaIdB = Convert.ToInt32(obj["IdeaIdB"]);
            this.WinnerId = Convert.ToInt32(obj["WinnerId"]);
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.Date = Convert.ToDateTime(obj["Date"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the Versus
        /// </summary>
        [InfoDatabase(DbType.Int32, "@VersusId")]
        public int? VersusId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentId")]
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the Versus idea a
        /// </summary>
        [InfoDatabase(DbType.Int32, "@IdeaIdA")]
        public int? IdeaIdA { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the Versus idea b
        /// </summary>
        [InfoDatabase(DbType.Int32, "@IdeaIdB")]
        public int? IdeaIdB { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the Versus winner idea
        /// </summary>
        [InfoDatabase(DbType.Int32, "@WinnerId")]
        public int? WinnerId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user that participated in the versus
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the date of the versus
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@Date")]
        public DateTime? Date { get; set; }
    }
}
