// <copyright file="CategoryBussiness.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTest")]
namespace Core.Bussiness
{
    using System.Collections.Generic;
    using Core.Bussiness.Contracts;
    using Core.Bussiness.Repositories;
    using Domain.Entities.Generic;    

    /// <summary>
    /// Representa la clase <see cref="Core.Bussiness.CategoryBussiness"/>.
    /// </summary>    
    internal class CategoryBussiness : ICategoryBussiness
    {
        /// <summary>
        /// Interfaz de tipo <see cref="Core.Bussiness.Repositories.ICategoryRepository"/> para acceder al repositorio de categorías.
        /// </summary>
        private ICategoryRepository categoryRepository;

        /// <summary>
        /// Inicialiaza una instancia de la clase <see cref="Core.Bussiness.CategoryBussiness"/>.
        /// </summary>
        /// <param name="categoryRepository">Interfaz de tipo <see cref="Core.Bussiness.Repositories.ICategoryRepository"/> para acceder al repositorio de categorías.</param>
        public CategoryBussiness(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        
        /// <summary>
        /// Consulta las categorías configuradas en el sistema.
        /// </summary>
        /// <param name="languageId">Id del lenguaje en el que se mostraran las categorías.</param>
        /// <returns>Una lista con las categorías configuradas en el sistema.</returns>
        public IList<Category> GetAll(int languageId)
        {
            return this.categoryRepository.GetAll(languageId);
        }
    }
}
