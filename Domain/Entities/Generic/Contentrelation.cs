// <copyright file="Contentrelation.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Contentrelation</c> object mapped table <c>Contentrelation</c>.
    /// </summary>
    public class Contentrelation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Contentrelation"/> class
        /// </summary>
        public Contentrelation()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contentrelation"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Contentrelation(IDataRecord obj)
        {
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.ContentIdChild = Convert.ToInt32(obj["ContentIdChild"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentId")]
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the relation content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentIdChild")]
        public int? ContentIdChild { get; set; }
    }
}