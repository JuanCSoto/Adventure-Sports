// <copyright file="UserRelationRepository.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>UserRelation</c>
    /// </summary>
    public sealed class UserRelationRepository : DataRepository<UserRelation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRelationRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public UserRelationRepository(ISession session)
            : base(session, "GXUserRelation")
        {
            this.Entity = new UserRelation();
        }

        /// <summary>
        /// Load the information from the table <c>UserRelation</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new UserRelation(this.Session.Reader);
            }
        }

        /// <summary>
        /// Check if the information exist in the table <c>UserRelation</c> according to the parameters send
        /// </summary>
        /// <returns>True if the information exist false if not</returns>
        public bool Exist()
        {
            bool result = false;
            base.Load();
            while (this.Session.Read())
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Load the information from the table <c>UserRelation</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new UserRelation(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>UserRelation</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>UserRelation</c></returns>
        public override List<UserRelation> GetAll()
        {
            List<UserRelation> col = new List<UserRelation>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new UserRelation(this.Session.Reader));
            }

            return col;
        }
    }
}