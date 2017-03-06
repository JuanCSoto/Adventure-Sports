// <copyright file="IdeaReport.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using Business.Services;
    using Domain.Entities;
    
    /// <summary>
    /// management the ideaReport information
    /// </summary>
    public class IdeaReport : IAdmin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdeaReport"/> class
        /// </summary>
        public IdeaReport()
        {
        }

        /// <summary>
        /// Gets or sets a list of ideaReport
        /// </summary>
        public IEnumerable<Domain.Entities.FrontEnd.IdeaReportPaging> CollIdeaReport { get; set; }

        /// <summary>
        /// Gets or sets a total number of ideaReports
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