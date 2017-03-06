// <copyright file="Friendlyurl.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Friendlyurl</c> object mapped table <c>Friendlyurl</c>.
    /// </summary>
    public class Friendlyurl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Friendlyurl"/> class
        /// </summary>
        public Friendlyurl()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Friendlyurl"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Friendlyurl(IDataRecord obj)
        {
            this.Friendlyurlid = Convert.ToString(obj["Friendlyurlid"]);
            this.Id = Convert.ToInt32(obj["Id"]);
            this.Type = (FriendlyType)Convert.ToInt16(obj["Type"]);
            this.LanguageId = Convert.ToInt32(obj["LanguageId"]);
        }

        /// <summary>
        /// Represents the type of the friendly url
        /// </summary>
        public enum FriendlyType : short
        {
            /// <summary>
            /// Type content
            /// </summary>
            Content = 1,

            /// <summary>
            /// Type section
            /// </summary>
            Section = 2,

            /// <summary>
            /// Type idea
            /// </summary>
            Idea = 3,

            /// <summary>
            /// Type blog
            /// </summary>
            BlogEntry = 4,

            /// <summary>
            /// Type Succes Case
            /// </summary>
            SuccessCase = 5

        }

        /// <summary>
        /// Gets or sets the identifier of the friendly url
        /// </summary>
        [InfoDatabase(DbType.String, "@Friendlyurlid")]
        public string Friendlyurlid { get; set; }

        /// <summary>
        /// Gets or sets the relation identifier
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Id")]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the friendly url
        /// </summary>
        [InfoDatabase(DbType.Int16, "@Type")]
        public FriendlyType? Type { get; set; }

        /// <summary>
        /// Gets or sets the identifier of language
        /// </summary>
        [InfoDatabase(DbType.Int32, "@LanguageId")]
        public int? LanguageId { get; set; }
    }
}