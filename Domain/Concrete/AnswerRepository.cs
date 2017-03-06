// <copyright file="AnswerRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the table <c>Answer</c>
    /// </summary>
    public sealed class AnswerRepository : DataRepository<Answer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnswerRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public AnswerRepository(ISession session)
            : base(session, "GXAnswer")
        {
            this.Entity = new Answer();
        }

        /// <summary>
        /// Load the information from the table <c>Answer</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Answer(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Answer</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Answer(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Answer</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Answer</c></returns>
        public override List<Answer> GetAll()
        {
            List<Answer> col = new List<Answer>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new Answer(this.Session.Reader));
            }

            return col;
        }
    }
}