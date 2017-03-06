// <copyright file="City.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// City object mapped table <c>City</c>.
    /// </summary>
    public class City
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="City"/> class
        /// </summary>
        public City()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="City"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public City(IDataRecord obj)
        {
            this.CityID = Convert.ToInt32(obj["CityID"]);
            this.CountryID = Convert.ToInt32(obj["CountryID"]);
            this.NameEn = obj["NameEn"] != DBNull.Value ? Convert.ToString(obj["NameEn"]) : this.NameEn;
            this.NameEs = obj["NameEs"] != DBNull.Value ? Convert.ToString(obj["NameEs"]) : this.NameEs;
            this.Subdivision = obj["Subdivision"] != DBNull.Value ? Convert.ToString(obj["Subdivision"]) : this.Subdivision;
        }

        /// <summary>
        /// Gets or sets the city id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@CityID")]
        public int? CityID { get; set; }

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

        /// <summary>
        /// Gets or sets the sub division
        /// </summary>
        [InfoDatabase(DbType.String, "@Subdivision")]
        public string Subdivision { get; set; }
    }
}