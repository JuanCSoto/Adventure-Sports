// <copyright file="Articulo.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business.FrontEnd
{
    using System.Collections.Generic;
    using System.Web;
    using System.Xml;
    using Domain.Abstract;
    using Domain.Concrete;
    using Domain.Entities;
    
    /// <summary>
    /// Represents the model for the front end
    /// </summary>
    public class Articulo : IFrontEnd
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Articulo"/> class
        /// </summary>
        public Articulo()
        {
        }

        /// <summary>
        /// Gets or sets the News object
        /// </summary>
        public News ObjNews { get; set; }

        /// <summary>
        /// Gets or sets the object content
        /// </summary>
        public Content ObjContent { get; set; }

        /// <summary>
        /// Gets or sets a list of contents
        /// </summary>
        public IEnumerable<Content> CollContent { get; set; }

        /// <summary>
        /// Gets or sets a list of files
        /// </summary>
        public List<Fileattach> CollFiles { get; set; }

        /// <summary>
        /// Gets or sets the Xml information
        /// </summary>
        public XmlDocument XmlInformation { get; set; }

        /// <summary>
        /// Bind the context and the session with the content
        /// </summary>
        /// <param name="context">Context page</param>
        /// <param name="session">Session object</param>
        /// <param name="id">Content ID</param>
        /// <param name="userId">current user ID</param>
        public void Bind(HttpContextBase context, ISession session, int? id, int? userId, int? LanguageId)
        {
            ContentRepository contentrepository = new ContentRepository(session);
            NewsRepository newsrepository = new NewsRepository(session);
            FileattachRepository file = new FileattachRepository(session);

            if (contentrepository.Entity != null)
            {
                contentrepository.Entity.ContentId = id;
                contentrepository.LoadByKey();

                if (contentrepository.Entity.Frienlyname != null)
                {
                    file.Entity.ContentId =
                    newsrepository.Entity.ContentId =
                        contentrepository.Entity.ContentId;

                    newsrepository.LoadByKey();

                    this.ObjContent = contentrepository.Entity;
                    this.ObjNews = newsrepository.Entity;
                    this.CollFiles = file.GetAll();
                }

                contentrepository.Entity = new Content();
                contentrepository.Entity.ContentId = this.ObjContent.ContentId;
                contentrepository.Entity.SectionId = this.ObjContent.SectionId;
            }

            this.CollContent = contentrepository.GetNewsRelationFrontEnd();
        }
    }
}
