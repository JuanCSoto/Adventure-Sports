// <copyright file="UserSetting.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// UserSetting object mapped table <c>UserSetting</c>.
    /// </summary>
    public class UserSetting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSetting"/> class
        /// </summary>
        public UserSetting()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSetting"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public UserSetting(IDataRecord obj)
        {
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.KeyWord = Convert.ToString(obj["KeyWord"]);
            this.Value = Convert.ToString(obj["Value"]);
        }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the keyword for the user setting
        /// </summary>
        [InfoDatabase(DbType.String, "@KeyWord")]
        public string KeyWord { get; set; }

        /// <summary>
        /// Gets or sets the value of the user setting
        /// </summary>
        [InfoDatabase(DbType.String, "@Value")]
        public string Value { get; set; }
    }
}