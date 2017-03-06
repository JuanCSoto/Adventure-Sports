// <copyright file="RolUserRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the table <c>RolUser</c>
    /// </summary>
    public sealed class RolUserRepository : DataRepository<RolUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RolUserRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public RolUserRepository(ISession session)
            : base(session, "GXRolUser")
        {
            this.Entity = new RolUser();
        }

        /// <summary>
        /// Load the information from the table <c>RolUser</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new RolUser(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>RolUser</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new RolUser(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>RolUser</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>RolUser</c></returns>
        public override List<RolUser> GetAll()
        {
            List<RolUser> col = new List<RolUser>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new RolUser(this.Session.Reader));
            }

            return col;
        }
    }
}