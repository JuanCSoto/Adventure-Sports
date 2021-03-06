﻿// <copyright file="Templates.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using Business.Services;
    using Domain.Entities;

    /// <summary>
    /// management the template information
    /// </summary>
    public class Templates : IAdmin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Templates"/> class
        /// </summary>
        public Templates()
        {
        }

        /// <summary>
        /// Gets or sets a list of templates
        /// </summary>
        public IEnumerable<Template> CollTemplate { get; set; }

        /// <summary>
        /// Gets or sets a template information
        /// </summary>
        public Template TemplateCustom { get; set; }

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