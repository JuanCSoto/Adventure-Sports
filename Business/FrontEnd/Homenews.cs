// <copyright file="Homenews.cs" company="Dasigno">
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
    public class Homenews : IFrontEnd
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Homenews"/> class
        /// </summary>
        public Homenews()
        {
        }
        
        /// <summary>
        /// Gets or sets a list of contents
        /// </summary>
        public IEnumerable<Content> Contents { get; set; }

        /// <summary>
        /// Gets or sets the last content
        /// </summary>
        public Content Contentlast { get; set; }

        /// <summary>
        /// Gets or sets a pager of a list
        /// </summary>
        public PaginInfo Paginator { get; set; }

        /// <summary>
        /// fills the object's fields
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="session">framework that establishes communication between the application and the database</param>
        /// <param name="id">identifier of section</param>
        /// <param name="userId">current user ID</param>
        public void Bind(HttpContextBase context, ISession session, int? id, int? userId,int? LanguageId)
        {
            ContentRepository content = new ContentRepository(session);
            this.Paginator = new PaginInfo() { PageIndex = 1, Size = 10 };

            content.Entity.SectionId = id;

            this.Contentlast = content.GetContentSectionLast();

            if (this.Contentlast != null)
            {
                content.Entity.ContentId = this.Contentlast.ContentId;
                this.Contents = content.GetNewsHome(this.Paginator, 1);
            }
        }
    }
}
