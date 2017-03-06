// <copyright file="NeighborhoodRepository.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>Neighborhood</c>
    /// </summary>
    public sealed class NeighborhoodRepository : DataRepository<Neighborhood>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NeighborhoodRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public NeighborhoodRepository(ISession session)
            : base(session, "GXNeighborhood")
        {
            this.Entity = new Neighborhood();
        }

        /// <summary>
        /// Load the information from the table <c>Neighborhood</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Neighborhood(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Neighborhood</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Neighborhood(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Neighborhood</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Neighborhood</c></returns>
        public override List<Neighborhood> GetAll()
        {
            List<Neighborhood> col = new List<Neighborhood>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Neighborhood(this.Session.Reader));
            }

            return col;
        }
    }
}