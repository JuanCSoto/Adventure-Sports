// <copyright file="IdeaReport.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// IdeaReport object mapped table <c>IdeaReport</c>.
    /// </summary>
    public class IdeaReport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdeaReport"/> class
        /// </summary>
        public IdeaReport()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdeaReport"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public IdeaReport(IDataRecord obj)
        {
            this.IdeaReportId = Convert.ToInt32(obj["IdeaReportId"]);
            this.IdeaId = Convert.ToInt32(obj["IdeaId"]);
            this.Date = Convert.ToDateTime(obj["Date"]);
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.Text = Convert.ToString(obj["Text"]);
            this.Motive = Convert.ToString(obj["Motive"]);
            this.Status = Convert.ToBoolean(obj["Status"]);            
        }

        /// <summary>
        /// Gets or sets the identifier of the ideaReport
        /// </summary>
        [InfoDatabase(DbType.Int32, "@IdeaReportId")]
        public int? IdeaReportId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@IdeaId")]
        public int? IdeaId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the ideaReport text
        /// </summary>
        [InfoDatabase(DbType.String, "@Text")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar el texto del reporte")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the idea Report motive
        /// </summary>
        [InfoDatabase(DbType.String, "@Motive")]
        [Required(ErrorMessage = "Debes ingresar el motivo del reporte")]
        public string Motive { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the ideaReport
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@Date")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the status of the ideaReport
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Status")]
        public bool? Status { get; set; }
    }
}