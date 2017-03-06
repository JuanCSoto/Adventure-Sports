// <copyright file="Banners.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using Business.Services;
    using Domain.Entities;
    
    /// <summary>
    /// management the banners information
    /// </summary>
    public class Banners : IAdmin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Banners"/> class
        /// </summary>
        public Banners()
        {
        }

        /// <summary>
        /// Gets or sets a list of banners
        /// </summary>
        public IEnumerable<Banner> CollBanners { get; set; }

        /// <summary>
        /// Gets or sets the information banner
        /// </summary>
        public Banner Banner { get; set; }

        /// <summary>
        /// Gets or sets a <c>PaginInfo</c> object
        /// </summary>
        public PaginInfo Pagininfo { get; set; }

        /// <summary>
        /// Gets or sets a list of Position items
        /// </summary>
        public IEnumerable<Position> Collposition { get; set; }

        /// <summary>
        /// Gets or sets a string to render a tree view
        /// </summary>
        public string TreeView { get; set; }

        /// <summary>
        /// Gets or sets identifiers of sections
        /// </summary>
        public string SectionsId { get; set; }

        /// <summary>
        /// Gets or sets if banners is a home
        /// </summary>
        public bool? IsHome { get; set; }

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