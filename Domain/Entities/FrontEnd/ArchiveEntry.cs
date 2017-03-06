// <copyright file="ArchiveEntry.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Domain.Entities.FrontEnd
{
    using System;
    using System.Data;

    /// <summary>
    /// Idea object mapped table <c>Archive</c>.
    /// </summary>
    public class ArchiveEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveEntry"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public ArchiveEntry(IDataRecord obj)
        {
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.Name = Convert.ToString(obj["Name"]);
            this.Joindate = Convert.ToDateTime(obj["Joindate"]);
            this.Friendlyurlid = Convert.ToString(obj["Friendlyurlid"]);
        }

        /// <summary>
        /// Gets or sets the content id
        /// </summary>
        public int ContentId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the join date
        /// </summary>
        public DateTime Joindate { get; set; }

        /// <summary>
        /// Gets or sets the friendly URL id
        /// </summary>
        public string Friendlyurlid { get; set; }
    }
}
