// <copyright file="Interest.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// Interest object mapped table <c>Interest</c>.
    /// </summary>
    public class Interest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Interest"/> class
        /// </summary>
        public Interest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Interest"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Interest(IDataRecord obj)
        {
            this.InterestId = Convert.ToInt32(obj["InterestId"]);
            this.Name = Convert.ToString(obj["Name"]);
            this.LanguageId = Convert.ToInt32(obj["LanguageId"]);
        }

        /// <summary>
        /// Gets or sets the interest id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@InterestId")]
        public int? InterestId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the interest id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@LanguageId")]
        public int? LanguageId { get; set; }
    }
}