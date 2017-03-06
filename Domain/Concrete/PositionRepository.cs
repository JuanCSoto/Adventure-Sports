// <copyright file="PositionRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the table <c>Position</c>
    /// </summary>
    public sealed class PositionRepository : DataRepository<Position>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public PositionRepository(ISession session)
            : base(session, "GXPosition")
        {
            this.Entity = new Position();
        }

        /// <summary>
        /// Load the information from the table <c>Position</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Position(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Position</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Position(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Position</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Position</c></returns>
        public override List<Position> GetAll()
        {
            List<Position> col = new List<Position>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Position(this.Session.Reader));
            }

            return col;
        }
    }
}