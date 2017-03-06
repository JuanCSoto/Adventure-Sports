// <copyright file="InfoCache.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business
{
    using System;
    using System.Web;
    
    /// <summary>
    /// management cache application
    /// </summary>
    /// <typeparam name="T">type of object</typeparam>
    public class InfoCache<T>
    {
        /// <summary>
        /// HTTP context
        /// </summary>
        private HttpContextBase context;

        /// <summary>
        /// the time out of cache
        /// </summary>
        private int timeOut = 30;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoCache{T}"/> class
        /// </summary>
        public InfoCache() 
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoCache{T}"/> class
        /// </summary>
        /// <param name="context">HTTP context</param>
        public InfoCache(HttpContextBase context) 
        {
            this.context = context;
        }

        /// <summary>
        /// Gets or sets the time out of cache
        /// </summary>
        public int TimeOut
        {
            get { return this.timeOut; }
            set { this.timeOut = value; }
        }

        /// <summary>
        /// Gets the object cache
        /// </summary>
        /// <param name="value">name of object</param>
        /// <returns>returns the object from the collection cache</returns>
        public T GetCache(string value)
        {
            if (this.context.Cache[value] != null)
            {
                return (T)this.context.Cache[value];
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Sets the object cache collection
        /// </summary>
        /// <param name="key">the key for the cache item</param>
        /// <param name="value">value of the cache item</param>
        public void SetCache(string key, T value)
        {
            if (this.context.Cache[key] != null)
            {
                this.context.Cache[key] = value;
            }
            else
            {
                this.context.Cache.Insert(key, value, null, DateTime.Now.AddMinutes(this.timeOut), TimeSpan.Zero);
            }
        }
    }
}
