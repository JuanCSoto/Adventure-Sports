// <copyright file="FEGeneralFind.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
namespace Webcore.Models
{
    using Business.Services;
    using Domain.Entities;
    using System.Collections.Generic;

    /// <summary>
    /// Clase Para la vista de resultados <see cref="FEGeneralFind"/>.
    /// </summary>
    public class FEGeneralFind : ILayout
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FEGeneralFind"/> class.
        /// </summary>
        public FEGeneralFind()
        {
        }

        /// <summary>
        /// Gets or sets a list of General Find.
        /// </summary>
        public List<Domain.Entities.FrontEnd.GeneralFindPaging> ListaGeneralFind { get; set; }

        /// <summary>
        /// Gets or sets a list of meta tags.
        /// </summary>
        public List<KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>> MetaTags { get; set; }

        /// <summary>
        /// Gets or sets a user application.
        /// </summary>
        public CustomPrincipal UserPrincipal { get; set; }

        /// <summary>
        /// Gets or sets the section.
        /// </summary>
        public Section Section { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public Content Content { get; set; }

        /// <summary>
        /// Gets or sets a list of banner.
        /// </summary>
        public List<Domain.Entities.Banner> Banners { get; set; }

        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the layout page.
        /// </summary>
        public string Layout { get; set; }

        /// <summary>
        /// Gets or sets the deep follower.
        /// </summary>
        public System.Web.HtmlString DeepFollower { get; set; }

        /// <summary>
        /// Gets or sets the language application.
        /// </summary>
        public Language CurrentLanguage { get; set; }

        /// <summary>
        /// Gets or sets the ideas count.
        /// </summary>
        public int IdeasCountAll { get; set; }
    }
}