// <copyright file="InfoIdeaReport.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    /// <summary>
    /// obtains a basic information idea
    /// </summary>
    public class InfoIdeaReport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InfoIdeaReport"/> class
        /// </summary>
        public InfoIdeaReport()
        {
        }

        /// <summary>
        /// Gets or sets the information of idea
        /// </summary>
        public Domain.Entities.FrontEnd.IdeaReportPaging IdeaReportPaging { get; set; }

        /// <summary>
        /// Gets or sets the deep follower idea
        /// </summary>
        public string DeepFollower { get; set; }

        /// <summary>
        /// Gets or sets the creator idea
        /// </summary>
        public string Autor { get; set; }
    }
}