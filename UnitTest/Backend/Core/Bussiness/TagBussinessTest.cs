// <copyright file="TagBussinessTest.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Sergio Sierra</author>
namespace UnitTest.Backend.CoreTest.Bussiness
{
    using System.Collections.Generic;
    using Core.Bussiness;
    using Core.Bussiness.Contracts;
    using Core.Bussiness.Repositories;
    using Domain.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Representa la clase <see cref="UnitTest.Backend.CoreTest.Bussiness.TagBussinessTest"/>
    /// </summary>
    [TestClass]
    public class TagBussinessTest
    {
        /// <summary>
        /// Mock para la interfaz <see cref="Core.Bussiness.Repositories.ITagRepository"/> para simular el acceso al repositorio de tags.
        /// </summary>
        private Mock<ITagRepository> tagRepository = null;

        /// <summary>
        /// Interfaz de tipo <see cref="Core.Bussiness.Contracts.ITagBussiness"/> para acceder al negocio de tags.
        /// </summary>
        private ITagBussiness tagBussiness = null;

        /// <summary>
        /// Inicializa la información necesaria para las pruebas.
        /// </summary>
        [TestInitialize]
        public void CategoryBussinessTestInitialize()
        {
            this.tagRepository = new Mock<ITagRepository>();
            this.tagBussiness = new TagBussiness(tagRepository.Object);
        }

        /// <summary>
        /// Verifica si el método GetAll del respositorio de tags es llamado desde el método GetAll del negocio de tags.
        /// </summary>
        [TestMethod]
        public void Test_Verificar_Llamado_Del_Metodo_GetAll_En_Repositorio()
        {
            this.tagRepository.Setup(it => it.GetAll()).Verifiable();
            IList<Tag> actualCategories = this.tagBussiness.GetAll();
            this.tagRepository.Verify(it => it.GetAll());
        }

        /// <summary>
        /// Verifica el escenario donde el método GetAll del repositorio de tags no retorna ningun tag.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todos_Los_Tags_No_Retorna_Registros()
        {
            IList<Tag> expectedTagList = new List<Tag>();
            this.tagRepository.Setup(it => it.GetAll()).Returns(expectedTagList);
            IList<Tag> actualCategoriesList = this.tagBussiness.GetAll();
            Assert.AreEqual(0, actualCategoriesList.Count);
        }

        /// <summary>
        /// Verifica el escenario donde el método GetAll del repositorio de tags retorna algun tag.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todos_Los_Tags_Retorna_Registros()
        {
            IList<Tag> expectedTagList = new List<Tag>();
            expectedTagList.Add(new Tag { TagId = 1 });
            this.tagRepository.Setup(it => it.GetAll()).Returns(expectedTagList);
            IList<Tag> actualCategoriesList = this.tagBussiness.GetAll();
            Assert.AreEqual(1, actualCategoriesList.Count);
        }

        /// <summary>
        /// Verifica el escenario donde el método GetAll del repositorio de tags retorna algun tag y valida que sean los esperados.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todos_Los_Tags_Validar_Que_Sean_Los_Esperados()
        {
            IList<Tag> expectedTagsList = new List<Tag>();
            expectedTagsList.Add(new Tag { TagId = 1 });
            this.tagRepository.Setup(it => it.GetAll()).Returns(expectedTagsList);
            IList<Tag> actualCategoriesList = this.tagBussiness.GetAll();
            Assert.AreSame(expectedTagsList, actualCategoriesList);
        }
        
        /// <summary>
        /// Verifica si el método GetBySuccessStoryPostulate del respositorio de tags es llamado desde el método GetBySuccessStoryPostulate del negocio de tags.
        /// </summary>
        [TestMethod]
        public void Test_Verificar_Llamado_Del_Metodo_GetBySuccessStoryPostulate_En_Repositorio()
        {
            this.tagRepository.Setup(it => it.GetBySuccessStoryPostulate(It.IsAny<int>())).Verifiable();
            IList<Tag> actualCategories = this.tagBussiness.GetBySuccessStoryPostulate(0);
            this.tagRepository.Verify(it => it.GetBySuccessStoryPostulate(It.IsAny<int>()));
        }

        /// <summary>
        /// Verifica el escenario donde el método GetBySuccessStoryPostulate del repositorio de tags no retorna ningun tag.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todos_Los_Tags_Por_Postulacion_De_Caso_De_Exito_No_Retorna_Registros()
        {
            IList<Tag> expectedTagList = new List<Tag>();
            this.tagRepository.Setup(it => it.GetBySuccessStoryPostulate(It.IsAny<int>())).Returns(expectedTagList);
            IList<Tag> actualCategoriesList = this.tagBussiness.GetBySuccessStoryPostulate(0);
            Assert.AreEqual(0, actualCategoriesList.Count);
        }

        /// <summary>
        /// Verifica el escenario donde el método GetBySuccessStoryPostulate del repositorio de tags retorna algun tag.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todos_Los_Tags_Por_Postulacion_De_Caso_De_Exito_Retorna_Registros()
        {
            IList<Tag> expectedTagList = new List<Tag>();
            expectedTagList.Add(new Tag { TagId = 1 });
            this.tagRepository.Setup(it => it.GetBySuccessStoryPostulate(It.IsAny<int>())).Returns(expectedTagList);
            IList<Tag> actualCategoriesList = this.tagBussiness.GetBySuccessStoryPostulate(0);
            Assert.AreEqual(1, actualCategoriesList.Count);
        }

        /// <summary>
        /// Verifica el escenario donde el método GetBySuccessStoryPostulate del repositorio de tags retorna algun tag y valida que sean los esperados.
        /// </summary>
        [TestMethod]
        public void Test_Consultar_Todos_Los_Tags_Por_Postulacion_De_Caso_De_Exito_Validar_Que_Sean_Los_Esperados()
        {
            IList<Tag> expectedTagsList = new List<Tag>();
            expectedTagsList.Add(new Tag { TagId = 1 });
            this.tagRepository.Setup(it => it.GetBySuccessStoryPostulate(It.IsAny<int>())).Returns(expectedTagsList);
            IList<Tag> actualCategoriesList = this.tagBussiness.GetBySuccessStoryPostulate(0);
            Assert.AreSame(expectedTagsList, actualCategoriesList);
        }
    }
}
