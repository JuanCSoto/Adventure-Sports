// <copyright file="HashTag.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// HashTag object mapped table <c>HashTag</c>.
    /// </summary>
    public class HashTag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HashTag"/> class
        /// </summary>
        public HashTag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashTag"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public HashTag(IDataRecord obj)
        {
            this.HashTagId = Convert.ToInt32(obj["HashTagId"]);
            this.Value = Convert.ToString(obj["Value"]);
            this.Count = Convert.ToInt32(obj["Count"]);
        }

        /// <summary>
        /// Gets or sets the hash tag id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@HashTagId")]
        public int? HashTagId { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        [InfoDatabase(DbType.String, "@Value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the count
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Count")]
        public int? Count { get; set; }
    }
}