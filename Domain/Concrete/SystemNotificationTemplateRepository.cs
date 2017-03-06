// <copyright file="SystemNotificationTemplateRepository.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>SystemNotificationTemplate</c>
    /// </summary>
    public sealed class SystemNotificationTemplateRepository : DataRepository<SystemNotificationTemplate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemNotificationTemplateRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public SystemNotificationTemplateRepository(ISession session)
            : base(session, "GXSystemNotificationTemplate")
        {
            this.Entity = new SystemNotificationTemplate();
        }

        /// <summary>
        /// Load the information from the table <c>SystemNotificationTemplate</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new SystemNotificationTemplate(this.Session.Reader);
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
                this.Entity = new SystemNotificationTemplate(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>SystemNotificationTemplate</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>SystemNotificationTemplate</c></returns>
        public override List<SystemNotificationTemplate> GetAll()
        {
            List<SystemNotificationTemplate> col = new List<SystemNotificationTemplate>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new SystemNotificationTemplate(this.Session.Reader));
            }

            return col;
        }
    }
}