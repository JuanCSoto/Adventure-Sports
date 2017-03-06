// <copyright file="InterestRepository.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>Interest</c>
    /// </summary>
    public sealed class InterestRepository : DataRepository<Interest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterestRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public InterestRepository(ISession session)
            : base(session, "GXInterest")
        {
            this.Entity = new Interest();
        }

        /// <summary>
        /// Load the information from the table <c>Interest</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Interest(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Interest</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Interest(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Interest</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Interest</c></returns>
        public override List<Interest> GetAll()
        {
            List<Interest> col = new List<Interest>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Interest(this.Session.Reader));
            }

            return col;
        }
    }
}