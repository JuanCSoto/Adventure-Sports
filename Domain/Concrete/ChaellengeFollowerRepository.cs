// <copyright file="ChaellengeFollowerRepository.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>ChaellengeFollower</c>
    /// </summary>
    public sealed class ChaellengeFollowerRepository : DataRepository<ChaellengeFollower>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChaellengeFollowerRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public ChaellengeFollowerRepository(ISession session)
            : base(session, "GXChaellengeFollower")
        {
            this.Entity = new ChaellengeFollower();
        }

        /// <summary>
        /// Load the information from the table <c>ChaellengeFollower</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new ChaellengeFollower(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>ChaellengeFollower</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new ChaellengeFollower(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>ChaellengeFollower</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>ChaellengeFollower</c></returns>
        public override List<ChaellengeFollower> GetAll()
        {
            List<ChaellengeFollower> col = new List<ChaellengeFollower>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new ChaellengeFollower(this.Session.Reader));
            }

            return col;
        }
    }
}