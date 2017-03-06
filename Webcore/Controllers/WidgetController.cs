// <copyright file="WidgetController.cs" company="Dasigno SAS">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Webcore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Domain.Concrete;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Action controller for the "Widget" actions
    /// </summary>
    public class WidgetController : FrontEndController
    {
        /// <summary>
        /// Controller for the "Index" action
        /// </summary>
        /// <param name="width">width for the widget</param>
        /// <param name="height">height for the widget</param>
        /// <returns>Return a dynamic view of the widget</returns>
        public ActionResult Index(int? width, int? height)
        {
            if (width.HasValue)
            {
                width = width < 300 ? 300 : width > 460 ? 460 : width;
            }
            else
            {
                width = 300;
            }

            if (height.HasValue)
            {
                height = height * 1;
                height = height < 400 ? 400 : height > 615 ? 615 : height;
            }
            else
            {
                height = 300;
            }

            int total = 0;
            Pulse pulseWidget = new Pulse();
            List<Pulse> pulses = new List<Pulse>();
            ContentRepository content = new ContentRepository(SessionCustom);
            UserRepository user = new UserRepository(SessionCustom);

            pulseWidget = content.PulsesWidget(0, 10, out total).Where(p => p.EndDate > DateTime.Now.Date).FirstOrDefault();
            if (pulseWidget != null)
            {
                pulses = content.Pulses(0, 10, out total, null,CurrentLanguage.LanguageId).Where(p => p.EndDate > DateTime.Now.Date && p.ContentId != pulseWidget.ContentId).ToList();
            }
            else
            {
                pulses = content.Pulses(0, 10, out total, null,CurrentLanguage.LanguageId).Where(p => p.EndDate > DateTime.Now.Date).ToList();
            }

            ViewBag.PulseWidget = pulseWidget;
            ViewBag.Pulses = pulses;

            int totalParticipants;
            ViewBag.Partipants = user.Participants(10, out totalParticipants);
            ViewBag.TotalParticipants = totalParticipants;

            ViewBag.Width = width;
            ViewBag.Height = height;

            return this.View();
        }
    }
}
