// <copyright file="Ally.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// Ally object mapped table <c>Ally</c>.
    /// </summary>
    public class Ally
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ally"/> class
        /// </summary>
        public Ally()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ally"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Ally(IDataRecord obj)
        {
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.Link = Convert.ToString(obj["Link"]);
            this.Size = Convert.ToInt32(obj["Size"]);
        }

        /// <summary>
        /// Gets or sets the content id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentId")]
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the content link
        /// </summary>
        [InfoDatabase(DbType.String, "@Link")]
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the "size" of the content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Size")]
        public int? Size { get; set; }

        /// <summary>
        /// Obtiene o establece los identificadores de los tags.
        /// </summary>        
        public string ExistingTags { get; set; }

        /// <summary>
        /// Obtiene o establece los nombres de los nuevos tags.
        /// </summary>        
        public string NewTags { get; set; }
    }
}