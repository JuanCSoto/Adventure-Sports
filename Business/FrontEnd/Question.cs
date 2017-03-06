// <copyright file="Question.cs" company="Dasigno">
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
    public class Question : IFrontEnd
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Question"/> class
        /// </summary>
        public Question()
        {
        }

        /// <summary>
        /// Gets or sets the ideas collection
        /// </summary>
        public List<IdeasPaging> CollIdeas { get; set; }

        /// <summary>
        /// Gets or sets the answers collection
        /// </summary>
        public List<Answer> CollAnswers { get; set; }

        /// <summary>
        /// Gets or sets the total ideas count
        /// </summary>
        public int IdeasCount { get; set; }

        /// <summary>
        /// Gets or sets the total comments count
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        /// Gets or sets the ideas of the content
        /// </summary>
        public List<Domain.Entities.FrontEnd.IdeasPaging> TopIdeas { get; set; }

        /// <summary>
        /// Gets or sets the blog entries of the content
        /// </summary>
        public List<Domain.Entities.FrontEnd.BlogEntriesPaging> BlogEntries { get; set; }

        /// <summary>
        /// Gets or sets the total blog entries count
        /// </summary>
        public int BlogEntriesCount { get; set; }

        /// <summary>
        /// Gets or sets the Question object
        /// </summary>
        public Domain.Entities.Question ObjQuestion { get; set; }

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
        /// Gets or sets the statistics collection
        /// </summary>
        public Dictionary<string, int> Statistics { get; set; }

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
                QuestionRepository questionrepository = new QuestionRepository(session);
                FileattachRepository file = new FileattachRepository(session);

                contentrepository.Entity.ContentId = id;
                contentrepository.LoadByKey();

                if (contentrepository.Entity.Frienlyname != null)
                {
                    file.Entity.ContentId =
                    questionrepository.Entity.ContentId =
                        contentrepository.Entity.ContentId;

                    questionrepository.LoadByKey();

                    this.ObjContent = contentrepository.Entity;
                    this.ObjQuestion = questionrepository.Entity;
                    this.CollFiles = file.GetAll();
                }

                contentrepository.Entity = new Content();
                contentrepository.Entity.ContentId = this.ObjContent.ContentId;
                contentrepository.Entity.SectionId = this.ObjContent.SectionId;

                if (id.HasValue)
                {
                    int totalCount = 0;
                    AnswerRepository answer = new AnswerRepository(session);
                    IdeaRepository idea = new IdeaRepository(session);
                    CommentRepository comment = new CommentRepository(session);
                    BlogEntryRepository blog = new BlogEntryRepository(session);
                    idea.Entity.ContentId = 
                        blog.Entity.ContentId = 
                        answer.Entity.ContentId = id.Value;
                    this.CollAnswers = answer.GetAll();

                    this.CollIdeas = idea.IdeasPaging(1, 6, out totalCount, id.Value, userId);
                    this.IdeasCount = totalCount;
                    foreach (IdeasPaging item in this.CollIdeas)
                    {
                        item.CollComment = comment.CommentsPaging(1, 3, out totalCount, item.IdeaId.Value);
                        this.CommentsCount = totalCount;
                    }

                    this.BlogEntries = blog.BlogContentEntriesPaging(1, 6, out totalCount);
                    this.BlogEntriesCount = totalCount;
                    foreach (BlogEntriesPaging blogEntry in this.BlogEntries)
                    {
                        blogEntry.CollComment = comment.CommentsPagingContent(1, 3, out totalCount, blogEntry.ContentId.Value);
                        this.CommentsCount = totalCount;
                    }

                    this.TopIdeas = idea.TopIdeas(10);
                    this.Statistics = contentrepository.GetContentStatistics(id.Value);
                }
            }
            
            this.CollContent = contentrepository.GetNewsRelationFrontEnd();            
        }
    }
}
