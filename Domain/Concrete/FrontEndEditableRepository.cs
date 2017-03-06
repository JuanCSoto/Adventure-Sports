// <copyright file="FrontEndEditableRepository.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>FrontEndEditable</c>
    /// </summary>
    public sealed class FrontEndEditableRepository : DataRepository<FrontEndEditable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrontEndEditableRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public FrontEndEditableRepository(ISession session)
            : base(session, "GXFrontEndEditable")
        {
            this.Entity = new FrontEndEditable();
        }

        /// <summary>
        /// Load the information from the table <c>FrontEndEditable</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new FrontEndEditable(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>FrontEndEditable</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new FrontEndEditable(this.Session.Reader);
            }
        }

          /// <summary>
        /// Load the information from the table <c>FrontEndEditable</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>FrontEndEditable</c></returns>
        public override List<FrontEndEditable> GetAll()
        {
            List<FrontEndEditable> col = new List<FrontEndEditable>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new FrontEndEditable(this.Session.Reader));
            }

            return col;
        }
    }
}