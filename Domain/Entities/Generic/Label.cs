// <copyright file="Label.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Juan Carlos Montoya</author>

namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Label</c> object mapped table <c>Label</c>.
    /// </summary>
    public class Label
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class
        /// </summary>
        public Label()
        {
        }

        public Label(IDataRecord obj)
        {
            this.LabelId = Convert.ToInt32(obj["LabelId"]);
            this.Name = Convert.ToString(obj["Name"]);
            this.Translation = Convert.ToString(obj["Translation"]);
            this.LanguageId = Convert.ToInt32(obj["LanguageId"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the Label
        /// </summary>
        [InfoDatabase(DbType.Int32, "@LabelId")]
        public int? LabelId { get; set; }

        /// <summary>
        /// Gets or sets the name of the Label
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        [Required(ErrorMessage = "Debes ingresar el Nombre del Label")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Translation of the Label
        /// </summary>
        [InfoDatabase(DbType.String, "@Translation")]
        [Required(ErrorMessage = "Debes ingresar el Translation del Label")]
        public string Translation { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the language
        /// </summary>
        [InfoDatabase(DbType.Int32, "@LanguageId")]
        public int? LanguageId { get; set; }
    }
}

