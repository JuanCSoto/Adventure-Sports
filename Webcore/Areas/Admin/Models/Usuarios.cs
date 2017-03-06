// <copyright file="Usuarios.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using Business.Services;
    using Domain.Entities;
    
    /// <summary>
    /// obtains the information user to administrate
    /// </summary>
    public class Usuarios : IAdmin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Usuarios"/> class
        /// </summary>
        public Usuarios()
        {
        }

        /// <summary>
        /// Gets or sets the list of users
        /// </summary>
        public IEnumerable<User> CollUsers { get; set; }

        /// <summary>
        /// Gets or sets the list of roles
        /// </summary>
        public IEnumerable<Rol> CollRols { get; set; }

        /// <summary>
        /// Gets or sets a list of <c>RolUser</c> object
        /// </summary>
        public IEnumerable<RolUser> CollUserrol { get; set; }

        /// <summary>
        /// Gets or sets the <c>Pagininfo</c> object
        /// </summary>
        public PaginInfo Pagininfo { get; set; }

        /// <summary>
        /// Gets or sets the information user
        /// </summary>
        public User UserCustom { get; set; }

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