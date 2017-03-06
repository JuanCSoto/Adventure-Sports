// <copyright file="Template.cs" company="Dasigno">
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
    /// <c>Template</c> object mapped table <c>Template</c>.
    /// </summary>
    public class Template
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template"/> class
        /// </summary>
        public Template()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Template"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Template(IDataRecord obj)
        {
            this.TemplateId = Convert.ToString(obj["TemplateId"]);
            this.Type = Convert.ToInt16(obj["Type"]);
            this.Nameclass = Convert.ToString(obj["Nameclass"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the template
        /// </summary>
        [InfoDatabase(DbType.String, "@TemplateId")]
        [Required(ErrorMessage = "Debes ingresar el nombre del archivo del template (template.cshtml)")]
        public string TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the type of template 
        /// </summary>
        [InfoDatabase(DbType.Int16, "@Type")]
        [Required(ErrorMessage = "Debes ingresar el tipo de template")]
        public short? Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the model represents view
        /// </summary>
        [InfoDatabase(DbType.String, "@Nameclass")]
        [Required(ErrorMessage = "Debes ingresar el nombre del modelo que representa la vista")]
        public string Nameclass { get; set; }
    }
}