// <copyright file="RecoveryTokenRepository.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>RecoveryToken</c>
    /// </summary>
    public sealed class RecoveryTokenRepository : DataRepository<RecoveryToken>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecoveryTokenRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public RecoveryTokenRepository(ISession session)
            : base(session, "GXRecoveryToken")
        {
            this.Entity = new RecoveryToken();
        }

        /// <summary>
        /// Load the information from the table <c>RecoveryToken</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new RecoveryToken(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>RecoveryToken</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new RecoveryToken(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>RecoveryToken</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>RecoveryToken</c></returns>
        public override List<RecoveryToken> GetAll()
        {
            List<RecoveryToken> col = new List<RecoveryToken>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new RecoveryToken(this.Session.Reader));
            }

            return col;
        }
    }
}