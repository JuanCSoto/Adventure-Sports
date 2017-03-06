// <copyright file="EmailNotificationTemplateRepository.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>EmailNotificationTemplate</c>
    /// </summary>
    public sealed class EmailNotificationTemplateRepository : DataRepository<EmailNotificationTemplate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailNotificationTemplateRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public EmailNotificationTemplateRepository(ISession session)
            : base(session, "GXEmailNotificationTemplate")
        {
            this.Entity = new EmailNotificationTemplate();
        }

        /// <summary>
        /// Load the information from the table <c>EmailNotificationTemplate</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new EmailNotificationTemplate(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>SystemNotificationTemplate</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new EmailNotificationTemplate(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>SystemNotificationTemplate</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>SystemNotificationTemplate</c></returns>
        public override List<EmailNotificationTemplate> GetAll()
        {
            List<EmailNotificationTemplate> col = new List<EmailNotificationTemplate>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new EmailNotificationTemplate(this.Session.Reader));
            }

            return col;
        }
    }
}