// <copyright file="NotificationKeyRepository.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>NotificationKey</c>
    /// </summary>
    public sealed class NotificationKeyRepository : DataRepository<NotificationKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationKeyRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public NotificationKeyRepository(ISession session)
            : base(session, "GXNotificationKey")
        {
            this.Entity = new NotificationKey();
        }

        /// <summary>
        /// Load the information from the table <c>NotificationKey</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new NotificationKey(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>NotificationKey</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new NotificationKey(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>NotificationKey</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>NotificationKey</c></returns>
        public override List<NotificationKey> GetAll()
        {
            List<NotificationKey> col = new List<NotificationKey>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new NotificationKey(this.Session.Reader));
            }

            return col;
        }
    }
}