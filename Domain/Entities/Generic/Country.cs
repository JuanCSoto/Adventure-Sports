// <copyright file="Country.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// Country object mapped table <c>Country</c>.
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class
        /// </summary>
        public Country()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Country(IDataRecord obj)
        {
            this.CountryID = Convert.ToInt32(obj["CountryID"]);
            this.NameEn = obj["NameEn"] != DBNull.Value ? Convert.ToString(obj["NameEn"]) : this.NameEn;
            this.NameEs = obj["NameEs"] != DBNull.Value ? Convert.ToString(obj["NameEs"]) : this.NameEs;
        }

        /// <summary>
        /// Gets or sets the country id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@CountryID")]
        public int? CountryID { get; set; }

        /// <summary>
        /// Gets or sets the name in english
        /// </summary>
        [InfoDatabase(DbType.String, "@NameEn")]
        public string NameEn { get; set; }

        /// <summary>
        /// Gets or sets the name in spanish
        /// </summary>
        [InfoDatabase(DbType.String, "@NameEs")]
        public string NameEs { get; set; }
    }
}