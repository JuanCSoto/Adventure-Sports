// <copyright file="StatisticsController.cs" company="Dasigno SAS">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Webcore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Business.Services;
    using Domain.Concrete;
    using Webcore.Models;

    /// <summary>
    /// Action controller for the "Widget" actions
    /// </summary>
    public class StatisticsController : FrontEndController
    {
        /// <summary>
        /// Controller for the "Ranking" action
        /// </summary>        
        /// <param name="page">page index</param>
        /// <param name="start">start date</param>
        /// <param name="end">end date</param>
        /// <returns>Return a dynamic view of the ranking list</returns>
        public ActionResult Ranking(int page, DateTime? start, DateTime? end)
        {
            if (end.HasValue)
            {
                end = end.Value.AddDays(1).AddSeconds(-1);
            }

            ViewBag.PageIndex = page;
            UserRepository user = new UserRepository(SessionCustom);
            return this.View("_StatisticsRankingList", user.CountActiveUserByDate(start, end, page, 6));
        }

        /// <summary>
        /// Return a view list with the categories
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="start">start date</param>
        /// <param name="end">end date</param>
        /// <returns>Return a dynamic view of the category list</returns>
        public ActionResult Category(int page, DateTime? start, DateTime? end)
        {
            if (end.HasValue)
            {
                end = end.Value.AddDays(1).AddSeconds(-1);
            }

            ViewBag.PageIndex = page;
            UserRepository user = new UserRepository(SessionCustom);
            DataTable category = user.CountCategoryPulsesByDate(start, end, page, 6);
            Dictionary<string, string> categories = new Dictionary<string, string>();
            if (category.Rows.Count > 0)
            {
                foreach (DataRow row in category.Rows)
                {
                    categories.Add(row["Category"].ToString(), row["Count"].ToString());
                }
            }

            category.Dispose();
            return this.View("_StatisticsCategoryList", categories);
        }

        /// <summary>
        /// Return a view list with the professions
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="start">start date</param>
        /// <param name="end">end date</param>
        /// <returns>Return a dynamic view of the profession list</returns>
        public ActionResult Profession(int page, DateTime? start, DateTime? end)
        {
            if (end.HasValue)
            {
                end = end.Value.AddDays(1).AddSeconds(-1);
            }

            ViewBag.PageIndex = page;
            UserRepository user = new UserRepository(SessionCustom);
            DataTable profession = user.CountProfessionByDate(start, end, page, 6);
            Dictionary<string, string> professions = new Dictionary<string, string>();
            if (profession.Rows.Count > 0)
            {
                foreach (DataRow row in profession.Rows)
                {
                    professions.Add(row["Profession"].ToString(), row["Count"].ToString());
                }
            }

            profession.Dispose();
            return this.View("_StatisticsProfessionList", professions);
        }

        /// <summary>
        /// Return a view list with the hash tag
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="start">start date</param>
        /// <param name="end">end date</param>
        /// <returns>Return a dynamic view of the hash tag list</returns>
        public ActionResult Hashtag(int page, DateTime? start, DateTime? end)
        {
            if (end.HasValue)
            {
                end = end.Value.AddDays(1).AddSeconds(-1);
            }

            ViewBag.PageIndex = page;
            UserRepository user = new UserRepository(SessionCustom);
            DataTable hashtag = user.CountHashTagByDate(start, end, page, 6);
            Dictionary<string, string> hashtags = new Dictionary<string, string>();
            if (hashtag.Rows.Count > 0)
            {
                foreach (DataRow row in hashtag.Rows)
                {
                    hashtags.Add(row["HashTag"].ToString(), row["Count"].ToString());
                }
            }

            hashtag.Dispose();
            return this.View("_StatisticsHashtagList", hashtags);
        }
    }
}
