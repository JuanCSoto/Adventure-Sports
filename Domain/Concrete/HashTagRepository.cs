// <copyright file="HashTagRepository.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>HashTag</c>
    /// </summary>
    public sealed class HashTagRepository : DataRepository<HashTag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HashTagRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public HashTagRepository(ISession session)
            : base(session, "GXHashTag")
        {
            this.Entity = new HashTag();
        }

        /// <summary>
        /// Load the information from the table <c>HashTag</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new HashTag(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>HashTag</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new HashTag(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>HashTag</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>HashTag</c></returns>
        public override List<HashTag> GetAll()
        {
            List<HashTag> col = new List<HashTag>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new HashTag(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>HashTag</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>HashTag</c></returns>
        public List<HashTag> SearchHashTag()
        {
            List<HashTag> col = new List<HashTag>();
            this.BindParameters(10);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new HashTag(this.Session.Reader));
            }

            return col;
        }
    }
}