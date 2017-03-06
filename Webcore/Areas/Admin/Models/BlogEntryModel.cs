// <copyright file="BlogEntryModel.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Xml;
    using Business.Services;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// management the Blog Entry information
    /// </summary>
    public class BlogEntryModel : IAdmin, IContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogEntryModel"/> class
        /// </summary>
        public BlogEntryModel()
        {
        }

        /// <summary>
        /// Gets or sets the blog entry information
        /// </summary>
        public BlogEntry BlogEntry { get; set; }

        /// <summary>
        /// Gets or sets the deep follower content
        /// </summary>
        public string DeepFollower { get; set; }

        /// <summary>
        /// Gets or sets the content information
        /// </summary>
        public Domain.Entities.Content IContent { get; set; }

        /// <summary>
        /// Gets or sets the list of relation content
        /// </summary>
        public IEnumerable<ContentRel> ListContent { get; set; }

        /// <summary>
        /// Gets or sets the list of relation files
        /// </summary>
        public IEnumerable<Fileattach> ListFiles { get; set; }

        /// <summary>
        /// Gets or sets the list of relation tags
        /// </summary>
        public IEnumerable<Tag> ListTags { get; set; }

        /// <summary>
        /// Gets or sets the output templates to a content
        /// </summary>
        public IEnumerable<string> Templates { get; set; }

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

        /// <summary>
        /// Gets or sets the list of comments
        /// </summary>
        public IEnumerable<CommentsPaging> ListComments { get; set; }

        /// <summary>
        /// Gets or sets the category collection
        /// </summary>
        public List<Category> Categories { get; set; }

        /// <summary>
        /// Obtiene o establece las palabras claves del caso de éxito.
        /// </summary>        
        public int[] IdsTag { get; set; }

        /// <summary>
        /// Obtiene o establece los tags.
        /// </summary>
        public IEnumerable<SelectListItem> Tags { get; set; }
    }
}