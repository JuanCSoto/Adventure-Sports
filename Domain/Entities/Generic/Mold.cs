// <copyright file="Mold.cs" company="Dasigno">
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
    /// <c>Mold</c> object mapped table <c>Mold</c>.
    /// </summary>
    public class Mold
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mold"/> class
        /// </summary>
        public Mold()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mold"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Mold(IDataRecord obj)
        {
            this.MoldId = Convert.ToInt32(obj["MoldId"]);
            this.Name = Convert.ToString(obj["Name"]);
            this.Xmlcontent = Convert.ToString(obj["Xmlcontent"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the Mold
        /// </summary>
        [InfoDatabase(DbType.Int32, "@MoldId")]
        public int? MoldId { get; set; }

        /// <summary>
        /// Gets or sets the name of the mold
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the xml template
        /// </summary>
        [InfoDatabase(DbType.String, "@Xmlcontent")]
        public string Xmlcontent { get; set; }
    }
}