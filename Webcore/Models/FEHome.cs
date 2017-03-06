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
    /// <History>
    /// Modificado por      :   Juan Carlos Soto Cruz (JCS)
    /// Descripción cambio  :   Se retira la propiedad para el control del Token y se traslada a la clase FESeccion.cs
    /// Fecha               :   2015/11/06 
    /// </History> 
    public class FEHome : ILayout
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FEHome"/> class
        /// </summary>
        public FEHome()
        {
        }

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
        /// Gets or sets the expiring question collection
        /// </summary>
        public List<Domain.Entities.FrontEnd.ExpiringQuestions> ExpiringQuestions { get; set; }

        /// <summary>
        /// Gets or sets the expiring challenge collection
        /// </summary>
        public List<Domain.Entities.FrontEnd.ExpiringChallenges> ExpiringChallenges { get; set; }

        /// <summary>
        /// Gets or sets the featured challenge question collection
        /// </summary>
        public List<Domain.Entities.FrontEnd.FeaturedChallengesQuestions> FeaturedChallengesQuestions { get; set; }

        /// <summary>
        /// Gets or sets the pulses collection
        /// </summary>
        public List<Domain.Entities.FrontEnd.Pulse> Pulses { get; set; }

        /// <summary>
        /// Gets or sets the participants collection
        /// </summary>
        public List<Domain.Entities.FrontEnd.UserProfilePaging> Participants { get; set; }

        /// <summary>
        /// Gets or sets the idea count
        /// </summary>
        public int IdeasCountAll { get; set; }

        /// <summary>
        /// Gets or sets the best of all content object
        /// </summary>
        public ContentBestOfAll BestOfAll { get; set; }

        /// <summary>
        /// Gets or sets the finished challenge question collection
        /// </summary>
        public List<Domain.Entities.FrontEnd.FeaturedChallengesQuestions> FinishedChallengesQuestions { get; set; }
    }
}