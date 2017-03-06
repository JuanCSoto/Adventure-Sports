// <copyright file="ILayout.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Models
{
    using System.Collections.Generic;
    using System.Web;
    using Business.Services;
    using Domain.Entities;
    
    /// <summary>
    /// defines a properties to fill layout page
    /// </summary>
    public interface ILayout
    {
        /// <summary>
        /// Gets or sets a list of meta tags
        /// </summary>
        List<KeyValuePair<KeyValue, KeyValue>> MetaTags { get; set; }
        
        /// <summary>
        /// Gets or sets a user application
        /// </summary>
        CustomPrincipal UserPrincipal { get; set; }

        /// <summary>
        /// Gets or sets the section
        /// </summary>
        Section Section { get; set; }

        /// <summary>
        /// Gets or sets the content
        /// </summary>
        Content Content { get; set; }

        /// <summary>
        /// Gets or sets a list of banner
        /// </summary>
        List<Banner> Banners { get; set; }

        /// <summary>
        /// Gets or sets the page title
        /// </summary>
        string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the layout page
        /// </summary>
        string Layout { get; set; }

        /// <summary>
        /// Gets or sets the deep follower
        /// </summary>
        HtmlString DeepFollower { get; set; }

        /// <summary>
        /// Gets or sets the language application
        /// </summary>
        Language CurrentLanguage { get; set; }

        /// <summary>
        /// Gets or sets the idea count
        /// </summary>
        int IdeasCountAll { get; set; }
    }
}
