// <copyright file="ChallengesPaging.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities.FrontEnd
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;

    /// <summary>
    /// represents the model to any content
    /// </summary>
    public class ChallengesPaging
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChallengesPaging"/> class
        /// </summary>
        public ChallengesPaging()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChallengesPaging"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public ChallengesPaging(IDataRecord obj)
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
            this.Image = Convert.ToString(obj["Image"]);
            this.Orderliness = Convert.ToInt32(obj["Orderliness"]);
            this.Private = Convert.ToBoolean(obj["Private"]);
            this.Views = Convert.ToInt32(obj["Views"]);
            this.Video = Convert.ToString(obj["Video"]);
            this.StartDate = Convert.ToDateTime(obj["StartDate"]);
            this.EndDate = Convert.ToDateTime(obj["EndDate"]);
            this.Type = (Domain.Entities.Challenge.TypeChallenge)Convert.ToInt16(obj["Type"]);
            this.Status = (Domain.Entities.Challenge.StatusChallenge)Convert.ToInt16(obj["Status"]);
            this.Budget = obj["Budget"] != DBNull.Value ? Convert.ToDecimal(obj["Budget"]) : this.Budget;
            this.Description = obj["Description"] != DBNull.Value ? Convert.ToString(obj["Description"]) : this.Description;
            this.DescriptionIngles = obj["Description2"] != DBNull.Value ? Convert.ToString(obj["Description2"]) : this.DescriptionIngles;
            this.Prize = obj["Prize"] != DBNull.Value ? Convert.ToString(obj["Prize"]) : this.Prize;
            this.PrizeIngles = obj["Prize2"] != DBNull.Value ? Convert.ToString(obj["Prize2"]) : this.PrizeIngles;
            this.XCoordinate = obj["XCoordinate"] != DBNull.Value ? Convert.ToSingle(obj["XCoordinate"]) : this.XCoordinate;
            this.YCoordinate = obj["YCoordinate"] != DBNull.Value ? Convert.ToSingle(obj["YCoordinate"]) : this.YCoordinate;
            this.People = obj["People"] != DBNull.Value ? Convert.ToInt32(obj["People"]) : this.People;
            this.Followers = obj["Followers"] != DBNull.Value ? Convert.ToInt32(obj["Followers"]) : this.People;
            this.Friendlyurlid = Convert.ToString(obj["Friendlyurlid"]);
            this.Ideas = obj["Ideas"] != DBNull.Value ? Convert.ToInt32(obj["Ideas"]) : this.Ideas;
        }

        /// <summary>
        /// Gets or sets the content id
        /// </summary>
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the module id
        /// </summary>
        public int? ModulId { get; set; }

        /// <summary>
        /// Gets or sets the section id
        /// </summary>
        public int? SectionId { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name para el 2do idioma
        /// </summary>
        public string NameIngles { get; set; }

        /// <summary>
        /// Gets or sets the join date
        /// </summary>
        public DateTime? Joindate { get; set; }

        /// <summary>
        /// Gets or sets the update date
        /// </summary>
        public DateTime? Updatedate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active or not
        /// </summary>
        public bool? Active { get; set; }

        /// <summary>
        /// Gets or sets the template
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets the short description
        /// </summary>
        public string Shortdescription { get; set; }

        /// <summary>
        /// Gets or sets the short description para el segundo idioma
        /// </summary>
        public string ShortdescriptionIngles { get; set; }

        /// <summary>
        /// Gets or sets the image
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the order
        /// </summary>
        public int? Orderliness { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the content is private or not
        /// </summary>
        public bool? Private { get; set; }

        /// <summary>
        /// Gets or sets the view count
        /// </summary>
        public int? Views { get; set; }

        /// <summary>
        /// Gets or sets the video
        /// </summary>
        public string Video { get; set; }

        /// <summary>
        /// Gets or sets the start date
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the challenge type
        /// </summary>
        public Domain.Entities.Challenge.TypeChallenge? Type { get; set; }

        /// <summary>
        /// Gets or sets the challenge status
        /// </summary>
        public Domain.Entities.Challenge.StatusChallenge? Status { get; set; }

        /// <summary>
        /// Gets or sets the budget
        /// </summary>
        public decimal? Budget { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the description para el segundo idioma.
        /// </summary>
        public string DescriptionIngles { get; set; }

        /// <summary>
        /// Gets or sets the prize
        /// </summary>
        public string Prize { get; set; }

        /// <summary>
        /// Gets or sets the prize para el segundo idioma.
        /// </summary>
        public string PrizeIngles { get; set; }

        /// <summary>
        /// Gets or sets the x coordinate
        /// </summary>
        public float XCoordinate { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate
        /// </summary>
        public float YCoordinate { get; set; }

        /// <summary>
        /// Gets or sets the people count
        /// </summary>
        public int People { get; set; }

        /// <summary>
        /// Gets or sets the follower count
        /// </summary>
        public int Followers { get; set; }

        /// <summary>
        /// Gets or sets the friendly URL id
        /// </summary>
        public string Friendlyurlid { get; set; }

        /// <summary>
        /// Gets or sets the ideas
        /// </summary>
        public int Ideas { get; set; }
    }
}