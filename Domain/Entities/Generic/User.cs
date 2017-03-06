// <copyright file="User.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using Domain.Abstract;
    using Domain.Entities.Basic;

    /// <summary>
    /// <c>User</c> object mapped table <c>User</c>.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public User(IDataRecord obj)
        {
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.Names = Convert.ToString(obj["Names"]);
            this.Password = obj["Password"] != DBNull.Value ? Convert.ToString(obj["Password"]) : null;
            this.Email = obj["Email"] != DBNull.Value ? Convert.ToString(obj["Email"]) : null;
            this.Joindate = Convert.ToDateTime(obj["Joindate"]);
            this.Image = obj["Image"] != DBNull.Value ? Convert.ToString(obj["Image"]) : this.Image;
            this.Active = Convert.ToBoolean(obj["Active"]);
            this.News = obj["News"] != DBNull.Value ? Convert.ToBoolean(obj["News"]) : this.News;
            this.Phone = obj["Phone"] != DBNull.Value ? obj["Phone"].ToString() : this.Phone;
            this.LanguageId = Convert.ToInt32(obj["LanguageId"]);

            this.Description = obj["Description"] != DBNull.Value ? Convert.ToString(obj["Description"]) : this.Description;
            this.FacebookLink = obj["FacebookLink"] != DBNull.Value ? Convert.ToString(obj["FacebookLink"]) : this.FacebookLink;
            this.LinkedinLink = obj["LinkedinLink"] != DBNull.Value ? Convert.ToString(obj["LinkedinLink"]) : this.LinkedinLink;
            this.GoogleLink = obj["GoogleLink"] != DBNull.Value ? Convert.ToString(obj["GoogleLink"]) : this.GoogleLink;
            this.TwitterLink = obj["TwitterLink"] != DBNull.Value ? Convert.ToString(obj["TwitterLink"]) : this.TwitterLink;
            this.DocumentTypeId = Convert.ToString(obj["DocumentTypeId"]);
            this.DocumentNumber = obj["DocumentNumber"] != DBNull.Value ? Convert.ToString(obj["DocumentNumber"]) : this.DocumentNumber;
            this.LocationId = obj["LocationId"] != DBNull.Value ? Convert.ToString(obj["LocationId"]) : this.LocationId;
            this.LocationType = obj["LocationType"] != DBNull.Value ? Convert.ToString(obj["LocationType"]) : this.LocationType;
            this.Medallos = obj["Medallos"] != DBNull.Value ? Convert.ToInt32(obj["Medallos"]) : this.Medallos;
            this.UserRank = obj["UserRank"] != DBNull.Value ? Convert.ToString(obj["UserRank"]) : this.UserRank;
            this.CountryId = obj["CountryId"] != DBNull.Value ? Convert.ToInt32(obj["CountryId"]) : this.CountryId;
            this.CityId = obj["CityId"] != DBNull.Value ? Convert.ToInt32(obj["CityId"]) : this.CityId;
            this.Genre = obj["Genre"] != DBNull.Value ? Convert.ToString(obj["Genre"]) : this.Genre;
            this.Age = obj["Age"] != DBNull.Value ? Convert.ToInt32(obj["Age"]) : this.Age;
            this.Profession = obj["Profession"] != DBNull.Value ? Convert.ToString(obj["Profession"]) : this.Profession;
        }

        /// <summary>
        /// Gets or sets the identifier of the user
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Medallos")]
        public int Medallos { get; set; }

        /// <summary>
        /// Gets or sets the full names of the user
        /// </summary>
        [InfoDatabase(DbType.String, "@Names")]
        [Required(ErrorMessageResourceType = typeof(Resources.Extend.Messages), ErrorMessageResourceName = "VALIDATIONNAME")]
        public string Names { get; set; }

        /// <summary>
        /// Gets or sets the password
        /// </summary>
        [InfoDatabase(DbType.String, "@Password")]
        [Required(ErrorMessageResourceType = typeof(Resources.Extend.Messages), ErrorMessageResourceName = "VALIDATIONPASSWORD")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password confirmation
        /// </summary>
        /// 
        [Required(ErrorMessageResourceType = typeof(Resources.Extend.Messages), ErrorMessageResourceName = "VALIDATIONPASSWORDCONFIR")]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Extend.Messages), ErrorMessageResourceName = "VALIDATIONPASSWORDMASH")]
        public string RePassword { get; set; }

        /// <summary>
        /// Gets or sets the email user
        /// </summary>
        [InfoDatabase(DbType.String, "@Email")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessageResourceType = typeof(Resources.Extend.Messages), ErrorMessageResourceName = "VALIDATIONCORREOINVALIDO")]
        [Required(ErrorMessageResourceType = typeof(Resources.Extend.Messages), ErrorMessageResourceName = "VALIDATIONCORREO")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the creation date
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@Joindate")]
        public DateTime? Joindate { get; set; }

        /// <summary>
        /// Gets or sets the image user
        /// </summary>
        [InfoDatabase(DbType.String, "@Image")]
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets if the user is active
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Active")]
        public bool? Active { get; set; }

        /// <summary>
        /// Gets or sets if the user accept emails
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@News")]
        public bool? News { get; set; }

        /// <summary>
        /// Gets or sets the phone number user
        /// </summary>
        [InfoDatabase(DbType.String, "@Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the language
        /// </summary>
        [InfoDatabase(DbType.String, "@LanguageId")]
        public int? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a user accepts the terms of the site
        /// </summary>
        [IsChecked(ErrorMessageResourceType = typeof(Resources.Extend.Messages), ErrorMessageResourceName = "VALIDATIONTERMSCONDITIONS")]
        public bool Terms { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a user accepts the policies of the site
        /// </summary>
        [IsChecked(ErrorMessageResourceType = typeof(Resources.Extend.Messages), ErrorMessageResourceName = "VALIDATIONPOLICIES")]
        public bool Policy { get; set; }

        /// <summary>
        /// Gets or sets the user rank
        /// </summary>
        [InfoDatabase(DbType.String, "@UserRank")]
        public string UserRank { get; set; }

        /// <summary>
        /// Gets or sets the facebook id
        /// </summary>
        [InfoDatabase(DbType.String, "@FacebookId")]
        public string FacebookId { get; set; }

        /// <summary>
        /// Gets or sets the linked in id
        /// </summary>
        [InfoDatabase(DbType.String, "@LinkedinId")]
        public string LinkedinId { get; set; }

        /// <summary>
        /// Gets or sets the google id
        /// </summary>
        [InfoDatabase(DbType.String, "@GoogleId")]
        public string GoogleId { get; set; }

        /// <summary>
        /// Gets or sets the twitter id
        /// </summary>
        [InfoDatabase(DbType.String, "@TwitterId")]
        public string TwitterId { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [InfoDatabase(DbType.String, "@Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the facebook link
        /// </summary>
        [InfoDatabase(DbType.String, "@FacebookLink")]
        public string FacebookLink { get; set; }

        /// <summary>
        /// Gets or sets the linked in link
        /// </summary>
        [InfoDatabase(DbType.String, "@LinkedinLink")]
        public string LinkedinLink { get; set; }

        /// <summary>
        /// Gets or sets the google link
        /// </summary>
        [InfoDatabase(DbType.String, "@GoogleLink")]
        public string GoogleLink { get; set; }

        /// <summary>
        /// Gets or sets the twitter link
        /// </summary>
        [InfoDatabase(DbType.String, "@TwitterLink")]
        public string TwitterLink { get; set; }

        /// <summary>
        /// Gets or sets the document type id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@DocumentTypeId")]
        public string DocumentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the document number
        /// </summary>
        [InfoDatabase(DbType.String, "@DocumentNumber")]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Gets or sets the location id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@LocationId")]
        public string LocationId { get; set; }

        /// <summary>
        /// Gets or sets the location type
        /// </summary>
        [InfoDatabase(DbType.String, "@LocationType")]
        [Required(ErrorMessageResourceType = typeof(Resources.Extend.Messages), ErrorMessageResourceName = "VALIDATIONLOCATION")]
        public string LocationType { get; set; }

        /// <summary>
        /// Gets or sets the city id
        /// </summary>        
        [InfoDatabase(DbType.Int32, "@CityId")]
        public int? CityId { get; set; }

        /// <summary>
        /// Gets or sets the country id
        /// </summary>        
        [InfoDatabase(DbType.Int32, "@CountryId")]
        public int? CountryId { get; set; }

        /// <summary>
        /// Gets or sets the genre
        /// </summary>
        [InfoDatabase(DbType.String, "@Genre")]
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets the age
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Age")]
        public int? Age { get; set; }

        /// <summary>
        /// Gets or sets the profession
        /// </summary>
        [InfoDatabase(DbType.String, "@Profession")]
        public string Profession { get; set; }

        /// <summary>
        /// Gets or sets the neighborhoods collection
        /// </summary>
        public List<Domain.Entities.Neighborhood> CollNeighborhood { get; set; }

        /// <summary>
        /// Gets or sets the cities collection
        /// </summary>
        public List<Domain.Entities.City> CollCity { get; set; }

        /// <summary>
        /// Gets or sets the departments collection
        /// </summary>
        public List<Domain.Entities.Department> CollDepartment { get; set; }

        /// <summary>
        /// Gets or sets the countries collection
        /// </summary>
        public List<Domain.Entities.Country> CollCountry { get; set; }

        /// <summary>
        /// Gets or sets the document types collection
        /// </summary>
        public List<Domain.Entities.DocumentType> CollDocumentType { get; set; }

        /// <summary>
        /// Gets or sets the interests collection
        /// </summary>
        public List<Domain.Entities.Interest> CollInterest { get; set; }

        /// <summary>
        /// Gets or sets the user interests collection
        /// </summary>
        public List<Domain.Entities.UserInterest> CollUserInterest { get; set; }

        /// <summary>
        /// Gets or sets the related users collection
        /// </summary>
        public List<Domain.Entities.User> CollRelatedUser { get; set; }
    }
}