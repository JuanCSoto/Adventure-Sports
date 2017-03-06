// <copyright file="RolmodulRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the table <c>Rolmodul</c>
    /// </summary>
    public sealed class RolmodulRepository : DataRepository<Rolmodul>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RolmodulRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public RolmodulRepository(ISession session)
            : base(session, "GXRolmodul")
        {
            this.Entity = new Rolmodul();
        }

        /// <summary>
        /// Load the information from the table <c>Rolmodul</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Rolmodul(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Rolmodul</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Rolmodul(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Rolmodul</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Rolmodul</c></returns>
        public override List<Rolmodul> GetAll()
        {
            List<Rolmodul> col = new List<Rolmodul>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Rolmodul(this.Session.Reader));
            }

            return col;
        }
    }
}