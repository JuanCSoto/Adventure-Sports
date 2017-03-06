// <copyright file="IContent.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// defines a properties to fill content administrator pages
    /// </summary>
    public interface IContent
    {
        /// <summary>
        /// Gets or sets the content information
        /// </summary>
        Domain.Entities.Content IContent { get; set; }

        /// <summary>
        /// Gets or sets the list of relation content
        /// </summary>
        IEnumerable<ContentRel> ListContent { get; set; }

        /// <summary>
        /// Gets or sets the list of relation files
        /// </summary>
        IEnumerable<Fileattach> ListFiles { get; set; }

        /// <summary>
        /// Gets or sets the list of relation tags
        /// </summary>
        IEnumerable<Tag> ListTags { get; set; }

        /// <summary>
        /// Gets or sets the output templates to a content
        /// </summary>
        IEnumerable<string> Templates { get; set; }

        /// <summary>
        /// Gets or sets the category collection
        /// </summary>
        List<Category> Categories { get; set; }

        /// <summary>
        /// Obtiene o establece las palabras claves del caso de éxito.
        /// </summary>        
        int[] IdsTag { get; set; }

        /// <summary>
        /// Obtiene o establece los tags.
        /// </summary>
        IEnumerable<SelectListItem> Tags { get; set; }
    }
}
