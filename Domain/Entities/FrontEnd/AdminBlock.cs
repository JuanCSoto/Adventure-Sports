// <copyright file="AdminBlock.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Domain.Entities.FrontEnd
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// property object for the administration block
    /// </summary>
    public class AdminBlock
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Gets or sets the type
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Gets or sets the parent type
        /// </summary>
        public string parentType { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        /// Gets or sets the location
        /// </summary>
        public string location { get; set; }
    }
}
