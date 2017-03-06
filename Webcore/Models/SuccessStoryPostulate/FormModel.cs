namespace Webcore.Models.SuccessStoryPostulate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Business.Services;
    using Domain.Entities;

    /// <summary>
    /// Representa el modelo para el formulario de postulación de casos de éxito.
    /// </summary>
    public class FormModel : ILayout
    {
        /// <summary>
        /// Obtiene o establece el id de la postulación del caso de éxito.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el id del usuario desde donde se postuló el caso de éxito.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Obtiene o establece el id de la categoría del caso de éxito.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Global.Messages), ErrorMessageResourceName = "REQUIRED_FIELD")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la persona o compañía responsable del proyecto.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Global.Messages), ErrorMessageResourceName = "REQUIRED_FIELD")]
        public string ResponsibleNames { get; set; }

        /// <summary>
        /// Obtiene o establece el email de la persona o compañía responsable del proyecto.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Global.Messages), ErrorMessageResourceName = "REQUIRED_FIELD")]
        public string ResponsibleEmail { get; set; }

        /// <summary>
        /// Obtiene o establece el titulo (profesión) de la persona o compañía responsable del proyecto.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Global.Messages), ErrorMessageResourceName = "REQUIRED_FIELD")]
        public string ResponsibleJobTitle { get; set; }

        /// <summary>
        /// Obtiene o establece la organización responsable del proyecto.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Global.Messages), ErrorMessageResourceName = "REQUIRED_FIELD")]
        public string ResponsibleOrganization { get; set; }

        /// <summary>
        /// Obtiene o establece el id del país para la cual se postuló el caso de éxito.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Global.Messages), ErrorMessageResourceName = "REQUIRED_FIELD")]
        public int CountryId { get; set; }

        /// <summary>
        /// Obtiene o establece el id de la ciudad para la cual se postuló el caso de éxito.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Global.Messages), ErrorMessageResourceName = "REQUIRED_FIELD")]
        public int CityId { get; set; }

        /// <summary>
        /// Obtiene o establece las entidades responsables del caso de éxito.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Global.Messages), ErrorMessageResourceName = "REQUIRED_FIELD")]
        public string ResponsibleEntities { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del caso de éxito.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Global.Messages), ErrorMessageResourceName = "REQUIRED_FIELD")]
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha en la que se postuló del caso de éxito.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del caso de éxito.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Global.Messages), ErrorMessageResourceName = "REQUIRED_FIELD")]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece los problemas a los cuales se les quiere dar solución con el caso de éxito.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Global.Messages), ErrorMessageResourceName = "REQUIRED_FIELD")]
        public string ConcreteProblems { get; set; }

        /// <summary>
        /// Obtiene o establece el por qué el caso de éxito es una solución urbana innovadora.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Global.Messages), ErrorMessageResourceName = "REQUIRED_FIELD")]
        public string InnovativeUrbanSolution { get; set; }

        /// <summary>
        /// Obtiene o establece las palabras claves del caso de éxito.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Global.Messages), ErrorMessageResourceName = "REQUIRED_FIELD")]
        public string[] IdsTag { get; set; }

        /// <summary>
        /// Obtiene o establece información relacionada con el caso de éxito.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Global.Messages), ErrorMessageResourceName = "REQUIRED_FIELD")]
        public string Documents { get; set; }

        /// <summary>
        /// Obtiene o establece el estado de la postulación del caso de éxito.
        /// </summary>
        public byte State { get; set; }

        /// <summary>
        /// Obtiene o establece el id del caso de éxito.
        /// </summary>
        public int? SuccessStoryId { get; set; }

        /// <summary>
        /// Obtiene o establece las categorias.
        /// </summary>
        public IEnumerable<SelectListItem> Categories { get; set; }

        /// <summary>
        /// Obtiene o establece los países.
        /// </summary>
        public IEnumerable<SelectListItem> Countries { get; set; }

        /// <summary>
        /// Obtiene o establece las ciudades.
        /// </summary>
        public IEnumerable<SelectListItem> Cities { get; set; }

        /// <summary>
        /// Obtiene o establece los tags.
        /// </summary>
        public IEnumerable<SelectListItem> Tags { get; set; }

        #region Implements ILayout

        /// <summary>
        /// Gets or sets a list of meta tags
        /// </summary>
        public List<KeyValuePair<KeyValue, KeyValue>> MetaTags { get; set; }

        /// <summary>
        /// Gets or sets a user application
        /// </summary>
        public CustomPrincipal UserPrincipal { get; set; }

        /// <summary>
        /// Gets or sets the section
        /// </summary>
        public Section Section { get; set; }

        /// <summary>
        /// Gets or sets the content
        /// </summary>
        public Content Content { get; set; }

        /// <summary>
        /// Gets or sets a list of banner
        /// </summary>
        public List<Banner> Banners { get; set; }

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
        public Language CurrentLanguage { get; set; }

        /// <summary>
        /// Gets or sets the idea count
        /// </summary>
        public int IdeasCountAll { get; set; }

        #endregion ILayout
    }
}