// <copyright file="ContentRepository.cs" company="Dasigno">
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
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Class responsible for interaction with the table Content
    /// </summary>
    public sealed class ContentRepository : DataRepository<Content>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public ContentRepository(ISession session)
            : base(session, "GXContent")
        {
            this.Entity = new Content();
        }

        /// <summary>
        /// Load the information from the table <c>Content</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Content(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Content</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Content(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Content</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Content</c></returns>
        public override List<Content> GetAll()
        {
            List<Content> col = new List<Content>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new Content(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// obtains a list of content according to parameters
        /// </summary>
        /// <param name="filter">search filter</param>
        /// <param name="paginInfo"><c>PaginInfo</c> object</param>
        /// <returns>returns a list of content</returns>
        public IEnumerable<Content> GetAllPaging(short? filter, PaginInfo paginInfo)
        {
            List<Content> col = new List<Content>();
            this.Session.CreateCommand("CTContentgeneralpagin", true);
            this.Session.AddParameter("@Name", this.Entity.Name, DbType.String);
            this.Session.AddParameter("@Active", this.Entity.Active, DbType.Boolean);
            this.Session.AddParameter("@SectionId", this.Entity.SectionId, DbType.Int32);
            this.Session.AddParameter("@LanguageId", this.Entity.LanguageId, DbType.Int32);
            this.Session.AddParameter("@ModulId", this.Entity.ModulId, DbType.Int32);
            this.Session.AddParameter("@Filter", filter, DbType.Int32);
            this.Session.AddParameter("@PageSize", paginInfo.Size, DbType.Int32);
            this.Session.AddParameter("@PageIndex", paginInfo.ActualRow, DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, ParameterDirection.InputOutput, DbType.Int32);

            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Content(this.Session.Reader));
            }

            paginInfo.TotalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// get a list of unrelated content
        /// </summary>
        /// <param name="paginInfo"><c>PaginInfo</c> object</param>
        /// <param name="languageId">identifier of language</param>
        /// <returns>returns a list of content</returns>
        public IEnumerable<ContentRel> GetContentNoRelation(PaginInfo paginInfo, int languageId)
        {
            List<ContentRel> col = new List<ContentRel>();
            this.Session.CreateCommand("CTContentrelationselect", true);
            this.Session.AddParameter("@ContentId", this.Entity.ContentId, DbType.Int32);
            this.Session.AddParameter("@Name", this.Entity.Name, DbType.String);
            this.Session.AddParameter("@LanguageId", languageId, DbType.Int32);
            this.Session.AddParameter("@PageSize", paginInfo.Size, DbType.Int32);
            this.Session.AddParameter("@PageIndex", paginInfo.ActualRow, DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, ParameterDirection.InputOutput, DbType.Int32);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new ContentRel(this.Session.Reader));
            }

            paginInfo.TotalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// obtains a list of <c>ContentRel</c> object according to language
        /// </summary>
        /// <param name="languageId">identifier of language</param>
        /// <returns>returns a list of <c>ContentRel</c></returns>
        public IEnumerable<ContentRel> GetContentRelation(int languageId)
        {
            List<ContentRel> col = new List<ContentRel>();
            this.Session.CreateCommand("CTContentrelationselect", true);
            this.Session.AddParameter("@ContentId", this.Entity.ContentId, DbType.Int32);
            this.Session.AddParameter("@LanguageId", languageId, DbType.Int32);
            this.Session.AddParameter("@Action", 1, DbType.Int32);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new ContentRel(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// obtains a content relation to a content
        /// </summary>
        /// <returns>returns a list of content</returns>
        public IEnumerable<Content> GetContentRelationFrontEnd()
        {
            this.Session.CreateCommand("CTContentrelation", true);
            this.Session.AddParameter("@ContentId", this.Entity.ContentId, DbType.Int32);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                yield return new Content(this.Session.Reader);
            }
        }

        /// <summary>
        /// obtains a list of content to section
        /// </summary>
        /// <returns>returns a list of content</returns>
        public IEnumerable<Content> GetNewsRelationFrontEnd()
        {
            this.Session.CreateCommand("CTNewsrelationselect", true);
            this.Session.AddParameter("@ContentId", this.Entity.ContentId, DbType.Int32);
            this.Session.AddParameter("@SectionId", this.Entity.SectionId, DbType.Int32);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                yield return new Content(this.Session.Reader);
            }
        }

        /// <summary>
        /// obtains a max order to the content table
        /// </summary>
        /// <returns>returns a max order</returns>
        public int GetMaxOrder()
        {
            this.Session.CreateCommand("CTContentmaxorder", true);
            object result = this.Session.ExecuteScalar();
            return result != DBNull.Value ? Convert.ToInt32(result) : 1;
        }

        /// <summary>
        /// change the order to the contents
        /// </summary>
        /// <param name="contentId">identifier of content</param>
        /// <param name="previousId">identifier of previous content</param>
        /// <param name="limit">limit of content</param>
        public void ChangeOrder(int contentId, int previousId, bool limit)
        {
            if (limit)
            {
                this.Session.CreateCommand("CTContentupdateorderlimit", true);
            }
            else
            {
                this.Session.CreateCommand("CTContentupdateorder", true);
            }

            this.Session.AddParameter("@ContentId", contentId, DbType.Int32);
            this.Session.AddParameter("@PrevId", previousId, DbType.Int32);
            this.Session.ExecuteNonQuery();
        }

        /// <summary>
        /// obtains a list of <c>Search</c> object according to criteria search
        /// </summary>
        /// <param name="key">criteria search</param>
        /// <param name="languageId">identifier of language</param>
        /// <returns>returns a list of <c>Search</c> object</returns>
        public IEnumerable<Search> GetSearchGeneral(string key, int languageId)
        {
            this.Session.CreateCommand("CTSearchgeneral", true);
            this.Session.AddParameter("@cont", key, DbType.String);
            this.Session.AddParameter("@LanguageId", languageId, DbType.Int32);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                yield return new Search(this.Session.Reader);
            }
        }

        /// <summary>
        /// obtains a content according to section
        /// </summary>
        public void GetContentBySection()
        {
            this.Session.CreateCommand("CTContentbysection", true);
            this.Session.AddParameter("@SectionId", this.Entity.SectionId, DbType.Int32);
            this.Session.AddParameter("@Private", this.Entity.Private, DbType.Boolean);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                this.Entity = new Content(this.Session.Reader);
            }
        }

        /// <summary>
        /// obtains a list of content according to section
        /// </summary>
        /// <returns>returns a list of content</returns>
        public List<Content> GetContentsBySection()
        {
            List<Content> coll = new List<Content>();

            this.Session.CreateCommand("CTContentsbysection", true);
            this.Session.AddParameter("@SectionId", this.Entity.SectionId, DbType.Int32);
            this.Session.AddParameter("@Private", this.Entity.Private, DbType.Boolean);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                coll.Add(new Content(this.Session.Reader));
            }

            return coll;
        }

        /// <summary>
        /// obtains a top list of content according to section
        /// </summary>
        /// <param name="top">top of content list</param>
        /// <returns>returns a list of content</returns>
        public List<Content> GetContentTopBySection(int top)
        {
            List<Content> coll = new List<Content>();

            this.Session.CreateCommand("CTContentselecttop", true);
            this.Session.AddParameter("@SectionId", this.Entity.SectionId, DbType.Int32);
            this.Session.AddParameter("@Top", top, DbType.Int32);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                coll.Add(new Content(this.Session.Reader));
            }

            return coll;
        }

        /// <summary>
        /// obtains a list of <c>Contentnew</c> object according to section
        /// </summary>
        /// <returns>returns a list of <c>Contentnew</c> object</returns>
        public List<Contentnew> GetContentNewsBySection()
        {
            List<Contentnew> coll = new List<Contentnew>();

            this.Session.CreateCommand("CTContentNewsbysection", true);
            this.Session.AddParameter("@SectionId", this.Entity.SectionId, DbType.Int32);
            this.Session.AddParameter("@Private", this.Entity.Private, DbType.Boolean);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                coll.Add(new Contentnew(this.Session.Reader));
            }

            return coll;
        }

        /// <summary>
        /// obtains a list of content according to the section
        /// </summary>
        /// <param name="paginInfo"><c>PaginInfo</c> object</param>
        /// <param name="action">filter to select</param>
        /// <returns>returns a list of content</returns>
        public List<Content> GetNewsHome(PaginInfo paginInfo, short action)
        {
            List<Content> coll = new List<Content>();

            this.Session.CreateCommand("CTContentNewshome", true);
            this.Session.AddParameter("@SectionId", this.Entity.SectionId, DbType.Int32);
            this.Session.AddParameter("@ContentId", this.Entity.ContentId, DbType.Int32);
            this.Session.AddParameter("@PageSize", paginInfo.Size, DbType.Int32);
            this.Session.AddParameter("@PageIndex", paginInfo.ActualRow, DbType.Int32);
            this.Session.AddParameter("@Action", action, DbType.Int16);
            this.Session.AddParameter("@TotalCount", null, ParameterDirection.InputOutput, DbType.Int32);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                coll.Add(new Content(this.Session.Reader));
            }

            paginInfo.TotalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));

            return coll;
        }

        /// <summary>
        /// obtains a las content to the table according to date join
        /// </summary>
        /// <returns>returns a list of content</returns>
        public Content GetContentSectionLast()
        {
            Content content = null;
            this.Session.CreateCommand("CTContentselectlast", true);
            this.Session.AddParameter("@SectionId", this.Entity.SectionId, DbType.Int32);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                content = new Content(this.Session.Reader);
            }

            return content;
        }

        /// <summary>
        /// Load the information from the table <c>Content</c> to the list of entities
        /// </summary>
        /// <returns>A list of entities</returns>
        public List<FeaturedChallengesQuestions> FeaturedChallengesQuestions(int? LanguageId)
        {
            List<FeaturedChallengesQuestions> coll = new List<FeaturedChallengesQuestions>();

            this.Session.CreateCommand("CTFeaturedChallengesQuestions", true);
            this.Session.AddParameter("@LanguageId", LanguageId, ParameterDirection.Input, System.Data.DbType.Int32);

            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                coll.Add(new FeaturedChallengesQuestions(this.Session.Reader));
            }

            return coll;
        }

        /// <summary>
        /// Load the information from the table <c>Content</c> to the list of entities
        /// </summary>
        /// <returns>A list of entities</returns>
        public List<FeaturedChallengesQuestions> FinishedChallengesQuestions(int? LanguageId)
        {
            List<FeaturedChallengesQuestions> coll = new List<FeaturedChallengesQuestions>();

            this.Session.CreateCommand("CTFinishedChallengesQuestions", true);
            this.Session.AddParameter("@LanguageId", LanguageId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);

            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                coll.Add(new FeaturedChallengesQuestions(this.Session.Reader));
            }

            return coll;
        }

        /// <summary>
        /// Load the information from the table <c>Content</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="text">search text</param>
        /// <param name="languageId">Language id.</param>
        /// <param name="categoryId">Category id.</param>
        /// <returns>A list of entities according to the parameters send</returns>
        public List<Pulse> Pulses(int pageIndex, int pageSize, out int totalCount, string text, int? languageId, int categoryId)
        {
            List<Pulse> coll = new List<Pulse>();

            this.Session.CreateCommand("CTPulses", true);
            this.Session.AddParameter("@TotalCount", null, ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@Text", text, ParameterDirection.Input, System.Data.DbType.String);
            this.Session.AddParameter("@LanguageId", languageId, ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@CategoryId", categoryId, ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                coll.Add(new Pulse(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));

            return coll;
        }

        /// <summary>
        /// Load the information from the table <c>Content</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="text">search text</param>
        /// <returns>A list of entities according to the parameters send</returns>
        public List<Pulse> Pulses(int pageIndex, int pageSize, out int totalCount, string text, int? languageId)
        {
            List<Pulse> coll = new List<Pulse>();

            this.Session.CreateCommand("CTPulses", true);
            this.Session.AddParameter("@TotalCount", null, ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@Text", text, ParameterDirection.Input, System.Data.DbType.String);
            this.Session.AddParameter("@LanguageId", languageId, ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                coll.Add(new Pulse(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));

            return coll;
        }

        /// <summary>
        /// Load the information from the table <c>Content</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <returns>A list of entities according to the parameters send</returns>
        public List<Pulse> PulsesWidget(int pageIndex, int pageSize, out int totalCount)
        {
            List<Pulse> coll = new List<Pulse>();

            this.Session.CreateCommand("CTPulsesWidget", true);
            this.Session.AddParameter("@TotalCount", null, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                coll.Add(new Pulse(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));

            return coll;
        }

        /// <summary>
        /// Load the information from the table <c>Content</c>
        /// </summary>
        /// <returns>A list of categories</returns>
        public List<Category> Categories()
        {
            List<Category> coll = new List<Category>();

            this.Session.CreateCommand("CTContentCategories", true);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                coll.Add(new Category(this.Session.Reader));
            }

            return coll;
        }

        /// <summary>
        /// Check the content id related to an answer id
        /// </summary>
        /// <param name="answerId">answer id</param>
        /// <returns>object with the content id</returns>
        public object GetContentIdByAnswerId(int answerId)
        {
            List<User> col = new List<User>();
            this.Session.CreateCommand("CTGetContentIdByAnswerId", true);
            this.Session.AddParameter("@AnswerId", answerId, DbType.Int32);

            return this.Session.ExecuteScalar();
        }

        /// <summary>
        /// Return a dictionary with the content statistics
        /// </summary>
        /// <param name="contentId">content id to check</param>
        /// <returns>A dictionary with the statistics information of the content</returns>
        public Dictionary<string, int> GetContentStatistics(int contentId)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            this.Session.CreateCommand("CTGetContentStatistics", true);
            this.Session.AddParameter("@ContentId", contentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);

            int count = 0;
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                if (this.Session.Reader["Name"] != DBNull.Value && this.Session.Reader["Count"] != DBNull.Value && int.TryParse(this.Session.Reader["Count"].ToString(), out count))
                {
                    result.Add(this.Session.Reader["Name"].ToString(), count);
                }
            }

            return result;
        }

        /// <summary>
        /// Return a report of the pulses information
        /// </summary>
        /// <returns>a table with a report of the pulses information</returns>
        public DataTable ReportPulses()
        {
            this.Session.CreateCommand("CTReportPulses", true);
            return this.Session.GetTable();
        }

        /// <summary>
        /// Return a report of the pulse information
        /// </summary>
        /// <param name="id">the pulse id</param>
        /// <returns>a report of the pulse information</returns>
        public DataSet PulseReportDetail(string id)
        {
            this.Session.CreateCommand("CTPulseReportDetail", true);
            this.Session.AddParameter("@ContentId", id, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            return this.Session.GetDataSet();
        }
    }
}