namespace Webcore.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Domain.Entities.Generic;

    /// <summary>
    /// Representa el detalle para la postulación de un caso de éxito.
    /// </summary>
    public class DetailModel : IAdmin, IContent
    {
        /// <summary>
        /// Obtiene o establece la postulación del caso de éxito.
        /// </summary>
        public SuccessStoryPostulate SuccessStoryPostulate { get; set; }

        /// <summary>
        /// Obtiene o establece los tags asociados a la postulación de casos de éxito.
        /// </summary>
        public string TagsText { get; set; }

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

        #region Implements Admin

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