// <copyright file="Modul.cs" company="Dasigno">
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
    /// <c>Modul</c> object mapped table <c>Modul</c>.
    /// </summary>
    public class Modul
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Modul"/> class
        /// </summary>
        public Modul()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Modul"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Modul(IDataRecord obj)
        {
            this.ModulId = Convert.ToInt32(obj["ModulId"]);
            this.ParentId = obj["ParentId"] != DBNull.Value ? Convert.ToInt32(obj["ParentId"]) : this.ParentId;
            this.Name = obj["Name"] != DBNull.Value ? Convert.ToString(obj["Name"]) : this.Name;
            this.Controller = obj["Controller"] != DBNull.Value ? Convert.ToString(obj["Controller"]) : this.Controller;
            this.Active = Convert.ToBoolean(obj["Active"]);
            this.IsContent = Convert.ToBoolean(obj["IsContent"]);
            this.IsBasic = Convert.ToBoolean(obj["IsBasic"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the module
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ModulId")]
        public int? ModulId { get; set; }

        /// <summary>
        /// Gets or sets the parent module of the field
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ParentId")]
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the module
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        [Required(ErrorMessage = "Debes ingresar el nombre del Módulo")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the controller of the module
        /// </summary>
        [InfoDatabase(DbType.String, "@Controller")]
        public string Controller { get; set; }

        /// <summary>
        /// Gets or sets if the module is active
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Active")]
        public bool? Active { get; set; }

        /// <summary>
        /// Gets or sets if the module is the content type or not
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@IsContent")]
        public bool? IsContent { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the language
        /// </summary>
        [InfoDatabase(DbType.Int32, "@LanguageId")]
        public int? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets if the module is the level basic of the application
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@IsBasic")]
        public bool? IsBasic { get; set; }
    }
}