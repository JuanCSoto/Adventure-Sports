// <copyright file="ChallengeRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Class responsible for interaction with the table <c>Challenge</c>
    /// </summary>
    public sealed class ChallengeRepository : DataRepository<Challenge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChallengeRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public ChallengeRepository(ISession session)
            : base(session, "GXChallenge")
        {
            this.Entity = new Challenge();
        }

        /// <summary>
        /// Load the information from the table <c>Challenge</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Challenge(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Challenge</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Challenge(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Challenge</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Challenge</c></returns>
        public override List<Challenge> GetAll()
        {
            List<Challenge> col = new List<Challenge>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new Challenge(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Challenge</c> to the list of entities
        /// </summary>
        /// <returns>A list of entities</returns>
        public List<ExpiringChallenges> ExpiringChallenges()
        {
            List<ExpiringChallenges> col = new List<ExpiringChallenges>();
            this.Session.CreateCommand("CTExpiringChallenges", true);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new ExpiringChallenges(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Challenge</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="active">To return active or inactive content</param>
        /// <returns>A list of entities according to the parameters send</returns>
        public List<ChallengesPaging> ChallengesPaging(int pageIndex, int pageSize, out int totalCount, bool? active,int? LanguageId)
        {
            totalCount = 0;
            List<ChallengesPaging> col = new List<ChallengesPaging>();
            this.Session.CreateCommand("CTChallengesPaging", true);
            this.Session.AddParameter("@TotalCount", 0, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@Active", active, System.Data.ParameterDirection.Input, System.Data.DbType.Boolean);
            this.Session.AddParameter("@LanguageId", LanguageId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new ChallengesPaging(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Challenge</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="active">To return active or inactive content</param>
        /// <returns>A list of entities according to the parameters send</returns>
        public List<ChallengesPaging> ChallengesPagingRecommended(int pageIndex, int pageSize, out int totalCount, bool active)
        {
            totalCount = 0;
            List<ChallengesPaging> col = new List<ChallengesPaging>();
            this.Session.CreateCommand("CTChallengesPagingRecommended", true);
            this.Session.AddParameter("@TotalCount", 0, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@Active", active, System.Data.ParameterDirection.Input, System.Data.DbType.Boolean);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new ChallengesPaging(this.Session.Reader));
            }

            return col;
        }
    }
}