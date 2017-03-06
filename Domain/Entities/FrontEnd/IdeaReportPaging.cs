// <copyright file="IdeaReportPaging.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Domain.Entities.FrontEnd
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// IdeaReportPaging object mapped table <c>IdeaReportPaging</c>.
    /// </summary>
    public class IdeaReportPaging
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdeaReportPaging"/> class
        /// </summary>
        public IdeaReportPaging()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdeaReportPaging"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public IdeaReportPaging(IDataRecord obj)
        {
            this.IdeaReportId = Convert.ToInt32(obj["IdeaReportId"]);
            this.IdeaId = Convert.ToInt32(obj["IdeaId"]);
            this.Date = Convert.ToDateTime(obj["Date"]);
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.Text = Convert.ToString(obj["Text"]);
            this.Motive = Convert.ToString(obj["Motive"]);
            this.Status = Convert.ToBoolean(obj["Status"]);
            this.IdeaUserId = Convert.ToInt32(obj["IdeaUserId"]);
            this.IdeaText = Convert.ToString(obj["IdeaText"]);
            this.IdeaImage = obj["IdeaImage"] != DBNull.Value ? Convert.ToString(obj["IdeaImage"]) : this.IdeaImage;
            this.IdeaVideo = obj["IdeaVideo"] != DBNull.Value ? Convert.ToString(obj["IdeaVideo"]) : this.IdeaVideo;
            this.IdeaActive = Convert.ToBoolean(obj["IdeaActive"]);
            this.IdeaUserNames = obj["IdeaUserNames"] != DBNull.Value ? Convert.ToString(obj["IdeaUserNames"]) : this.IdeaVideo;
            this.IdeaUserActive = Convert.ToBoolean(obj["IdeaUserActive"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the ideaReport
        /// </summary>
        public int? IdeaReportId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the content
        /// </summary>
        public int? IdeaId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the ideaReport text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the idea Report motive
        /// </summary>
        public string Motive { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the ideaReport
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the status of the ideaReport
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// Gets or sets the idea text
        /// </summary>
        public string IdeaText { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user owner of the idea
        /// </summary>
        public int? IdeaUserId { get; set; }

        /// <summary>
        /// Gets or sets the image of the idea
        /// </summary>
        public string IdeaImage { get; set; }

        /// <summary>
        /// Gets or sets the video of the idea
        /// </summary>
        public string IdeaVideo { get; set; }

        /// <summary>
        /// Gets or sets the status of the idea
        /// </summary>
        public bool? IdeaActive { get; set; }

        /// <summary>
        /// Gets or sets the names of user owner of the idea
        /// </summary>
        public string IdeaUserNames { get; set; }

        /// <summary>
        /// Gets or sets the status of the idea user
        /// </summary>
        public bool? IdeaUserActive { get; set; }
    }
}