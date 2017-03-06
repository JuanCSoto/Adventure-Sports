// <copyright file="FEListBlogs.cs" company="Intergrupo">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
namespace Webcore.Models
{
    using Domain.Entities.FrontEnd;
    using System.Collections.Generic;

    /// <summary>
    /// represents the model to any content
    /// </summary>
    public class FEListBlogs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FEListBlogs"/> class
        /// </summary>
        public FEListBlogs()
        {
        }

        /// <summary>
        /// Gets or sets list of blogs
        /// </summary>
        public List<BlogEntriesPaging> BlogsList { get; set; }

        /// <summary>
        /// Gets or sets the language application
        /// </summary>
        public Domain.Entities.Language CurrentLanguage { get; set; }
    }
}