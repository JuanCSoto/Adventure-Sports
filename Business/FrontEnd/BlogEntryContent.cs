// <copyright file="BlogEntryContent.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Business.FrontEnd
{
    using System.Collections.Generic;
    using System.Web;
    using System.Xml;
    using Domain.Abstract;
    using Domain.Concrete;    
    using Domain.Entities;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Represents the model for the front end
    /// </summary>
    public class BlogEntryContent : IFrontEnd
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogEntryContent"/> class
        /// </summary>
        public BlogEntryContent()
        {
        }

        /// <summary>
        /// Gets or sets the comments collection
        /// </summary>
        public List<CommentsPaging> CollComments { get; set; }

        /// <summary>
        /// Gets or sets the BlogEntry object
        /// </summary>
        public Domain.Entities.BlogEntry ObjBlogEntry { get; set; }

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
        /// Bind the context and the session with the content
        /// </summary>
        /// <param name="context">Context page</param>
        /// <param name="session">Session object</param>
        /// <param name="id">Content ID</param>
        /// <param name="userId">current user ID</param>
        public void Bind(HttpContextBase context, ISession session, int? id, int? userId, int? LanguageId)
        {
            ContentRepository contentrepository = new ContentRepository(session);            

            if (contentrepository.Entity != null)
            {
                BlogEntryRepository blogentryrepository = new BlogEntryRepository(session);
                FileattachRepository file = new FileattachRepository(session);
                CommentRepository comment = new CommentRepository(session);

                contentrepository.Entity.ContentId = id;
                contentrepository.LoadByKey();

                if (contentrepository.Entity.Frienlyname != null)
                {
                    file.Entity.ContentId =
                    blogentryrepository.Entity.ContentId =
                        contentrepository.Entity.ContentId;

                    blogentryrepository.LoadByKey();

                    this.ObjContent = contentrepository.Entity;
                    this.ObjBlogEntry = blogentryrepository.Entity;
                    this.CollFiles = file.GetAll();
                }

                contentrepository.Entity = new Content();
                contentrepository.Entity.ContentId = this.ObjContent.ContentId;
                contentrepository.Entity.SectionId = this.ObjContent.SectionId;

                if (id.HasValue)
                {
                    CommentRepository commentRepository = new CommentRepository(session);
                    int totalCount;
                    this.CollComments = commentRepository.CommentsPagingContent(1, 3, out totalCount, id.Value);
                }
            }
            
            this.CollContent = contentrepository.GetNewsRelationFrontEnd();
        }
    }
}
