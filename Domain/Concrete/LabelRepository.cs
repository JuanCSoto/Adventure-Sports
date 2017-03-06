// <copyright file="LabelRepository.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Juan carlos Montoya</author>

namespace Domain.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the table <c>Label</c>
    /// </summary>
    public sealed class LabelRepository : DataRepository<Label>
    {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LabelRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public LabelRepository(ISession session):base(session, "GXLabel")              
        {
            this.Entity = new Label();
        }

        /// <summary>
        /// Load the information from the table <c>label</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new Label(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>label</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Label(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Label</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Tag</c></returns>
        public override List<Label> GetAll()
        {
            List<Label> col = new List<Label>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                col.Add(new Label(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// obtains a list of tags according criteria search
        /// </summary>
        /// <param name="name">criteria search</param>
        /// <param name="paginInfo"><c>PaginInfo</c> object</param>
        /// <param name="languageId">identifier of language</param>
        /// <returns>returns a list of labels</returns>
        public List<Label> GetAllPaging(string name, string translation, PaginInfo paginInfo, int languageId)
        {
            List<Label> col = new List<Label>();
            this.Session.CreateCommand("CTLabelpagin", true);
            this.Session.AddParameter("@LanguageId", languageId, DbType.Int32);
            this.Session.AddParameter("@Name", name, DbType.String);
            this.Session.AddParameter("@Translation", translation, DbType.String);
            this.Session.AddParameter("@PageSize", paginInfo.Size, DbType.Int32);
            this.Session.AddParameter("@PageIndex", paginInfo.ActualRow, DbType.Int32);
            this.Session.AddParameter("@TotalCount", null, ParameterDirection.InputOutput, DbType.Int32);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Label(this.Session.Reader));
            }

            paginInfo.TotalCount = Convert.ToInt32(this.Session.GetOutParameter("@TotalCount"));
            return col;
        }

        /// <summary>
        /// obtains a list of <c>KeyValue</c> objects
        /// </summary>
        /// <returns>list of <c>KeyValue</c> objects</returns>
        public List<KeyValue> GetLabels()
        {
            List<KeyValue> coll = new List<KeyValue>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (Session.Read())
            {
                coll.Add(new KeyValue(this.Session.GetValue(0).ToString(), this.Session.GetValue(1).ToString()));
            }

            return coll;
        }

        /// <summary>
        /// obtains a list of tags according criteria search
        /// </summary>
        /// <param name="name">criteria search</param>
        /// <param name="paginInfo"><c>PaginInfo</c> object</param>
        /// <param name="languageId">identifier of language</param>
        /// <returns>returns a list of labels</returns>
        public string GetLabelByName(string name,int languageId)
        {
            Label col = new Label();
            this.Session.CreateCommand("CTLabelTranslation", true);
            this.Session.AddParameter("@LanguageId", languageId, DbType.Int32);
            this.Session.AddParameter("@Name", name, DbType.String);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                 col = new Label(this.Session.Reader);
            }
          
            return col.Translation;
        }

     
    }

       

}
