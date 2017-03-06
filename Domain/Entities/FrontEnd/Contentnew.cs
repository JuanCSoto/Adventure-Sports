// <copyright file="Contentnew.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;

    /// <summary>
    /// Custom object
    /// </summary>
    public class Contentnew
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Contentnew"/> class
        /// </summary>
        public Contentnew() 
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contentnew"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Contentnew(IDataRecord obj)
        {
            this.Content = new Content(obj);
            this.News = new News(obj);
            this.Filename = obj["Filename"] != DBNull.Value ? obj["Filename"].ToString() : this.Filename;
        }

        /// <summary>
        /// Gets or sets the contents object
        /// </summary>
        public Content Content { get; set; }

        /// <summary>
        /// Gets or sets the news object
        /// </summary>
        public News News { get; set; }

        /// <summary>
        /// Gets or sets the file name
        /// </summary>
        public string Filename { get; set; }
    }
}