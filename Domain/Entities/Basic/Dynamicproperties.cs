// <copyright file="Dynamicproperties.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Xml;
    
    /// <summary>
    /// represents a object with properties dynamics
    /// </summary>
    public class Dynamicproperties
    {
        /// <summary>
        /// XML document object
        /// </summary>
        private XmlDocument xmlDocument;

        /// <summary>
        /// Collection of keys and values 
        /// </summary>
        private Dictionary<string, object> properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dynamicproperties"/> class
        /// </summary>
        /// <param name="xml">string to XMl information</param>
        public Dynamicproperties(string xml)
        {
            this.xmlDocument = new XmlDocument();
            this.xmlDocument.LoadXml(xml);
            this.properties = new Dictionary<string, object>();

            foreach (XmlNode item in this.xmlDocument.SelectNodes("/content/node"))
            {
                this.properties.Add(item.Attributes["id"].InnerText, item.InnerText);
            }
        }

        /// <summary>
        /// Gets or sets the property value
        /// </summary>
        /// <param name="name">name of property</param>
        /// <returns>returns the property value</returns>
        public object this[string name]
        {
            get
            {
                if (this.properties.ContainsKey(name))
                {
                    return this.properties[name];
                }

                return null;
            }

            set
            {
                this.properties[name] = value;
            }
        }
    }
}
