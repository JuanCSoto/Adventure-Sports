// <copyright file="QuestionRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Domain.Abstract;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;
    using Microsoft.SqlServer.Server;

    /// <summary>
    /// Class responsible for interaction with the table <c>Question</c>
    /// </summary>
    public sealed class QuestionRepository : DataRepository<Question>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public QuestionRepository(ISession session)
            : base(session, "GXQuestion")
        {
            this.Entity = new Question();
        }

        /// <summary>
        /// Load the information from the table <c>Question</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Question(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Question</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Question(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Question</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Question</c></returns>
        public override List<Question> GetAll()
        {
            List<Question> col = new List<Question>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new Question(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Question</c> to the list of entities
        /// </summary>
        /// <returns>a list of entities</returns>
        public List<ExpiringQuestions> ExpiringQuestions()
        {
            List<ExpiringQuestions> col = new List<ExpiringQuestions>();
            this.Session.CreateCommand("CTExpiringQuestions", true);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new ExpiringQuestions(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Question</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="active">To return active or inactive content</param>
        /// <returns>A list of entities according to the parameters send</returns>
        public List<QuestionsPaging> QuestionsPaging(int pageIndex, int pageSize, out int totalCount, bool? active)
        {
            totalCount = 0;
            List<QuestionsPaging> col = new List<QuestionsPaging>();
            this.Session.CreateCommand("CTQuestionsPaging", true);
            this.Session.AddParameter("@TotalCount", 0, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@Active", active, System.Data.ParameterDirection.Input, System.Data.DbType.Boolean);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new QuestionsPaging(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Question</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="active">To return active or inactive content</param>
        /// <returns>A list of entities according to the parameters send</returns>
        public List<QuestionsPaging> QuestionsPagingTop(int pageIndex, int pageSize, out int totalCount, bool? active)
        {
            totalCount = 0;
            List<QuestionsPaging> col = new List<QuestionsPaging>();
            this.Session.CreateCommand("CTQuestionsPagingTop", true);
            this.Session.AddParameter("@TotalCount", 0, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@Active", active, System.Data.ParameterDirection.Input, System.Data.DbType.Boolean);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new QuestionsPaging(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Question</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="active">To return active or inactive content</param>
        /// <returns>A list of entities according to the parameters send</returns>
        public List<QuestionsPaging> QuestionsPagingRecommended(int pageIndex, int pageSize, out int totalCount, bool? active)
        {
            totalCount = 0;
            List<QuestionsPaging> col = new List<QuestionsPaging>();
            this.Session.CreateCommand("CTQuestionsPagingRecommended", true);
            this.Session.AddParameter("@TotalCount", 0, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@Active", active, System.Data.ParameterDirection.Input, System.Data.DbType.Boolean);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new QuestionsPaging(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>Question</c> to the list of entities according to the parameters send
        /// </summary>        
        /// <param name="pageSize">page size</param>        
        /// <param name="active">To return active or inactive content</param>
        /// <param name="questionsID">A list of question to exclude from the result</param>
        /// <returns>A list of entities according to the parameters send</returns>        
        public List<QuestionsPaging> QuestionsPagingRandom(int pageSize, bool? active, int[] questionsID)
        {
            List<QuestionsPaging> col = new List<QuestionsPaging>();
            this.Session.CreateCommand("CTQuestionsPagingRandom", true);
            List<SqlDataRecord> listItemsID = new List<SqlDataRecord>();
            SqlMetaData[] tvp_definition = { new SqlMetaData("QuestionsId", SqlDbType.Int) };
            SqlParameter parameter = new SqlParameter("@QuestionsId", SqlDbType.Structured);
            parameter.Direction = ParameterDirection.Input;
            parameter.TypeName = "int_list";
            if (questionsID != null && questionsID.Length > 0)
            {
                foreach (int id in questionsID)
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
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@Active", active, System.Data.ParameterDirection.Input, System.Data.DbType.Boolean);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new QuestionsPaging(this.Session.Reader));
            }

            return col;
        }
    }
}