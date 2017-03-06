// <copyright file="UserProfilePaging.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Domain.Entities.FrontEnd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using Domain.Abstract;
    using Domain.Entities.Basic;

    /// <summary>
    /// <c>UserProfilePaging</c> object mapped table <c>UserProfilePaging</c>.
    /// </summary>
    public class UserProfilePaging
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfilePaging"/> class
        /// </summary>
        public UserProfilePaging()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfilePaging"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public UserProfilePaging(IDataRecord obj)
        {
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.Names = Convert.ToString(obj["Names"]);
            this.Email = Convert.ToString(obj["Email"]);
            this.Joindate = Convert.ToDateTime(obj["Joindate"]);
            this.Image = obj["Image"] != DBNull.Value ? Convert.ToString(obj["Image"]) : this.Image;
            this.UserRank = Convert.ToString(obj["UserRank"]);
            this.Medallos = Convert.ToInt32(obj["Medallos"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the user
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the full names of the user
        /// </summary>
        public string Names { get; set; }

        /// <summary>
        /// Gets or sets the email user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the creation date
        /// </summary>
        public DateTime? Joindate { get; set; }

        /// <summary>
        /// Gets or sets the image user
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the image user
        /// </summary>
        public int? Medallos { get; set; }

        /// <summary>
        /// Gets or sets the image user
        /// </summary>
        public string UserRank { get; set; }
    }
}