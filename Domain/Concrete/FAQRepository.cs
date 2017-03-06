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
    public sealed class FAQRepository : DataRepository<FAQ>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FAQRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public FAQRepository(ISession session)
            : base(session, "GXFAQ")
        {
            this.Entity = new FAQ();
        }

        /// <summary>
        /// Load the information from the table <c>FAQ</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new FAQ(this.Session.Reader);
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
                this.Entity = new FAQ(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>FAQ</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>FAQ</c></returns>
        public override List<FAQ> GetAll()
        {
            List<FAQ> col = new List<FAQ>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new FAQ(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>FAQ</c> to the list of entities 
        /// </summary>
        /// <returns>a list of entities</returns>
        public List<FAQList> GetAllFrontEnd(int? LanguageId)
        {
            List<FAQList> col = new List<FAQList>();
            this.Session.CreateCommand("CTFAQFrontEnd", true);
            this.Session.AddParameter("@Active", true, System.Data.ParameterDirection.Input, System.Data.DbType.Boolean);
            this.Session.AddParameter("@LanguageId", LanguageId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new FAQList(this.Session.Reader));
            }

            return col;
        }
    }
}