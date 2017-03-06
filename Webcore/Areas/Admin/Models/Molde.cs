// <copyright file="Molde.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Xml;
    using Business.Services;
    using Domain.Entities;

    /// <summary>
    /// management the mold information
    /// </summary>
    public class Molde : IAdmin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Molde"/> class
        /// </summary>
        public Molde()
        {
        }

        /// <summary>
        /// Gets or sets a list of molds
        /// </summary>
        public IEnumerable<Mold> CollMold { get; set; }

        /// <summary>
        /// Gets or sets the mold information
        /// </summary>
        public Mold Mold { get; set; }

        /// <summary>
        /// Gets or sets the <c>XmlDocument</c> to build a template
        /// </summary>
        public XmlDocument Xmldocument { get; set; }

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