// <copyright file="GeneralFindBussinessTest.cs" company="Intergrupo">
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
    using System.Collections.Generic;

    [TestClass]
    public class GeneralFindBussinessTest
    {
        private Mock<IGeneralFindRepository> findRepository = null;

        private IGeneralFindBussiness findBussiness = null;

        [TestInitialize]
        public void GeneralFindBussinessTestInitialize()
        {
            this.findRepository = new Mock<IGeneralFindRepository>();
            this.findBussiness = new GeneralFindBussiness(findRepository.Object);
        }

        [TestMethod]
        public void Test_GetByFindAnd_Verificar_Llamado_Metodo_GetById_Del_Repositorio()
        {
            int total = 0;
            this.findRepository.Setup(it => it.GetByFindAnd(It.IsAny<string>(), It.IsAny<string>(), 0, 0, out total)).Verifiable();
            List<GeneralFindPaging> actual = this.findBussiness.GetByFindAnd(string.Empty, string.Empty, 0, 0, out total);
            this.findRepository.Verify(it => it.GetByFindAnd(It.IsAny<string>(), It.IsAny<string>(), 0, 0, out total));
        }

        [TestMethod]
        public void Test_GetByFindAnd_Consultar_Todos_Los_CasosExito_No_Retorna_Registros()
        {
            int total = 0;
            List<GeneralFindPaging> expected = new List<GeneralFindPaging>();
            this.findRepository.Setup(it => it.GetByFindAnd(It.IsAny<string>(), It.IsAny<string>(), 0, 0, out total)).Returns(expected);
            List<GeneralFindPaging> actual = this.findBussiness.GetByFindAnd(string.Empty, string.Empty, 0, 0, out total);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_GetByFindAnd_Consultar_Todos_Los_CasosExito_Retorna_Registros()
        {
            int total = 0;
            List<GeneralFindPaging> expected = new List<GeneralFindPaging>();
            this.findRepository.Setup(it => it.GetByFindAnd(It.IsAny<string>(), It.IsAny<string>(), 0, 0, out total)).Returns(expected);
            List<GeneralFindPaging> actual = this.findBussiness.GetByFindAnd(string.Empty, string.Empty, 0, 0, out total);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_GetByFindAnd_Consultar_Todas_Los_CasosExito_Validar_Que_Sean_Las_Esperadas()
        {
            int total = 0;
            List<GeneralFindPaging> expected = new List<GeneralFindPaging>();
            this.findRepository.Setup(it => it.GetByFindAnd(It.IsAny<string>(), It.IsAny<string>(), 0, 0, out total)).Returns(expected);
            List<GeneralFindPaging> actual = this.findBussiness.GetByFindAnd(string.Empty, string.Empty, 0, 0, out total);
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void Test_GetByFindOr_Verificar_Llamado_Metodo_GetById_Del_Repositorio()
        {
            int total = 0;
            this.findRepository.Setup(it => it.GetByFindOr(It.IsAny<string>(), It.IsAny<string>(), 0, 0, out total)).Verifiable();
            List<GeneralFindPaging> actual = this.findBussiness.GetByFindOr(string.Empty, string.Empty, 0, 0, out total);
            this.findRepository.Verify(it => it.GetByFindAnd(It.IsAny<string>(), It.IsAny<string>(), 0, 0, out total));
        }

        [TestMethod]
        public void Test_GetByFindOr_Consultar_Todos_Los_CasosExito_No_Retorna_Registros()
        {
            int total = 0;
            List<GeneralFindPaging> expected = new List<GeneralFindPaging>();
            this.findRepository.Setup(it => it.GetByFindOr(It.IsAny<string>(), It.IsAny<string>(), 0, 0, out total)).Returns(expected);
            List<GeneralFindPaging> actual = this.findBussiness.GetByFindOr(string.Empty, string.Empty, 0, 0, out total);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_GetByFindOr_Consultar_Todos_Los_CasosExito_Retorna_Registros()
        {
            int total = 0;
            List<GeneralFindPaging> expected = new List<GeneralFindPaging>();
            this.findRepository.Setup(it => it.GetByFindOr(It.IsAny<string>(), It.IsAny<string>(), 0, 0, out total)).Returns(expected);
            List<GeneralFindPaging> actual = this.findBussiness.GetByFindOr(string.Empty, string.Empty, 0, 0, out total);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_GetByFindOr_Consultar_Todas_Los_CasosExito_Validar_Que_Sean_Las_Esperadas()
        {
            int total = 0;
            List<GeneralFindPaging> expected = new List<GeneralFindPaging>();
            this.findRepository.Setup(it => it.GetByFindOr(It.IsAny<string>(), It.IsAny<string>(), 0, 0, out total)).Returns(expected);
            List<GeneralFindPaging> actual = this.findBussiness.GetByFindOr(string.Empty, string.Empty, 0, 0, out total);
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void Test_GetAll_Verificar_Llamado_Metodo_GetById_Del_Repositorio()
        {
            this.findRepository.Setup(it => it.GetAll()).Verifiable();
            List<GeneralFind> actual = this.findBussiness.GetAll();
            this.findRepository.Verify(it => it.GetAll());
        }

        [TestMethod]
        public void Test_GetAll_Consultar_Todos_Los_CasosExito_No_Retorna_Registros()
        {
            List<GeneralFind> expected = new List<GeneralFind>();
            this.findRepository.Setup(it => it.GetAll()).Returns(expected);
            List<GeneralFind> actual = this.findBussiness.GetAll();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_GetAll_Consultar_Todos_Los_CasosExito_Retorna_Registros()
        {
            List<GeneralFind> expected = new List<GeneralFind>();
            this.findRepository.Setup(it => it.GetAll()).Returns(expected);
            List<GeneralFind> actual = this.findBussiness.GetAll();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_GetAll_Consultar_Todas_Los_CasosExito_Validar_Que_Sean_Las_Esperadas()
        {
            List<GeneralFind> expected = new List<GeneralFind>();
            this.findRepository.Setup(it => it.GetAll()).Returns(expected);
            List<GeneralFind> actual = this.findBussiness.GetAll();
            Assert.AreSame(expected, actual);
        }
    }
}