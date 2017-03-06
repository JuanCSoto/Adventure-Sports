// <copyright file="Parametros.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Configuration;
    using Business.Services;
    using Domain.Entities;

    /// <summary>
    /// management the parameters information
    /// </summary>
    public class Parametros : IAdmin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parametros"/> class
        /// </summary>
        public Parametros()
        {
        }

        /// <summary>
        /// Gets or sets a list of <c>KeyValue</c> object
        /// </summary>
        public KeyValueConfigurationCollection CollValues { get; set; }

        /// <summary>
        /// Gets or sets a key value pair
        /// </summary>
        public KeyValue Keyvalue { get; set; }

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