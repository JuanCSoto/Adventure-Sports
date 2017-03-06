// <copyright file="KeyValue.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    
    /// <summary>
    /// represents a key value pair
    /// </summary>
    public class KeyValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValue"/> class
        /// </summary>
        public KeyValue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValue"/> class
        /// </summary>
        /// <param name="key">item key</param>
        /// <param name="value">item value</param>
        public KeyValue(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the item key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the item value
        /// </summary>
        [DataType(DataType.MultilineText)] 
        public string Value { get; set; }
    }
}
