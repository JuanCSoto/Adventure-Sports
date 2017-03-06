// <copyright file="CustomPrincipal.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business.Services
{
    using System;
    using System.Security.Principal;
    
    /// <summary>
    /// Defines the basic functionality of a principal object
    /// </summary>
    public class CustomPrincipal : IPrincipal
    {
        /// <summary>
        /// Defines the basic functionality of an identity object.
        /// </summary>
        private IIdentity identity;

        /// <summary>
        /// Defines a user roles
        /// </summary>
        private string[] roles;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPrincipal"/> class
        /// </summary>
        /// <param name="identity">identity of the session</param>
        public CustomPrincipal(IIdentity identity)
        {
            this.UserId = 0;
            this.identity = identity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPrincipal"/> class
        /// </summary>
        /// <param name="identity">Defines the basic functionality of an identity object.</param>
        /// <param name="info">user information</param>
        public CustomPrincipal(IIdentity identity, string[] info)
        {
            this.identity = identity;
            this.Email = info[0];
            this.UserId = int.Parse(info[1]);
            this.roles = info[2].Split('-');
            this.Image = info[3];
            this.Medallos = int.Parse(info[4]);
            if (info.Length >= 6)
            {
                this.IsFrontEndAdmin = bool.Parse(info[5]);
            }
        }

        /// <summary>
        /// Gets or sets a user mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a user image
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets a user points
        /// </summary>
        public int Medallos { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is front end admin or not
        /// </summary>
        public bool IsFrontEndAdmin { get; set; }

        /// <summary>
        /// Gets or sets a identifier user
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets the basic functionality of an identity object.
        /// </summary>
        public IIdentity Identity
        {
            get
            {
                return this.identity;
            }
        }

        /// <summary>
        /// obtained if the user has the role
        /// </summary>
        /// <param name="role">role name</param>
        /// <returns>returns true if user has the role</returns>
        public bool IsInRole(string role)
        {
            return Array.BinarySearch(this.roles, role) >= 0;
        }
    }
}