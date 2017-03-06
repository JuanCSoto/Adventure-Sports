// <copyright file="Contentsearch.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;

    /// <summary>
    /// Custom object for search
    /// </summary>
    public class Contentsearch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Contentsearch"/> class
        /// </summary>
        public Contentsearch()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contentsearch"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Contentsearch(IDataRecord obj)
        {
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.Name = obj["Name"].ToString();
            this.Shortdescription = obj["Shortdescription"].ToString();
            this.Frienlyname = obj["Frienlyname"].ToString();
            this.Section = obj["Section"].ToString();
            this.Sectionurl = obj["Sectionurl"].ToString();
        }

        /// <summary>
        /// Gets or sets the field <c>ContentId</c>
        /// </summary>
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the field <c>Name</c>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the field <c>Shortdescription</c>
        /// </summary>
        public string Shortdescription { get; set; }

        /// <summary>
        /// Gets or sets the field <c>Frienlyname</c>
        /// </summary>
        public string Frienlyname { get; set; }

        /// <summary>
        /// Gets or sets the field <c>Section</c>
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// Gets or sets the field <c>Sectionurl</c>
        /// </summary>
        public string Sectionurl { get; set; }
    }
}