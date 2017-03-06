// <copyright file="GeneralFindPaging.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
namespace Domain.Entities.FrontEnd
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data;

    /// <summary>
    /// represents the model to General Find Paging
    /// </summary>
    public class GeneralFindPaging
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralFindPaging"/> class
        /// </summary>
        public GeneralFindPaging()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralFindPaging"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public GeneralFindPaging(IDataRecord obj)
        {
            this.Identificador = obj["ID"] != DBNull.Value ? Convert.ToInt32(obj["ID"]) : this.Identificador;
            this.Name = obj["Name"] != DBNull.Value ? Convert.ToString(obj["Name"]) : this.Name;
            this.NameEnglish = obj["Name2"] != DBNull.Value ? Convert.ToString(obj["Name2"]) : this.NameEnglish;
            this.LanguageId = obj["LanguageId"] != DBNull.Value ? Convert.ToInt32(obj["LanguageId"]) : this.LanguageId;
            this.JoinDate = obj["Joindate"] != DBNull.Value ? Convert.ToDateTime(obj["Joindate"]) : this.JoinDate;
            this.Type = obj["Type"] != DBNull.Value ? Convert.ToString(obj["Type"]) : this.Type;
            this.Friendlyurlid = obj["Friendlyurlid"] != DBNull.Value ? Convert.ToString(obj["Friendlyurlid"]) : this.Friendlyurlid;
        }

        /// <summary>
        /// Gets or sets Id from select.
        /// </summary>
        public int Identificador { get; set; }

        /// <summary>
        /// Gets or sets Name from select.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Name in english from select.
        /// </summary>
        public string NameEnglish { get; set; }

        /// <summary>
        /// Gets or sets Language from select.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets Type from select.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets JoinDate from select.
        /// </summary>
        public DateTime JoinDate { get; set; }

        /// <summary>
        /// Gets or sets Friendlyurlid from select.
        /// </summary>
        public string Friendlyurlid { get; set; }
    }
}
