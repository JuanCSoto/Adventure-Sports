namespace Domain.Entities.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Representa la clase <see cref="Domain.Entities.Generic.Category"/>.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Domain.Entities.Generic.Category"/>.
        /// </summary>        
        public Category()
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Domain.Entities.Generic.Category"/>.
        /// </summary>
        /// <param name="dataRecord"></param>
        public Category(IDataRecord dataRecord)
        {
            this.CategoryId = Convert.ToInt32(dataRecord["CategoryId"]);
            this.Image = Convert.ToString(dataRecord["Image"]);
            this.CategoryLanguage = new List<CategoryLanguage>();
            this.CategoryLanguage.Add(new CategoryLanguage
            {
                CategoryId = Convert.ToInt32(dataRecord["CategoryId"]),
                LanguageId = Convert.ToInt32(dataRecord["LanguageId"]),
                Name = Convert.ToString(dataRecord["Name"]),
                Description = Convert.ToString(dataRecord["Description"]),
            });
        }

        /// <summary>
        /// Obtiene o establece el id de la categoría.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Obtiene o establece la imagen de la categoría.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Obtiene o establece la colección de categorías por lenguaje.
        /// </summary>
        public virtual ICollection<CategoryLanguage> CategoryLanguage { get; set; }
    }
}
