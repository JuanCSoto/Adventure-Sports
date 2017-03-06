// <copyright file="ContentRel.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;

    /// <summary>
    /// represent a basic information to content
    /// </summary>
    public class ContentRel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentRel"/> class
        /// </summary>
        public ContentRel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentRel"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public ContentRel(IDataRecord obj)
        {
            this.Name = obj["Name"].ToString();
            this.ModulName = obj["ModulName"] != DBNull.Value ? obj["ModulName"].ToString() : this.ModulName;
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
        }

        /// <summary>
        /// Gets or sets the content name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the module name
        /// </summary>
        public string ModulName { get; set; }

        /// <summary>
        /// Gets or sets content identifier
        /// </summary>
        public int ContentId { get; set; }
    }
}
