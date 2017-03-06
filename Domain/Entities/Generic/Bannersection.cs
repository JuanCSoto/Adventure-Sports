// <copyright file="Bannersection.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Bannersection</c> object mapped table <c>Bannersection</c>.
    /// </summary>
    public class Bannersection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bannersection"/> class
        /// </summary>
        public Bannersection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bannersection"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Bannersection(IDataRecord obj)
        {
            this.BannerId = Convert.ToInt32(obj["BannerId"]);
            this.SectionId = Convert.ToInt32(obj["SectionId"]);
            this.Orderliness = Convert.ToInt32(obj["Orderliness"]);
        }

        /// <summary>
        /// Gets or sets the identifier of banner
        /// </summary>
        [InfoDatabase(DbType.Int32, "@BannerId")]
        public int? BannerId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of section
        /// </summary>
        [InfoDatabase(DbType.Int32, "@SectionId")]
        public int? SectionId { get; set; }

        /// <summary>
        /// Gets or sets the order of the banner
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Orderliness")]
        public int? Orderliness { get; set; }
    }
}