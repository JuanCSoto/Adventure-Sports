// <copyright file="DocumentType.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// DocumentType object mapped table <c>DocumentType</c>.
    /// </summary>
    public class DocumentType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentType"/> class
        /// </summary>
        public DocumentType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentType"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public DocumentType(IDataRecord obj)
        {
            this.DocumentTypeId = Convert.ToInt32(obj["DocumentTypeId"]);
            this.Name = Convert.ToString(obj["Name"]);
        }

        /// <summary>
        /// Gets or sets the document type id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@DocumentTypeId")]
        public int? DocumentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [InfoDatabase(DbType.String, "@Name")]
        public string Name { get; set; }
    }
}