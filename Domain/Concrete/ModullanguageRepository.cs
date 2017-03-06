// <copyright file="ModullanguageRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>Modullanguage</c>
    /// </summary>
    public sealed class ModullanguageRepository : DataRepository<Modullanguage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModullanguageRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public ModullanguageRepository(ISession session)
            : base(session, "GXModullanguage")
        {
            this.Entity = new Modullanguage();
        }

        /// <summary>
        /// Load the information from the table <c>Modullanguage</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Modullanguage(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Modullanguage</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Modullanguage(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Modullanguage</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Modullanguage</c></returns>
        public override List<Modullanguage> GetAll()
        {
            List<Modullanguage> col = new List<Modullanguage>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Modullanguage(this.Session.Reader));
            }

            return col;
        }
    }
}