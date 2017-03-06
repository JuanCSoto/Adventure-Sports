// <copyright file="Answer.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Answer</c> object mapped table <c>Answer</c>.
    /// </summary>
    public class Answer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Answer"/> class
        /// </summary>
        public Answer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Answer"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Answer(IDataRecord obj)
        {
            this.AnswerId = Convert.ToInt32(obj["AnswerId"]);
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.Text = obj["Text"] != DBNull.Value ? Convert.ToString(obj["Text"]) : this.Text;
            this.Count = Convert.ToInt32(obj["Count"]);
            this.Image = obj["Image"] != DBNull.Value ? Convert.ToString(obj["Image"]) : this.Image;
            this.Video = obj["Video"] != DBNull.Value ? Convert.ToString(obj["Video"]) : this.Video;
        }

        /// <summary>
        /// Gets or sets the identifier of the answer
        /// </summary>
        [InfoDatabase(DbType.Int32, "@AnswerId")]
        public int? AnswerId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentId")]
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the answer text
        /// </summary>
        [InfoDatabase(DbType.String, "@Text")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar el texto de la respuesta")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the count of times answered
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Count")]
        public int? Count { get; set; }

        /// <summary>
        /// Gets or sets the principal image of the answer
        /// </summary>
        [InfoDatabase(DbType.String, "@Image")]
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the principal video of the answer
        /// </summary>
        [InfoDatabase(DbType.String, "@Video")]
        [RegularExpression(@"(^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|&v=)([^#\&\?]*).*)|(^.*(vimeo.com\/)([^#\&\?]*).*)", ErrorMessage = "Video no válido")]
        public string Video { get; set; }
    }
}
