// <copyright file="UserRepository.cs" company="Dasigno">
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
    /// Class responsible for interaction with the table <c>User</c>
    /// </summary>
    public sealed class UserRepository : DataRepository<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public UserRepository(ISession session)
            : base(session, "GXUser")
        {
            this.Entity = new User();
        }

        /// <summary>
        /// Load the information from the table <c>User</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new User(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>User</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new User(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>User</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>User</c></returns>
        public override List<User> GetAll()
        {
            List<User> col = new List<User>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new User(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>User</c> to the list of entities according to the <c>FacebookId</c>
        /// </summary>
        public void LoadByFacebookId()
        {
            this.BindParameters(5);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                this.Entity = new User(this.Session.Reader);
            }
        }

        /// <summary>
        /// obtains a list of users according to criteria search
        /// </summary>
        /// <param name="name">criteria search</param>
        /// <param name="paginInfo"><c>PaginInfo</c> object</param>
        /// <returns>returns a list of users</returns>
        public List<User> GetAllPaging(string name, PaginInfo paginInfo)
        {
            List<User> col = new List<User>();
            this.Session.CreateCommand("CTUserpagin", true);
            this.Session.AddParameter("@Name", name, DbType.String);
            this.Session.AddParameter("@PageSize", paginInfo.Size, DbType.Int32);
            this.Session.AddParameter("@PageIndex", paginInfo.ActualRow, DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, ParameterDirection.InputOutput, DbType.Int32);

            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new User(this.Session.Reader));
            }

            paginInfo.TotalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// return a random list of active users
        /// </summary>
        /// <param name="count">number of users to return</param>
        /// <param name="totalCount">out parameter with the total count of entries</param>
        /// <returns>a random list of active users</returns>
        public List<Entities.FrontEnd.UserProfilePaging> Participants(int count, out int totalCount)
        {
            List<Entities.FrontEnd.UserProfilePaging> col = new List<Entities.FrontEnd.UserProfilePaging>();
            this.Session.CreateCommand("CTUserListHome", true);
            this.Session.AddParameter("@Count", count, ParameterDirection.Input, DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, ParameterDirection.InputOutput, DbType.Int32);

            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Entities.FrontEnd.UserProfilePaging(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));

            return col;
        }

        /// <summary>
        /// return a top of the more active users
        /// </summary>
        /// <param name="count">number of users to return</param>
        /// <returns>a top of the more active users</returns>
        public List<Entities.FrontEnd.UserProfilePaging> InnovativeUserListHome(int count)
        {
            List<Entities.FrontEnd.UserProfilePaging> col = new List<Entities.FrontEnd.UserProfilePaging>();
            this.Session.CreateCommand("CTInnovativeUserListHome", true);
            this.Session.AddParameter("@Count", count, ParameterDirection.Input, DbType.Int32);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Entities.FrontEnd.UserProfilePaging(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Deletes the information of a user permanently
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>true if the action was successful false if not</returns>
        public bool CleanUser(int userId)
        {
            List<Entities.FrontEnd.UserProfilePaging> col = new List<Entities.FrontEnd.UserProfilePaging>();
            this.Session.CreateCommand("CTCleanUser", true);
            this.Session.AddParameter("@UserId", userId, ParameterDirection.Input, DbType.Int32);
            bool result = this.Session.ExecuteNonQuery() > 0 ? true : false;
            
            return result;
        }

        /// <summary>
        /// Return a list of users that had liked at least one idea from a content
        /// </summary>
        /// <param name="contentId">content id</param>
        /// <param name="userId">user id to be excluded from the result</param>
        /// <returns>a list of users that had liked at least one idea from a content</returns>
        public List<User> UserLikeByContentId(int contentId, int userId)
        {
            List<User> col = new List<User>();
            this.Session.CreateCommand("CTUserLikeByContentId", true);
            this.Session.AddParameter("@ContentId", contentId, DbType.Int32);
            this.Session.AddParameter("@UserId", userId, DbType.Int32);

            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new User(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Return a list of users that had answer the a content
        /// </summary>
        /// <param name="contentId">content id</param>
        /// <param name="userId">user id to be excluded from the result</param>
        /// <returns>a list of users that had answer the a content</returns>
        public List<User> UserAnswerByContentId(int contentId, int userId)
        {
            List<User> col = new List<User>();
            this.Session.CreateCommand("CTUserAnswerByContentId", true);
            this.Session.AddParameter("@ContentId", contentId, DbType.Int32);
            this.Session.AddParameter("@UserId", userId, DbType.Int32);

            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new User(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Return a list of users that had choose the answer
        /// </summary>
        /// <param name="answerId">answer id</param>
        /// <param name="userId">user id to be excluded from the result</param>
        /// <returns>a list of users that had choose the answer</returns>
        public List<User> UserAnswerByAnswerId(int answerId, int userId)
        {
            List<User> col = new List<User>();
            this.Session.CreateCommand("CTUserAnswerByAnswerId", true);
            this.Session.AddParameter("@AnswerId", answerId, DbType.Int32);
            this.Session.AddParameter("@UserId", userId, DbType.Int32);

            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new User(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Load the information from the table <c>User</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="contentId">content id</param>
        /// <param name="userId">user id</param>
        /// <param name="text">search text</param>
        /// <param name="order">order parameter</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public List<User> Users(int pageIndex, int pageSize, out int totalCount, int? contentId, int? userId, string text, int? order)
        {
            List<User> col = new List<User>();
            this.Session.CreateCommand("CTUsers", true);
            this.Session.AddParameter("@ContentId", contentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@Text", text, System.Data.ParameterDirection.Input, System.Data.DbType.String);
            this.Session.AddParameter("@Order", order, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new User(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// Load the information from the table <c>User</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="contentId">content id</param>
        /// <param name="userId">user id</param>
        /// <param name="text">search text</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public List<User> RelatedUsers(int pageIndex, int pageSize, out int totalCount, int? contentId, int? userId, string text)
        {
            List<User> col = new List<User>();
            this.Session.CreateCommand("CTRelatedUsers", true);
            this.Session.AddParameter("@ContentId", contentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@Text", text, System.Data.ParameterDirection.Input, System.Data.DbType.String);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new User(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// Load the information from the table <c>User</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">out parameter with the total count of entries according to the parameters send</param>
        /// <param name="contentId">content id</param>
        /// <param name="userId">user id</param>
        /// <param name="text">search text</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public List<User> RelatedUsersZero(int pageIndex, int pageSize, out int totalCount, int? contentId, int? userId, string text)
        {
            List<User> col = new List<User>();
            this.Session.CreateCommand("CTRelatedUsersZero", true);
            this.Session.AddParameter("@ContentId", contentId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@Text", text, System.Data.ParameterDirection.Input, System.Data.DbType.String);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new User(this.Session.Reader));
            }

            totalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// Return the total count of registered user by date grouped by gender
        /// </summary>
        /// <param name="start">start date</param>
        /// <param name="end">end date</param>
        /// <returns>the total count of registered user by date grouped by gender</returns>
        public DataTable CountUserByDate(DateTime? start, DateTime? end)
        {
            this.Session.CreateCommand("CTCountUserByDate", true);
            this.Session.AddParameter("@Start", start, System.Data.ParameterDirection.Input, System.Data.DbType.DateTime);
            this.Session.AddParameter("@End", end, System.Data.ParameterDirection.Input, System.Data.DbType.DateTime);

            return this.Session.GetTable();
        }

        /// <summary>
        /// Load the information from the table <c>User</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="start">start date</param>
        /// <param name="end">end date</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public List<User> CountActiveUserByDate(DateTime? start, DateTime? end, int pageIndex, int pageSize)
        {
            List<User> col = new List<User>();
            this.Session.CreateCommand("CTCountActiveUserByDate", true);
            this.Session.AddParameter("@Start", start, System.Data.ParameterDirection.Input, System.Data.DbType.DateTime);
            this.Session.AddParameter("@End", end, System.Data.ParameterDirection.Input, System.Data.DbType.DateTime);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new User(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Return the total count of registered user by date grouped by age
        /// </summary>
        /// <param name="start">start date</param>
        /// <param name="end">end date</param>
        /// <returns>the total count of registered user by date grouped by age</returns>
        public DataTable CountAgeByDate(DateTime? start, DateTime? end)
        {
            this.Session.CreateCommand("CTCountAgeByDate", true);
            this.Session.AddParameter("@Start", start, System.Data.ParameterDirection.Input, System.Data.DbType.DateTime);
            this.Session.AddParameter("@End", end, System.Data.ParameterDirection.Input, System.Data.DbType.DateTime);

            return this.Session.GetTable();
        }

        /// <summary>
        /// Return the total count of registered user by date grouped by interest
        /// </summary>
        /// <param name="start">start date</param>
        /// <param name="end">end date</param>
        /// <returns>the total count of registered user by date grouped by interest</returns>
        public DataTable CountInterestByDate(DateTime? start, DateTime? end)
        {
            this.Session.CreateCommand("CTCountInterestByDate", true);
            this.Session.AddParameter("@Start", start, System.Data.ParameterDirection.Input, System.Data.DbType.DateTime);
            this.Session.AddParameter("@End", end, System.Data.ParameterDirection.Input, System.Data.DbType.DateTime);            

            return this.Session.GetTable();
        }

        /// <summary>
        /// Return the total count of registered user by date grouped by profession
        /// </summary>
        /// <param name="start">start date</param>
        /// <param name="end">end date</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public DataTable CountProfessionByDate(DateTime? start, DateTime? end, int pageIndex, int pageSize)
        {
            this.Session.CreateCommand("CTCountProfessionByDate", true);
            this.Session.AddParameter("@Start", start, System.Data.ParameterDirection.Input, System.Data.DbType.DateTime);
            this.Session.AddParameter("@End", end, System.Data.ParameterDirection.Input, System.Data.DbType.DateTime);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);

            return this.Session.GetTable();
        }

        /// <summary>
        /// Return the total count of registered pulses by date grouped by category
        /// </summary>
        /// <param name="start">start date</param>
        /// <param name="end">end date</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public DataTable CountCategoryPulsesByDate(DateTime? start, DateTime? end, int pageIndex, int pageSize)
        {
            this.Session.CreateCommand("CTCountCategoryPulsesByDate", true);
            this.Session.AddParameter("@Start", start, System.Data.ParameterDirection.Input, System.Data.DbType.DateTime);
            this.Session.AddParameter("@End", end, System.Data.ParameterDirection.Input, System.Data.DbType.DateTime);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);

            return this.Session.GetTable();
        }

        /// <summary>
        /// Return the total count of registered hash tags by date grouped by tag
        /// </summary>
        /// <param name="start">start date</param>
        /// <param name="end">end date</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public DataTable CountHashTagByDate(DateTime? start, DateTime? end, int pageIndex, int pageSize)
        {
            this.Session.CreateCommand("CTCountHashTagByDate", true);
            this.Session.AddParameter("@Start", start, System.Data.ParameterDirection.Input, System.Data.DbType.DateTime);
            this.Session.AddParameter("@End", end, System.Data.ParameterDirection.Input, System.Data.DbType.DateTime);
            this.Session.AddParameter("@PageIndex", pageIndex, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", pageSize, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);

            return this.Session.GetTable();
        }

        /// <summary>
        /// Set the default value for the user notification settings
        /// </summary>
        /// <param name="userId">The user ID to initialize</param>
        public void UserSettingInit(int userId)
        {
            this.Session.CreateCommand("CTUserSettingInit", true);            
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);

            this.Session.ExecuteNonQuery();
        }

        /// <summary>
        /// Return a list of user id administrators
        /// </summary>
        /// <returns>a list of user id administrators</returns>
        public List<int> AdminUsers()
        {
            List<int> col = new List<int>();
            this.Session.CreateCommand("CTAdminUsers", true);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(Convert.ToInt32(this.Session.Reader[0]));
            }

            return col;
        }

        /// <summary>
        /// Return the statistic information of all the users
        /// </summary>
        /// <returns>the statistic information of all the users</returns>
        public DataTable ReportUsers()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            this.Session.CreateCommand("CTReportUsers", true);
            return this.Session.GetTable();
        }
    }
}