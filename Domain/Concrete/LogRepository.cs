// <copyright file="LogRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the table <c>Log</c>
    /// </summary>
    public sealed class LogRepository : DataRepository<Log>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public LogRepository(ISession session)
            : base(session, "GXLog")
        {
            this.Entity = new Log();
        }

        /// <summary>
        /// Load the information from the table <c>Log</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Log(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Log</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Log(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Log</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Log</c></returns>
        public override List<Log> GetAll()
        {
            List<Log> col = new List<Log>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Log(this.Session.Reader));
            }

            return col;
        }
    }
}