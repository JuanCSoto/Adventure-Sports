// <copyright file="CategoryBussinessTest.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace UnitTest.Backend.CoreTest.Bussiness
{
    using System.Collections.Generic;
    using Core.Bussiness;
    using Core.Bussiness.Contracts;
    using Core.Bussiness.Repositories;
    using Domain.Entities.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Representa la clase <see cref="UnitTest.Backend.CoreTest.Bussiness.CategoryBussinessTest"/>
    /// </summary>
    [TestClass]
    public class CategoryBussinessTest
    {
        /// <summary>
        /// Mock para la interfaz <see cref="Core.Bussiness.Repositories.ICategoryRepository"/> para simular el acceso al repositorio de categorías.
        /// </summary>
        private Mock<ICategoryRepository> categoryRepository = null;

        /// <summary>
        /// Interfaz de tipo <see cref="Core.Bussiness.Contracts.ICategoryBussiness"/> para acceder al negocio de las categorías.
        /// </summary>
        private ICategoryBussiness categoryBussiness = null;

        /// <summary>
        /// Inicializa la información necesaria para las pruebas.
        /// </summary>
        [TestInitialize]
        public void CategoryBussinessTestInitialize()
        {
            this.categoryRepository = new Mock<ICategoryRepository>();
            this.categoryBussiness = new CategoryBussiness(categoryRepository.Object);
        }

        /// <summary>
        /// Verifica si el método GetAll del respositorio de categorías es llamado desde el método GetAll del negocio de categorías.
        /// </summary>
        [TestMethod]
        public void Test_Verificar_Llamado_Del_Metodo_GetAll_En_Repositorio()
        {
            this.categoryRepository.Setup(it => it.GetAll(It.IsAny<int>())).Verifiable();
            IList<Category> actualCategories = this.categoryBussiness.GetAll(0);
            this.categoryRepository.Verify(it => it.GetAll(It.IsAny<int>()));
        }

        /// <summary>
        /// Verifica el escenario donde el método GetAll del repositorio de categorías no retorna ninguna categoría.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todas_Las_Categorias_No_Retorna_Registros()
        {
            IList<Category> expectedCategories = new List<Category>();
            this.categoryRepository.Setup(it => it.GetAll(It.IsAny<int>())).Returns(expectedCategories);
            IList<Category> actualCategories = this.categoryBussiness.GetAll(0);
            Assert.AreEqual(0, actualCategories.Count);
        }

        /// <summary>
        /// Verifica el escenario donde el método GetAll del repositorio de categorías retorna alguna categoría.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todas_Las_Categorias_Retorna_Registros()
        {
            IList<Category> expectedCategories = new List<Category>();
            expectedCategories.Add(new Category { CategoryId = 1 });
            this.categoryRepository.Setup(it => it.GetAll(It.IsAny<int>())).Returns(expectedCategories);
            IList<Category> actualCategories = this.categoryBussiness.GetAll(0);
            Assert.AreEqual(1, actualCategories.Count);
        }

        /// <summary>
        /// Verifica el escenario donde el método GetAll del repositorio de categorías retorna alguna categoría y valida que sean las esperadas.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todas_Las_Categorias_Validar_Que_Sean_Las_Esperadas()
        {
            IList<Category> expectedCategories = new List<Category>();
            expectedCategories.Add(new Category { CategoryId = 1 });
            this.categoryRepository.Setup(it => it.GetAll(It.IsAny<int>())).Returns(expectedCategories);
            IList<Category> actualCategories = this.categoryBussiness.GetAll(0);
            Assert.AreSame(expectedCategories, actualCategories);
        }
    }
}
