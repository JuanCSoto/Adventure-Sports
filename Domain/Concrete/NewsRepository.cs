// <copyright file="NewsRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the table <c>News</c>
    /// </summary>
    public sealed class NewsRepository : DataRepository<News>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public NewsRepository(ISession session)
            : base(session, "GXNews")
        {
            this.Entity = new News();
        }

        /// <summary>
        /// Load the information from the table <c>News</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new News(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>News</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new News(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>News</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>News</c></returns>
        public override List<News> GetAll()
        {
            List<News> col = new List<News>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new News(this.Session.Reader));
            }

            return col;
        }
    }
}