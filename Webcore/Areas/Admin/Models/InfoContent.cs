// <copyright file="InfoContent.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    /// <summary>
    /// obtains a basic information content
    /// </summary>
    public class InfoContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InfoContent"/> class
        /// </summary>
        public InfoContent()
        {
        }

        /// <summary>
        /// Gets or sets the information of content
        /// </summary>
        public Domain.Entities.Content Content { get; set; }

        /// <summary>
        /// Gets or sets the deep follower content
        /// </summary>
        public string DeepFollower { get; set; }

        /// <summary>
        /// Gets or sets the creator content
        /// </summary>
        public string Autor { get; set; }

        /// <summary>
        /// Gets or sets the number of relation content
        /// </summary>
        public int CountContent { get; set; }
    }
}