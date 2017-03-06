// <copyright file="Neighborhood.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// Neighborhood object mapped table <c>Neighborhood</c>.
    /// </summary>
    public class Neighborhood
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Neighborhood"/> class
        /// </summary>
        public Neighborhood()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Neighborhood"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Neighborhood(IDataRecord obj)
        {
            this.NeighborhoodId = Convert.ToInt32(obj["NeighborhoodId"]);
            this.CityId = Convert.ToInt32(obj["CityId"]);
            this.Name = Convert.ToString(obj["Name"]);
        }

        /// <summary>
        /// Gets or sets the neighborhood id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@NeighborhoodId")]
        public int? NeighborhoodId { get; set; }

        /// <summary>
        /// Gets or sets the city id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@CityId")]
        public int? CityId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        public string Name { get; set; }
    }
}