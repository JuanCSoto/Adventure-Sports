// <copyright file="AuditRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>using System.Collections.Generic;
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using System.Data;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the table Audit
    /// </summary>
    public sealed class AuditRepository : DataRepository<Audit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public AuditRepository(ISession session)
            : base(session, "GXAudit")
        {
            this.Entity = new Audit();
        }

        /// <summary>
        /// Load the information from the table <c>Audit</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();

            while (this.Session.Read())
            {
                this.Entity = new Audit(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Audit</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Audit(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Audit</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Audit</c></returns>
        public override List<Audit> GetAll()
        {
            List<Audit> col = new List<Audit>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new Audit(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Gets the firsts 8 fields of the <c>Audit</c> table
        /// </summary>
        /// <returns>List of the object type <c>Audit</c></returns>
        public IEnumerable<Audit> GetAutidtop()
        {
            this.Session.CreateCommand("CTAuditselecttop", true);
            this.Session.AddParameter("@UserId", Entity.Username, DbType.Int32);
            this.Session.ExecuteReader();

            while (Session.Read())
            {
                yield return new Audit(this.Session.Reader);
            }
        }

        /// <summary>
        /// Gets the list of all fields of the <c>Audit</c> table
        /// </summary>
        /// <returns>List of the object type <c>AuditComp</c></returns>
        public List<AuditComp> GetList()
        {
            List<AuditComp> col = new List<AuditComp>();
            this.Session.CreateCommand("CTAuditlist", true);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new AuditComp(this.Session.Reader));
            }

            return col;
        }
    }
}