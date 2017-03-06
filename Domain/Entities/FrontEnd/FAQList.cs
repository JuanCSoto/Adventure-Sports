// <copyright file="FAQList.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities.FrontEnd
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;

    /// <summary>
    /// represents the model to any content
    /// </summary>
    public class FAQList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FAQList"/> class
        /// </summary>
        public FAQList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FAQList"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public FAQList(IDataRecord obj)
        {
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.Name = Convert.ToString(obj["Name"]);
            this.Description = obj["Description"] != DBNull.Value ? Convert.ToString(obj["Description"]) : this.Description;
        }

        /// <summary>
        /// Gets or sets the content id
        /// </summary>
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }
    }
}