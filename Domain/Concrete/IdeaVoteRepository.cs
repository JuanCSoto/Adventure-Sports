// <copyright file="IdeaVoteRepository.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>IdeaVote</c>
    /// </summary>
    public sealed class IdeaVoteRepository : DataRepository<IdeaVote>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdeaVoteRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public IdeaVoteRepository(ISession session)
            : base(session, "GXIdeaVote")
        {
            this.Entity = new IdeaVote();
        }

        /// <summary>
        /// Load the information from the table <c>IdeaVote</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new IdeaVote(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>IdeaVote</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new IdeaVote(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>IdeaVote</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>IdeaVote</c></returns>
        public override List<IdeaVote> GetAll()
        {
            List<IdeaVote> col = new List<IdeaVote>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new IdeaVote(this.Session.Reader));
            }

            return col;
        }
    }
}