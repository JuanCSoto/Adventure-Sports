// <copyright file="ContenttagRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the table <c>Contenttag</c>
    /// </summary>
    public sealed class ContenttagRepository : DataRepository<Contenttag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContenttagRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public ContenttagRepository(ISession session)
            : base(session, "GXContenttag")
        {
            this.Entity = new Contenttag();
        }

        /// <summary>
        /// Load the information from the table <c>Contenttag</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Contenttag(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Contenttag</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Contenttag(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Contenttag</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Contenttag</c></returns>
        public override List<Contenttag> GetAll()
        {
            List<Contenttag> col = new List<Contenttag>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Contenttag(this.Session.Reader));
            }

            return col;
        }
    }
}