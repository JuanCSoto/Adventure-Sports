// <copyright file="Allies.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno</author>
namespace Business.FrontEnd
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;
    using System.Xml;
    using Domain.Abstract;
    using Domain.Concrete;
    using Domain.Entities;
    using Domain.Entities.FrontEnd;

    /// <summary>
    /// Represents the model for the front end
    /// </summary>
    public class Allies : IFrontEnd
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Allies"/> class
        /// </summary>
        public Allies()
        {
        }

        /// <summary>
        /// Gets or sets the allies collection
        /// </summary>
        public List<Ally> AlliesDetail { get; set; }

        /// <summary>
        /// Gets or sets the content collection
        /// </summary>
        public List<Content> Contents { get; set; }

        /// <summary>
        /// Binds the information of the content with the allies
        /// </summary>
        /// <param name="context">Context page</param>
        /// <param name="session">Session object</param>
        /// <param name="id">Content ID</param>
        /// <param name="userId">current user ID</param>
        public void Bind(HttpContextBase context, ISession session, int? id, int? userId,int? LanguageId)
        {
            ContentRepository content = new ContentRepository(session);
            content.Entity.ModulId = 62;
            content.Entity.Active = true;
            this.Contents = content.GetAll();

            AllyRepository ally = new AllyRepository(session);
            this.AlliesDetail = ally.GetAll();
        }
    }
}
