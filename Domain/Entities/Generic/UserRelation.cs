// <copyright file="UserRelation.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// UserRelation object mapped table <c>UserRelation</c>.
    /// </summary>
    public class UserRelation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRelation"/> class
        /// </summary>
        public UserRelation()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRelation"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public UserRelation(IDataRecord obj)
        {
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.UserRelationId = Convert.ToInt32(obj["UserRelationId"]);
            this.RelationId = Convert.ToInt32(obj["RelationId"]);
            this.Action = Convert.ToString(obj["ActionRelation"]);
            this.Value = Convert.ToInt32(obj["Value"]);
            this.CreationDate = Convert.ToDateTime(obj["CreationDate"]);
        }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the user relation id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserRelationId")]
        public int? UserRelationId { get; set; }

        /// <summary>
        /// Gets or sets the relation id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@RelationId")]
        public int? RelationId { get; set; }

        /// <summary>
        /// Gets or sets the action of the relation
        /// </summary>
        [InfoDatabase(DbType.String, "@ActionRelation")]
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Value")]
        public int? Value { get; set; }

        /// <summary>
        /// Gets or sets the creation date
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@CreationDate")]
        public DateTime? CreationDate { get; set; }
    }
}