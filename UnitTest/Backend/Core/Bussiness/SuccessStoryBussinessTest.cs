// <copyright file="SuccessStoryBussinessTest.cs" company="Intergrupo">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
namespace UnitTest.Backend.CoreTest.Bussiness
{
    using Core.Bussiness;
    using Core.Bussiness.Contracts;
    using Core.Bussiness.Repositories;
    using Domain.Entities.FrontEnd;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class SuccessStoryBussinessTest
    {
        private Mock<ISuccessStoryRepository> successStoryRepository = null;

        private ISuccessStoryBussiness successStoryBussiness = null;

        [TestInitialize]
        public void SuccessStoryBussinessTestInitialize()
        {
            this.successStoryRepository = new Mock<ISuccessStoryRepository>();
            this.successStoryBussiness = new SuccessStoryBussiness(successStoryRepository.Object);
        }

        [TestMethod]
        public void Test_Verificar_Llamado_Metodo_GetById_Del_Repositorio()
        {
            this.successStoryRepository.Setup(it => it.GetById(It.IsAny<int>(), It.IsAny<int>())).Verifiable();
            SuccessStoryList actual = this.successStoryBussiness.GetById(0, 0);
            this.successStoryRepository.Verify(it => it.GetById(It.IsAny<int>(), It.IsAny<int>()));
        }

        [TestMethod]
        public void Test_Consultar_Todos_Los_CasosExito_No_Retorna_Registros()
        {
            SuccessStoryList expected = new SuccessStoryList();
            this.successStoryRepository.Setup(it => it.GetById(It.IsAny<int>(), It.IsAny<int>())).Returns(expected);
            SuccessStoryList actual = this.successStoryBussiness.GetById(0, 0);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Consultar_Todos_Los_CasosExito_Retorna_Registros()
        {
            SuccessStoryList expected = new SuccessStoryList();
            this.successStoryRepository.Setup(it => it.GetById(It.IsAny<int>(), It.IsAny<int>())).Returns(expected);
            SuccessStoryList actual = this.successStoryBussiness.GetById(1, 1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Consultar_Todas_Los_CasosExito_Validar_Que_Sean_Las_Esperadas()
        {
            SuccessStoryList expected = new SuccessStoryList();
            this.successStoryRepository.Setup(it => it.GetById(It.IsAny<int>(), It.IsAny<int>())).Returns(expected);
            SuccessStoryList actual = this.successStoryBussiness.GetById(0, 0);
            Assert.AreSame(expected, actual);
        }
    }
}
