// <copyright file="ModulRepository.cs" company="Dasigno">
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
    /// Class responsible for interaction with the table <c>Modul</c>
    /// </summary>
    public sealed class ModulRepository : DataRepository<Modul>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModulRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public ModulRepository(ISession session)
            : base(session, "GXModul")
        {
            this.Entity = new Modul();
        }

        /// <summary>
        /// Load the information from the table <c>Modul</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Modul(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Modul</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Modul(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Modul</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Modul</c></returns>
        public override List<Modul> GetAll()
        {
            List<Modul> col = new List<Modul>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Modul(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// obtains a list of modules according to user
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <param name="languageId">language identifier</param>
        /// <returns>returns a list of modules</returns>
        public List<Modul> GetModulsbyuser(int userId, int languageId)
        {
            List<Modul> col = new List<Modul>();
            this.Session.CreateCommand("CTModulsbyuser", true);
            this.Session.AddParameter("@UserId", userId, DbType.Int32);
            this.Session.AddParameter("@LanguageId", languageId, DbType.Int32);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Modul(this.Session.Reader));
            }

            return col;
        }
    }
}