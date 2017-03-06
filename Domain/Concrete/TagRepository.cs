// <copyright file="TagRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the table <c>Tag</c>
    /// </summary>
    public sealed class TagRepository : DataRepository<Tag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public TagRepository(ISession session)
            : base(session, "GXTag")
        {
            this.Entity = new Tag();
        }

        /// <summary>
        /// Load the information from the table <c>Tag</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Tag(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Tag</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Tag(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Tag</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Tag</c></returns>
        public override List<Tag> GetAll()
        {
            List<Tag> col = new List<Tag>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new Tag(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// obtains a list of tags according criteria search
        /// </summary>
        /// <param name="name">criteria search</param>
        /// <param name="paginInfo"><c>PaginInfo</c> object</param>
        /// <param name="languageId">identifier of language</param>
        /// <returns>returns a list of tags</returns>
        public List<Tag> GetAllPaging(string name, PaginInfo paginInfo, int languageId)
        {
            List<Tag> col = new List<Tag>();
            this.Session.CreateCommand("CTTagpagin", true);
            this.Session.AddParameter("@LanguageId", languageId, DbType.Int32);
            this.Session.AddParameter("@Name", name, DbType.String);
            this.Session.AddParameter("@PageSize", paginInfo.Size, DbType.Int32);
            this.Session.AddParameter("@PageIndex", paginInfo.ActualRow, DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, ParameterDirection.InputOutput, DbType.Int32);

            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Tag(this.Session.Reader));
            }

            paginInfo.TotalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// obtains a list of <c>KeyValue</c> objects
        /// </summary>
        /// <returns>list of <c>KeyValue</c> objects</returns>
        public List<KeyValue> GetTags()
        {
            List<KeyValue> coll = new List<KeyValue>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                coll.Add(new KeyValue(this.Session.GetValue(0).ToString(), this.Session.GetValue(1).ToString()));
            }

            return coll;
        }

        /// <summary>
        /// obtains a list of tags according to content
        /// </summary>
        /// <param name="contentId">identifier of content</param>
        /// <returns>returns a list of tags</returns>
        public IEnumerable<Tag> GetTagbycontent(int contentId)
        {
            this.Session.CreateCommand("CTTagbycontent", true);
            this.Session.AddParameter("@ContentId", contentId, DbType.Int32);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                yield return new Tag(this.Session.Reader);
            }
        }
    }
}