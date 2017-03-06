// <copyright file="UserInterestRepository.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>UserInterest</c>
    /// </summary>
    public sealed class UserInterestRepository : DataRepository<UserInterest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterestRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public UserInterestRepository(ISession session)
            : base(session, "GXUserInterest")
        {
            this.Entity = new UserInterest();
        }

        /// <summary>
        /// Load the information from the table <c>UserInterest</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new UserInterest(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>UserInterest</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new UserInterest(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>UserInterest</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>UserInterest</c></returns>
        public override List<UserInterest> GetAll()
        {
            List<UserInterest> col = new List<UserInterest>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new UserInterest(this.Session.Reader));
            }

            return col;
        }
    }
}