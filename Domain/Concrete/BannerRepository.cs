// <copyright file="BannerRepository.cs" company="Dasigno">
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
    /// Class responsible for interaction with the table Banner
    /// </summary>
    public sealed class BannerRepository : DataRepository<Banner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BannerRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public BannerRepository(ISession session)
            : base(session, "GXBanner")
        {
            this.Entity = new Banner();
        }

        /// <summary>
        /// Load the information from the table <c>Banner</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Banner(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Banner</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Banner(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Banner</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Banner</c></returns>
        public override List<Banner> GetAll()
        {
            List<Banner> col = new List<Banner>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Banner(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// get list of objects paged Banner
        /// </summary>
        /// <param name="name">criteria search</param>
        /// <param name="paginInfo">object <c>PaginInfo</c></param>
        /// <param name="languageId">identifier of language</param>
        /// <returns>returns a list of <c>Banner</c></returns>
        public List<Banner> GetAllPaging(string name, PaginInfo paginInfo, int languageId)
        {
            List<Banner> col = new List<Banner>();
            this.Session.CreateCommand("CTBannerpagin", true);
            this.Session.AddParameter("@LanguageId", languageId, DbType.Int32);
            this.Session.AddParameter("@Name", name, DbType.String);
            this.Session.AddParameter("@PageSize", paginInfo.Size, DbType.Int32);
            this.Session.AddParameter("@PageIndex", paginInfo.ActualRow, DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, ParameterDirection.InputOutput, DbType.Int32);

            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Banner(this.Session.Reader));
            }

            paginInfo.TotalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// obtains a list of <c>Banner</c> according to section
        /// </summary>
        /// <param name="sectionId">identifier of section</param>
        /// <param name="languageId">identifier of language</param>
        /// <returns>returns a list of <c>Banner</c></returns>
        public List<Banner> GetBannersBySection(int sectionId, int languageId)
        {
            List<Banner> col = new List<Banner>();
            this.Session.CreateCommand("CTBannerbyposition", true);
            this.Session.AddParameter("@SectionId", sectionId, DbType.Int32);
            this.Session.AddParameter("@LanguageId", languageId, DbType.Int32);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Banner(this.Session.Reader));
            }

            return col;
        }
    }
}