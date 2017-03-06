// <copyright file="Banner.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Banner</c> object mapped table <c>Banner</c>.
    /// </summary>
    public class Banner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Banner"/> class
        /// </summary>
        public Banner()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Banner"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Banner(IDataRecord obj)
        {
            this.BannerId = Convert.ToInt32(obj["BannerId"]);
            this.Name = Convert.ToString(obj["Name"]);
            this.Bannerfile = Convert.ToString(obj["Bannerfile"]);
            this.Navigateurl = obj["Navigateurl"] != DBNull.Value ? Convert.ToString(obj["Navigateurl"]) : this.Navigateurl;
            this.Onclick = obj["Onclick"] != DBNull.Value ? Convert.ToString(obj["Onclick"]) : this.Onclick;
            this.Height = obj["Height"] != DBNull.Value ? Convert.ToInt32(obj["Height"]) : this.Height;
            this.Width = obj["Width"] != DBNull.Value ? Convert.ToInt32(obj["Width"]) : this.Width;
            this.Bannertype = Convert.ToInt16(obj["Bannertype"]);
            this.PositionId = Convert.ToInt32(obj["PositionId"]);
            this.Bannerdate = Convert.ToDateTime(obj["Bannerdate"]);
            this.Active = Convert.ToBoolean(obj["Active"]);
            this.LanguageId = Convert.ToInt32(obj["LanguageId"]);
        }

        /// <summary>
        /// Gets or sets the identifier of banner
        /// </summary>
        [InfoDatabase(DbType.Int32, "@BannerId")]
        public int? BannerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the banner
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        [Required(ErrorMessage = "Debes ingresar el nombre del banner")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the banner's file
        /// </summary>
        [InfoDatabase(DbType.String, "@Bannerfile")]
        public string Bannerfile { get; set; }

        /// <summary>
        /// Gets or sets the navigation url of the banner
        /// </summary>
        [InfoDatabase(DbType.String, "@Navigateurl")]
        public string Navigateurl { get; set; }

        /// <summary>
        /// Gets or sets the name of function when execute the event click
        /// </summary>
        [InfoDatabase(DbType.String, "@Onclick")]
        public string Onclick { get; set; }

        /// <summary>
        /// Gets or sets the height of the banner
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Height")]
        public int? Height { get; set; }

        /// <summary>
        /// Gets or sets the width of the banner
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Width")]
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the type of the banner
        /// </summary>
        [InfoDatabase(DbType.Int16, "@Bannertype")]
        public short? Bannertype { get; set; }

        /// <summary>
        /// Gets or sets the position of the banner
        /// </summary>
        [InfoDatabase(DbType.Int32, "@PositionId")]
        [Required(ErrorMessage = "Debes seleccionar la posición del banner")]
        public int? PositionId { get; set; }

        /// <summary>
        /// Gets or sets the date of the banner
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@Bannerdate")]
        public DateTime? Bannerdate { get; set; }

        /// <summary>
        /// Gets or sets if the banner is active
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Active")]
        public bool? Active { get; set; }

        /// <summary>
        /// Gets or sets the identifier language
        /// </summary>
        [InfoDatabase(DbType.Int32, "@LanguageId")]
        public int? LanguageId { get; set; }
    }
}