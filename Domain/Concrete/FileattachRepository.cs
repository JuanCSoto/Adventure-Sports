// <copyright file="FileattachRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using System.Data;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the table <c>Fileattach</c>
    /// </summary>
    public sealed class FileattachRepository : DataRepository<Fileattach>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileattachRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public FileattachRepository(ISession session)
            : base(session, "GXFileattach")
        {
            this.Entity = new Fileattach();
        }

        /// <summary>
        /// Load the information from the table <c>Fileattach</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Fileattach(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Fileattach</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Fileattach(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Fileattach</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Fileattach</c></returns>
        public override List<Fileattach> GetAll()
        {
            List<Fileattach> col = new List<Fileattach>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Fileattach(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// change order to file list
        /// </summary>
        /// <param name="fileattachId">file attach identifier</param>
        /// <param name="previousId">previous identifier</param>
        /// <param name="limit">limit of file attach</param>
        public void ChangeOrder(int fileattachId, int previousId, bool limit)
        {
            if (limit)
            {
                this.Session.CreateCommand("CTFileattachupdateorderlimit", true);
            }
            else
            {
                this.Session.CreateCommand("CTFileattachupdateorder", true);
            }

            this.Session.AddParameter("@FileattachId", fileattachId, DbType.Int32);
            this.Session.AddParameter("@PrevId", previousId, DbType.Int32);
            this.Session.ExecuteNonQuery();
        }
    }
}