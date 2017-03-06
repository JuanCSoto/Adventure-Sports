// <copyright file="AutomaticController.cs" company="Dasigno">
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
    using Domain.Entities;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Action controller for the "Automatic" actions
    /// </summary>
    public class AutomaticController : FrontEndController
    {
        /// <summary>
        /// Send all the finishing process notifications
        /// </summary>
        /// <param name="id">Security token</param>
        /// <returns>A JSON OBJECT with the result of the operation</returns>
        [HttpGet]
        public JsonResult FinishingProcess(string id)
        {
            bool result = false;
            if (this.CheckToken(id))
            {
                UserSettingRepository setting = new UserSettingRepository(SessionCustom);
                setting.Entity.KeyWord = "send-finishing-process";
                List<UserSetting> userSettings = setting.GetAll().Where(us => us.Value.Equals("true")).ToList();

                setting.Entity.KeyWord = "value-finishing-process";
                List<UserSetting> userSettingsValue = setting.GetAll();

                DateTime nowDate = DateTime.Now.Date;

                ContentRepository content = new ContentRepository(SessionCustom);
                int total = 0;
                content.Pulses(0, 0, out total, null,CurrentLanguage.LanguageId);
                List<Pulse> pulses = content.Pulses(0, total, out total, null,CurrentLanguage.LanguageId);
                List<Pulse> pulsesFilter = null;

                if (userSettings.Count > 0)
                {
                    UserSetting userSettingValue = null;
                    int days = 0;
                    foreach (UserSetting userSetting in userSettings)
                    {
                        userSettingValue = userSettingsValue.Where(usv => usv.UserId == userSetting.UserId).FirstOrDefault();
                        if (userSettingValue != null)
                        {
                            if (int.TryParse(userSettingValue.Value, out days))
                            {
                                pulsesFilter = pulses.Where(p => p.EndDate.HasValue && (p.EndDate.Value.Date - nowDate).TotalDays == days).ToList();
                                foreach (Pulse pulse in pulsesFilter)
                                {
                                    Business.Utilities.Notification.NewNotification(userSettingValue.UserId.Value, Domain.Entities.Basic.EmailNotificationType.FINISHING_PROCESS, null, null, string.Concat("/", pulse.Friendlyurlid), pulse.ContentId, pulse.ContentId.Value, null, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
                                }
                            }
                        }
                    }
                }

                int finishingId = (int)Domain.Entities.Basic.SystemNotificationType.FINISHING_PROCESS;

                SystemNotificationTemplateRepository system = new SystemNotificationTemplateRepository(SessionCustom);
                system.Entity.ContentId = finishingId;
                system.LoadByKey();
                if (system.Entity.SendValue.HasValue && system.Entity.SendValue.Value > 0)
                {
                    pulsesFilter = pulses.Where(p => p.EndDate.HasValue && (p.EndDate.Value.Date - nowDate).TotalDays == system.Entity.SendValue.Value).ToList();
                    foreach (Pulse pulse in pulsesFilter)
                    {
                        Business.Utilities.Notification.StartFinishingProcess(string.Concat("/", pulse.Friendlyurlid), pulse.ContentId.Value, this.HttpContext, this.CurrentLanguage);
                    }
                }

                result = true;
            }

            return this.Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Send all the finished process notifications
        /// </summary>
        /// <param name="id">Security token</param>
        /// <returns>A JSON OBJECT with the result of the operation</returns>
        [HttpGet]
        public JsonResult FinishedProcess(string id)
        {
            bool result = false;
            if (this.CheckToken(id))
            {
                UserSettingRepository setting = new UserSettingRepository(SessionCustom);
                setting.Entity.KeyWord = "send-finished-process";
                List<UserSetting> userSettings = setting.GetAll().Where(us => us.Value.Equals("true")).ToList();

                int total = 0;
                ContentRepository content = new ContentRepository(SessionCustom);
                content.Pulses(0, 0, out total, null,CurrentLanguage.LanguageId);
                List<Pulse> pulses = content.Pulses(0, total, out total, null,CurrentLanguage.LanguageId);
                List<Pulse> pulsesFilter = null;

                DateTime nowDate = DateTime.Now.Date;
                pulsesFilter = pulses.Where(p => p.EndDate.HasValue && (p.EndDate.Value.Date - nowDate).TotalDays == 0).ToList();

                if (userSettings.Count > 0)
                {
                    foreach (UserSetting userSetting in userSettings)
                    {
                        foreach (Pulse pulse in pulsesFilter)
                        {
                            Business.Utilities.Notification.NewNotification(userSetting.UserId.Value, Domain.Entities.Basic.EmailNotificationType.FINISHED_PROCESS, null, null, string.Concat("/", pulse.Friendlyurlid), pulse.ContentId, pulse.ContentId.Value, null, null, null, this.SessionCustom, this.HttpContext, this.CurrentLanguage);
                        }
                    }
                }

                foreach (Pulse pulse in pulsesFilter)
                {
                    Business.Utilities.Notification.StartFinishedProcess(string.Concat("/", pulse.Friendlyurlid), pulse.ContentId.Value, this.HttpContext, this.CurrentLanguage);
                }

                result = true;
            }

            return this.Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validates the security token send to execute certain methods
        /// </summary>
        /// <param name="token">Security token to check</param>
        /// <returns>True if the token is valid, false otherwise</returns>
        private bool CheckToken(string token)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(token))
            {
                DateTime date = DateTime.Now.AddMinutes(-5);

                string check = string.Empty;
                for (int i = 0; i < 10; i++)
                {
                    check = Business.Utils.EncriptMD5(string.Concat(Business.Utils.SecretWord(), date.AddMinutes(i).ToString("yyyyMMddhhmm")));
                    if (token.Equals(check))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
