// <copyright file="CountryRepository.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>Country</c>
    /// </summary>
    public sealed class CountryRepository : DataRepository<Country>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountryRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public CountryRepository(ISession session)
            : base(session, "GXCountry")
        {
            this.Entity = new Country();
        }

        /// <summary>
        /// Load the information from the table <c>Country</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Country(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Country</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Country(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Country</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Country</c></returns>
        public override List<Country> GetAll()
        {
            List<Country> col = new List<Country>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Country(this.Session.Reader));
            }

            return col;
        }
    }
}