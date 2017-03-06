// <copyright file="UserInterest.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// UserInterest object mapped table <c>UserInterest</c>.
    /// </summary>
    public class UserInterest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterest"/> class
        /// </summary>
        public UserInterest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterest"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public UserInterest(IDataRecord obj)
        {
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.InterestId = Convert.ToInt32(obj["InterestId"]);
        }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the interest id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@InterestId")]
        public int? InterestId { get; set; }
    }
}