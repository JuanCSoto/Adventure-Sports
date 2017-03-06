// <copyright file="CityRepository.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>City</c>
    /// </summary>
    public sealed class CityRepository : DataRepository<City>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CityRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public CityRepository(ISession session)
            : base(session, "GXCity")
        {
            this.Entity = new City();
        }

        /// <summary>
        /// Load the information from the table <c>City</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new City(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>City</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new City(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>City</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>City</c></returns>
        public override List<City> GetAll()
        {
            List<City> col = new List<City>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new City(this.Session.Reader));
            }

            return col;
        }
    }
}