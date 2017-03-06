// <copyright file="Listanews.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business.FrontEnd
{
    using System.Collections.Generic;
    using System.Web;
    using Domain.Abstract;
    using Domain.Concrete;
    using Domain.Entities;

    /// <summary>
    /// Represents the model for the front end
    /// </summary>
    public class Listanews : IFrontEnd
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Listanews"/> class
        /// </summary>
        public Listanews()
        {
        }
        
        /// <summary>
        /// Gets or sets a list of content
        /// </summary>
        public IEnumerable<Contentnew> Contents { get; set; }

        /// <summary>
        /// fills the object's fields
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="session">framework that establishes communication between the application and the database</param>
        /// <param name="id">identifier of section</param>
        /// <param name="userId">current user ID</param>
        public void Bind(HttpContextBase context, ISession session, int? id, int? userId,int? LanguageId )
        {
            ContentRepository content = new ContentRepository(session);

            content.Entity.SectionId = id;

            this.Contents = content.GetContentNewsBySection();
        }
    }
}
