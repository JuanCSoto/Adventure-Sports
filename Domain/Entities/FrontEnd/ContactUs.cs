// <copyright file="ContactUs.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Domain.Entities.FrontEnd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    /// <summary>
    /// contact us entity object
    /// </summary>
    public class ContactUs
    {
        /// <summary>
        /// Gets or sets the names
        /// </summary>
        
        [Required(ErrorMessageResourceType = typeof(Resources.Extend.Messages), ErrorMessageResourceName = "VALIDATIONNAME")]
        public string Names { get; set; }
        
        /// <summary>
        /// Gets or sets the email
        /// </summary>
        /// 
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessageResourceType = typeof(Resources.Extend.Messages), ErrorMessageResourceName = "VALIDATIONCORREOINVALIDO")]
        [Required(ErrorMessageResourceType = typeof(Resources.Extend.Messages), ErrorMessageResourceName = "VALIDATIONCORREO")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        
        [Required(ErrorMessageResourceType = typeof(Resources.Extend.Messages), ErrorMessageResourceName = "VALIDATIONSUBJECT")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        
        [Required(ErrorMessageResourceType = typeof(Resources.Extend.Messages), ErrorMessageResourceName = "VALIDATIONMESSAGE")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the email was sent or not
        /// </summary>
        public bool Sent { get; set; }
    }
}
