// <copyright file="FilterConfig.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// set the applications filters
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// register a global filters
        /// </summary>
        /// <param name="filters">Represents a class that contains all the global filters.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}