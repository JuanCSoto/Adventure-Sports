// <copyright file="FAQList.cs" company="Dasigno">
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
    public class SuccessStoryList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FAQList"/> class
        /// </summary>
        public SuccessStoryList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FAQList"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public SuccessStoryList(IDataRecord obj)
        {
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.Name = Convert.ToString(obj["Name"]);
            this.Description = obj["Description"] != DBNull.Value ? Convert.ToString(obj["Description"]) : this.Description;
            this.InstitutionImplements = obj["InstitutionImplements"] != DBNull.Value ? Convert.ToString(obj["InstitutionImplements"]) : this.InstitutionImplements;
            this.InstitutionSource = obj["InstitutionSource"] != DBNull.Value ? Convert.ToString(obj["InstitutionSource"]) : this.InstitutionSource;
            this.Image = obj["Image"] != DBNull.Value ? Convert.ToString(obj["Image"]) : this.Image;
            this.Video = obj["Video"] != DBNull.Value ? Convert.ToString(obj["Video"]) : this.Video;
            this.CityID = obj["CityID"] != DBNull.Value ? Convert.ToInt32(obj["CityID"]) : this.CityID;
            this.Category = obj["Category"] != DBNull.Value ? Convert.ToString(obj["Category"]) : this.Category;
            this.NameEs = obj["NameEs"] != DBNull.Value ? Convert.ToString(obj["NameEs"]) : this.NameEs;
            this.Url = obj["Url"] != DBNull.Value ? Convert.ToString(obj["Url"]) : this.Url;
            this.Friendlyurlid = Convert.ToString(obj["Friendlyurlid"]);
            this.Joindate = obj["Joindate"] != DBNull.Value ? Convert.ToDateTime(obj["Joindate"]) : this.Joindate;
            this.Country = obj["Country"] != DBNull.Value ? Convert.ToString(obj["Country"]) : this.Country;
            this.UserId = obj["UserId"] != DBNull.Value ? Convert.ToInt32(obj["UserId"]) : this.UserId;
            this.Shortdescription = obj["Shortdescription"] != DBNull.Value ? Convert.ToString(obj["Shortdescription"]) : this.Shortdescription;
            this.ProblemsSolved = obj["ProblemsSolved"] != DBNull.Value ? Convert.ToString(obj["ProblemsSolved"]) : this.ProblemsSolved;
            this.SocialImpact = obj["SocialImpact"] != DBNull.Value ? Convert.ToString(obj["SocialImpact"]) : this.SocialImpact;
        }

        /// <summary>
        /// Gets or sets the content id
        /// </summary>
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Institution Implements
        /// </summary>
        public string InstitutionImplements { get; set; }

        /// <summary>
        /// Gets or sets the Institution Source
        /// </summary>
        public string InstitutionSource { get; set; }

        /// <summary>
        /// Gets or sets the Institution Source
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the Institution Source
        /// </summary>
        public string Shortdescription { get; set; }

        /// <summary>
        /// Gets or sets the Institution Source
        /// </summary>
        public string Video { get; set; }

        /// <summary>
        /// Gets or sets the Institution Source
        /// </summary>
        public int CityID { get; set; }

        /// <summary>
        /// Gets or sets the Institution Source
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets Nombre de ciudad
        /// </summary>
        public string NameEs { get; set; }

        /// <summary>
        /// Gets or sets the Institution Source
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the friendly URL id
        /// </summary>
        public string Friendlyurlid { get; set; }

        /// <summary>
        /// Gets or sets the Joindate
        /// </summary>
        public DateTime Joindate { get; set; }

        /// <summary>
        /// Gets or sets the Joindate
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the UserId
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the text of the ProblemsSolved
        /// </summary>
        public string ProblemsSolved { get; set; }

        /// <summary>
        /// Gets or sets the text of the SocialImpact
        /// </summary>
        public string SocialImpact { get; set; }

        /// <summary>
        /// Gets or sets the text of the ProblemsSolved ingles
        /// </summary>
        public string ProblemsSolvedIngles { get; set; }

        /// <summary>
        /// Gets or sets the text of the SocialImpact ingles
        /// </summary>
        public string SocialImpactIngles { get; set; }
    }
}