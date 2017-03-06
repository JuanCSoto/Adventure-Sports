// <copyright file="Content.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Content</c> object mapped table <c>Content</c>.
    /// </summary>
    public class Content
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Content"/> class
        /// </summary>
        public Content()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Content"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Content(IDataRecord obj)
        {
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.ModulId = Convert.ToInt32(obj["ModulId"]);
            this.SectionId = Convert.ToInt32(obj["SectionId"]);
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.Name = Convert.ToString(obj["Name"]);
            this.NameIngles = Convert.ToString(obj["Name2"]);
            this.Joindate = Convert.ToDateTime(obj["Joindate"]);
            this.Updatedate = Convert.ToDateTime(obj["Updatedate"]);
            this.Active = Convert.ToBoolean(obj["Active"]);
            this.Template = Convert.ToString(obj["Template"]);
            this.Shortdescription = Convert.ToString(obj["Shortdescription"]);
            this.ShortdescriptionIngles = Convert.ToString(obj["Shortdescription2"]);
            this.Image = obj["Image"] != DBNull.Value ? Convert.ToString(obj["Image"]) : this.Image;
            this.Orderliness = Convert.ToInt32(obj["Orderliness"]);
            this.Private = Convert.ToBoolean(obj["Private"]);
            this.Views = Convert.ToInt32(obj["Views"]);
            this.Video = obj["Video"] != DBNull.Value ? Convert.ToString(obj["Video"]) : this.Video;
            this.Category = obj["Category"] != DBNull.Value ? Convert.ToString(obj["Category"]) : this.Category;
            this.Widget = obj["Widget"] != DBNull.Value ? Convert.ToBoolean(obj["Widget"]) : this.Widget;
            this.LanguageId = Convert.ToInt32(obj["LanguageId"]);
            this.Frienlyname = Convert.ToString(obj["Friendlyurlid"]);
            this.QuestionType = Question.TypeQuestion.Abierta;
        }

        /// <summary>
        /// Gets or sets the identifier of the content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentId")]
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the Module
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ModulId")]
        public int? ModulId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the section
        /// </summary>
        [InfoDatabase(DbType.Int32, "@SectionId")]
        public int? SectionId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the creator of the content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the content
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        [Required(ErrorMessage = "Debes ingresar el nombre del Contenido")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the content second language
        /// </summary>
        [InfoDatabase(DbType.String, "@Name2")]
        [Required(ErrorMessage = "Debes ingresar el nombre del Contenido segundo lenguaje")]
        public string NameIngles { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the content
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@Joindate")]
        public DateTime? Joindate { get; set; }

        /// <summary>
        /// Gets or sets of the update date of the content
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@Updatedate")]
        public DateTime? Updatedate { get; set; }

        /// <summary>
        /// Gets or sets if the content is active
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Active")]
        public bool? Active { get; set; }

        /// <summary>
        /// Gets or sets the template content output
        /// </summary>
        [InfoDatabase(DbType.String, "@Template")]
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets the short description of the content
        /// </summary>
        [InfoDatabase(DbType.String, "@Shortdescription")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar la introducción del contenido")]
        public string Shortdescription { get; set; }

        /// <summary>
        /// Gets or sets the short description of the content second language
        /// </summary>
        [InfoDatabase(DbType.String, "@Shortdescription2")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar la introducción del contenido segundo lenguaje")]
        public string ShortdescriptionIngles { get; set; }

        /// <summary>
        /// Gets or sets the principal image of the content
        /// </summary>
        [InfoDatabase(DbType.String, "@Image")]
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the principal image of the content
        /// </summary>
        [InfoDatabase(DbType.String, "@CoverImage")]
        public string CoverImage { get; set; }

        /// <summary>
        /// Gets or sets the order of the content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Orderliness")]
        public int? Orderliness { get; set; }

        /// <summary>
        /// Gets or sets if the content is private
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Private")]
        public bool? Private { get; set; }

        /// <summary>
        /// Gets or sets if the content will show in the widget
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Widget")]
        public bool? Widget { get; set; }

        /// <summary>
        /// Gets or sets the number of views of the content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Views")]
        public int? Views { get; set; }

        /// <summary>
        /// Gets or sets the principal video of the content
        /// </summary>
        [InfoDatabase(DbType.String, "@Video")]
        [RegularExpression(@"(^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|&v=)([^#\&\?]*).*)|(^.*(vimeo.com\/)([^#\&\?]*).*)", ErrorMessage = "Video no válido")]
        public string Video { get; set; }

        /// <summary>
        /// Gets or sets the identifier language
        /// </summary>
        [InfoDatabase(DbType.Int32, "@LanguageId")]
        public int? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the category of the content
        /// </summary>
        [InfoDatabase(DbType.String, "@Category")]
        [Required(ErrorMessage = "Debes ingresar la categoría del Contenido")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the friendly name for the url
        /// </summary>
        public string Frienlyname { get; set; }

        /// <summary>
        /// Gets or sets the ideas count
        /// </summary>
        public int Ideas { get; set; }

        /// <summary>
        /// Gets or sets the text length
        /// </summary>
        public int TextLenght { get; set; }

        /// <summary>
        /// Gets or sets the question type
        /// </summary>
        public Domain.Entities.Question.TypeQuestion QuestionType { get; set; }
    }
}