// <copyright file="AboutUs.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Business.FrontEnd
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Domain.Abstract;
    using Domain.Concrete;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;    

    /// <summary>
    /// Represents the model for the front end
    /// </summary>
    public class AboutUs : IFrontEnd
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutUs"/> class
        /// </summary>
        public AboutUs()
        {
        }

        /// <summary>
        /// Gets or sets the blog entries collection
        /// </summary>
        public List<BlogEntriesPaging> CollBlogEntries { get; set; }

        /// <summary>
        /// Gets or sets the total number of entries
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of comments
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        /// Bind the context and the session with the content
        /// </summary>
        /// <param name="context">Context page</param>
        /// <param name="session">Session object</param>
        /// <param name="id">Content ID</param>
        /// <param name="userId">current user ID</param>
        public void Bind(HttpContextBase context, ISession session, int? id, int? userId, int? LanguageId)
        {
            int total = 0;
            BlogEntryRepository blogEntryRepository = new BlogEntryRepository(session);
            FileattachRepository fileRepository = new FileattachRepository(session);
            CommentRepository comment = new CommentRepository(session);

            this.CollBlogEntries = blogEntryRepository.BlogEntriesPaging(1, 6, out total, true, LanguageId);
            this.TotalCount = total;
            int totalCount = 0;
            foreach (BlogEntriesPaging blogEntry in this.CollBlogEntries)
            {
                blogEntry.CollComment = comment.CommentsPagingContent(1, 3, out totalCount, blogEntry.ContentId.Value);
                if (blogEntry.CollComment.Count > 0)
                {
                    blogEntry.CollComment[0].CommentContentOwnerId = blogEntry.UserId.Value;
                }

                fileRepository.Entity.ContentId = blogEntry.ContentId;
                Fileattach file = fileRepository.GetAll().FirstOrDefault(t => t.Type == Domain.Entities.Fileattach.TypeFile.Video);
                if (file != null)
                {
                    blogEntry.Video = file.Filename;
                }

                this.CommentsCount = totalCount;
            }
        }
    }
}
