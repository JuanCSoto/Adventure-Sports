// <copyright file="RolRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using System.Data;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the table <c>Rol</c>
    /// </summary>
    public sealed class RolRepository : DataRepository<Rol>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RolRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public RolRepository(ISession session)
            : base(session, "GXRol")
        {
            this.Entity = new Rol();
        }

        /// <summary>
        /// Load the information from the table <c>Rol</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Rol(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Rol</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Rol(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Rol</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Rol</c></returns>
        public override List<Rol> GetAll()
        {
            List<Rol> col = new List<Rol>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Rol(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// obtains a list of roles according to user
        /// </summary>
        /// <param name="userId">identifier of user</param>
        /// <returns>returns a list of roles</returns>
        public IEnumerable<Rol> GetRols(int userId)
        {
            this.Session.CreateCommand("CTUserroles", true);
            this.Session.AddParameter("@UserId", userId, DbType.Int32);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                yield return new Rol(this.Session.Reader);
            }
        }
    }
}