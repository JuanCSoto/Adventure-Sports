// <copyright file="Principal.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using Business.Services;
    using Domain.Entities;

    /// <summary>
    /// management the information to principal page administrator
    /// </summary>
    public class Principal : IAdmin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Principal"/> class
        /// </summary>
        public Principal()
        {
        }

        /// <summary>
        /// Gets or sets a list of roles 
        /// </summary>
        public IEnumerable<Rol> CollRols { get; set; }

        /// <summary>
        /// Gets or sets a list of audit actions
        /// </summary>
        public IEnumerable<Audit> CollAudit { get; set; }

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