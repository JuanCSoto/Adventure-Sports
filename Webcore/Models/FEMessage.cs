// <copyright file="FEMessage.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Models
{
    using System.Collections.Generic;
    using System.Web;
    using Business.Services;
    
    /// <summary>
    /// represents the model for custom message page
    /// </summary>
    public class FEMessage : ILayout
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FEMessage"/> class
        /// </summary>
        public FEMessage()
        {
        }

        /// <summary>
        /// Gets or sets the title of page
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description page
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the message page
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a list of banner
        /// </summary>
        public List<Domain.Entities.Banner> Banners { get; set; }

        /// <summary>
        /// Gets or sets a list of meta tags
        /// </summary>
        public List<KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>> MetaTags { get; set; }

        /// <summary>
        /// Gets or sets a user application
        /// </summary>
        public CustomPrincipal UserPrincipal { get; set; }

        /// <summary>
        /// Gets or sets the section
        /// </summary>
        public Domain.Entities.Section Section { get; set; }

        /// <summary>
        /// Gets or sets the content
        /// </summary>
        public Domain.Entities.Content Content { get; set; }

        /// <summary>
        /// Gets or sets the page title
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the layout page
        /// </summary>
        public string Layout { get; set; }

        /// <summary>
        /// Gets or sets the deep follower
        /// </summary>
        public HtmlString DeepFollower { get; set; }

        /// <summary>
        /// Gets or sets the language application
        /// </summary>
        public Domain.Entities.Language CurrentLanguage { get; set; }

        /// <summary>
        /// Gets or sets the idea count
        /// </summary>
        public int IdeasCountAll { get; set; }
    }
}