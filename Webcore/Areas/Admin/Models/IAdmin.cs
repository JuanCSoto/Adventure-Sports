// <copyright file="IAdmin.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using Business.Services;
    using Domain.Entities;
    
    /// <summary>
    /// defines a properties to fill administrator pages
    /// </summary>
    public interface IAdmin
    {
        /// <summary>
        /// Gets or sets the information module
        /// </summary>
        Modul Module { get; set; }

        /// <summary>
        /// Gets or sets the list of modules
        /// </summary>
        IEnumerable<Modul> ColModul { get; set; }

        /// <summary>
        /// Gets or sets a user application
        /// </summary>
        CustomPrincipal UserPrincipal { get; set; }

        /// <summary>
        /// Gets or sets the language application
        /// </summary>
        Language CurrentLanguage { get; set; }
    }
}
