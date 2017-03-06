// <copyright file="FESeccion.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Models
{
    using System.Collections.Generic;
    using System.Web;
    using Business.Services;
    
    /// <summary>
    /// represents the model for section home
    /// </summary>
    /// <typeparam name="T">type of content</typeparam>
    /// <History>
    /// Modificado por      :   Juan Carlos Soto Cruz (JCS)
    /// Descripción cambio  :   Se adiciona la propiedad para el control del Token que se encontraba en la clase FEHome.cs
    /// Fecha               :   2015/11/06 
    /// </History>
    public class FESeccion<T> : ILayout
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FESeccion{T}"/> class
        /// </summary>
        public FESeccion()
        {
        }

        /// <summary>
        /// Gets or sets the complementary information content
        /// </summary>
        public T Entity { get; set; }

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
        /// Gets or sets the token
        /// </summary>
        public string Token { get; set; }
    }
}