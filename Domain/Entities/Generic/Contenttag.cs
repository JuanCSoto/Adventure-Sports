// <copyright file="Contenttag.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Contenttag</c> object mapped table <c>Contenttag</c>.
    /// </summary>
    public class Contenttag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Contenttag"/> class
        /// </summary>
        public Contenttag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contenttag"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Contenttag(IDataRecord obj)
        {
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.TagId = Convert.ToInt32(obj["TagId"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the Content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentId")]
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the tag
        /// </summary>
        [InfoDatabase(DbType.Int32, "@TagId")]
        public int? TagId { get; set; }
    }
}