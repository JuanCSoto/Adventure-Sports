// <copyright file="MoldRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>Mold</c>
    /// </summary>
    public sealed class MoldRepository : DataRepository<Mold>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoldRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public MoldRepository(ISession session)
            : base(session, "GXMold")
        {
            this.Entity = new Mold();
        }

        /// <summary>
        /// Load the information from the table <c>Mold</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Mold(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Mold</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Mold(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Mold</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Mold</c></returns>
        public override List<Mold> GetAll()
        {
            List<Mold> col = new List<Mold>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Mold(this.Session.Reader));
            }

            return col;
        }
    }
}