// <copyright file="FriendlyurlRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>Friendlyurl</c>
    /// </summary>
    public sealed class FriendlyurlRepository : DataRepository<Friendlyurl>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FriendlyurlRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public FriendlyurlRepository(ISession session)
            : base(session, "GXFriendlyurl")
        {
            this.Entity = new Friendlyurl();
        }

        /// <summary>
        /// Load the information from the table <c>Friendlyurl</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Friendlyurl(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Friendlyurl</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Friendlyurl(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Friendlyurl</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Friendlyurl</c></returns>
        public override List<Friendlyurl> GetAll()
        {
            List<Friendlyurl> col = new List<Friendlyurl>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Friendlyurl(this.Session.Reader));
            }

            return col;
        }
    }
}