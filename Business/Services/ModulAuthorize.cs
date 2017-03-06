// <copyright file="ModulAuthorize.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business.Services
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Domain.Abstract;
    using Domain.Entities;
    
    /// <summary>
    /// Represents an attribute that is used to restrict access by callers to an
    /// action method.
    /// </summary>
    public class ModulAuthorize : AuthorizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModulAuthorize"/> class
        /// </summary>
        public ModulAuthorize()
        {
        }

        /// <summary>
        /// provides an entry point for custom authorization checks.
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <returns>true if the user is authorized; otherwise, false.</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (base.AuthorizeCore(httpContext))
            {
                if (httpContext.Request.QueryString["mod"] != null)
                {
                    int modulId = 0;
                    int.TryParse(httpContext.Request.QueryString["mod"], out modulId);
                    int userId = (httpContext.User as CustomPrincipal).UserId;
                    List<Modul> moduls = CustomMemberShipProvider.GetModuls(userId, new SqlSession(), httpContext);
                    return moduls.Exists(t => t.ModulId == modulId);
                }
                else if (httpContext.Request.RequestContext.RouteData.Values.ContainsKey("controller"))
                {
                    string controller = httpContext.Request.RequestContext.RouteData.GetRequiredString("controller");
                    int userId = (httpContext.User as CustomPrincipal).UserId;
                    List<Modul> moduls = CustomMemberShipProvider.GetModuls(userId, new SqlSession(), httpContext);
                    return moduls.Exists(t => t.Controller == controller);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}