// <copyright file="FAQ.cs" company="Dasigno">
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
    /// <c>Challenge</c> object mapped table <c>Challenge</c>.
    /// </summary>
    public class FAQ
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FAQ"/> class
        /// </summary>
        public FAQ()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FAQ"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public FAQ(IDataRecord obj)
        {
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.Description = Convert.ToString(obj["Description"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentId")]
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the FAQ description
        /// </summary>
        [InfoDatabase(DbType.String, "@Description")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar la descripción")]
        public string Description { get; set; }

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
