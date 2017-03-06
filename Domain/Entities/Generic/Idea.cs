// <copyright file="Idea.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// Idea object mapped table <c>Idea</c>.
    /// </summary>
    public class Idea
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Idea"/> class
        /// </summary>
        public Idea()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Idea"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Idea(IDataRecord obj)
        {
            this.IdeaId = Convert.ToInt32(obj["IdeaId"]);
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.Text = Convert.ToString(obj["Text"]);
            this.Image = obj["Image"] != DBNull.Value ? Convert.ToString(obj["Image"]) : this.Image;
            this.XCoordinate = obj["XCoordinate"] != DBNull.Value ? Convert.ToDouble(obj["XCoordinate"]) : this.XCoordinate;
            this.YCoordinate = obj["YCoordinate"] != DBNull.Value ? Convert.ToDouble(obj["YCoordinate"]) : this.YCoordinate;
            this.Video = obj["Video"] != DBNull.Value ? Convert.ToString(obj["Video"]) : this.Video;
            this.Creationdate = Convert.ToDateTime(obj["Creationdate"]);
            this.Active = Convert.ToBoolean(obj["Active"]);
            this.Distinguished = Convert.ToBoolean(obj["Distinguished"]);
            this.Recommended = Convert.ToBoolean(obj["Recommended"]);
            this.Score = Convert.ToInt32(obj["Score"]);
            this.Likes = Convert.ToInt32(obj["Likes"]);
            this.NoLikes = Convert.ToInt32(obj["NoLikes"]);
            this.Views = Convert.ToInt32(obj["Views"]);
            this.Friendlyurlid = obj["Friendlyurlid"] != DBNull.Value ? Convert.ToString(obj["Friendlyurlid"]) : this.Friendlyurlid;
        }

        /// <summary>
        /// Gets or sets the identifier of the idea
        /// </summary>
        [InfoDatabase(DbType.Int32, "@IdeaId")]
        public int? IdeaId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentId")]
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the idea text
        /// </summary>
        [InfoDatabase(DbType.String, "@Text")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar el texto de la idea")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the image of the idea
        /// </summary>
        [InfoDatabase(DbType.String, "@Image")]
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the idea x coordinate
        /// </summary>
        [InfoDatabase(DbType.Double, "@XCoordinate")]
        public double? XCoordinate { get; set; }

        /// <summary>
        /// Gets or sets the idea y coordinate
        /// </summary>
        [InfoDatabase(DbType.Double, "@YCoordinate")]
        public double? YCoordinate { get; set; }

        /// <summary>
        /// Gets or sets the video of the idea
        /// </summary>
        [InfoDatabase(DbType.String, "@Video")]
        [RegularExpression(@"(^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|&v=)([^#\&\?]*).*)|(^.*(vimeo.com\/)([^#\&\?]*).*)", ErrorMessage = "Video no válido")]
        public string Video { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the idea
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@Creationdate")]
        public DateTime? Creationdate { get; set; }

        /// <summary>
        /// Gets or sets if the idea is active
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Active")]
        public bool? Active { get; set; }

        /// <summary>
        /// Gets or sets if the idea is distinguished
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Distinguished")]
        public bool? Distinguished { get; set; }

        /// <summary>
        /// Gets or sets if the idea is recommended
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Recommended")]
        public bool? Recommended { get; set; }

        /// <summary>
        /// Gets or sets the score
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Score")]
        public int? Score { get; set; }

        /// <summary>
        /// Gets or sets the likes
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Likes")]
        public int? Likes { get; set; }

        /// <summary>
        /// Gets or sets the hates
        /// </summary>
        [InfoDatabase(DbType.Int32, "@NoLikes")]
        public int? NoLikes { get; set; }

        /// <summary>
        /// Gets or sets the amount of views
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Views")]
        public int? Views { get; set; }

        /// <summary>
        /// Gets or sets the friendly url
        /// </summary>
        [InfoDatabase(DbType.String, "@Friendlyurlid")]
        public string Friendlyurlid { get; set; }
    }
}