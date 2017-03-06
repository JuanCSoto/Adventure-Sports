// <copyright file="FEHome.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Models
{
    using System.Collections.Generic;
    using System.Web;
    using Business.Services;
    using Domain.Entities.FrontEnd;
    
    /// <summary>
    /// represents the model for home application
    /// </summary>
    public class FEHomeSlide : ILayout
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FEHome"/> class
        /// </summary>
        public FEHomeSlide()
        {
        }

       /// <summary>
        /// Gets or sets the content
        /// </summary>
        public object Entity { get; set; }

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

        /// <summary>
        /// Gets or sets the comment collection
        /// </summary>
        public List<Domain.Entities.FrontEnd.CommentsPaging> CollComments { get; set; }

        /// <summary>
        /// Gets or sets the content object
        /// </summary>
        public Domain.Entities.Content ObjContent { get; set; }

        /// <summary>
        /// Gets or sets the user object
        /// </summary>
        public Domain.Entities.User ObjUser { get; set; }

        /// <summary>
        /// Gets or sets the comment count
        /// </summary>
        public int CommentCount { get; set; }
       
    
    }
}