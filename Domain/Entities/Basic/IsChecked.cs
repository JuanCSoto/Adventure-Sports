// <copyright file="IsChecked.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Domain.Entities.Basic
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    /// <summary>
    /// validation property for unobtrusive
    /// </summary>
    public class IsChecked : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// return the element value has a boolean
        /// </summary>
        /// <param name="value">object value</param>
        /// <returns>a boolean value of the element</returns>
        public override bool IsValid(object value)
        {
            return value is bool && (bool)value;
        }

        /// <summary>
        /// A model client validation rule for the checkbox
        /// </summary>
        /// <param name="metadata">The model metadata</param>
        /// <param name="context">The controller context</param>
        /// <returns>A model client validation rule</returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new ModelClientValidationRule[] 
            {
                new ModelClientValidationRule
                {
                    ValidationType = "checkboxtrue",
                    ErrorMessage = FormatErrorMessage(metadata.DisplayName)
                }
            };
        }        
    }
}
