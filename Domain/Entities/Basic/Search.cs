// <copyright file="Search.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    
    /// <summary>
    /// Represent a item with a information to search contents or modules
    /// </summary>
    public class Search
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Search"/> class
        /// </summary>
        public Search()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Search"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Search(IDataRecord obj)
        {
            this.Id = Convert.ToInt32(obj["Id"]);
            this.Name = obj["Name"].ToString();
            this.Type = obj["Type"].ToString();
            this.Controller = obj["Controller"].ToString();
            this.ModulId = Convert.ToInt32(obj["ModulId"]);
            this.IsContent = Convert.ToBoolean(obj["IsContent"]);
        }
        
        /// <summary>
        /// Gets or sets Identifier of item search
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets a name of item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a type of module
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a name of controller
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Gets or sets the identifier module
        /// </summary>
        public int? ModulId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is a content.
        /// </summary>
        public bool IsContent { get; set; }
    }
}
