// <copyright file="ContentBlogIdea.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Domain.Entities.FrontEnd
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    
    /// <summary>
    /// content blog idea entity object
    /// </summary>
    public class ContentBlogIdea
    {
        /// <summary>
        /// Gets or sets the ideas collection
        /// </summary>
        public List<IdeasPaging> Ideas { get; set; }

        /// <summary>
        /// Gets or sets the blog entry collection
        /// </summary>
        public List<BlogEntriesPaging> BlogEntries { get; set; }
    }
}
