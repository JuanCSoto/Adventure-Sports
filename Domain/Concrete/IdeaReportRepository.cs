// <copyright file="IdeaReportRepository.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
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
    /// Class responsible for interaction with the board <c>IdeaReport</c>
    /// </summary>
    public sealed class IdeaReportRepository : DataRepository<IdeaReport>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdeaReportRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public IdeaReportRepository(ISession session)
            : base(session, "GXIdeaReport")
        {
            this.Entity = new IdeaReport();
        }

        /// <summary>
        /// Load the information from the table <c>IdeaReport</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new IdeaReport(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>IdeaReport</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new IdeaReport(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>IdeaReport</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>IdeaReport</c></returns>
        public override List<IdeaReport> GetAll()
        {
            List<IdeaReport> col = new List<IdeaReport>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new IdeaReport(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// obtains a list of content according to parameters
        /// </summary>
        /// <param name="filter">search filter</param>
        /// <param name="paginInfo"><c>PaginInfo</c> object</param>
        /// <returns>returns a list of content</returns>
        public IEnumerable<IdeaReportPaging> GetAllPaging(short? filter, PaginInfo paginInfo)
        {
            List<IdeaReportPaging> col = new List<IdeaReportPaging>();
            this.Session.CreateCommand("CTIdeaReportgeneralpaging", true);
            this.Session.AddParameter("@TotalCount", null, ParameterDirection.InputOutput, DbType.Int32);
            this.Session.AddParameter("@Text", this.Entity.Text, DbType.String);
            this.Session.AddParameter("@PageIndex", paginInfo.ActualRow, DbType.Int32);
            this.Session.AddParameter("@PageSize", paginInfo.Size, DbType.Int32);
            this.Session.AddParameter("@Status", this.Entity.Status, DbType.Boolean);
            this.Session.AddParameter("@Filter", filter, DbType.Int32);

            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new IdeaReportPaging(this.Session.Reader));
            }

            paginInfo.TotalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// Load an idea report paging by the idea report id
        /// </summary>
        /// <param name="id">idea report id</param>
        /// <returns>A list of reported ideas</returns>
        public IdeaReportPaging GetIdeaReportPagingById(int id)
        {
            IdeaReportPaging ideaReport = new IdeaReportPaging();
            this.Session.CreateCommand("CTIdeaReportPagingById", true);
            this.Session.AddParameter("@IdeaReportId", id, DbType.Int32);

            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                ideaReport = new IdeaReportPaging(this.Session.Reader);
            }

            return ideaReport;
        }
    }
}