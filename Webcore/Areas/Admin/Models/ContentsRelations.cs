// <copyright file="ContentsRelations.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using Domain.Entities;
    
    /// <summary>
    /// obtains the information to a model content relation
    /// </summary>
    public class ContentsRelations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentsRelations"/> class
        /// </summary>
        public ContentsRelations()
        {
        }

        /// <summary>
        /// Gets or sets the relation contents
        /// </summary>
        public IEnumerable<ContentRel> CollContentRel { get; set; }

        /// <summary>
        /// Gets or sets the unrelated contents
        /// </summary>
        public IEnumerable<ContentRel> CollContentNoRel { get; set; }

        /// <summary>
        /// Gets or sets the <c>PaginInfo</c> object
        /// </summary>
        public PaginInfo Pagininfo { get; set; }
    }
}