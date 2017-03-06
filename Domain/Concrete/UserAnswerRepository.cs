// <copyright file="UserAnswerRepository.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System;
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>UserAnswer</c>
    /// </summary>
    public sealed class UserAnswerRepository : DataRepository<UserAnswer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAnswerRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public UserAnswerRepository(ISession session)
            : base(session, "GXUserAnswer")
        {
            this.Entity = new UserAnswer();
        }

        /// <summary>
        /// Load the information from the table <c>UserAnswer</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new UserAnswer(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>UserAnswer</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new UserAnswer(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>UserAnswer</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>UserAnswer</c></returns>
        public override List<UserAnswer> GetAll()
        {
            List<UserAnswer> col = new List<UserAnswer>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new UserAnswer(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Check if a user had already answer a content
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="contentId">content id</param>
        /// <param name="answerId">answer id</param>
        /// <returns>true if the user had already voted false if not</returns>
        public bool CheckUserVoted(int userId, int? contentId, int? answerId)
        {
            this.Session.CreateCommand("CTCheckUserVoted", true);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@ContentId", contentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@AnswerId", answerId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);

            return Convert.ToBoolean(this.Session.ExecuteScalar());
        }
    }
}