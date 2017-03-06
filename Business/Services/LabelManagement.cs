// <copyright file="CustomPrincipal.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Juan Carlos Montoya</author>

namespace Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Remoting;
    using System.Web;
    using Business.FrontEnd;
    using Domain.Abstract;
    using Domain.Concrete;
    using Domain.Entities;

     public class LabelManagement
    {
        /// <summary>
        /// framework that establishes communication between the application and the database
        /// </summary>
        private ISession session;

        /// <summary>
        /// HTTP context
        /// </summary>
        private HttpContextBase context;

        public LabelManagement(ISession session, HttpContextBase context)
        {
            this.session = session;
            this.context = context;
        }

         /// <summary>
         /// 
         /// </summary>
         /// <param name="name"></param>
         /// <param name="languageid">Lenguage</param>
        /// <param name="session">framework that establishes communication between the application and the database</param>
         /// <param name="context"></param>
         /// <returns></returns>
        public string GetLabelByName(string name,int languageid)
        {
            LabelRepository objrep = new LabelRepository(session);
            if (objrep.GetLabelByName(name, languageid) != null)
            {
                return objrep.GetLabelByName(name, languageid);
            }
            else
            {
                return name;
            }
        }

    }
}
