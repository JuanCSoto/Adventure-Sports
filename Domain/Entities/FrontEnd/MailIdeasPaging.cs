// <copyright file="MailIdeasPaging.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities.FrontEnd
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Domain.Abstract;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Idea object mapped table <c>Idea</c>.
    /// </summary>
    public class MailIdeasPaging
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MailIdeasPaging"/> class
        /// </summary>
        public MailIdeasPaging()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailIdeasPaging"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public MailIdeasPaging(IDataRecord obj)
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

            this.UserNames = Convert.ToString(obj["UserNames"]);
            this.Email = Convert.ToString(obj["Email"]);
            this.Joindate = Convert.ToDateTime(obj["Joindate"]);
            this.UserImage = obj["UserImage"] != DBNull.Value ? Convert.ToString(obj["UserImage"]) : this.UserImage;
            this.Active = Convert.ToBoolean(obj["Active"]);
            this.News = obj["News"] != DBNull.Value ? Convert.ToBoolean(obj["News"]) : this.News;
            this.Phone = obj["Phone"] != DBNull.Value ? obj["Phone"].ToString() : this.Phone;
            this.LanguageId = Convert.ToInt32(obj["LanguageId"]);
            this.Friendlyurlid = Convert.ToString(obj["Friendlyurlid"]);

            this.Likes = Convert.ToInt32(obj["Likes"]);
            this.NoLikes = Convert.ToInt32(obj["NoLikes"]);
            this.UserLike = Convert.ToBoolean(obj["UserLike"]);
            this.UserNoLike = Convert.ToBoolean(obj["UserNoLike"]);

            this.CommentsCount = Convert.ToInt32(obj["CommentsCount"]);
        }

        /// <summary>
        /// Gets or sets the content object
        /// </summary>
        public Content objContent { get; set; }

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
        /// Gets or sets the image
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the x coordinate
        /// </summary>
        public double? XCoordinate { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate
        /// </summary>
        public double? YCoordinate { get; set; }

        /// <summary>
        /// Gets or sets the video
        /// </summary>
        public string Video { get; set; }

        /// <summary>
        /// Gets or sets the creation date
        /// </summary>
        public DateTime? Creationdate { get; set; }

        /// <summary>
        /// Gets or sets the comment collection
        /// </summary>
        public List<CommentsPaging> CollComment { get; set; }

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
        /// Gets or sets a value indicating whether the user accepts emails or not
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
        /// Gets or sets the friendly URL id
        /// </summary>
        public string Friendlyurlid { get; set; }

        /// <summary>
        /// Gets or sets the like count
        /// </summary>
        public int? Likes { get; set; }

        /// <summary>
        /// Gets or sets the don't like count
        /// </summary>
        public int? NoLikes { get; set; }

        /// <summary>
        /// Gets or sets the a value indicating whether a user already liked the idea
        /// </summary>
        public bool? UserLike { get; set; }

        /// <summary>
        /// Gets or sets the a value indicating whether a user already don't liked the idea
        /// </summary>
        public bool? UserNoLike { get; set; }

        /// <summary>
        /// Gets or sets the comment count
        /// </summary>
        public int CommentsCount { get; set; }
    }
}