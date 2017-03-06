// <copyright file="SystemNotificationRepository.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
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
    using Microsoft.SqlServer.Server;

    /// <summary>
    /// Class responsible for interaction with the board <c>SystemNotification</c>
    /// </summary>
    public sealed class SystemNotificationRepository : DataRepository<SystemNotification>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemNotificationRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public SystemNotificationRepository(ISession session)
            : base(session, "GXSystemNotification")
        {
            this.Entity = new SystemNotification();
        }

        /// <summary>
        /// Load the information from the table <c>SystemNotification</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new SystemNotification(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>SystemNotification</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new SystemNotification(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>SystemNotification</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>SystemNotification</c></returns>
        public override List<SystemNotification> GetAll()
        {
            List<SystemNotification> col = new List<SystemNotification>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new SystemNotification(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Return the total count of a type of notification for an user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="templateId">template id</param>
        /// <param name="elementId">element id</param>
        /// <returns>the total count of a type of notification for an user</returns>
        public int SystemNotificationCount(int userId, int templateId, int elementId)
        {
            List<int> col = new List<int>();
            this.Session.CreateCommand("CTSystemNotificationCount", true);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@TemplateId", templateId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@ElementId", elementId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            object result = this.Session.ExecuteScalar();

            int count = 0;
            if (int.TryParse(result.ToString(), out count))
            {
                return count;
            }

            return 0;
        }

        /// <summary>
        /// Load the information from the table <c>SystemNotification</c> to the list of entities according to the parameters send
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="size">page size</param>
        /// <param name="userId">user id</param>
        /// <param name="total">out parameter with the total count of entries according to the parameters send</param>
        /// <returns>a list of entities according to the parameters send</returns>
        public List<SystemNotification> Notifications(int page, int size, int userId, out int total)
        {
            List<SystemNotification> col = new List<SystemNotification>();
            this.Session.CreateCommand("CTNotifications", true);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageIndex", page, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@PageSize", size, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, System.Data.ParameterDirection.InputOutput, System.Data.DbType.Int32);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new SystemNotification(this.Session.Reader));
            }

            total = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));

            return col;
        }

        /// <summary>
        /// Mark the list of notifications as seen
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="notificationsId">seen notification id list</param>
        public void MarkNotifications(int userId, int[] notificationsId)
        {
            this.Session.CreateCommand("CTMarkNotifications", true);

            List<SqlDataRecord> listItemsID = new List<SqlDataRecord>();
            SqlMetaData[] tvp_definition = { new SqlMetaData("NotificationsId", SqlDbType.Int) };
            SqlParameter parameter = new SqlParameter("@NotificationsId", SqlDbType.Structured);
            parameter.Direction = ParameterDirection.Input;
            parameter.TypeName = "int_list";
            if (notificationsId != null && notificationsId.Length > 0)
            {
                foreach (int id in notificationsId)
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
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            this.Session.ExecuteNonQuery();
        }
    }
}