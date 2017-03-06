// <copyright file="IFrontEnd.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business.FrontEnd
{
    using System.Web;
    using Domain.Abstract;
    
    /// <summary>
    /// Defines the contract that a class must implement to be a model to frond end
    /// </summary>
    public interface IFrontEnd
    {
        /// <summary>
        /// Bind the context and the session with the content
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="session">framework that establishes communication between the application and the database</param>
        /// <param name="id">identifier or content section</param>
        /// <param name="userId">current user ID</param>
        void Bind(HttpContextBase context, ISession session, int? id, int? userId,int? LanguageId);
    }
}
