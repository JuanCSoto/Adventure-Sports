﻿// <copyright file="Content.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using Business.Services;
    using Domain.Entities;
    
    /// <summary>
    /// management the content information
    /// </summary>
    public class Content : IAdmin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Content"/> class
        /// </summary>
        public Content()
        {
        }

        /// <summary>
        /// Gets or sets a list of content
        /// </summary>
        public IEnumerable<Domain.Entities.Content> CollContent { get; set; }

        /// <summary>
        /// Gets or sets a list of sections
        /// </summary>
        public IEnumerable<Domain.Entities.Section> CollSection { get; set; }

        /// <summary>
        /// Gets or sets a total number of contents
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets a name of controller
        /// </summary>
        public string Controller { get; set; }

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