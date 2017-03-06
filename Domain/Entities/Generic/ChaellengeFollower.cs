// <copyright file="ChaellengeFollower.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// ChallengeFollower object mapped table <c>ChaellengeFollower</c>.
    /// </summary>
    public class ChaellengeFollower
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChaellengeFollower"/> class
        /// </summary>
        public ChaellengeFollower()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChaellengeFollower"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public ChaellengeFollower(IDataRecord obj)
        {
            this.ChallengeId = Convert.ToInt32(obj["ChallengeId"]);
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.Date = Convert.ToDateTime(obj["Date"]);
        }

        /// <summary>
        /// Gets or sets the challenge id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ChallengeId")]
        public int? ChallengeId { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the challenge date
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@Date")]
        public DateTime? Date { get; set; }
    }
}