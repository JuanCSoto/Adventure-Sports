// <copyright file="EmailNotificationRepository.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System;
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>EmailNotification</c>
    /// </summary>
    public sealed class EmailNotificationRepository : DataRepository<EmailNotification>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailNotificationRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public EmailNotificationRepository(ISession session)
            : base(session, "GXEmailNotification")
        {
            this.Entity = new EmailNotification();
        }

        /// <summary>
        /// Load the information from the table <c>EmailNotification</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new EmailNotification(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>EmailNotification</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new EmailNotification(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>EmailNotification</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>EmailNotification</c></returns>
        public override List<EmailNotification> GetAll()
        {
            List<EmailNotification> col = new List<EmailNotification>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new EmailNotification(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Return a list of user id to receive the new process notification
        /// </summary>
        /// <returns>a list of user id to receive the new process notification</returns>
        public List<int> SendNewProcessNotification()
        {
            List<int> col = new List<int>();
            this.Session.CreateCommand("CTSendNewProcessNotification", true);

            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(Convert.ToInt32(this.Session.Reader[0]));
            }

            return col;
        }
    }
}