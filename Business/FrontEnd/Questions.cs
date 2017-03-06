// <copyright file="Questions.cs" company="Dasigno">
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
    public class Questions : IFrontEnd
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Questions"/> class
        /// </summary>
        public Questions()
        {
        }

        /// <summary>
        /// Gets or sets the questions collection
        /// </summary>
        public List<QuestionsPaging> CollQuestions { get; set; }

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
            QuestionRepository questionrepository = new QuestionRepository(session);
            this.CollQuestions = questionrepository.QuestionsPaging(1, 6, out total, null);
        }
    }
}
