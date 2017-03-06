// ---------------------------------------------------------------------
// <copyright file="Captcha.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno</author>
// ---------------------------------------------------------------------

namespace Webcore.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// Class used for a captcha in the forms
    /// </summary>
    public class Captcha
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Captcha" /> class.
        /// </summary>
        public Captcha()
        {
        }

        /// <summary>
        /// Gets or sets the value of the captcha
        /// </summary>
        [Display(Name = "Captcha", Order = 20)]
        [Remote("ValidateCaptcha", "Captcha", "", ErrorMessage = "La imagen no coincide con el texto ingresado")]
        [Required(ErrorMessage = "Ingresa el captcha")]
        public virtual string CaptchaValue { get; set; }
    }
}