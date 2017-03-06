// <copyright file="IdeaHashTagRepository.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>ContentHashTag</c>
    /// </summary>
    public sealed class IdeaHashTagRepository : DataRepository<IdeaHashTag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdeaHashTagRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public IdeaHashTagRepository(ISession session)
            : base(session, "GXIdeaHashTag")
        {
            this.Entity = new IdeaHashTag();
        }

        /// <summary>
        /// Load the information from the table <c>ContentHashTag</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new IdeaHashTag(this.Session.Reader);
            }
        }

        /// <summary>
        /// Check if the information exist in the table <c>ContentHashTag</c> according to the parameters send
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
        /// Load the information from the table <c>ContentHashTag</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new IdeaHashTag(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>ContentHashTag</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>ContentHashTag</c></returns>
        public override List<IdeaHashTag> GetAll()
        {
            List<IdeaHashTag> col = new List<IdeaHashTag>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new IdeaHashTag(this.Session.Reader));
            }

            return col;
        }
    }
}