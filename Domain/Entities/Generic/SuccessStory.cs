// <copyright file="SuccessStory.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved.
// </copyright>
// <author>Diego Toro</author>
namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>BlogEntry</c> object mapped table <c>SuccessStory</c>.
    /// </summary>
    public class SuccessStory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessStory"/> class.
        /// </summary>
        public SuccessStory()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuccessStory"/> class.
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public SuccessStory(IDataRecord obj)
        {
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.Description = Convert.ToString(obj["Description"]);
            this.DescriptionIngles = Convert.ToString(obj["Description2"]);
            this.InstitutionSource = Convert.ToString(obj["InstitutionSource"]);
            this.InstitutionImplements = Convert.ToString(obj["InstitutionImplements"]);
            this.Category = Convert.ToString(obj["Category"]);
            this.Url = Convert.ToString(obj["Url"]);
            this.ProblemsSolved = Convert.ToString(obj["ProblemsSolved"]);
            this.SocialImpact = Convert.ToString(obj["SocialImpact"]);
            this.ProblemsSolvedIngles = Convert.ToString(obj["ProblemsSolved2"]);
            this.SocialImpactIngles = Convert.ToString(obj["SocialImpact2"]);
            this.CityID = Convert.ToInt32(obj["CityID"]);
        }

        /// <summary>
        /// Gets or sets the text of the entry.
        /// </summary>
        [InfoDatabase(DbType.String, "@Category")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Debes ingresar la Categoria.")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets id for city.
        /// </summary>
        [InfoDatabase(DbType.Int32, "@CityID")]
        public int? CityID { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the content.
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentId")]
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the text of the entry.
        /// </summary>
        [InfoDatabase(DbType.String, "@Description")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar el cuerpo del contenido.")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the text of the entry.
        /// </summary>
        [InfoDatabase(DbType.String, "@Description2")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar el cuerpo del contenido. (Ingles)")]
        public string DescriptionIngles { get; set; }

        /// <summary>
        /// Gets or sets the text of the entry.
        /// </summary>
        [InfoDatabase(DbType.String, "@InstitutionImplements")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Debes ingresar la institucion que implementa.")]
        public string InstitutionImplements { get; set; }

        /// <summary>
        /// Gets or sets the text of the entry.
        /// </summary>
        [InfoDatabase(DbType.String, "@InstitutionSource")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Debes ingresar la institucion origen.")]
        public string InstitutionSource { get; set; }
        /// <summary>
        /// Gets or sets the text of the ProblemsSolved.
        /// </summary>
        [InfoDatabase(DbType.String, "@ProblemsSolved")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar el cuerpo del contenido problema resuelto (Español).")]
        public string ProblemsSolved { get; set; }

        /// <summary>
        /// Gets or sets the text of the ProblemsSolved ingles.
        /// </summary>
        [InfoDatabase(DbType.String, "@ProblemsSolved2")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar el cuerpo del contenido problema resuelto (Inlges).")]
        public string ProblemsSolvedIngles { get; set; }

        /// <summary>
        /// Gets or sets the text of the SocialImpact.
        /// </summary>
        [InfoDatabase(DbType.String, "@SocialImpact")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar el cuerpo del contenido impacto social (Español).")]
        public string SocialImpact { get; set; }

        /// <summary>
        /// Gets or sets the text of the SocialImpact ingles.
        /// </summary>
        [InfoDatabase(DbType.String, "@SocialImpact2")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar el cuerpo del contenido impacto social (Inlges).")]
        public string SocialImpactIngles { get; set; }

        /// <summary>
        /// Gets or sets the text of the entry.
        /// </summary>
        [InfoDatabase(DbType.String, "@Url")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar la Url.")]
        public string Url { get; set; }

        /// <summary>
        /// Obtiene o establece los identificadores de los tags.        
        /// </summary>
        [InfoDatabase(DbType.String, "@ExistingTags")]
        public string ExistingTags { get; set; }

        /// <summary>
        /// Obtiene o establece los nombres de los nuevos tags.
        /// </summary>
        [InfoDatabase(DbType.String, "@NewTags")]
        public string NewTags { get; set; }
    }
}
