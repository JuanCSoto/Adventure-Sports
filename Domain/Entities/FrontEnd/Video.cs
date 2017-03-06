// <copyright file="Video.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
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
    /// video entity object
    /// </summary>
    public class Video
    {
        /// <summary>
        /// Gets or sets the video type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the video id
        /// </summary>
        public string ID { get; set; }
    }
}
