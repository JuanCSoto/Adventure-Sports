// <copyright file="BlogEntryRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Domain.Concrete
{
    using System;
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Class responsible for interaction with the table <c>BlogEntry</c>
    /// </summary>
    public sealed class BlogEntryRepository : DataRepository<BlogEntry>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogEntryRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public BlogEntryRepository(ISession session)
            : base(session, "GXBlogEntry")
        {
            this.Entity = new BlogEntry();
        }

        /// <summary>
        /// Load the information from the table <c>BlogEntry</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new BlogEntry(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>BlogEntry</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new BlogEntry(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>BlogEntry</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>BlogEntry</c></returns>
        public override List<BlogEntry> GetAll()
        {
            List<BlogEntry> col = new List<BlogEntry>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new BlogEntry(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>BlogEntry</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="active">To return active or inactive content</param>
        /// <returns>A list of entities according to the parameters send</returns>
        public List<BlogEntriesPaging> BlogEntriesPaging(int pageIndex, int pageSize, out int totalCount, bool active, int? LanguageId)
        {
            totalCount = 0;
            List<BlogEntriesPaging> col = new List<BlogEntriesPaging>();
            this.Session.CreateCommand("CTBlogEntriesPaging", true);
            this.Session.AddParameter("@TotalCount", 0, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@Active", active, System.Data.ParameterDirection.Input, System.Data.DbType.Boolean);
            this.Session.AddParameter("@LanguageId", LanguageId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new BlogEntriesPaging(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>BlogEntry</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="contentId">content id</param>
        /// <returns>A list of entities according to the parameters send</returns>
        public List<BlogEntriesPaging> BlogEntriesById(int contentId)
        {
            List<BlogEntriesPaging> col = new List<BlogEntriesPaging>();
            this.Session.CreateCommand("CTBlogEntriesById", true);
            this.Session.AddParameter("@ContentId", contentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);                        
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new BlogEntriesPaging(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>BlogEntry</c> to the list of entities
        /// </summary>
        /// <returns>A list of entries</returns>
        public List<ArchiveEntry> ArchiveEntries(int? LanguageId)
        {
            List<ArchiveEntry> col = new List<ArchiveEntry>();
            this.Session.CreateCommand("CTBlogArchiveEntries", true);
            this.Session.AddParameter("@LanguageId", LanguageId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);   
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new ArchiveEntry(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>BlogEntry</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <returns>A list of entities according to the parameters send</returns>
        public List<BlogEntriesPaging> BlogContentEntriesPaging(int pageIndex, int pageSize, out int totalCount)
        {
            totalCount = 0;
            List<BlogEntriesPaging> col = new List<BlogEntriesPaging>();
            this.Session.CreateCommand("CTBlogContentEntriesPaging", true);
            this.Session.AddParameter("@TotalCount", 0, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@ContentId", Entity.ContentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new BlogEntriesPaging(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }
    }
}