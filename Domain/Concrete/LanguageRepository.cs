// <copyright file="LanguageRepository.cs" company="Dasigno">
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
    /// Class responsible for interaction with the board <c>Language</c>
    /// </summary>
    public sealed class LanguageRepository : DataRepository<Language>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public LanguageRepository(ISession session)
            : base(session, "GXLanguage")
        {
            this.Entity = new Language();
        }

        /// <summary>
        /// Load the information from the table <c>Language</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Language(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Language</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Language(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Language</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Language</c></returns>
        public override List<Language> GetAll()
        {
            List<Language> col = new List<Language>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Language(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// obtains a default user's language
        /// </summary>
        /// <param name="userId">identifier of user</param>
        public void GetByUser(int userId)
        {
            this.Session.CreateCommand("CTLanguagebyuser", true);
            this.Session.AddParameter("@Userid", userId, DbType.Int32);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                this.Entity = new Language(this.Session.Reader);
            }
        }

        /// <summary>
        /// obtains a default user's language
        /// </summary>
        /// <param name="userId">identifier of user</param>
        public void GetLanguageDefault()
        {
            this.Session.CreateCommand("CTLanguageDefault", true);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                this.Entity = new Language(this.Session.Reader);
            }
        }

        /// <summary>
        /// reset all fields of language table to not default
        /// </summary>
        public void UpdateLanguage()
        {
            this.Session.CreateCommand("CTLanguageupdate", true);
            this.Session.ExecuteNonQuery();
        }
    }
}