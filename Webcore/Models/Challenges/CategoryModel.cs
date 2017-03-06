namespace Webcore.Models.Challenges
{
    /// <summary>
    /// Representa el modelo para categorias.
    /// </summary>
    public class CategoryModel
    {
        /// <summary>
        /// Obtiene o establece el id de la categoría.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del lenguaje.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Obtiene o establece la imagen de la categoría.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la categoría.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción de la categoría.
        /// </summary>
        public string Description { get; set; }        
    }
}