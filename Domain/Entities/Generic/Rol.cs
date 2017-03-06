// <copyright file="Rol.cs" company="Dasigno">
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
    /// <c>Rol</c> object mapped table <c>Rol</c>.
    /// </summary>
    public class Rol
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rol"/> class
        /// </summary>
        public Rol()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rol"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Rol(IDataRecord obj)
        {
            this.RolId = Convert.ToInt32(obj["RolId"]);
            this.Name = Convert.ToString(obj["Name"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the role
        /// </summary>
        [InfoDatabase(DbType.Int32, "@RolId")]
        public int? RolId { get; set; }

        /// <summary>
        /// Gets or sets the name of the role
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        [Required(ErrorMessage = "Debes ingresar el nombre del Rol")]
        public string Name { get; set; }
    }
}