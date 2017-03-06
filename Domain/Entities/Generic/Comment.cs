// <copyright file="Comment.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// Comment object mapped table <c>Comment</c>.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class
        /// </summary>
        public Comment()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Comment(IDataRecord obj)
        {
            this.CommentId = Convert.ToInt32(obj["CommentId"]);
            this.IdeaId = obj["IdeaId"] != DBNull.Value ? Convert.ToInt32(obj["IdeaId"]) : this.IdeaId;
            this.ContentId = obj["ContentId"] != DBNull.Value ? Convert.ToInt32(obj["ContentId"]) : this.ContentId;
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.Text = Convert.ToString(obj["Text"]);
            this.Creationdate = Convert.ToDateTime(obj["Creationdate"]);
            this.Active = Convert.ToBoolean(obj["Active"]);
        }

        /// <summary>
        /// Gets or sets the comment id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@CommentId")]
        public int? CommentId { get; set; }

        /// <summary>
        /// Gets or sets the idea id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@IdeaId")]
        public int? IdeaId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentId")]
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the text
        /// </summary>
        [InfoDatabase(DbType.String, "@Text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the creation date
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@Creationdate")]
        public DateTime? Creationdate { get; set; }

        /// <summary>
        /// Gets or sets if the comment is active
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Active")]
        public bool? Active { get; set; }
    }
}