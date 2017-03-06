// <copyright file="UserMail.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Domain.Entities.FrontEnd
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// user mail entity object
    /// </summary>
    public class UserMail
    {
        /// <summary>
        /// Gets or sets the user object
        /// </summary>
        public User objUser { get; set; }

        /// <summary>
        /// Gets or sets the idea collection
        /// </summary>
        public List<MailIdeasPaging> CollIdeas { get; set; }

        /// <summary>
        /// Gets or sets the challenge collection
        /// </summary>
        public List<ChallengesPaging> CollChallenges { get; set; }

        /// <summary>
        /// Gets or sets the question collection
        /// </summary>
        public List<QuestionsPaging> CollQuestions { get; set; }
    }
}
