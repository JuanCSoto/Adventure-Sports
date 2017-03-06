// <copyright file="FEPerfil.cs" company="Dasigno">
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
    /// represents the model to any content
    /// </summary>
    public class FEPerfil : ILayout
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FEPerfil"/> class
        /// </summary>
        public FEPerfil()
        {
        }

        /// <summary>
        /// Gets or sets the country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the setting collection
        /// </summary>
        public List<Domain.Entities.UserSetting> CollSetting { get; set; }

        /// <summary>
        /// Gets or sets the interest collection
        /// </summary>
        public List<Domain.Entities.Interest> CollInterest { get; set; }

        /// <summary>
        /// Gets or sets the user interest collection
        /// </summary>
        public List<Domain.Entities.UserInterest> CollUserInterest { get; set; }

        /// <summary>
        /// Gets or sets the related user collection
        /// </summary>
        public List<Domain.Entities.User> CollRelatedUsers { get; set; }

        /// <summary>
        /// Gets or sets the "my" idea collection
        /// </summary>
        public List<Domain.Entities.FrontEnd.MyIdeasPaging> CollMyIdeas { get; set; }

        /// <summary>
        /// Gets or sets the idea collection
        /// </summary>
        public List<Domain.Entities.FrontEnd.IdeasPaging> CollIdeas { get; set; }

        /// <summary>
        /// Gets or sets the user object
        /// </summary>
        public Domain.Entities.User ObjUser { get; set; }

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
        /// Gets or sets the user points
        /// </summary>
        public int Medallos { get; set; }

        /// <summary>
        /// Gets or sets the user rank
        /// </summary>
        public string Rank { get; set; }
    }
}