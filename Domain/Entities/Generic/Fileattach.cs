// <copyright file="Fileattach.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Fileattach</c> object mapped table <c>Fileattach</c>.
    /// </summary>
    public class Fileattach
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Fileattach"/> class
        /// </summary>
        public Fileattach()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fileattach"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Fileattach(IDataRecord obj)
        {
            this.FileattachId = Convert.ToInt32(obj["FileattachId"]);
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.Name = obj["Name"] != DBNull.Value ? Convert.ToString(obj["Name"]) : this.Name;
            this.Filename = Convert.ToString(obj["Filename"]);
            this.Joindate = Convert.ToDateTime(obj["Joindate"]);
            this.Type = (TypeFile)Convert.ToInt16(obj["Type"]);
            this.Orderliness = Convert.ToInt32(obj["Orderliness"]);
        }

        /// <summary>
        /// Defines the type of the file
        /// </summary>
        public enum TypeFile : short
        {
            /// <summary>
            /// File type image
            /// </summary>
            Image = 0,

            /// <summary>
            /// File type video
            /// </summary>
            Video = 1,

            /// <summary>
            /// Different file image or video
            /// </summary>
            File = 2
        }

        /// <summary>
        /// Gets or sets the identifier of the attach file
        /// </summary>
        [InfoDatabase(DbType.Int32, "@FileattachId")]
        public int? FileattachId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentId")]
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the alias of the file
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the file
        /// </summary>
        [InfoDatabase(DbType.String, "@Filename")]
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the file
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@Joindate")]
        public DateTime? Joindate { get; set; }

        /// <summary>
        /// Gets or sets the type file
        /// </summary>
        [InfoDatabase(DbType.Int16, "@Type")]
        public TypeFile? Type { get; set; }

        /// <summary>
        /// Gets or sets the Order of the file
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Orderliness")]
        public int? Orderliness { get; set; }
    }
}