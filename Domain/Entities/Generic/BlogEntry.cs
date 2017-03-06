// <copyright file="BlogEntry.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>BlogEntry</c> object mapped table <c>BlogEntry</c>.
    /// </summary>
    public class BlogEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogEntry"/> class
        /// </summary>
        public BlogEntry()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogEntry"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public BlogEntry(IDataRecord obj)
        {
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.Description = Convert.ToString(obj["Description"]);
            this.DescriptionIngles = Convert.ToString(obj["Description2"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentId")]
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the text of the entry
        /// </summary>
        [InfoDatabase(DbType.String, "@Description")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar el cuerpo del contenido")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the text of the entry second language
        /// </summary>
        [InfoDatabase(DbType.String, "@Description2")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar el cuerpo del contenido")]
        public string DescriptionIngles { get; set; }

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
