// <copyright file="TemplateRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the table <c>Template</c>
    /// </summary>
    public sealed class TemplateRepository : DataRepository<Template>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public TemplateRepository(ISession session)
            : base(session, "GXTemplate")
        {
            this.Entity = new Template();
        }

        /// <summary>
        /// Load the information from the table <c>Template</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Template(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Template</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Template(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Template</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Template</c></returns>
        public override List<Template> GetAll()
        {
            List<Template> col = new List<Template>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new Template(this.Session.Reader));
            }

            return col;
        }
    }
}