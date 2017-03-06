// <copyright file="AllyRepository.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>Ally</c>
    /// </summary>
    public sealed class AllyRepository : DataRepository<Ally>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllyRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public AllyRepository(ISession session)
            : base(session, "GXAlly")
        {
            this.Entity = new Ally();
        }

        /// <summary>
        /// Load the information from the table <c>Ally</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Ally(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Ally</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Ally(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Ally</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Ally</c></returns>
        public override List<Ally> GetAll()
        {
            List<Ally> col = new List<Ally>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Ally(this.Session.Reader));
            }

            return col;
        }
    }
}