// <copyright file="GeneralFind.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
namespace Domain.Entities.FrontEnd
{
    using System;
    using System.Data;

    /// <summary>
    /// represents the model to General Find
    /// </summary>
    public class GeneralFind
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralFind"/> class
        /// </summary>
        public GeneralFind()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralFind"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public GeneralFind(IDataRecord obj)
        {
            this.Name = obj["Name"] != DBNull.Value ? Convert.ToString(obj["Name"]) : this.Name;
        }

        /// <summary>
        /// Gets or sets Name from select.
        /// </summary>
        public string Name { get; set; }
    }
}