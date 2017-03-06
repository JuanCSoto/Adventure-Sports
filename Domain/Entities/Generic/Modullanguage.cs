// <copyright file="Modullanguage.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Modullanguage</c> object mapped table <c>Modullanguage</c>.
    /// </summary>
    public class Modullanguage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Modullanguage"/> class
        /// </summary>
        public Modullanguage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Modullanguage"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Modullanguage(IDataRecord obj)
        {
            this.ModulId = Convert.ToInt32(obj["ModulId"]);
            this.LanguageId = Convert.ToInt32(obj["LanguageId"]);
            this.Name = Convert.ToString(obj["Name"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the module
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ModulId")]
        public int? ModulId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the language
        /// </summary>
        [InfoDatabase(DbType.Int32, "@LanguageId")]
        public int? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the name of the Module according with the language
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        public string Name { get; set; }
    }
}