﻿// <copyright file="BannersectionRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the table <c>Bannersection</c>
    /// </summary>
    public sealed class BannersectionRepository : DataRepository<Bannersection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BannersectionRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public BannersectionRepository(ISession session)
            : base(session, "GXBannersection")
        {
            this.Entity = new Bannersection();
        }

        /// <summary>
        /// Load the information from the table <c>Bannersection</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Bannersection(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Bannersection</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Bannersection(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Bannersection</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Bannersection</c></returns>
        public override List<Bannersection> GetAll()
        {
            List<Bannersection> col = new List<Bannersection>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Bannersection(this.Session.Reader));
            }

            return col;
        }
    }
}