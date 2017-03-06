// <copyright file="Language.cs" company="Dasigno">
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
    /// <c>Language</c> object mapped table <c>Language</c>.
    /// </summary>
    public class Language
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Language"/> class
        /// </summary>
        public Language()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Language"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Language(IDataRecord obj)
        {
            this.LanguageId = Convert.ToInt32(obj["LanguageId"]);
            this.Name = Convert.ToString(obj["Name"]);
            this.Culturename = Convert.ToString(obj["Culturename"]);
            this.IsDefault = Convert.ToBoolean(obj["IsDefault"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the language
        /// </summary>
        [InfoDatabase(DbType.Int32, "@LanguageId")]
        public int? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the name of the language
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        [Required(ErrorMessage = "El campo nombre es requerido")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the culture name associated with the language
        /// </summary>
        [InfoDatabase(DbType.String, "@Culturename")]
        [Required(ErrorMessage = "El campo prefijo es requerido")]
        public string Culturename { get; set; }

        /// <summary>
        /// Gets or sets if the language is default of the application
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@IsDefault")]
        public bool? IsDefault { get; set; }
    }
}