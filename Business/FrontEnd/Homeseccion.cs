// <copyright file="Homeseccion.cs" company="Dasigno">
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
    public class Homeseccion : IFrontEnd
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Homeseccion"/> class
        /// </summary>
        public Homeseccion()
        {
        }
        
        /// <summary>
        /// Gets or sets a news object
        /// </summary>
        public News News { get; set; }

        /// <summary>
        /// Gets or sets a content object
        /// </summary>
        public Content Content { get; set; }

        /// <summary>
        /// Gets or sets a list of files
        /// </summary>
        public List<Fileattach> Files { get; set; }
        
        /// <summary>
        /// fills the object's fields
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="session">framework that establishes communication between the application and the database</param>
        /// <param name="id">identifier of section</param>
        /// <param name="userId">current user ID</param>
        public void Bind(HttpContextBase context, ISession session, int? id, int? userId,int? languageId)
        {
            ContentRepository contentrepository = new ContentRepository(session);
            NewsRepository newsrepository = new NewsRepository(session);
            FileattachRepository file = new FileattachRepository(session);

            contentrepository.Entity.Private = true;
            contentrepository.Entity.SectionId = id;
            contentrepository.GetContentBySection();

            if (contentrepository.Entity.Frienlyname != null)
            {
                file.Entity.ContentId =
                newsrepository.Entity.ContentId = 
                    contentrepository.Entity.ContentId;

                newsrepository.LoadByKey();

                this.Content = contentrepository.Entity;
                this.News = newsrepository.Entity;
                this.Files = file.GetAll();
            }
        }
    }
}
