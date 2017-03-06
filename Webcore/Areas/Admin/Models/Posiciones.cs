// <copyright file="Posiciones.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using Business.Services;
    using Domain.Entities;

    /// <summary>
    /// management the positions information
    /// </summary>
    public class Posiciones : IAdmin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Posiciones"/> class
        /// </summary>
        public Posiciones()
        {
        }

        /// <summary>
        /// Gets or sets a list of positions
        /// </summary>
        public IEnumerable<Position> CollPositions { get; set; }

        /// <summary>
        /// Gets or sets the position information
        /// </summary>
        public Position Position { get; set; }

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