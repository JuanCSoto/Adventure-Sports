// <copyright file="IdeaRepository.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Domain.Abstract;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;
    using Microsoft.SqlServer.Server;

    /// <summary>
    /// Class responsible for interaction with the board <c>Idea</c>
    /// </summary>
    public sealed class IdeaRepository : DataRepository<Idea>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdeaRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public IdeaRepository(ISession session)
            : base(session, "GXIdea")
        {
            this.Entity = new Idea();
        }

        /// <summary>
        /// Load the information from the table <c>Idea</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Idea(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Idea</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Idea(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Idea</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Idea</c></returns>
        public override List<Idea> GetAll()
        {
            List<Idea> col = new List<Idea>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Idea(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Idea</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="ideaId">idea id</param>
        /// <param name="userId">user id</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public IdeasPaging IdeaPagingById(int ideaId, int? userId)
        {
            IdeasPaging idea = new IdeasPaging();
            this.Session.CreateCommand("CTIdeaPagingById", true);
            this.Session.AddParameter("@IdeaId", ideaId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                idea = new IdeasPaging(this.Session.Reader);
            }

            return idea;
        }

        /// <summary>
        /// Load the information from the table <c>Idea</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="contentId">content id</param>
        /// <param name="userId">user id</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public List<IdeasPaging> IdeasPaging(int pageIndex, int pageSize, out int totalCount, int contentId, int? userId)
        {
            List<IdeasPaging> col = new List<IdeasPaging>();
            this.Session.CreateCommand("CTIdeasPaging", true);
            this.Session.AddParameter("@ContentId", contentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new IdeasPaging(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Idea</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="contentId">content id</param>
        /// <param name="userId">user id</param>
        /// <param name="text">search text</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public List<IdeasPaging> Ideas(int pageIndex, int pageSize, out int totalCount, int? contentId, int? userId, string text)
        {
            List<IdeasPaging> col = new List<IdeasPaging>();
            this.Session.CreateCommand("CTIdeas", true);
            this.Session.AddParameter("@ContentId", contentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@Text", text, System.Data.ParameterDirection.Input, System.Data.DbType.String);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new IdeasPaging(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Idea</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="contentId">content id</param>
        /// <param name="userId">user id</param>
        /// <param name="ideasId">ideas id to not be returned in the random result</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public List<IdeasPaging> IdeasPagingRandom(int pageSize, out int totalCount, int contentId, int? userId, int[] ideasId)
        {
            this.Session.CreateCommand("CTIdeasPagingRandom", true);
            List<IdeasPaging> col = new List<IdeasPaging>();
            List<SqlDataRecord> listItemsID = new List<SqlDataRecord>();
            SqlMetaData[] tvp_definition = { new SqlMetaData("IdeasId", SqlDbType.Int) };
            SqlParameter parameter = new SqlParameter("@IdeasId", SqlDbType.Structured);
            parameter.Direction = ParameterDirection.Input;
            parameter.TypeName = "int_list";
            if (ideasId != null && ideasId.Length > 0)
            {
                foreach (int id in ideasId)
                {
                    SqlDataRecord rec = new SqlDataRecord(tvp_definition);
                    rec.SetInt32(0, id);
                    listItemsID.Add(rec);
                }
            }
            else
            {
                SqlDataRecord rec = new SqlDataRecord(tvp_definition);
                rec.SetInt32(0, 0);
                listItemsID.Add(rec);
            }

            parameter.Value = listItemsID;

            this.Session.AddParameter(parameter);
            this.Session.AddParameter("@ContentId", contentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new IdeasPaging(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Idea</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="contentId">content id</param>
        /// <param name="userId">user id</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public List<IdeasPaging> IdeasPagingTop(int pageIndex, int pageSize, out int totalCount, int contentId, int? userId)
        {
            List<IdeasPaging> col = new List<IdeasPaging>();
            this.Session.CreateCommand("CTIdeasPagingTop", true);
            this.Session.AddParameter("@ContentId", contentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new IdeasPaging(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Idea</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="contentId">content id</param>
        /// <param name="userId">user id</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public List<IdeasPaging> IdeasPagingRecommended(int pageIndex, int pageSize, out int totalCount, int contentId, int? userId)
        {
            List<IdeasPaging> col = new List<IdeasPaging>();
            this.Session.CreateCommand("CTIdeasPagingRecommended", true);
            this.Session.AddParameter("@ContentId", contentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new IdeasPaging(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// Count all the active ideas on the site
        /// </summary>
        /// <returns>return the total active ideas on the site</returns>
        public int IdeasCountAll()
        {
            this.Session.CreateCommand("CTIdeasCount", true);
            object result = this.Session.ExecuteScalar();
            int count = 0;
            int.TryParse(result.ToString(), out count);
            return count;
        }

        /// <summary>
        /// Count all the active ideas on the site by date
        /// </summary>
        /// <param name="start">start date</param>
        /// <param name="end">end date</param>
        /// <returns>return the total active ideas on the site by date</returns>
        public int CountIdeaByDate(DateTime? start, DateTime? end)
        {
            this.Session.CreateCommand("CTCountIdeaByDate", true);
            this.Session.AddParameter("@Start", start, System.Data.ParameterDirection.Input, System.Data.DbType.DateTime);
            this.Session.AddParameter("@End", end, System.Data.ParameterDirection.Input, System.Data.DbType.DateTime);
            object result = this.Session.ExecuteScalar();
            int count = 0;
            int.TryParse(result.ToString(), out count);
            return count;
        }

        /// <summary>
        /// obtains a list of content according to parameters
        /// </summary>
        /// <param name="filter">search filter</param>
        /// <param name="paginInfo"><c>PaginInfo</c> object</param>
        /// <returns>returns a list of content</returns>
        public IEnumerable<Idea> GetAllPaging(short? filter, PaginInfo paginInfo)
        {
            List<Idea> col = new List<Idea>();
            this.Session.CreateCommand("CTIdeageneralpaging", true);
            this.Session.AddParameter("@TotalCount", null, ParameterDirection.InputOutput, DbType.Int32);
            this.Session.AddParameter("@Text", this.Entity.Text, DbType.String);
            this.Session.AddParameter("@PageIndex", paginInfo.ActualRow, DbType.Int32);
            this.Session.AddParameter("@PageSize", paginInfo.Size, DbType.Int32);
            this.Session.AddParameter("@Active", this.Entity.Active, DbType.Boolean);
            this.Session.AddParameter("@Filter", filter, DbType.Int32);

            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Idea(this.Session.Reader));
            }

            paginInfo.TotalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// Obtiene las mejores ideas segun versus para un contenido
        /// </summary>
        /// <param name="count">Cantidad de ideas</param>
        /// <returns>Listado de ideas</returns>
        public List<IdeasPaging> TopIdeas(int count)
        {
            List<IdeasPaging> col = new List<IdeasPaging>();
            this.Session.CreateCommand("CTTopIdeas", true);
            this.Session.AddParameter("@ContentId", this.Entity.ContentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@Count", count, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new IdeasPaging(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Idea</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="userId">user id</param>
        /// <param name="currenUserId">current user id</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public List<IdeasPaging> MyIdeasPaging(int pageIndex, int pageSize, out int totalCount, int userId, int? currenUserId)
        {
            List<IdeasPaging> col = new List<IdeasPaging>();
            this.Session.CreateCommand("CTMyIdeasPaging", true);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@CurrentUserId", currenUserId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new IdeasPaging(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Idea</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="userId">user id</param>
        /// <param name="currenUserId">current user id</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public List<IdeasPaging> MyIdeasPagingCommented(int pageIndex, int pageSize, out int totalCount, int userId, int? currenUserId)
        {
            List<IdeasPaging> col = new List<IdeasPaging>();
            this.Session.CreateCommand("CTMyIdeasPagingCommented", true);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@CurrentUserId", currenUserId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new IdeasPaging(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Idea</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="userId">user id</param>
        /// <param name="currenUserId">current user id</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public List<IdeasPaging> MyConversationsPaging(int pageIndex, int pageSize, out int totalCount, int userId, int? currenUserId)
        {
            List<IdeasPaging> col = new List<IdeasPaging>();
            this.Session.CreateCommand("CTMyConversationsPaging", true);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@CurrentUserId", currenUserId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new IdeasPaging(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// Obtiene las mejores ideas
        /// </summary>
        /// <param name="count">Cantidad de ideas</param>
        /// <returns>Listado de ideas</returns>
        public List<IdeasPaging> TopIdeasHome(int count)
        {
            List<IdeasPaging> col = new List<IdeasPaging>();
            this.Session.CreateCommand("CTTopIdeasHome", true);
            this.Session.AddParameter("@Count", count, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new IdeasPaging(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Obtiene las mejores ideas votadas
        /// </summary>
        /// <param name="count">Cantidad de ideas</param>
        /// <returns>Listado de ideas</returns>
        public List<IdeasPaging> TopVotedIdeasHome(int count)
        {
            List<IdeasPaging> col = new List<IdeasPaging>();
            this.Session.CreateCommand("CTTopVotedIdeasHome", true);
            this.Session.AddParameter("@Count", count, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new IdeasPaging(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Ideas to be send in the weekly email
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="count">ideas to take</param>
        /// <returns>a list of ideas</returns>
        public List<MailIdeasPaging> IdeasUserMail(int userId, int? count)
        {
            List<MailIdeasPaging> col = new List<MailIdeasPaging>();
            this.Session.CreateCommand("CTIdeasUserMail", true);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@Count", count, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new MailIdeasPaging(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// return a list of user ids related to an idea
        /// </summary>
        /// <param name="ideaId">idea id</param>
        /// <returns>a list of user ids related to an idea</returns>
        public List<int> IdeaRelatedUsers(int ideaId)
        {
            List<int> col = new List<int>();
            this.Session.CreateCommand("CTIdeaRelatedUsers", true);
            this.Session.AddParameter("@IdeaId", ideaId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(Convert.ToInt32(this.Session.Reader["UserId"]));
            }

            return col;
        }

        /// <summary>
        /// Check if the idea is in the top 10
        /// </summary>
        /// <param name="ideaId">idea id</param>
        /// <returns>True if the idea is in the top false if not</returns>
        public bool IsIdeaInTop10(int ideaId)
        {
            List<IdeasPaging> ideas = this.TopIdeas(10);
            return ideas.Exists(s => s.IdeaId == ideaId);
        }

        /// <summary>
        /// Check if the idea is in the top 5
        /// </summary>
        /// <param name="ideaId">idea id</param>
        /// <returns>True if the idea is in the top false if not</returns>
        public bool IsIdeaInTop5Home(int ideaId)
        {
            List<IdeasPaging> ideas = this.TopIdeasHome(5);
            return ideas.Exists(s => s.IdeaId == ideaId);
        }

        /// <summary>
        /// Check if the idea is in the top 5
        /// </summary>
        /// <param name="ideaId">idea id</param>
        /// <returns>True if the idea is in the top false if not</returns>
        public bool IsIdeaInTopVoted5Home(int ideaId)
        {
            List<IdeasPaging> ideas = this.TopVotedIdeasHome(5);
            return ideas.Exists(s => s.IdeaId == ideaId);
        }
    }
}