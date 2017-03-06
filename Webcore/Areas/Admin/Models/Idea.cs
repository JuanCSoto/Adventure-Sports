// <copyright file="Idea.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using Business.Services;
    using Domain.Entities;
    
    /// <summary>
    /// management the idea information
    /// </summary>
    public class Idea : IAdmin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Idea"/> class
        /// </summary>
        public Idea()
        {
        }

        /// <summary>
        /// Gets or sets a list of idea
        /// </summary>
        public IEnumerable<Domain.Entities.Idea> CollIdea { get; set; }

        /// <summary>
        /// Gets or sets a total number of ideas
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