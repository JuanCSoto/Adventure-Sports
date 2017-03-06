// <copyright file="News.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>News</c> object mapped table <c>News</c>.
    /// </summary>
    public class News
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="News"/> class
        /// </summary>
        public News()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="News"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public News(IDataRecord obj)
        {
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.Xmlcontent = obj["Xmlcontent"] != DBNull.Value ? obj["Xmlcontent"].ToString() : this.Xmlcontent;
            this.MoldId = obj["MoldId"] != DBNull.Value ? Convert.ToInt32(obj["MoldId"]) : this.MoldId;
            this.Dynamicinformation = obj["Xmlcontent"] != DBNull.Value ? new Dynamicproperties(obj["Xmlcontent"].ToString()) : null;
        }

        /// <summary>
        /// Gets or sets the identifier of the content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentId")]
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the xml content
        /// </summary>
        [InfoDatabase(DbType.String, "@Xmlcontent")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public string Xmlcontent { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the mold
        /// </summary>
        [InfoDatabase(DbType.Int32, "@MoldId")]
        public int? MoldId { get; set; }

        /// <summary>
        /// Gets or sets the XML properties
        /// </summary>
        public Dynamicproperties Dynamicinformation { get; set; }

        /// <summary>
        /// Obtiene o establece los identificadores de los tags.
        /// </summary>
        [InfoDatabase(DbType.String, "@ExistingTags")]
        public string ExistingTags { get; set; }

        /// <summary>
        /// Obtiene o establece los nombres de los nuevos tags.
        /// </summary>
        [InfoDatabase(DbType.String, "@NewTags")]
        public string NewTags { get; set; }
    }
}