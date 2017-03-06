namespace Domain.Entities.Generic
{
    /// <summary>
    /// Representa la clase <see cref="Domain.Entities.Generic.CategoryLanguage"/>.
    /// </summary>
    public class CategoryLanguage
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Domain.Entities.Generic.CategoryLanguage"/>.
        /// </summary>
        public CategoryLanguage()
        {
        }

        /// <summary>
        /// Obtiene o establece el id de la categoría.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del lenguaje.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la categoría.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción de la categoría.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece la categoría.
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Obtiene o establece el lenguaje.
        /// </summary>
        public virtual Language Language { get; set; }
    }
}
