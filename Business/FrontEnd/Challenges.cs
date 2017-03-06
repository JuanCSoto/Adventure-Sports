// <copyright file="Challenges.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Business.FrontEnd
{
    using System.Collections.Generic;
    using System.Web;
    using System.Xml;
    using Domain.Abstract;
    using Domain.Concrete;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Represents the model for the front end
    /// </summary>
    public class Challenges : IFrontEnd
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Challenges"/> class
        /// </summary>
        public Challenges()
        {
        }

        /// <summary>
        /// Gets or sets the Challenge collection
        /// </summary>
        public List<ChallengesPaging> CollChallenges { get; set; }

        /// <summary>
        /// Gets or sets the the total of challenges
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Bind the context and the session with the content
        /// </summary>
        /// <param name="context">Context page</param>
        /// <param name="session">Session object</param>
        /// <param name="id">Content ID</param>
        /// <param name="userId">current user ID</param>
        public void Bind(HttpContextBase context, ISession session, int? id, int? userId,int? LanguageId)
        {
            int total = 0;
            ChallengeRepository challengerepository = new ChallengeRepository(session);
            this.CollChallenges = challengerepository.ChallengesPaging(1, 6, out total, null,LanguageId);
            this.TotalCount = total;
        }
    }
}
