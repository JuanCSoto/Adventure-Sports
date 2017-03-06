// <copyright file="ErrorController.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Webcore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// controller for the error action
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// gets the error page
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
