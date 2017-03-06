// <copyright file="Lenguaje.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using Business.Services;
    using Domain.Entities;

    /// <summary>
    /// management the language information
    /// </summary>
    public class Lenguaje : IAdmin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Lenguaje"/> class
        /// </summary>
        public Lenguaje()
        {
        }

        /// <summary>
        /// Gets or sets a list of language items
        /// </summary>
        public IEnumerable<Language> CollLanguage { get; set; }

        /// <summary>
        /// Gets or sets the language information
        /// </summary>
        public Language EntityLanguage { get; set; }

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