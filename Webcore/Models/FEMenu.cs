// <copyright file="FEMenu.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Models
{
    using System.Collections.Generic;
    using Domain.Entities;
    
    /// <summary>
    /// obtains the information to building the menu
    /// </summary>
    public class FEMenu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FEMenu"/> class
        /// </summary>
        public FEMenu()
        {
        }

        /// <summary>
        /// Gets or sets a list of sections
        /// </summary>
        public IEnumerable<Domain.Entities.Section> Sections { get; set; }

        /// <summary>
        /// Gets or sets the identifier section
        /// </summary>
        public int? SectionId { get; set; }

        /// <summary>
        /// Gets or sets the identifier parent section
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the language application
        /// </summary>
        public Language CurrentLanguage { get; set; }

        /// <summary>
        /// Gets or sets the idea count
        /// </summary>
        public int IdeasCountAll { get; set; }
    }
}