// <copyright file="ForntEndEditable.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Domain.Entities.Basic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Entity for the front end editable
    /// </summary>
    public class ForntEndEditable
    {
        /// <summary>
        /// Gets or sets the type id of the editable (template id)
        /// </summary>
        public int TypeID { get; set; }

        /// <summary>
        /// Gets or sets the value type
        /// </summary>
        public string ValueType { get; set; }

        /// <summary>
        /// Gets or sets the value text
        /// </summary>
        public string Value { get; set; }
    }
}
