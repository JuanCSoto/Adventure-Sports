// <copyright file="UnityDependencyResolver.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    
    /// <summary>
    /// Defines the methods that simplify service location and dependency resolution.
    /// </summary>
    public class UnityDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// behavior of the Unity dependency injection container.
        /// </summary>
        private IUnityContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityDependencyResolver"/> class
        /// </summary>
        /// <param name="container">behavior of the Unity dependency injection container.</param>
        public UnityDependencyResolver(IUnityContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Resolves singly registered services that support arbitrary object creation.
        /// </summary>
        /// <param name="serviceType">The type of the requested service or object.</param>
        /// <returns>The requested service or object.</returns>
        public object GetService(Type serviceType)
        {
            try
            {
                return this.container.Resolve(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Resolves multiply registered services.
        /// </summary>
        /// <param name="serviceType">The type of the requested services.</param>
        /// <returns>The requested services.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return this.container.ResolveAll(serviceType);
            }
            catch (Exception)
            {
                return new List<object>();
            }
        }
    }
}