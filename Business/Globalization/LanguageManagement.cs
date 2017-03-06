// <copyright file="LanguageManagement.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business.Globalization
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Domain.Abstract;
    using Domain.Concrete;
    using Domain.Entities;

    /// <summary>
    /// management language object
    /// </summary>
    public class LanguageManagement
    {
        /// <summary>
        /// framework that establishes communication between the application and the database
        /// </summary>
        private ISession session;

        /// <summary>
        /// HTTP context
        /// </summary>
        private HttpContextBase context;

        /// <summary>
        /// list a language object
        /// </summary>
        private List<Language> colllang;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageManagement"/> class
        /// </summary>
        /// <param name="session">framework that establishes communication between the application and the database</param>
        /// <param name="context">HTTP context</param>
        public LanguageManagement(ISession session, HttpContextBase context)
        {
            this.session = session;
            this.context = context;

            InfoCache<List<Language>> lang = new InfoCache<List<Language>>(context);
            this.colllang = lang.GetCache("languages");

            if (this.colllang == null || this.colllang.Any().Equals(0))
            {
                LanguageRepository langrepo = new LanguageRepository(session);
                this.colllang = langrepo.GetAll();

                lang.SetCache("languages", this.colllang);
            }
        }

        /// <summary>
        /// Gets the object language according to the culture name
        /// </summary>
        /// <param name="culture">name of culture</param>
        /// <returns>returns a language object</returns>
        public Language GetLanguage(string culture)
        {
            Language language = this.colllang.FirstOrDefault(t => t.Culturename == culture || t.Culturename == culture.Split('-')[0]);
            if (null == language)
            {
                language = this.GetLanguageDefault();
            }

            return language;
        }

        /// <summary>
        /// Gets a default language of application
        /// </summary>
        /// <returns>returns a language object</returns>
        public Language GetLanguageDefault()
        {
            return this.colllang.FirstOrDefault(t => t.IsDefault.Value == true);
        }
    }
}