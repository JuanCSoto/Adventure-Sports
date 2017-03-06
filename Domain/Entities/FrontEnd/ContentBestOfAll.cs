// <copyright file="ContentBestOfAll.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Domain.Entities.FrontEnd
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    
    /// <summary>
    /// content best of all entity object
    /// </summary>
    public class ContentBestOfAll
    {
        /// <summary>
        /// Gets or sets the innovative user collection
        /// </summary>
        public List<UserProfilePaging> InnovativeUsers { get; set; }

        /// <summary>
        /// Gets or sets the top ideas collection
        /// </summary>
        public List<IdeasPaging> TopIdeas { get; set; }

        /// <summary>
        /// Gets or sets the top voted ideas collection
        /// </summary>
        public List<IdeasPaging> TopVotedIdeas { get; set; }
    }
}
