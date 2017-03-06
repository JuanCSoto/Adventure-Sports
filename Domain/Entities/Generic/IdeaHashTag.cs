// <copyright file="IdeaHashTag.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// ContentHashTag object mapped table <c>ContentHashTag</c>.
    /// </summary>
    public class IdeaHashTag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdeaHashTag"/> class
        /// </summary>
        public IdeaHashTag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdeaHashTag"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public IdeaHashTag(IDataRecord obj)
        {
            this.IdeaId = Convert.ToInt32(obj["IdeaId"]);
            this.HashTagId = Convert.ToInt32(obj["HashTagId"]);
        }

        /// <summary>
        /// Gets or sets the idea id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@IdeaId")]
        public int? IdeaId { get; set; }

        /// <summary>
        /// Gets or sets the hash tag id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@HashTagId")]
        public int? HashTagId { get; set; }
    }
}