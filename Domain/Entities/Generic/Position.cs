// <copyright file="Position.cs" company="Dasigno">
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
    /// <c>Position</c> object mapped table <c>Position</c>.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> class
        /// </summary>
        public Position()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Position(IDataRecord obj)
        {
            this.PositionId = Convert.ToInt32(obj["PositionId"]);
            this.Name = Convert.ToString(obj["Name"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the position of the banner
        /// </summary>
        [InfoDatabase(DbType.Int32, "@PositionId")]
        public int? PositionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the position
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        [Required(ErrorMessage = "Debes ingresar el nombre de la posición")]
        public string Name { get; set; }
    }
}