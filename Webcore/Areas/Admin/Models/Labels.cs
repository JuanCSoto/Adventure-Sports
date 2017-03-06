// <copyright file="Labels.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Juan Carlos Montoya</author>

namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using Business.Services;
    using Domain.Entities;

    /// <summary>
    /// management the lab information
    /// </summary>
    public class Labels : IAdmin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Labels"/> class
        /// </summary>
        public Labels()
        {
        }

        /// <summary>
        /// Gets or sets a list of Label
        /// </summary>
        public IEnumerable<Label> CollLabels{ get; set; }

        /// <summary>
        /// Gets or sets the Label information
        /// </summary>
        public Label LabelCustom { get; set; }

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