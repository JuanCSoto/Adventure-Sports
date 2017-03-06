// <copyright file="FrontEndEditable.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// FrontEndEditable object mapped table <c>FrontEndEditable</c>.
    /// </summary>
    public class FrontEndEditable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrontEndEditable"/> class
        /// </summary>
        public FrontEndEditable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrontEndEditable"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public FrontEndEditable(IDataRecord obj)
        {
            this.EditableId = Convert.ToInt32(obj["EditableId"]);
            this.Name = Convert.ToString(obj["Name"]);
            this.OriginalValue = Convert.ToString(obj["OriginalValue"]);
            this.CurrentValue = obj["CurrentValue"] != DBNull.Value ? Convert.ToString(obj["CurrentValue"]) : this.CurrentValue;
        }

        /// <summary>
        /// Gets or sets the editable id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@EditableId")]
        public int? EditableId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the original value
        /// </summary>
        [InfoDatabase(DbType.String, "@OriginalValue")]
        public string OriginalValue { get; set; }

        /// <summary>
        /// Gets or sets the current value
        /// </summary>
        [InfoDatabase(DbType.String, "@CurrentValue")]
        public string CurrentValue { get; set; }

        /// <summary>
        /// Gets or sets the editable id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@LanguageId")]
        public int? LanguageId { get; set; }
    }
}