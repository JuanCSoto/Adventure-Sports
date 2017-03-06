// <copyright file="Statistics.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno</author>
namespace Business.FrontEnd
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Xml;
    using Domain.Abstract;
    using Domain.Concrete;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Represents the model for the front end
    /// </summary>
    public class Statistics : IFrontEnd
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Statistics"/> class
        /// </summary>
        public Statistics()
        {
        }

        /// <summary>
        /// Gets or sets the view of the information to display
        /// </summary>
        public string View { get; set; }

        /// <summary>
        /// Gets or sets the total user count
        /// </summary>
        public int TotalUsers { get; set; }

        /// <summary>
        /// Gets or sets the male user count
        /// </summary>
        public int Male { get; set; }

        /// <summary>
        /// Gets or sets the female user count
        /// </summary>
        public int Female { get; set; }

        /// <summary>
        /// Gets or sets the unknown user gender count
        /// </summary>
        public int Unknown { get; set; }

        /// <summary>
        /// Gets or sets the total ideas count
        /// </summary>
        public int TotalIdeas { get; set; }

        /// <summary>
        /// Gets or sets the date filter in a "raw" string format
        /// </summary>
        public string RawDate { get; set; }

        /// <summary>
        /// Gets or sets the active user collection
        /// </summary>
        public List<User> ActiveUsers { get; set; }

        /// <summary>
        /// Gets or sets the user ages statistics 
        /// </summary>
        public Dictionary<string, string> Ages { get; set; }

        /// <summary>
        /// Gets or sets the user ages statistics 
        /// </summary>
        public Dictionary<string, string> Interests { get; set; }

        /// <summary>
        /// Gets or sets the user professions statistics 
        /// </summary>
        public Dictionary<string, string> Professions { get; set; }

        /// <summary>
        /// Gets or sets the Categories statistics 
        /// </summary>
        public Dictionary<string, string> Categories { get; set; }

        /// <summary>
        /// Gets or sets the Hash tags statistics 
        /// </summary>
        public Dictionary<string, string> Hashtags { get; set; }

        /// <summary>
        /// Gets or sets the ideas collection
        /// </summary>
        public List<IdeasPaging> Ideas { get; set; }

        /// <summary>
        /// Gets or sets the start time
        /// </summary>
        public DateTime? Start { get; set; }

        /// <summary>
        /// Gets or sets the the End time
        /// </summary>
        public DateTime? End { get; set; }        

        /// <summary>
        /// Bind the context and the session with the content
        /// </summary>
        /// <param name="context">Context page</param>
        /// <param name="session">Session object</param>
        /// <param name="id">Content ID</param>
        /// <param name="userId">current user ID</param>
        public void Bind(HttpContextBase context, ISession session, int? id, int? userId, int? LanguageId)
        {
            int count = 0;
            this.RawDate = string.Empty;

            this.Start = null;
            this.End = null;

            string rawUrl = context.Request.RawUrl.ToLower();
            string rangeDate = rawUrl.Split('/').LastOrDefault();
            this.View = string.Empty;

            if (rawUrl.IndexOf("/ranking") >= 0)
            {
                this.View = "ranking";
            }

            if (rawUrl.IndexOf("/categorias") >= 0)
            {
                this.View = "categorias";
            }

            if (rawUrl.IndexOf("/profesiones") >= 0)
            {
                this.View = "profesiones";
            }

            if (rawUrl.IndexOf("/tendencias") >= 0)
            {
                this.View = "tendencias";
            }

            if (rangeDate.Length == 21)
            {
                int[] dateParams = rangeDate.Split('-').Select(s => Convert.ToInt32(s)).ToArray();
                if (dateParams.Count() == 6)
                {
                    this.Start = new DateTime(dateParams[0], dateParams[1], dateParams[2]);
                    this.End = new DateTime(dateParams[3], dateParams[4], dateParams[5]);
                    this.End = this.End.Value.AddDays(1).AddSeconds(-1);

                    this.RawDate = string.Concat(this.Start.Value.ToString("yyyy-MM-dd"), " - ", this.End.Value.ToString("yyyy-MM-dd"));
                }
            }

            if (string.IsNullOrEmpty(this.View))
            {
                IdeaRepository idea = new IdeaRepository(session);
                UserRepository user = new UserRepository(session);

                this.ActiveUsers = user.CountActiveUserByDate(this.Start, this.End, 0, 6);

                DataTable profession = user.CountProfessionByDate(this.Start, this.End, 0, 6);
                this.Professions = new Dictionary<string, string>();
                if (profession.Rows.Count > 0)
                {
                    foreach (DataRow row in profession.Rows)
                    {
                        this.Professions.Add(row["Profession"].ToString(), row["Count"].ToString());
                    }
                }

                profession.Dispose();

                DataTable category = user.CountCategoryPulsesByDate(this.Start, this.End, 0, 6);
                this.Categories = new Dictionary<string, string>();
                if (category.Rows.Count > 0)
                {
                    foreach (DataRow row in category.Rows)
                    {
                        this.Categories.Add(row["Category"].ToString(), row["Count"].ToString());
                    }
                }

                category.Dispose();
                DataTable hashtag = user.CountHashTagByDate(this.Start, this.End, 0, 6);
                this.Hashtags = new Dictionary<string, string>();
                if (hashtag.Rows.Count > 0)
                {
                    foreach (DataRow row in hashtag.Rows)
                    {
                        this.Hashtags.Add(row["HashTag"].ToString(), row["Count"].ToString());
                    }
                }

                hashtag.Dispose();

                this.Ideas = idea.TopIdeasHome(8);
                this.TotalIdeas = idea.CountIdeaByDate(this.Start, this.End);
                DataTable userCount = user.CountUserByDate(this.Start, this.End);
                if (userCount.Rows.Count > 0)
                {
                    if (int.TryParse(userCount.Rows[0]["Total"].ToString(), out count))
                    {
                        this.TotalUsers = count;
                    }

                    if (int.TryParse(userCount.Rows[0]["Male"].ToString(), out count))
                    {
                        this.Male = count;
                    }

                    if (int.TryParse(userCount.Rows[0]["Female"].ToString(), out count))
                    {
                        this.Female = count;
                    }

                    if (int.TryParse(userCount.Rows[0]["Unknown"].ToString(), out count))
                    {
                        this.Unknown = count;
                    }
                }

                userCount.Dispose();

                DataTable ageCount = user.CountAgeByDate(this.Start, this.End);
                this.Ages = new Dictionary<string, string>();
                if (ageCount.Rows.Count > 0)
                {
                    foreach (DataColumn column in ageCount.Columns)
                    {
                        this.Ages.Add(column.ColumnName, ageCount.Rows[0][column.ColumnName].ToString());
                    }
                }

                ageCount.Dispose();

                DataTable interest = user.CountInterestByDate(this.Start, this.End);
                this.Interests = new Dictionary<string, string>();
                if (interest.Rows.Count > 0)
                {
                    foreach (DataRow row in interest.Rows)
                    {
                        this.Interests.Add(row["Name"].ToString(), row["Interest"].ToString());
                    }
                }

                interest.Dispose();
            }
        }
    }
}
