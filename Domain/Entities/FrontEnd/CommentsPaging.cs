// <copyright file="CommentsPaging.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities.FrontEnd
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// Idea object mapped table <c>Idea</c>.
    /// </summary>
    public class CommentsPaging
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsPaging"/> class
        /// </summary>
        public CommentsPaging()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsPaging"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public CommentsPaging(IDataRecord obj)
        {
            this.CommentId = Convert.ToInt32(obj["CommentId"]);
            this.IdeaId = obj["IdeaId"] != DBNull.Value ? Convert.ToInt32(obj["IdeaId"]) : this.IdeaId;
            this.ContentId = obj["ContentId"] != DBNull.Value ? Convert.ToInt32(obj["ContentId"]) : this.ContentId;
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.Text = Convert.ToString(obj["Text"]);
            this.Creationdate = Convert.ToDateTime(obj["Creationdate"]);

            this.UserNames = Convert.ToString(obj["UserNames"]);
            this.Email = Convert.ToString(obj["Email"]);
            this.Joindate = Convert.ToDateTime(obj["Joindate"]);
            this.UserImage = obj["UserImage"] != DBNull.Value ? Convert.ToString(obj["UserImage"]) : this.UserImage;
            this.Active = Convert.ToBoolean(obj["Active"]);
            this.News = obj["News"] != DBNull.Value ? Convert.ToBoolean(obj["News"]) : this.News;
            this.Phone = obj["Phone"] != DBNull.Value ? obj["Phone"].ToString() : this.Phone;
            this.LanguageId = Convert.ToInt32(obj["LanguageId"]);
        }

        /// <summary>
        /// Gets or sets the comment id
        /// </summary>
        public int? CommentId { get; set; }

        /// <summary>
        /// Gets or sets the idea id
        /// </summary>
        public int? IdeaId { get; set; }

        /// <summary>
        /// Gets or sets the content id
        /// </summary>
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the creation date
        /// </summary>
        public DateTime? Creationdate { get; set; }

        /// <summary>
        /// Gets or sets the user names
        /// </summary>
        public string UserNames { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the join date
        /// </summary>
        public DateTime? Joindate { get; set; }

        /// <summary>
        /// Gets or sets the user image
        /// </summary>
        public string UserImage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active or not
        /// </summary>
        public bool? Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the comment is for the blog or not
        /// </summary>
        public bool? News { get; set; }

        /// <summary>
        /// Gets or sets the phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the language id
        /// </summary>
        public int? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a user accepts the terms of the site
        /// </summary>
        public bool Terms { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a user accepts the policies of the site
        /// </summary>
        public bool Policy { get; set; }

        /// <summary>
        /// Gets or sets the comment count
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// Gets or sets the comment content owner id
        /// </summary>
        public int CommentContentOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the question type
        /// </summary>
        public Domain.Entities.Question.TypeQuestion? QuestionType { get; set; }
    }
}