// <copyright file="Modelsearch.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using Business.Services;
    using Domain.Entities;
    
    /// <summary>
    /// obtains the information to a search model
    /// </summary>
    public class Modelsearch : IAdmin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Modelsearch"/> class
        /// </summary>
        public Modelsearch()
        {
        }

        /// <summary>
        /// Gets or sets a list of search object
        /// </summary>
        public IEnumerable<Search> CollResult { get; set; }

        /// <summary>
        /// Gets or sets the criteria search
        /// </summary>
        public string Criteria { get; set; }

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