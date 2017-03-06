// <copyright file="IdeaVote.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// IdeaVote object mapped table <c>IdeaVote</c>.
    /// </summary>
    public class IdeaVote
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdeaVote"/> class
        /// </summary>
        public IdeaVote()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdeaVote"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public IdeaVote(IDataRecord obj)
        {
            this.IdeaId = Convert.ToInt32(obj["IdeaId"]);
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.Favorable = Convert.ToBoolean(obj["Favorable"]);
            this.Date = Convert.ToDateTime(obj["Date"]);
        }

        /// <summary>
        /// Gets or sets the idea id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@IdeaId")]
        public int? IdeaId { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets a value indication whether the vote has favorable or not
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Favorable")]
        public bool? Favorable { get; set; }

        /// <summary>
        /// Gets or sets the date
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@Date")]
        public DateTime? Date { get; set; }
    }
}