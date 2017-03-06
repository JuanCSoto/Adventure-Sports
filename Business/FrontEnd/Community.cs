// <copyright file="Community.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno</author>
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
    public class Community : IFrontEnd
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Community"/> class
        /// </summary>
        public Community()
        {
        }

        /// <summary>
        /// Gets or sets the total number of active users
        /// </summary>
        public int TotalParticipants { get; set; }

        /// <summary>
        /// Bind the context and the session with the content
        /// </summary>
        /// <param name="context">Context page</param>
        /// <param name="session">Session object</param>
        /// <param name="id">Content ID</param>
        /// <param name="userId">current user ID</param>
        public void Bind(HttpContextBase context, ISession session, int? id, int? userId,int? LanguageId)
        {
            UserRepository user = new UserRepository(session);
            int totalParticipants;
            List<Domain.Entities.FrontEnd.UserProfilePaging> partipants = user.Participants(0, out totalParticipants);

            this.TotalParticipants = totalParticipants;
        }
    }
}
