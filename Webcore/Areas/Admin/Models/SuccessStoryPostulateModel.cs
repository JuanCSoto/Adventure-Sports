// <copyright file="SuccessStoryPostulateModel.cs" company="Intergrupo">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
namespace Webcore.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Domain.Entities.FrontEnd;
    using System.Web.Mvc;

    /// <summary>
    /// management the Success Story PostulateModel information
    /// </summary>
    public class SuccessStoryPostulateModel : IAdmin, IContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessStoryPostulateModel"/> class
        /// </summary>
        public SuccessStoryPostulateModel()
        {
        }

        /// <summary>
        /// Gets or sets Lista Success Story Postulate
        /// </summary>
        public IList<SuccessStoryPostulatePaging> ListaSuccessStoryPostulate { get; set; }

        #region Implements IContent
        /// <summary>
        /// Gets or sets the content information
        /// </summary>
        public Domain.Entities.Content IContent { get; set; }

        /// <summary>
        /// Gets or sets the list of relation content
        /// </summary>
        public IEnumerable<Domain.Entities.ContentRel> ListContent { get; set; }

        /// <summary>
        /// Gets or sets the list of relation files
        /// </summary>
        public IEnumerable<Domain.Entities.Fileattach> ListFiles { get; set; }

        /// <summary>
        /// Gets or sets the list of relation tags
        /// </summary>
        public IEnumerable<Domain.Entities.Tag> ListTags { get; set; }

        /// <summary>
        /// Gets or sets the output templates to a content
        /// </summary>
        public IEnumerable<string> Templates { get; set; }

        /// <summary>
        /// Gets or sets the category collection
        /// </summary>
        public List<Domain.Entities.FrontEnd.Category> Categories { get; set; }

        /// <summary>
        /// Obtiene o establece las palabras claves del caso de éxito.
        /// </summary>        
        public int[] IdsTag { get; set; }

        /// <summary>
        /// Obtiene o establece los tags.
        /// </summary>
        public IEnumerable<SelectListItem> Tags { get; set; }
        #endregion

        #region implements Admin
        /// <summary>
        /// Gets or sets the information module
        /// </summary>
        public Domain.Entities.Modul Module { get; set; }

        /// <summary>
        /// Gets or sets the list of modules
        /// </summary>
        public IEnumerable<Domain.Entities.Modul> ColModul { get; set; }

        /// <summary>
        /// Gets or sets a user application
        /// </summary>
        public Business.Services.CustomPrincipal UserPrincipal { get; set; }

        /// <summary>
        /// Gets or sets the language application
        /// </summary>
        public Domain.Entities.Language CurrentLanguage { get; set; }
        #endregion
    }
}