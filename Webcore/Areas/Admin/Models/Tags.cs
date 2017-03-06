// <copyright file="Tags.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using Business.Services;
    using Domain.Entities;

    /// <summary>
    /// management the tag information
    /// </summary>
    public class Tags : IAdmin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tags"/> class
        /// </summary>
        public Tags()
        {
        }

        /// <summary>
        /// Gets or sets a list of tags
        /// </summary>
        public IEnumerable<Tag> CollTags { get; set; }

        /// <summary>
        /// Gets or sets the tag information
        /// </summary>
        public Tag TagCustom { get; set; }

        /// <summary>
        /// Gets or sets a <c>PaginInfo</c> object
        /// </summary>
        public PaginInfo Pagininfo { get; set; }

        /// <summary>
        /// Gets or sets the information module
        /// </summary>
        public Modul Module { get; set; }

        /// <summary>
        /// Gets or sets the list of modules
        /// </summary>
        public IEnumerable<Modul> ColModul { get; set; }

        /// <summary>
        /// Gets or sets a user application
        /// </summary>
        public CustomPrincipal UserPrincipal { get; set; }

        /// <summary>
        /// Gets or sets the language application
        /// </summary>
        public Language CurrentLanguage { get; set; }
    }
}