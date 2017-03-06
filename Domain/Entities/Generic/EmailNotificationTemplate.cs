// <copyright file="EmailNotificationTemplate.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// SystemNotificationTemplate object mapped table <c>EmailNotificationTemplate</c>.
    /// </summary>
    public class EmailNotificationTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailNotificationTemplate"/> class
        /// </summary>
        public EmailNotificationTemplate()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailNotificationTemplate"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public EmailNotificationTemplate(IDataRecord obj)
        {
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.Description = Convert.ToString(obj["Description"]);
            this.SenderName = obj["SenderName"] != DBNull.Value ? Convert.ToString(obj["SenderName"]) : this.SenderName;
            this.DescriptionIngles = obj["DescriptionEng"] != DBNull.Value ? Convert.ToString(obj["DescriptionEng"]) : this.DescriptionIngles;
        }

        /// <summary>
        /// Gets or sets the content id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentId")]
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the sender name
        /// </summary>
        [InfoDatabase(DbType.String, "@SenderName")]
        public string SenderName { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [InfoDatabase(DbType.String, "@Description")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar la notificación")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [InfoDatabase(DbType.String, "@DescriptionEng")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar la notificación en ingles")]
        public string DescriptionIngles { get; set; }

        /// <summary>
        /// Obtiene o establece los identificadores de los tags.
        /// </summary>        
        public string ExistingTags { get; set; }

        /// <summary>
        /// Obtiene o establece los nombres de los nuevos tags.
        /// </summary>        
        public string NewTags { get; set; }
    }
}