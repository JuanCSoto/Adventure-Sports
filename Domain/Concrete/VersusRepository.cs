// <copyright file="VersusRepository.cs" company="Dasigno">VersusRepository
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Class responsible for interaction with the table <c>Versus</c>
    /// </summary>
    public sealed class VersusRepository : DataRepository<Versus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersusRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public VersusRepository(ISession session)
            : base(session, "GXVersus")
        {
            this.Entity = new Versus();
        }

        /// <summary>
        /// Load the information from the table <c>Versus</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Versus(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Versus</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Versus(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Versus</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Versus</c></returns>
        public override List<Versus> GetAll()
        {
            List<Versus> col = new List<Versus>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new Versus(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// obtiene un versus para un contenido
        /// </summary>
        /// <returns>un versus</returns>
        public List<IdeasPaging> GetVersus()
        {
            List<IdeasPaging> col = new List<IdeasPaging>();
            this.Session.CreateCommand("CTGetVersus", true);
            this.Session.AddParameter("@ContentId", this.Entity.ContentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@UserId", this.Entity.UserId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new IdeasPaging(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Revisa si alguna de las ideas del versus ya fue votada en otro
        /// </summary>
        /// <returns>verdadero si ya voto falso si no</returns>
        public bool VoteExists()
        {
            bool result = true;
            Versus versus = new Versus();
            this.Session.CreateCommand("CTVersusVoteExists", true);
            this.Session.AddParameter("@IdeaIdA", this.Entity.IdeaIdA, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@IdeaIdB", this.Entity.IdeaIdB, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@UserId", this.Entity.UserId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);

            this.Session.ExecuteReader();
            while (Session.Read())
            {
                versus = new Versus(this.Session.Reader);
            }

            if (versus == null || versus.VersusId == null)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Load all the versus an idea has won
        /// </summary>
        /// <returns>list of the entities type <c>Versus</c></returns>
        public List<Versus> VersusIdeaWon()
        {
            List<Versus> versusList = new List<Versus>();
            this.Session.CreateCommand("CTVersusIdeaWon", true);
            this.Session.AddParameter("@WinnerID", this.Entity.WinnerId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);

            this.Session.ExecuteReader();
            while (Session.Read())
            {
                versusList.Add(new Versus(this.Session.Reader));
            }

            return versusList;
        }
    }
}