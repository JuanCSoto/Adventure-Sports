// <copyright file="Category.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Domain.Entities.FrontEnd
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Category entity object
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Category(IDataRecord obj)
        {
            this.Name = obj["Category"].ToString();
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }
    }
}
