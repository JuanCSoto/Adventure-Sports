// <copyright file="LegalController.cs" company="Dasigno">
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
    /// controller for the legal action
    /// </summary>
    public class LegalController : Controller
    {
        /// <summary>
        /// Return the terms of the application
        /// </summary>
        /// <returns>Terms of the application</returns>
        public ActionResult Terms()
        {
            return this.View();
        }

        /// <summary>
        /// Return the privacy policy of the application
        /// </summary>
        /// <returns>Privacy policy of the application</returns>
        public ActionResult Privacy()
        {
            return this.View();
        }
    }
}
