// <copyright file="DocumentTypeRepository.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>DocumentType</c>
    /// </summary>
    public sealed class DocumentTypeRepository : DataRepository<DocumentType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public DocumentTypeRepository(ISession session)
            : base(session, "GXDocumentType")
        {
            this.Entity = new DocumentType();
        }

        /// <summary>
        /// Load the information from the table <c>DocumentType</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new DocumentType(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>DocumentType</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new DocumentType(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>DocumentType</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>DocumentType</c></returns>
        public override List<DocumentType> GetAll()
        {
            List<DocumentType> col = new List<DocumentType>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new DocumentType(this.Session.Reader));
            }

            return col;
        }
    }
}