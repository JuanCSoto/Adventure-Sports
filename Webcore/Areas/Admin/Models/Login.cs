// <copyright file="Login.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Models
{
    using System.ComponentModel.DataAnnotations;
    
    /// <summary>
    /// obtains the information to login application
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Gets or sets the email user
        /// </summary>
        [Required(ErrorMessage = "Debes ingresar tu correo electrónico")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password user
        /// </summary>
        [Required(ErrorMessage = "Debes ingresar tu contraseña")]
        public string Password { get; set; }
    }
}