// <copyright file="Section.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Section</c> object mapped table <c>Section</c>.
    /// </summary>
    public class Section
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class
        /// </summary>
        public Section()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Section(IDataRecord obj)
        {
            this.SectionId = Convert.ToInt32(obj["SectionId"]);
            this.ParentId = obj["ParentId"] != DBNull.Value ? Convert.ToInt32(obj["ParentId"]) : this.ParentId;
            this.Name = Convert.ToString(obj["Name"]);
            this.Description = Convert.ToString(obj["Description"]);
            this.Layout = Convert.ToString(obj["Layout"]);
            this.Template = Convert.ToString(obj["Template"]);
            this.Sectiontype = Convert.ToInt16(obj["Sectiontype"]);
            this.Target = Convert.ToString(obj["Target"]);
            this.Active = Convert.ToBoolean(obj["Active"]);
            this.Private = Convert.ToBoolean(obj["Private"]);
            this.Url = Convert.ToBoolean(obj["Url"]);
            this.Navigateurl = obj["Navigateurl"] != DBNull.Value ? Convert.ToString(obj["Navigateurl"]) : this.Navigateurl;
            this.Sectionorder = Convert.ToInt32(obj["Sectionorder"]);
            this.Friendlyname = Convert.ToString(obj["Friendlyurlid"]);
            this.LanguageId = Convert.ToInt32(obj["LanguageId"]);
        }

        /// <summary>
        /// Gets or sets the identifier of the section
        /// </summary>
        [InfoDatabase(DbType.Int32, "@SectionId")]
        public int? SectionId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parent section
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ParentId")]
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the section
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the section
        /// </summary>
        [InfoDatabase(DbType.String, "@Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the layout of the section
        /// </summary>
        [InfoDatabase(DbType.String, "@Layout")]
        public string Layout { get; set; }

        /// <summary>
        /// Gets or sets the output template of the section
        /// </summary>
        [InfoDatabase(DbType.String, "@Template")]
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets the section type
        /// </summary>
        [InfoDatabase(DbType.Int16, "@Sectiontype")]
        public short? Sectiontype { get; set; }

        /// <summary>
        /// Gets or sets the target of the hyperlink
        /// </summary>
        [InfoDatabase(DbType.String, "@Target")]
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets if the section is active
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Active")]
        public bool? Active { get; set; }

        /// <summary>
        /// Gets or sets if the section is private
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Private")]
        public bool? Private { get; set; }

        /// <summary>
        /// Gets or sets if the section have a hyperlink
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Url")]
        public bool? Url { get; set; }

        /// <summary>
        /// Gets or sets the hyperlink of the section
        /// </summary>
        [InfoDatabase(DbType.String, "@Navigateurl")]
        public string Navigateurl { get; set; }

        /// <summary>
        /// Gets or sets the order of the section
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Sectionorder")]
        public int? Sectionorder { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the language
        /// </summary>
        [InfoDatabase(DbType.Int32, "@LanguageId")]
        public int? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of the section
        /// </summary>
        public string Friendlyname { get; set; }

        /// <summary>
        /// Gets or sets the max value of the section in the complete list
        /// </summary>
        public int MaxValue { get; set; }

        /// <summary>
        /// Gets or sets the old value when the section is change the order
        /// </summary>
        public int? OldOrder { get; set; }
    }
}