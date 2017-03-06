// <copyright file="FAQRepository.cs" company="Dasigno">
//  Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Class responsible for interaction with the board <c>FAQ</c>
    /// </summary>
    public sealed class SuccessStoryRepository : DataRepository<SuccessStory>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FAQRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public SuccessStoryRepository(ISession session)
            : base(session, "GXSuccessStory")
        {
            this.Entity = new SuccessStory();
        }

        /// <summary>
        /// Load the information from the table <c>FAQ</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new SuccessStory(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>FAQ</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new SuccessStory(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>FAQ</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>FAQ</c></returns>
        public override List<SuccessStory> GetAll()
        {
            List<SuccessStory> col = new List<SuccessStory>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new SuccessStory(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>SuccessStory</c> to the list of entities 
        /// </summary>
        /// <returns>a list of entities</returns>
        public List<SuccessStoryList> GetAllFrontEnd(int? LanguageId)
        {
            List<SuccessStoryList> col = new List<SuccessStoryList>();
            this.Session.CreateCommand("CTSuccessStoryFrontEnd", true);
            this.Session.AddParameter("@Active", true, System.Data.ParameterDirection.Input, System.Data.DbType.Boolean);
            this.Session.AddParameter("@LanguageId", LanguageId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new SuccessStoryList(this.Session.Reader));
            }

            return col;
        }
    }
}