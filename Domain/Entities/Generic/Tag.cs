// <copyright file="Tag.cs" company="Dasigno">
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
    /// <c>Tag</c> object mapped table <c>Tag</c>.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class
        /// </summary>
        public Tag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Tag(IDataRecord obj)
        {
            this.TagId = obj["TagId"] != DBNull.Value ? Convert.ToInt32(obj["TagId"]) : this.TagId;
            this.Name = Convert.ToString(obj["Name"]);

            if (obj["LanguageId"] != DBNull.Value)
            {
                this.LanguageId = Convert.ToInt32(obj["LanguageId"]);
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the tag
        /// </summary>
        [InfoDatabase(DbType.Int32, "@TagId")]
        public int? TagId { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        [Required(ErrorMessage = "Debes ingresar el Nombre del tag")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the language
        /// </summary>
        [InfoDatabase(DbType.Int32, "@LanguageId")]
        public int? LanguageId { get; set; }
    }
}