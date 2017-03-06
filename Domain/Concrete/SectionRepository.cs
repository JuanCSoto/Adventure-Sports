// <copyright file="SectionRepository.cs" company="Dasigno">
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
    /// Class responsible for interaction with the table Section
    /// </summary>
    public sealed class SectionRepository : DataRepository<Section>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SectionRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public SectionRepository(ISession session)
            : base(session, "GXSection")
        {
            this.Entity = new Section();
        }

        /// <summary>
        /// Load the information from the table <c>Section</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Section(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Section</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Section(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Section</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Section</c></returns>
        public override List<Section> GetAll()
        {
            List<Section> col = new List<Section>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Section(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// obtains a max order to section list according to join date
        /// </summary>
        /// <returns>returns a list of sections</returns>
        public int GetMaxOrder()
        {
            this.Session.CreateCommand("CTSectionmaxorder", true);
            object result = this.Session.ExecuteScalar();
            return result != DBNull.Value ? Convert.ToInt32(result) : 1;
        }

        /// <summary>
        /// change the order to section items
        /// </summary>
        /// <param name="newOrder">new order to section</param>
        /// <param name="oldOrder">old order to section</param>
        public void ChangeOrder(int newOrder, int oldOrder)
        {
            this.Session.CreateCommand("CTSectionchangeorder", true);
            this.Session.AddParameter("@NewOrden", newOrder, DbType.Int32);
            this.Session.AddParameter("@OldOrden", oldOrder, DbType.Int32);
            this.Session.ExecuteNonQuery();
        }
    }
}