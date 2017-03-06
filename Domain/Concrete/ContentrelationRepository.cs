// <copyright file="ContentrelationRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the table <c>Contentrelation</c>
    /// </summary>
    public sealed class ContentrelationRepository : DataRepository<Contentrelation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentrelationRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public ContentrelationRepository(ISession session)
            : base(session, "GXContentrelation")
        {
            this.Entity = new Contentrelation();
        }

        /// <summary>
        /// Load the information from the table <c>Contentrelation</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Contentrelation(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Contentrelation</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Contentrelation(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Contentrelation</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Contentrelation</c></returns>
        public override List<Contentrelation> GetAll()
        {
            List<Contentrelation> col = new List<Contentrelation>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new Contentrelation(this.Session.Reader));
            }

            return col;
        }
    }
}