// <copyright file="CommentRepository.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System;
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Class responsible for interaction with the board <c>Comment</c>
    /// </summary>
    public sealed class CommentRepository : DataRepository<Comment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public CommentRepository(ISession session)
            : base(session, "GXComment")
        {
            this.Entity = new Comment();
        }

        /// <summary>
        /// Load the information from the table <c>Comment</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Comment(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Comment</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Comment(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Comment</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Comment</c></returns>
        public override List<Comment> GetAll()
        {
            List<Comment> col = new List<Comment>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Comment(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Comment</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="ideaId">idea id</param>
        /// <returns>A list of entities according to the parameters send</returns>
        public List<CommentsPaging> CommentsPaging(int pageIndex, int pageSize, out int totalCount, int ideaId)
        {
            List<CommentsPaging> col = new List<CommentsPaging>();
            this.Session.CreateCommand("CTCommentsPaging", true);
            this.Session.AddParameter("@IdeaId", ideaId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@TotalCount", 0, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new CommentsPaging(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Comment</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="commentId">comment id</param>
        /// <param name="ideaId">idea id</param>
        /// <param name="contentId">content id</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <returns>A list of entities according to the parameters send</returns>
        public List<CommentsPaging> CommentsPagingById(int commentId, int? ideaId, int? contentId, out int totalCount)
        {
            List<CommentsPaging> col = new List<CommentsPaging>();
            this.Session.CreateCommand("CTCommentsPagingById", true);
            this.Session.AddParameter("@CommentId", commentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@IdeaId", ideaId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@ContentId", contentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@TotalCount", 0, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new CommentsPaging(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Comment</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="contentId">content id</param>
        /// <returns>A list of entities according to the parameters send</returns>
        public List<CommentsPaging> CommentsPagingContent(int pageIndex, int pageSize, out int totalCount, int contentId)
        {
            List<CommentsPaging> col = new List<CommentsPaging>();
            this.Session.CreateCommand("CTCommentsPagingContent", true);
            this.Session.AddParameter("@ContentId", contentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@TotalCount", 0, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new CommentsPaging(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));

            return col;
        }
    }
}